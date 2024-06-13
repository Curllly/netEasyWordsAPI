using System.ComponentModel.DataAnnotations;

namespace EasyWordsAPI.Models;

public class Word
{
    public int                                 Id { get; set; }
    [StringLength(100)] public string      Source { get; set; } = string.Empty;
    [StringLength(100)] public string Translation { get; set; } = string.Empty;
    public DateTime                   CreatedDate { get; set; } = DateTime.Now;
    public DateTime                NextRepeatDate { get; set; } = DateTime.Now;
}