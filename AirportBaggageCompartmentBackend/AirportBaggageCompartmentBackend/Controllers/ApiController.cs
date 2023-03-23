using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AirportBaggageCompartmentBackend.Controllers;

public class ApiController : Controller
{
    private readonly DatabaseContext _db;
    private readonly string _set = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "set " : "export ";
    
    public ApiController(DatabaseContext db)
    {
        _db = db;
    }
    
    public List<string> Backup()
    {
        var list = new List<string>();
        const string dirName = @"C:\tmp";
        if (Directory.Exists(dirName))
        {
            var files = Directory.GetFiles(dirName);
            list.AddRange(files.Select(Path.GetFileName)!.Where(x => x!.Contains("sql"))!);
        }
        else
            Directory.CreateDirectory(@"C:\tmp");
         
        return list;
    }
    
    public async Task ExportData()
    {
        const string dirName = @"C:\tmp\csv";
        if (!Directory.Exists(dirName)) Directory.CreateDirectory(@"C:\tmp\csv");
        await _db.Database.ExecuteSqlRawAsync("call BackupCSV();");
    }

    public async Task Dump()
    {
        var date = DateTime.Now;
        var fileName = $"{date.Year}{date.Month}{date.Day}_{date.Hour}{date.Minute}";
        await PSqlDump(
            @"C:\Program Files\PostgreSQL\14\bin\", 
            "333", 
            "postgres", 
            "Airport", 
            $@"C:/tmp/{fileName}");
    } 
    
  
    
    public async Task Restore(string name)
    {
        await PSqlRestore(
            @"C:\Program Files\PostgreSQL\14\bin\", 
            "333", 
            "postgres", 
            "Airport", 
            $"/tmp/{name}");
    }
    
    public void OpenFolder()
    {
        Process.Start("explorer", @"C:\tmp\csv");
    }

    private async Task PSqlDump(string pathToExecutableFile, string password, string login, string database, string outputFile)
    {
        string[] commands =
        {
            $"cd {pathToExecutableFile}", 
            $"{_set} PGPASSWORD={password}", 
            $"pg_dump.exe -U {login} {database} > {outputFile}.sql"
        };
        await RunCommands(commands);
    }
    
    private async Task PSqlRestore(string pathToExecutableFile, string password, string login, string database, string inputFile)
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