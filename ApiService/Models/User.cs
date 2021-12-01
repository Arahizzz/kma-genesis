using System.ComponentModel.DataAnnotations;

namespace ApiService.Models;

public class User
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    [EmailAddress]
    public string? Email { get; set; }
}