using System.ComponentModel.DataAnnotations;

namespace EasyWordsAPI.Models.DataTransfer;

public class WordDTO
{
    [StringLength(100)] public string      Source { get; set; }
    [StringLength(100)] public string Translation { get; set; }

    public Word ToWord()
    {
        return new Word()
        {
            Source = Source,
            Translation = Translation
        };
    }
}