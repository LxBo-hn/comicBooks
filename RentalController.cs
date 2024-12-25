using ComicRental.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Để sử dụng ToListAsync()
using System.Threading.Tasks;
using ASP;

public class RentalsController : Controller
{
    private readonly ApplicationDbContext _context;

    public RentalsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Rentals/Report
    public async Task<IActionResult> Report(DateTime? startDate, DateTime? endDate)
    {
        var query = from r in _context.Rentals
            join rd in _context.RentalDetails on r.RentalID equals rd.RentalID
            join c in _context.Customers on r.CustomerID equals c.CustomerID
            join cb in _context.ComicBooks on rd.ComicBookID equals cb.ComicBookID
            where (!startDate.HasValue || r.RentalDate >= startDate) &&
                  (!endDate.HasValue || r.RentalDate <= endDate)
            select new _Page_Views_Rentals_Report_cshtml()
            {
                BookName = cb.Title,
                RentalDate = r.RentalDate,
                ReturnDate = r.ReturnDate,
                CustomerName = c.FullName,
                Quantity = rd.Quantity
            };

        return View(await query.ToListAsync());
    }
}