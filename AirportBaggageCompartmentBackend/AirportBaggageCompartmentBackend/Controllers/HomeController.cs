using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AirportBaggageCompartmentBackend.Models;

namespace AirportBaggageCompartmentBackend.Controllers;

public class HomeController : Controller
{

    private readonly DatabaseContext _db;
    
    public HomeController(DatabaseContext db)
    {
        _db = db;
    }

    public IActionResult EasyData() => View();
    
    public IActionResult Charts()
    {
        var model = new ChartModel();

        var barData = new Dictionary<string, int>();
        var lineData = new Dictionary<string, int>();
        
        
        var listFlightDestination = _db.Flights.Select(x => x.Destination).Distinct().ToList();
        listFlightDestination.ForEach(destination =>
        {
            var count = _db.Flights.Where(x => x.Destination == destination).Count();
            barData.Add(destination, count);
        });

        var date = new DateTime(2023, 1, 1);
        while (date.Month < 12)
        {
            var label = date.ToString("MMMM");
            var data = _db.Flights.Count(x => x.ArrivalTime.Month == date.Month);
            lineData.Add(label, data);
            date = date.AddMonths(1);
        }
        
        
        model.BarData = barData;
        model.LineData = lineData;
        return View(model);
    }
    
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}