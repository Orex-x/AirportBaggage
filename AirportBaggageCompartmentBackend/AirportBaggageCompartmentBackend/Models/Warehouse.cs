namespace AirportBaggageCompartmentBackend.Models;

public class Warehouse
{
    public int Id { get; set; }
    public string Location { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }

    public virtual ICollection<Employee> Employees { get; set; }
    public virtual ICollection<Baggage> Baggages { get; set; }
}