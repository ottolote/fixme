using FixMe.Access.Customer.Interface;
using Microsoft.Data.Sqlite;
using CustomerModel = FixMe.Access.Customer.Interface.Customer;

namespace FixMe.Access.Customer.Service;

internal sealed class CustomerResource : ICustomerResource
{
    internal static readonly CustomerResource Shared = new(Path.Combine(AppContext.BaseDirectory, "fixme-customers.db"));

    private readonly object _sync = new();
    private readonly string _connectionString;

    public CustomerResource()
        : this(Path.Combine(Path.GetTempPath(), $"fixme-customers-{Guid.NewGuid():N}.db"))
    {
    }

    internal CustomerResource(string databasePath)
        : this(new SqliteConnectionStringBuilder { DataSource = databasePath }.ToString(), initialize: true)
    {
    }

    private CustomerResource(string connectionString, bool initialize)
    {
        _connectionString = connectionString;

        if (initialize)
        {
            InitializeSchema();
        }
    }

    public Task<CustomerModel?> Find(CustomerLookup lookup)
    {
        ArgumentNullException.ThrowIfNull(lookup);

        lock (_sync)
        {
            using SqliteConnection connection = OpenConnection();
            using SqliteCommand command = connection.CreateCommand();
            (string column, string value) = ToColumnValue(lookup);
            string comparison = lookup.Field == CustomerLookupField.Email
                ? $"{column} = $value COLLATE NOCASE"
                : $"{column} = $value";
            command.CommandText = $"""
                SELECT CustomerId,
                       Email,
                       IsEmailConfirmed,
                       ConfirmationToken,
                       ConfirmationTokenExpiresAt,
                       ConfirmationTokenUsedAt,
                       ProfileReference,
                       PasswordHash,
                       Name,
                       PhoneNumber,
                       Location
                FROM Customers
                WHERE {comparison}
                LIMIT 2;
                """;
            command.Parameters.AddWithValue("$value", value);

            using SqliteDataReader reader = command.ExecuteReader();
            CustomerModel? match = null;
            if (reader.Read())
            {
                match = ReadCustomer(reader);
            }

            if (reader.Read())
            {
                throw new InvalidOperationException("Customer conflicts with an existing unique identity.");
            }

            return Task.FromResult(match is null ? null : Clone(match));
        }
    }

    public Task<CustomerModel> Store(CustomerModel customer)
    {
        ArgumentNullException.ThrowIfNull(customer);

        lock (_sync)
        {
            using SqliteConnection connection = OpenConnection();
            using SqliteTransaction transaction = connection.BeginTransaction();
            CustomerModel? existing = FindExistingCustomer(connection, transaction, customer);
            if (existing is not null)
            {
                if (HasEstablishedIdentityConflict(existing, customer) || HasUniqueIdentityConflict(connection, transaction, existing, customer))
                {
                    throw new InvalidOperationException("Customer conflicts with an existing unique identity.");
                }

                UpdateCustomer(connection, transaction, customer, existing.CustomerId, existing.Email);
            }
            else
            {
                if (HasUniqueIdentityConflict(connection, transaction, null, customer))
                {
                    throw new InvalidOperationException("Customer conflicts with an existing unique identity.");
                }

                InsertCustomer(connection, transaction, customer);
            }

            transaction.Commit();

            return Task.FromResult(Clone(customer));
        }
    }

    private void InitializeSchema()
    {
        lock (_sync)
        {
            using SqliteConnection connection = OpenConnection();
            using SqliteCommand command = connection.CreateCommand();
            command.CommandText = """
                CREATE TABLE IF NOT EXISTS Customers (
                    CustomerId TEXT NULL,
                    Email TEXT NULL COLLATE NOCASE,
                    IsEmailConfirmed INTEGER NOT NULL,
                    ConfirmationToken TEXT NULL,
                    ConfirmationTokenExpiresAt TEXT NULL,
                    ConfirmationTokenUsedAt TEXT NULL,
                    ProfileReference TEXT NULL,
                    PasswordHash TEXT NULL,
                    Name TEXT NULL,
                    PhoneNumber TEXT NULL,
                    Location TEXT NULL
                );

                CREATE UNIQUE INDEX IF NOT EXISTS IX_Customers_CustomerId
                    ON Customers(CustomerId)
                    WHERE CustomerId IS NOT NULL AND CustomerId <> '';
                CREATE UNIQUE INDEX IF NOT EXISTS IX_Customers_Email
                    ON Customers(Email COLLATE NOCASE)
                    WHERE Email IS NOT NULL AND Email <> '';
                CREATE UNIQUE INDEX IF NOT EXISTS IX_Customers_ConfirmationToken
                    ON Customers(ConfirmationToken)
                    WHERE ConfirmationToken IS NOT NULL AND ConfirmationToken <> '';
                CREATE UNIQUE INDEX IF NOT EXISTS IX_Customers_ProfileReference
                    ON Customers(ProfileReference)
                    WHERE ProfileReference IS NOT NULL AND ProfileReference <> '';
                """;
            command.ExecuteNonQuery();
        }
    }

    private SqliteConnection OpenConnection()
    {
        SqliteConnection connection = new(_connectionString);
        connection.Open();
        return connection;
    }

    private static (string Column, string Value) ToColumnValue(CustomerLookup lookup)
    {
        return lookup.Field switch
        {
            CustomerLookupField.CustomerId => ("CustomerId", lookup.Value),
            CustomerLookupField.Email => ("Email", lookup.Value),
            CustomerLookupField.ConfirmationToken => ("ConfirmationToken", lookup.Value),
            CustomerLookupField.ProfileReference => ("ProfileReference", lookup.Value),
            _ => throw new ArgumentOutOfRangeException(nameof(lookup)),
        };
    }

    private static CustomerModel? FindExistingCustomer(SqliteConnection connection, SqliteTransaction transaction, CustomerModel customer)
    {
        using SqliteCommand command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = """
            SELECT CustomerId,
                   Email,
                   IsEmailConfirmed,
                   ConfirmationToken,
                   ConfirmationTokenExpiresAt,
                   ConfirmationTokenUsedAt,
                   ProfileReference,
                   PasswordHash,
                   Name,
                   PhoneNumber,
                   Location
            FROM Customers
            WHERE ($customerId IS NOT NULL AND CustomerId = $customerId)
               OR ($email IS NOT NULL AND Email = $email COLLATE NOCASE)
               OR ($confirmationToken IS NOT NULL AND ConfirmationToken = $confirmationToken)
               OR ($profileReference IS NOT NULL AND ProfileReference = $profileReference)
            LIMIT 2;
            """;
        AddIdentityParameters(command, customer);

        using SqliteDataReader reader = command.ExecuteReader();
        CustomerModel? match = null;
        if (reader.Read())
        {
            match = ReadCustomer(reader);
        }

        if (reader.Read())
        {
            throw new InvalidOperationException("Customer conflicts with an existing unique identity.");
        }

        return match;
    }

    private static bool HasUniqueIdentityConflict(
        SqliteConnection connection,
        SqliteTransaction transaction,
        CustomerModel? replacementTarget,
        CustomerModel customer)
    {
        using SqliteCommand command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = """
            SELECT CustomerId, Email, ConfirmationToken, ProfileReference
            FROM Customers
            WHERE (($customerId IS NOT NULL AND CustomerId = $customerId)
               OR ($email IS NOT NULL AND Email = $email COLLATE NOCASE)
               OR ($confirmationToken IS NOT NULL AND ConfirmationToken = $confirmationToken)
               OR ($profileReference IS NOT NULL AND ProfileReference = $profileReference))
              AND NOT (($existingCustomerId IS NOT NULL AND CustomerId = $existingCustomerId)
                   OR ($existingEmail IS NOT NULL AND Email = $existingEmail COLLATE NOCASE));
            """;
        AddIdentityParameters(command, customer);
        AddNullableParameter(command, "$existingCustomerId", Normalize(replacementTarget?.CustomerId));
        AddNullableParameter(command, "$existingEmail", Normalize(replacementTarget?.Email));

        using SqliteDataReader reader = command.ExecuteReader();
        return reader.Read();
    }

    private static void InsertCustomer(SqliteConnection connection, SqliteTransaction transaction, CustomerModel customer)
    {
        using SqliteCommand command = CreateWriteCommand(connection, transaction, customer);
        command.CommandText = """
            INSERT INTO Customers (
                CustomerId,
                Email,
                IsEmailConfirmed,
                ConfirmationToken,
                ConfirmationTokenExpiresAt,
                ConfirmationTokenUsedAt,
                ProfileReference,
                PasswordHash,
                Name,
                PhoneNumber,
                Location)
            VALUES (
                $customerId,
                $email,
                $isEmailConfirmed,
                $confirmationToken,
                $confirmationTokenExpiresAt,
                $confirmationTokenUsedAt,
                $profileReference,
                $passwordHash,
                $name,
                $phoneNumber,
                $location);
            """;
        command.ExecuteNonQuery();
    }

    private static void UpdateCustomer(
        SqliteConnection connection,
        SqliteTransaction transaction,
        CustomerModel customer,
        string? existingCustomerId,
        string? existingEmail)
    {
        using SqliteCommand command = CreateWriteCommand(connection, transaction, customer);
        command.CommandText = """
            UPDATE Customers
            SET CustomerId = $customerId,
                Email = $email,
                IsEmailConfirmed = $isEmailConfirmed,
                ConfirmationToken = $confirmationToken,
                ConfirmationTokenExpiresAt = $confirmationTokenExpiresAt,
                ConfirmationTokenUsedAt = $confirmationTokenUsedAt,
                ProfileReference = $profileReference,
                PasswordHash = $passwordHash,
                Name = $name,
                PhoneNumber = $phoneNumber,
                Location = $location
            WHERE ($existingCustomerId IS NOT NULL AND CustomerId = $existingCustomerId)
               OR ($existingEmail IS NOT NULL AND Email = $existingEmail COLLATE NOCASE);
            """;
        AddNullableParameter(command, "$existingCustomerId", Normalize(existingCustomerId));
        AddNullableParameter(command, "$existingEmail", Normalize(existingEmail));
        command.ExecuteNonQuery();
    }

    private static SqliteCommand CreateWriteCommand(SqliteConnection connection, SqliteTransaction transaction, CustomerModel customer)
    {
        SqliteCommand command = connection.CreateCommand();
        command.Transaction = transaction;
        AddNullableParameter(command, "$customerId", Normalize(customer.CustomerId));
        AddNullableParameter(command, "$email", Normalize(customer.Email));
        command.Parameters.AddWithValue("$isEmailConfirmed", customer.IsEmailConfirmed ? 1 : 0);
        AddNullableParameter(command, "$confirmationToken", Normalize(customer.ConfirmationToken));
        AddNullableParameter(command, "$confirmationTokenExpiresAt", FormatDateTimeOffset(customer.ConfirmationTokenExpiresAt));
        AddNullableParameter(command, "$confirmationTokenUsedAt", FormatDateTimeOffset(customer.ConfirmationTokenUsedAt));
        AddNullableParameter(command, "$profileReference", Normalize(customer.ProfileReference));
        AddNullableParameter(command, "$passwordHash", Normalize(customer.PasswordHash));
        AddNullableParameter(command, "$name", Normalize(customer.Name));
        AddNullableParameter(command, "$phoneNumber", Normalize(customer.PhoneNumber));
        AddNullableParameter(command, "$location", Normalize(customer.Location));
        return command;
    }

    private static void AddIdentityParameters(SqliteCommand command, CustomerModel customer)
    {
        AddNullableParameter(command, "$customerId", Normalize(customer.CustomerId));
        AddNullableParameter(command, "$email", Normalize(customer.Email));
        AddNullableParameter(command, "$confirmationToken", Normalize(customer.ConfirmationToken));
        AddNullableParameter(command, "$profileReference", Normalize(customer.ProfileReference));
    }

    private static void AddNullableParameter(SqliteCommand command, string name, string? value)
    {
        command.Parameters.AddWithValue(name, value is null ? DBNull.Value : value);
    }

    private static CustomerModel ReadCustomer(SqliteDataReader reader)
    {
        return new CustomerModel
        {
            CustomerId = ReadString(reader, 0),
            Email = ReadString(reader, 1),
            IsEmailConfirmed = reader.GetInt32(2) != 0,
            ConfirmationToken = ReadString(reader, 3),
            ConfirmationTokenExpiresAt = ReadDateTimeOffset(reader, 4),
            ConfirmationTokenUsedAt = ReadDateTimeOffset(reader, 5),
            ProfileReference = ReadString(reader, 6),
            PasswordHash = ReadString(reader, 7),
            Name = ReadString(reader, 8),
            PhoneNumber = ReadString(reader, 9),
            Location = ReadString(reader, 10),
        };
    }

    private static string? ReadString(SqliteDataReader reader, int ordinal)
    {
        return reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal);
    }

    private static DateTimeOffset? ReadDateTimeOffset(SqliteDataReader reader, int ordinal)
    {
        string? value = ReadString(reader, ordinal);
        return value is null ? null : DateTimeOffset.Parse(value, null, System.Globalization.DateTimeStyles.RoundtripKind);
    }

    private static CustomerModel Clone(CustomerModel customer)
    {
        return new CustomerModel
        {
            CustomerId = customer.CustomerId,
            Email = customer.Email,
            IsEmailConfirmed = customer.IsEmailConfirmed,
            ConfirmationToken = customer.ConfirmationToken,
            ConfirmationTokenExpiresAt = customer.ConfirmationTokenExpiresAt,
            ConfirmationTokenUsedAt = customer.ConfirmationTokenUsedAt,
            ProfileReference = customer.ProfileReference,
            PasswordHash = customer.PasswordHash,
            Name = customer.Name,
            PhoneNumber = customer.PhoneNumber,
            Location = customer.Location,
        };
    }

    private static bool HasEstablishedIdentityConflict(CustomerModel existing, CustomerModel customer)
    {
        return (!string.IsNullOrWhiteSpace(existing.CustomerId)
                && !string.IsNullOrWhiteSpace(customer.CustomerId)
                && !EqualsOrdinal(existing.CustomerId, customer.CustomerId))
            || (!string.IsNullOrWhiteSpace(existing.Email)
                && !string.IsNullOrWhiteSpace(customer.Email)
                && !EqualsOrdinalIgnoreCase(existing.Email, customer.Email));
    }

    private static string? Normalize(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value;
    }

    private static string? FormatDateTimeOffset(DateTimeOffset? value)
    {
        return value?.ToString("O");
    }

    private static bool EqualsOrdinal(string? left, string? right)
    {
        return string.Equals(left, right, StringComparison.Ordinal);
    }

    private static bool EqualsOrdinalIgnoreCase(string? left, string? right)
    {
        return string.Equals(left, right, StringComparison.OrdinalIgnoreCase);
    }
}
