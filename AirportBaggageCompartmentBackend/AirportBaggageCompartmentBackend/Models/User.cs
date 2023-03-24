using EasyData.EntityFrameworkCore;

namespace AirportBaggageCompartmentBackend.Models;

[MetaEntity(DisplayName = "Пользователь", DisplayNamePlural = "Пользователи", Description = "Выборка пользователей")]
public class User
{
    public int Id { get; set; } 
    public string Login { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public Role Role { get; set; }
}