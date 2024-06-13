using System.ComponentModel.DataAnnotations;

namespace EasyWordsAPI.Models.DataTransfer;

public class RegistrationDTO
{
    [Required] [EmailAddress] public string    Email { get; set; } = string.Empty;
    [Required] [MinLength(5)] public string Username { get; set; } = string.Empty;
    [Required] [MinLength(8)] public string Password { get; set; } = string.Empty;
}