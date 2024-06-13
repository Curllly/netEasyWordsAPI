using Microsoft.EntityFrameworkCore;

namespace EasyWordsAPI.Models;

[PrimaryKey(nameof(DeckCode), nameof(WordId))]
public class DeckWord
{
    public string DeckCode { get; set; }
    public Deck       Deck { get; set; }
    public int      WordId { get; set; }
    public Word       Word { get; set; }
}