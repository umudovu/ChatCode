namespace SignalR_Intro.Helpers
{
    public class Helper
    {
        public enum UserRoles
        {
            Admin,
            Member
        }

        public static void DeleteImage(string path)
        {
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }
    }
}
