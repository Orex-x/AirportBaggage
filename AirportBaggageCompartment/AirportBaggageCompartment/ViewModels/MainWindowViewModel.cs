using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AirportBaggageCompartment.Models;
using AirportBaggageCompartment.Services;
using Avalonia.Threading;
using Microsoft.AspNetCore.Identity;
using ReactiveUI;

namespace AirportBaggageCompartment.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public const string DirDumps = @"C:\db_dumps";
    public const string DirCVS = @"C:\db_dumps\csv";
    
    private ObservableCollection<string> _dumpFiles = new ObservableCollection<string>();
    private ObservableCollection<string> _scvFiles = new ObservableCollection<string>();
    
    private ObservableCollection<Customer> _clients = new ObservableCollection<Customer>();
    private ObservableCollection<Flight> _flights = new ObservableCollection<Flight>();
    
    public ObservableCollection<string> DumpFiles { get { return _dumpFiles; } set { _dumpFiles = value; } }
    public ObservableCollection<string> SCVFiles { get { return _scvFiles; } set { _scvFiles = value; } }
    
    public ObservableCollection<Customer> Clients { get { return _clients; } set { _clients = value; } }
    public ObservableCollection<Flight> Flights { get { return _flights; } set { _flights = value; } }
    
    public ICommand ClickDump { get; private set; }
    public ICommand ClickRestore { get; private set; }
    public ICommand ClickExport { get; private set; }
    public ICommand ClickOpenFolder { get; private set; }
    public ICommand ClickCreateCodeWorld { get; private set; }
    public ICommand ClickCheckCodeWorld { get; private set; }
    
    
    
    private string _selectedDumpFile;
    public string SelectedDumpFile { get { return _selectedDumpFile; } set { _selectedDumpFile = value; } }

    
    
    private Customer _selectedClient;
    public Customer SelectedClient { get { return _selectedClient; } set { _selectedClient = value; } }
    
    private Flight _selectedFlights;
    public Flight SelectedFlights { get { return _selectedFlights; } set { _selectedFlights = value; } }
    
   
    
    private string _codeWorld;
    public string CodeWorld { get { return _codeWorld; } set { _codeWorld = value; } }
    
    private string _password;
    public string Password { get { return _password; } set { _password = value; } }
    

    private string _codeResult;

    public string CodeResult
    {
        get => _codeResult;
        set => this.RaiseAndSetIfChanged(ref _codeResult, value);
    }
    
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
        Clients = new(ApiClient.Get<Customer>());
        Flights = new(ApiClient.Get<Flight>());
        
        if (!Directory.Exists(DirDumps)) Directory.CreateDirectory(DirDumps);
        
        if (!Directory.Exists(DirCVS)) Directory.CreateDirectory(DirCVS);
        
        
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
        
        ClickCheckCodeWorld = ReactiveCommand.CreateFromTask(async () =>
        {
            var list = ApiClient.Get<Ticket>();
            var ticket = list.FirstOrDefault(x => x.CodeWorld == CodeWorld);
            if (ticket != null)
            {
                var hasher = new PasswordHasher<Ticket>();
                var s = hasher.VerifyHashedPassword(ticket, ticket.Password, Password);
                if (s == PasswordVerificationResult.Success)
                {
                    CodeResult = $"Номер рейса: {ticket.Flight.FlightNumber} | Вовота: {ticket.Flight.ArrivalGate} | Пассажир: {ticket.Customer.FirstName} {ticket.Customer.LastName}";
                }
            }
        });
        
        
        ClickCreateCodeWorld = ReactiveCommand.CreateFromTask(async () =>
        {
            var ticket = new Ticket()
            {
                FlightId = SelectedFlights.Id,
                CustomerId = SelectedClient.Id,
                CodeWorld = CodeWorld
            };
                
            var hasher = new PasswordHasher<Ticket>();
            var hash = hasher.HashPassword(ticket, Password);

            ticket.Password = hash;
            ApiClient.Create(ticket);
        });

        UpdateFolder();
    }
}