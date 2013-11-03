using EnvDTE;

namespace KnockoutGenerator.Core.Extensions
{
    public static class ProjectItemExtension
    {
        public static string GetFullPath(this ProjectItem p)
        {
            return p.Properties.Item("FullPath").Value.ToString();
        }

    }
}
