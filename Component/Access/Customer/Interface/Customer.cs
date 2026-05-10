namespace FixMe.Access.Customer.Interface
{
    public class Customer
    {
        public string? CustomerId { get; set; }

        public string? Email { get; set; }

        public bool IsEmailConfirmed { get; set; }

        public bool MayRegisterEquipment => IsEmailConfirmed;

        public string? ConfirmationToken { get; set; }

        public DateTimeOffset? ConfirmationTokenExpiresAt { get; set; }

        public DateTimeOffset? ConfirmationTokenUsedAt { get; set; }

        public string? ProfileReference { get; set; }

        public string? PasswordHash { get; set; }

        public string? Name { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Location { get; set; }
    }
}
