using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AirportBaggageCompartment.Services;
using Avalonia.Threading;
using ReactiveUI;

namespace AirportBaggageCompartment.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public const string DirDumps = @"C:\tmp";
    public const string DirCVS = @"C:\tmp\csv";
    
    private ObservableCollection<string> _dumpFiles = new ObservableCollection<string>();
    private ObservableCollection<string> _scvFiles = new ObservableCollection<string>();
    
    public ObservableCollection<string> DumpFiles { get { return _dumpFiles; } set { _dumpFiles = value; } }
    public ObservableCollection<string> SCVFiles { get { return _scvFiles; } set { _scvFiles = value; } }
    
    public ICommand ClickDump { get; private set; }
    public ICommand ClickRestore { get; private set; }
    public ICommand ClickExport { get; private set; }
    public ICommand ClickOpenFolder { get; private set; }
    
    
    private string _selectedDumpFile;
    public string SelectedDumpFile { get { return _selectedDumpFile; } set { _selectedDumpFile = value; } }

    public async void UpdateFolder()
    {
        await Task.Run(async () => {
            while (true)
            {
                var filesDumps = Directory.GetFiles(DirDumps);
                var filesCVS = Directory.GetFiles(DirCVS);
                DumpFiles.Clear();
                SCVFiles.Clear();
                var listDumps = filesDumps.Select(Path.GetFileName)!.Where(x => x.Contains(".sql"));
                var listCSV = filesCVS.Select(Path.GetFileName)!.Where(x => x.Contains(".csv"));
                foreach (var s in listDumps) DumpFiles.Add(s!);
                foreach (var s in listCSV) SCVFiles.Add(s!);
                
                await Task.Delay(1000);
            }
        });
    }
    
    public MainWindowViewModel()
    {
        ClickDump = ReactiveCommand.CreateFromTask(async () =>
        {
            await DatabaseService.Dump();
        });
        
        ClickRestore = ReactiveCommand.CreateFromTask(async () =>
        {
            await DatabaseService.Restore(SelectedDumpFile);
        });
        
        ClickExport = ReactiveCommand.CreateFromTask(async () =>
        {
            await ApiClient.Export();
        }); 
        
        ClickOpenFolder = ReactiveCommand.CreateFromTask(async () =>
        {
            Process.Start("explorer", DirCVS);
        });

        UpdateFolder();
    }
}