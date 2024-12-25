using ComicRental.Data;
using Microsoft.AspNetCore.Mvc;

public class CustomersController : Controller
{
    private readonly ApplicationDbContext _context;

    public CustomersController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Customers/Register
    public IActionResult Register()
    {
        return View();
    }

    // POST: Customers/Register
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register([Bind("FullName,PhoneNumber")] Customer customer)
    {
        if (ModelState.IsValid)
        {
            customer.RegistrationDate = DateTime.Now;
            _context.Add(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(customer);
    }
}