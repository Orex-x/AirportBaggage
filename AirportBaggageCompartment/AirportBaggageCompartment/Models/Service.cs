using System.Collections.Generic;

namespace AirportBaggageCompartment.Models;

public class Service
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public virtual ICollection<Baggage> Baggages { get; set; }
}