namespace Trcont.Cud.Story.User
{
    using System;
    using System.ComponentModel;

    public class CreateUserStoryContext
    {
        public Guid? FirmGuid { get; set; }

        public Guid? ContractGuid { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        // Свойства помеченные этим атрибутом не проверяются на NULL
        [DefaultValue("")]
        public string FirstName { get; set; }

        [DefaultValue("")]
        public string MiddleName { get; set; }

        [DefaultValue("")]
        public string LastName { get; set; }

        [DefaultValue("")]
        public string PhoneNumber { get; set; }

        [DefaultValue("")]
        public Guid? ClientGuid { get; set; }
    }
}
