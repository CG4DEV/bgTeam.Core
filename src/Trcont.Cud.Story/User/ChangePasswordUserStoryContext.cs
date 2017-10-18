namespace Trcont.Cud.Story.User
{
    public class ChangePasswordUserStoryContext
    {
        public System.Guid UserGuid { get; set; }

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
