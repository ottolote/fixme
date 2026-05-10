using System.Text.Json;
using FixMe.Access.Equipment.Interface;
using Microsoft.Data.Sqlite;
using EquipmentModel = FixMe.Access.Equipment.Interface.Equipment;

namespace FixMe.Access.Equipment.Service
{
    internal sealed class EquipmentResource
    {
        private readonly string _connectionString;
        private readonly object _syncRoot = new();

        public EquipmentResource()
            : this(CreateDefaultConnectionString())
        {
        }

        public EquipmentResource(string connectionString)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

            _connectionString = connectionString;
            Initialize();
        }

        public EquipmentModel? Filter(EquipmentCriteria criteria)
        {
            lock (_syncRoot)
            {
                using SqliteConnection connection = OpenConnection();
                using SqliteCommand command = connection.CreateCommand();
                command.CommandText = """
                    SELECT EquipmentId, CustomerId, EquipmentTypeId, RegistrationId, IsRegistered, IsEligibleForMaintenancePlan, AttributesJson
                    FROM Equipment
                    ORDER BY EquipmentId
                    """;

                using SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    EquipmentModel equipment = ReadEquipment(reader);
                    if (Matches(equipment, criteria))
                    {
                        return equipment;
                    }
                }

                return null;
            }
        }

        public EquipmentType? Filter(EquipmentTypeCriteria criteria)
        {
            lock (_syncRoot)
            {
                using SqliteConnection connection = OpenConnection();
                using SqliteCommand command = connection.CreateCommand();
                command.CommandText = """
                    SELECT EquipmentTypeId, Code, Name, IsSupported
                    FROM EquipmentType
                    ORDER BY EquipmentTypeId
                    """;

                using SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    EquipmentType equipmentType = ReadEquipmentType(reader);
                    if (Matches(equipmentType, criteria))
                    {
                        return equipmentType;
                    }
                }

                return null;
            }
        }

        public PendingRegistration? Filter(PendingRegistrationCriteria criteria)
        {
            lock (_syncRoot)
            {
                using SqliteConnection connection = OpenConnection();
                using SqliteCommand command = connection.CreateCommand();
                command.CommandText = """
                    SELECT PendingRegistrationId, CustomerId, EquipmentTypeId, Status, Decision, RejectionReason, EquipmentDataJson
                    FROM PendingRegistration
                    ORDER BY PendingRegistrationId
                    """;

                using SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    PendingRegistration registration = ReadPendingRegistration(reader);
                    if (Matches(registration, criteria))
                    {
                        return registration;
                    }
                }

                return null;
            }
        }

        public PendingRegistration Store(PendingRegistration registration)
        {
            lock (_syncRoot)
            {
                using SqliteConnection connection = OpenConnection();
                using SqliteCommand command = connection.CreateCommand();
                command.CommandText = """
                    INSERT INTO PendingRegistration (PendingRegistrationId, CustomerId, EquipmentTypeId, Status, Decision, RejectionReason, EquipmentDataJson)
                    VALUES ($pendingRegistrationId, $customerId, $equipmentTypeId, $status, $decision, $rejectionReason, $equipmentDataJson)
                    ON CONFLICT(PendingRegistrationId) DO UPDATE SET
                        CustomerId = excluded.CustomerId,
                        EquipmentTypeId = excluded.EquipmentTypeId,
                        Status = excluded.Status,
                        Decision = excluded.Decision,
                        RejectionReason = excluded.RejectionReason,
                        EquipmentDataJson = excluded.EquipmentDataJson
                    """;

                command.Parameters.AddWithValue("$pendingRegistrationId", registration.PendingRegistrationId!);
                command.Parameters.AddWithValue("$customerId", registration.CustomerId!);
                AddNullableString(command, "$equipmentTypeId", registration.EquipmentTypeId);
                command.Parameters.AddWithValue("$status", registration.Status!.Value.ToString());
                AddNullableString(command, "$decision", registration.Decision?.ToString());
                AddNullableString(command, "$rejectionReason", registration.RejectionReason);
                command.Parameters.AddWithValue("$equipmentDataJson", JsonSerializer.Serialize(registration.EquipmentData));
                command.ExecuteNonQuery();

                return registration;
            }
        }

        public EquipmentModel Store(EquipmentModel equipment)
        {
            lock (_syncRoot)
            {
                using SqliteConnection connection = OpenConnection();
                using SqliteCommand command = connection.CreateCommand();
                command.CommandText = """
                    INSERT INTO Equipment (EquipmentId, CustomerId, EquipmentTypeId, RegistrationId, IsRegistered, IsEligibleForMaintenancePlan, AttributesJson)
                    VALUES ($equipmentId, $customerId, $equipmentTypeId, $registrationId, $isRegistered, $isEligibleForMaintenancePlan, $attributesJson)
                    ON CONFLICT(EquipmentId) DO UPDATE SET
                        CustomerId = excluded.CustomerId,
                        EquipmentTypeId = excluded.EquipmentTypeId,
                        RegistrationId = excluded.RegistrationId,
                        IsRegistered = excluded.IsRegistered,
                        IsEligibleForMaintenancePlan = excluded.IsEligibleForMaintenancePlan,
                        AttributesJson = excluded.AttributesJson
                    """;

                command.Parameters.AddWithValue("$equipmentId", equipment.EquipmentId!);
                command.Parameters.AddWithValue("$customerId", equipment.CustomerId!);
                command.Parameters.AddWithValue("$equipmentTypeId", equipment.EquipmentTypeId!);
                command.Parameters.AddWithValue("$registrationId", equipment.RegistrationId!);
                command.Parameters.AddWithValue("$isRegistered", equipment.IsRegistered ? 1 : 0);
                command.Parameters.AddWithValue("$isEligibleForMaintenancePlan", equipment.IsEligibleForMaintenancePlan ? 1 : 0);
                command.Parameters.AddWithValue("$attributesJson", JsonSerializer.Serialize(equipment.Attributes));
                command.ExecuteNonQuery();

                return equipment;
            }
        }

        private static string CreateDefaultConnectionString()
        {
            string path = Path.Combine(Path.GetTempPath(), "fixme-equipment", $"{Guid.NewGuid():N}.db");
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            return new SqliteConnectionStringBuilder { DataSource = path }.ToString();
        }

        private void Initialize()
        {
            lock (_syncRoot)
            {
                using SqliteConnection connection = OpenConnection();
                using SqliteCommand command = connection.CreateCommand();
                command.CommandText = """
                    CREATE TABLE IF NOT EXISTS EquipmentType (
                        EquipmentTypeId TEXT NOT NULL PRIMARY KEY,
                        Code TEXT NOT NULL,
                        Name TEXT NOT NULL,
                        IsSupported INTEGER NOT NULL
                    );

                    CREATE UNIQUE INDEX IF NOT EXISTS IX_EquipmentType_Code ON EquipmentType (Code);

                    CREATE TABLE IF NOT EXISTS Equipment (
                        EquipmentId TEXT NOT NULL PRIMARY KEY,
                        CustomerId TEXT NOT NULL,
                        EquipmentTypeId TEXT NOT NULL,
                        RegistrationId TEXT NOT NULL,
                        IsRegistered INTEGER NOT NULL,
                        IsEligibleForMaintenancePlan INTEGER NOT NULL,
                        AttributesJson TEXT NOT NULL
                    );

                    CREATE TABLE IF NOT EXISTS PendingRegistration (
                        PendingRegistrationId TEXT NOT NULL PRIMARY KEY,
                        CustomerId TEXT NOT NULL,
                        EquipmentTypeId TEXT NULL,
                        Status TEXT NOT NULL,
                        Decision TEXT NULL,
                        RejectionReason TEXT NULL,
                        EquipmentDataJson TEXT NOT NULL
                    );
                    """;
                command.ExecuteNonQuery();
                SeedEquipmentTypes(connection);
            }
        }

        private static void SeedEquipmentTypes(SqliteConnection connection)
        {
            EquipmentType[] equipmentTypes =
            [
                new EquipmentType { EquipmentTypeId = "car", Code = "car", Name = "Car" },
                new EquipmentType { EquipmentTypeId = "e-bike", Code = "e-bike", Name = "E-bike" },
                new EquipmentType { EquipmentTypeId = "lawnmower", Code = "lawnmower", Name = "Lawnmower" },
                new EquipmentType { EquipmentTypeId = "boat", Code = "boat", Name = "Boat" },
                new EquipmentType { EquipmentTypeId = "other", Code = "other", Name = "Other" }
            ];

            foreach (EquipmentType equipmentType in equipmentTypes)
            {
                using SqliteCommand command = connection.CreateCommand();
                command.CommandText = """
                    INSERT INTO EquipmentType (EquipmentTypeId, Code, Name, IsSupported)
                    VALUES ($equipmentTypeId, $code, $name, $isSupported)
                    ON CONFLICT(EquipmentTypeId) DO NOTHING
                    """;
                command.Parameters.AddWithValue("$equipmentTypeId", equipmentType.EquipmentTypeId!);
                command.Parameters.AddWithValue("$code", equipmentType.Code!);
                command.Parameters.AddWithValue("$name", equipmentType.Name!);
                command.Parameters.AddWithValue("$isSupported", equipmentType.IsSupported ? 1 : 0);
                command.ExecuteNonQuery();
            }
        }

        private SqliteConnection OpenConnection()
        {
            SqliteConnection connection = new(_connectionString);
            connection.Open();
            return connection;
        }

        private static EquipmentModel ReadEquipment(SqliteDataReader reader)
        {
            return new EquipmentModel
            {
                EquipmentId = reader.GetString(0),
                CustomerId = reader.GetString(1),
                EquipmentTypeId = reader.GetString(2),
                RegistrationId = reader.GetString(3),
                IsRegistered = reader.GetInt32(4) != 0,
                IsEligibleForMaintenancePlan = reader.GetInt32(5) != 0,
                Attributes = DeserializeDictionary(reader.GetString(6))
            };
        }

        private static EquipmentType ReadEquipmentType(SqliteDataReader reader)
        {
            return new EquipmentType
            {
                EquipmentTypeId = reader.GetString(0),
                Code = reader.GetString(1),
                Name = reader.GetString(2),
                IsSupported = reader.GetInt32(3) != 0
            };
        }

        private static PendingRegistration ReadPendingRegistration(SqliteDataReader reader)
        {
            return new PendingRegistration
            {
                PendingRegistrationId = reader.GetString(0),
                CustomerId = reader.GetString(1),
                EquipmentTypeId = reader.IsDBNull(2) ? null : reader.GetString(2),
                Status = Enum.Parse<PendingRegistrationStatus>(reader.GetString(3)),
                Decision = reader.IsDBNull(4) ? null : Enum.Parse<RegistrationDecision>(reader.GetString(4)),
                RejectionReason = reader.IsDBNull(5) ? null : reader.GetString(5),
                EquipmentData = DeserializeDictionary(reader.GetString(6))
            };
        }

        private static Dictionary<string, string> DeserializeDictionary(string value)
        {
            return JsonSerializer.Deserialize<Dictionary<string, string>>(value) ?? [];
        }

        private static void AddNullableString(SqliteCommand command, string name, string? value)
        {
            command.Parameters.AddWithValue(name, string.IsNullOrWhiteSpace(value) ? DBNull.Value : value);
        }

        private static bool Matches(EquipmentModel equipment, EquipmentCriteria criteria)
        {
            return Matches(criteria.EquipmentId, equipment.EquipmentId)
                && Matches(criteria.CustomerId, equipment.CustomerId)
                && Matches(criteria.EquipmentTypeId, equipment.EquipmentTypeId)
                && Matches(criteria.RegistrationId, equipment.RegistrationId)
                && Matches(criteria.IsRegistered, equipment.IsRegistered)
                && Matches(criteria.IsEligibleForMaintenancePlan, equipment.IsEligibleForMaintenancePlan);
        }

        private static bool Matches(EquipmentType equipmentType, EquipmentTypeCriteria criteria)
        {
            return equipmentType.IsSupported
                && Matches(criteria.EquipmentTypeId, equipmentType.EquipmentTypeId)
                && Matches(criteria.Code, equipmentType.Code);
        }

        private static bool Matches(PendingRegistration registration, PendingRegistrationCriteria criteria)
        {
            return Matches(criteria.PendingRegistrationId, registration.PendingRegistrationId)
                && Matches(criteria.CustomerId, registration.CustomerId)
                && Matches(criteria.EquipmentTypeId, registration.EquipmentTypeId)
                && Matches(criteria.Status, registration.Status);
        }

        private static bool Matches(string? expected, string? actual)
        {
            return string.IsNullOrWhiteSpace(expected) || string.Equals(expected, actual, StringComparison.OrdinalIgnoreCase);
        }

        private static bool Matches<T>(T? expected, T actual)
            where T : struct
        {
            return expected is null || EqualityComparer<T>.Default.Equals(expected.Value, actual);
        }

        private static bool Matches<T>(T? expected, T? actual)
            where T : struct
        {
            return expected is null || EqualityComparer<T>.Default.Equals(expected.Value, actual.GetValueOrDefault());
        }
    }
}
