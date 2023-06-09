﻿using EasyData.EntityFrameworkCore;

namespace AirportBaggageCompartmentBackend.Models;

[MetaEntity(DisplayName = "Сотрудник", DisplayNamePlural = "Сотрудники", Description = "Выборка сотрудников")]
public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }

    public int WarehouseId { get; set; }
    public virtual Warehouse Warehouse { get; set; }

    public virtual ICollection<Baggage> Baggages { get; set; }
    
    public int PositionId { get; set; }
    public virtual Position Position { get; set; }
}