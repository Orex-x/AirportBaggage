namespace AirportBaggageCompartment.Models;

public class Ticket
{
    public int Id { get; set; }
    
    public int FlightId { get; set; }
    public virtual Flight Flight { get; set; }
    
    public int CustomerId { get; set; }
    public virtual Customer Customer { get; set; }
    
    public string CodeWorld { get; set; }
    public string Password { get; set; }
}