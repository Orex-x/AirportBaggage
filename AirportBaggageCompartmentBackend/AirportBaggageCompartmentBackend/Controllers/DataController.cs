using AirportBaggageCompartmentBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AirportBaggageCompartmentBackend.Controllers;

[Route("/api/easydata/models/__default/sources/")]
public class DataController : Controller
{
    private readonly DatabaseContext _db;
    
    public DataController(DatabaseContext db)
    {
        _db = db;
    }
    
    [Route("Customer/get")]
    public async Task<IEnumerable<Customer>> Customers() => await _db.Customers
        .ToListAsync();
    
        [Route("Flight/get")]
    public async Task<IEnumerable<Flight>> Flights() => await _db.Flights
        .ToListAsync();
    
    [Route("Ticket/get")]
    public async Task<IEnumerable<Ticket>> Tickets() => await _db.Tickets
        .Include(x => x.Customer)
        .Include(x => x.Flight)
        .ToListAsync();
}