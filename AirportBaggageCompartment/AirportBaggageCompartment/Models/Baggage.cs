namespace AirportBaggageCompartment.Models;

public class Baggage
{
    public int Id { get; set; }
    public float Weight { get; set; }
    public string Size { get; set; }

    public int FlightId { get; set; }
    public virtual Flight Flight { get; set; }

    public int CustomerId { get; set; }
    public virtual Customer Customer { get; set; }

    public int ServiceId { get; set; }
    public virtual Service Service { get; set; }

    public int WarehouseId { get; set; }
    public virtual Warehouse Warehouse { get; set; }

    public int EmployeeId { get; set; }
    public virtual Employee Employee { get; set; }
}