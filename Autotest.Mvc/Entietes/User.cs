using Autotest.Mvc.Services;

namespace DataBase.Entietes;

public class User
{
    public string Id { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? PhotoPath { get; set; }

    public int? CurrentTicketIndex { get; set; }
    public string Language = "Uzb";

    public Ticket? CurrentTicket { get; set; }
    public List<Ticket> Tickets { get; set; }

}