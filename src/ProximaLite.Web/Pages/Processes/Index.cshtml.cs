using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProximaLite.Domain.Entities;
using ProximaLite.Infrastructure.Data;

namespace ProximaLite.Web.Pages.Processes;

public class IndexModel : PageModel
{
    private readonly AppDbContext _db;

    public IndexModel(AppDbContext db)
    {
        _db = db;
    }

    public List<Process> Processes { get; set; } = new();

    public async Task OnGetAsync()
    {
        Processes = await _db.Processes
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }
}
