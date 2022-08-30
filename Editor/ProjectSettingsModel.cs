namespace LittleBit.Modules.CICD.Editor
{
    public class ProjectSettingsModel
    {
        public PlayerSettings PlayerSettings { get; set; }
    }
    
    public class PlayerSettings
    {
        public ApplicationIdentifier applicationIdentifier { get; set; }
    }

    public class ApplicationIdentifier
    {
        public string Android { get; set; }
        public string Standalone { get; set; }
        public string iPhone { get; set; }
    }
}