using EasyData.EntityFrameworkCore;

namespace AirportBaggageCompartmentBackend.Models;

[MetaEntity(DisplayName = "Сервис", DisplayNamePlural = "Сервисы", Description = "Выборка сервесов")]
public class Service
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public virtual ICollection<Baggage> Baggages { get; set; }
}