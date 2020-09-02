namespace bgTeam.ProjectTemplate
{
    using System.ComponentModel;

    internal enum ProjectTypeEnum
    {
        [Description("9A19103F-16F7-4668-BE54-9A1E7A4F7556")]
        Compile,

        [Description("2150E333-8FDC-42A3-9474-1A3956D46DE8")]
        Folder,

        [Description("FAE04EC0-301F-11D3-BF4B-00C04F79EFBC")]
        Tests,
    }
}
