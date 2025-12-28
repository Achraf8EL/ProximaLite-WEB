using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProximaLite.Domain.Entities;
using ProximaLite.Infrastructure.Data;

namespace ProximaLite.Web.Pages.Processes;

public class DetailsModel : PageModel
{
    private readonly AppDbContext _db;

    public DetailsModel(AppDbContext db)
    {
        _db = db;
    }

    public Process? Process { get; set; }

    [BindProperty]
    public Step NewStep { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Process = await _db.Processes
            .Include(p => p.Steps)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (Process == null)
            return NotFound();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var process = await _db.Processes.FirstOrDefaultAsync(p => p.Id == id);
        if (process == null)
            return NotFound();

        if (string.IsNullOrWhiteSpace(NewStep.Name))
        {
            ModelState.AddModelError("NewStep.Name", "Name is required.");
        }

        if (NewStep.Yield < 0 || NewStep.Yield > 1)
        {
            ModelState.AddModelError("NewStep.Yield", "Yield must be between 0 and 1.");
        }

        if (!ModelState.IsValid)
        {
            Process = await _db.Processes.Include(p => p.Steps).FirstAsync(p => p.Id == id);
            return Page();
        }

        NewStep.ProcessId = id;

        _db.Steps.Add(NewStep);
        await _db.SaveChangesAsync();

        return RedirectToPage(new { id });
    }
}
