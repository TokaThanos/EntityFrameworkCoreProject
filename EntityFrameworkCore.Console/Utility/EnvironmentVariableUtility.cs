using System.IO;

namespace EntityFrameworkCore.Console.Utility;
internal class EnvironmentVariableUtility
{
    public static void LoadEnv()
    {
        var dir = new DirectoryInfo(Directory.GetCurrentDirectory());

        while (dir != null)
        {
            var envPath = Path.Combine(dir.FullName, ".env");
            if (File.Exists(envPath))
            {
                DotNetEnv.Env.Load(envPath);
                return;
            }
            dir = dir.Parent;
        }
        throw new FileNotFoundException($".env file not found in any parent directory.");
    }
}