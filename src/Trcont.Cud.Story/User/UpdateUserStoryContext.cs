namespace Trcont.Cud.Story.User
{
    using System;
    using System.ComponentModel;

    public class UpdateUserStoryContext
    {
        public Guid UserGuid { get; set; }

        public Guid FirmGuid { get; set; }

        public Guid? ClientGuid { get; set; }

        public Guid? ContractGuid { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public bool? DisableTwoFactorAuth { get; set; }

        public string TwoFactorAuthTokenKey { get; set; }
    }
}
