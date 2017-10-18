using System;
using System.Collections.Generic;
using System.Text;

namespace Trcont.Cud.Domain.Dto
{
    public class FullUserDto : UserDto
    {
        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }

        public bool Active { get; set; }

        public bool IsClient { get; set; }

        public bool IsExtAgent { get; set; }

        public bool? EmailVerified { get; set; }

        public bool? PhoneVerified { get; set; }
    }
}
