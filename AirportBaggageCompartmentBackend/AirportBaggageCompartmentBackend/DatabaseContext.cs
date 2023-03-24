using AirportBaggageCompartmentBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace AirportBaggageCompartmentBackend;

public class DatabaseContext : DbContext
{
    
    public DbSet<Baggage> Baggages { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Flight> Flights { get; set; }
    public DbSet<Position> Positions { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<Warehouse> Warehouses { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<User> Users { get; set; }
    
    public DatabaseContext()
    {
        try
        {
            Database.EnsureCreated();
        }
        catch (Exception)
        {
            // ignored
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(
            "host=localhost;port=5432;database=Airport;username=postgres;password=123");
    }
}