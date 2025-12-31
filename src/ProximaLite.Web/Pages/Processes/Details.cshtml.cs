using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProximaLite.Domain.Entities;
using ProximaLite.Domain.Services;
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
    public ProcessKpiResult? Kpis { get; set; }

    public List<ProcessEvaluation> EvaluationHistory { get; set; } = new();

    [BindProperty]
    public Step NewStep { get; set; } = new();

    [BindProperty]
    public string? Notes { get; set; } // optionnel : commentaire evaluation

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Process = await _db.Processes
            .Include(p => p.Steps)
            .Include(p => p.Evaluations)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (Process == null)
            return NotFound();

        Kpis = ProcessKpiCalculator.Calculate(Process);

        EvaluationHistory = Process.Evaluations
            .OrderByDescending(e => e.CreatedAt)
            .ToList();

        return Page();
    }

    public async Task<IActionResult> OnPostAddStepAsync(int id)
    {
        var process = await _db.Processes
            .Include(p => p.Steps)
            .Include(p => p.Evaluations)
            .FirstOrDefaultAsync(p => p.Id == id);

            

        if (process == null)
            return NotFound();

        if (string.IsNullOrWhiteSpace(NewStep.Name))
            ModelState.AddModelError("NewStep.Name", "Name is required.");

        if (NewStep.Yield < 0 || NewStep.Yield > 1)
            ModelState.AddModelError("NewStep.Yield", "Yield must be between 0 and 1.");

        if (!ModelState.IsValid)
        {
            Process = process;
            Kpis = ProcessKpiCalculator.Calculate(process);
            EvaluationHistory = process.Evaluations.OrderByDescending(e => e.CreatedAt).ToList();
            return Page();
        }

        NewStep.ProcessId = id;
        _db.Steps.Add(NewStep);
        await _db.SaveChangesAsync();

        return RedirectToPage(new { id });
    }

    public async Task<IActionResult> OnPostEvaluateAsync(int id)
    {
        var process = await _db.Processes
            .Include(p => p.Steps)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (process == null)
            return NotFound();

        var kpis = ProcessKpiCalculator.Calculate(process);

        var evaluation = new ProcessEvaluation
        {
            ProcessId = id,
            CreatedAt = DateTime.UtcNow,
            TotalDurationMin = kpis.TotalDurationMin,
            TotalCostEuro = kpis.TotalCostEuro,
            GlobalYield = kpis.GlobalYeild,
            Notes = string.IsNullOrWhiteSpace(Notes) ? null : Notes.Trim()
        };

        _db.ProcessEvaluations.Add(evaluation);
        await _db.SaveChangesAsync();

        return RedirectToPage(new { id });
    }
}
