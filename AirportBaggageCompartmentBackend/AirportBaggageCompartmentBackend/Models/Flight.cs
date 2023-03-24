using EasyData.EntityFrameworkCore;

namespace AirportBaggageCompartmentBackend.Models;

[MetaEntity(DisplayName = "Полет", DisplayNamePlural = "Полеты", Description = "Выборка полетов")]
public class Flight
{
    public int Id { get; set; }
    public string FlightNumber { get; set; }
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public string Destination { get; set; }
    public string DepartureGate { get; set; }
    public string ArrivalGate { get; set; }

    public virtual ICollection<Baggage> Baggages { get; set; }
}