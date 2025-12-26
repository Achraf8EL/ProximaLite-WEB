using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProximaLite.Domain.Entities;
using ProximaLite.Infrastructure.Data;

namespace ProximaLite.Web.Pages.Processes;

public class CreateModel : PageModel
{
    private readonly AppDbContext _db;

    public CreateModel(AppDbContext db)
    {
        _db = db;
    }

    [BindProperty]
    public Process Process { get; set; } = new();

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrWhiteSpace(Process.Name))
        {
            ModelState.AddModelError("Process.Name", "Name is required.");
            return Page();
        }

        Process.CreatedAt = DateTime.UtcNow;

        _db.Processes.Add(Process);
        await _db.SaveChangesAsync();

        return RedirectToPage("Index");
    }
}
