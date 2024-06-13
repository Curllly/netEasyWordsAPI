using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace EasyWordsAPI.Models;

public class Deck
{
    [Key] public string              Code { get; set; } = string.Empty;
    [StringLength(50)] public string Name { get; set; } = string.Empty;
    public int                     UserId { get; set; }
    public IdentityUser<int>         User { get; set; } = null!;
    public bool                  IsPublic { get; set; } = false;
}