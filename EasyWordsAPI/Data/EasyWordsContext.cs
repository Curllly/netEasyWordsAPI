using EasyWordsAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EasyWordsAPI.Data;

public class EasyWordsContext : 
    IdentityDbContext<IdentityUser<int>, IdentityRole<int>, int>
{
    public DbSet<Word>         Words { get; set; }
    public DbSet<Deck>         Decks { get; set; }
    public DbSet<DeckWord> DeckWords { get; set; }
    
    public EasyWordsContext(DbContextOptions options) : base(options) {}
    
    
    
}