using System.IO;

namespace EntityFrameworkCore.Data.Utility;

public static class EnvironmentVariableUtility
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
    
    public static string GetEnvironmentVariable(string key)
    {
        var value = Environment.GetEnvironmentVariable(key);
        if (string.IsNullOrEmpty(value))
        {
            throw new InvalidOperationException($"Environment variable {key} is not set.");
        }
        return value;
    }
}