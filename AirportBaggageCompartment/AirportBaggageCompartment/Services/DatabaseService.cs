using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace AirportBaggageCompartment.Services;

public class DatabaseService
{ 
    private static readonly string _set = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "set " : "export ";
    
    public static async Task Dump()
    {
        var date = DateTime.Now;
        var fileName = $"{date.Year}{date.Month}{date.Day}_{date.Hour}{date.Minute}";
        await PSqlDump(
            @"C:\Program Files\PostgreSQL\14\bin\", 
            "123", 
            "postgres", 
            "Airport", 
            $@"C:/db_dumps/{fileName}");
    } 
    
  
    
    public static async Task Restore(string name)
    {
        await PSqlRestore(
            @"C:\Program Files\PostgreSQL\14\bin\", 
            "123", 
            "postgres", 
            "Airport", 
            $"/db_dumps/{name}");
    }
    
    public static void OpenFolder()
    {
        Process.Start("explorer", @"C:\db_dumps\csv");
    }

    private static async Task PSqlDump(string pathToExecutableFile, string password, string login, string database, string outputFile)
    {
        string[] commands =
        {
            $"cd {pathToExecutableFile}", 
            $"{_set} PGPASSWORD={password}", 
            $"pg_dump.exe -U {login} {database} > {outputFile}.sql"
        };
        await RunCommands(commands);
    }
    
    private static async Task PSqlRestore(string pathToExecutableFile, string password, string login, string database, string inputFile)
    {
        string[] commands =
        {
            $"cd {pathToExecutableFile}", 
            $"{_set} PGPASSWORD={password}", 
            $"psql -U {login} -d {database} -c \"select pg_terminate_backend(pid) from pg_stat_activity where datname = '{database}'",
            $"dropdb -U {login} {database}",
            $"createdb -U {login} {database}",
            $"psql -U {login} -d {database} -f {inputFile}",
        };
        await RunCommands(commands);
    }

    private static async Task RunCommands(string[] commands)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                RedirectStandardInput = true,
                UseShellExecute = false

            }
        };
        process.Start();
        await using var pWriter = process.StandardInput;
        if (pWriter.BaseStream.CanWrite)
        {
            foreach (var line in commands)
                await pWriter.WriteLineAsync(line);
        }
    }
}