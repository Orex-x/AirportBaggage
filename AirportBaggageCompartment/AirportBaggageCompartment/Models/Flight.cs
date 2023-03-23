using System;
using System.Collections.Generic;

namespace AirportBaggageCompartment.Models;

public class Flight
{
    public int Id { get; set; }
    public int FlightNumber { get; set; }
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public string Destination { get; set; }
    public string DepartureGate { get; set; }
    public string ArrivalGate { get; set; }

    public virtual ICollection<Baggage> Baggages { get; set; }
}