using EasyData.EntityFrameworkCore;

namespace AirportBaggageCompartmentBackend.Models;

[MetaEntity(DisplayName = "Клиент", DisplayNamePlural = "Клиенты", Description = "Выборка клиентов")]
public class Customer
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }

    public virtual ICollection<Baggage> Baggage { get; set; }
}