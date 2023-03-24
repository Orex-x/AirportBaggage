using EasyData.EntityFrameworkCore;

namespace AirportBaggageCompartmentBackend.Models;

[MetaEntity(DisplayName = "Должность", DisplayNamePlural = "Должности", Description = "Выборка должностей")]

public class Position
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Salary { get; set; }
}