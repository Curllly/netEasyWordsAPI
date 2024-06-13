using EasyWordsAPI.Data;
using EasyWordsAPI.Models;
using EasyWordsAPI.Models.DataTransfer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EasyWordsAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WordsController : ControllerBase
{
    private EasyWordsContext _context;

    public WordsController(EasyWordsContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<List<Word>> GetWords()
    {
        return await _context.Words.ToListAsync();
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<Word>> GetWord(int id)
    {
        var word = await _context.Words.FindAsync(id);
        
        if (word is null)
            return NotFound();

        return word;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddWord(WordDTO dto)
    {
        var wordToAdd = dto.ToWord();
        _context.Words.Add(wordToAdd);
        await _context.SaveChangesAsync();
        return Ok(wordToAdd);
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateWord(WordDTO dto, int id)
    {
        var wordToUpdate = await _context.Words.FindAsync(id);
        if (wordToUpdate is null)
        {
            return NotFound();
        }

        wordToUpdate.Source = dto.Source;
        wordToUpdate.Translation = dto.Translation;

        await _context.SaveChangesAsync();
        return Ok(wordToUpdate);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteWord(int id)
    {
        var wordToDelete = await _context.Words.FindAsync(id);
        if (wordToDelete is null)
        {
            return NotFound();
        }

        _context.Words.Remove(wordToDelete);
        await _context.SaveChangesAsync();

        return Ok(wordToDelete);
    }

    [HttpPut("successful_repeat/{id}")]
    public async Task<IActionResult> SuccessfulRepeat(int id)
    {
        var wordToUpdate = await _context.Words.FindAsync(id);
        if (wordToUpdate is null)
        {
            return NotFound();
        }

        int daysSinceCreate = (int)(wordToUpdate.NextRepeatDate - wordToUpdate.CreatedDate)
            .TotalDays;
        if (daysSinceCreate == 0)
        {
            wordToUpdate.NextRepeatDate = wordToUpdate.NextRepeatDate
                .AddDays(1);
        }
        else
        {
            wordToUpdate.NextRepeatDate = wordToUpdate.NextRepeatDate
                .AddDays(2 * daysSinceCreate + 1 - daysSinceCreate);
        }

        await _context.SaveChangesAsync();
        return Ok(wordToUpdate);
    }

    [HttpPut("failed_repeat/{id}")]
    public async Task<IActionResult> FailedRepeat(int id)
    {
        var wordToUpdate = await _context.Words.FindAsync(id);
        if (wordToUpdate is null)
        {
            return NotFound();
        }
        wordToUpdate.CreatedDate = DateTime.Now;
        await _context.SaveChangesAsync();
        return Ok(wordToUpdate);
    }
}