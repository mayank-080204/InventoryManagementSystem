
using InventoryManagementSystem.Data;
using InventoryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Controllers
{
    public class StockTransactionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StockTransactionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Display all stock transactions
        public async Task<IActionResult> Index()
        {
            var transactions = await _context.StockTransactions
                .Include(t => t.Product)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();

            return View(transactions);
        }

        // Display the form to create a transaction
        public async Task<IActionResult> Create()
        {
            ViewBag.Products = await _context.Products
                .OrderBy(p => p.ProductName)
                .ToListAsync();

            return View();
        }

        // Save a new stock transaction
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StockTransaction transaction)
        {
            if (transaction.Quantity <= 0)
            {
                ModelState.AddModelError(
                    nameof(transaction.Quantity),
                    "Quantity must be greater than zero.");
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == transaction.ProductId);

            if (product == null)
            {
                ModelState.AddModelError(
                    nameof(transaction.ProductId),
                    "Please select a valid product.");
            }

            if (ModelState.IsValid && product != null)
            {
                if (transaction.TransactionType == "IN")
                {
                    product.Quantity += transaction.Quantity;
                }
                else if (transaction.TransactionType == "OUT")
                {
                    if (product.Quantity < transaction.Quantity)
                    {
                        ModelState.AddModelError(
                            nameof(transaction.Quantity),
                            "Insufficient stock available.");
                    }
                    else
                    {
                        product.Quantity -= transaction.Quantity;
                    }
                }
                else
                {
                    ModelState.AddModelError(
                        nameof(transaction.TransactionType),
                        "Transaction type must be IN or OUT.");
                }

                if (ModelState.IsValid)
                {
                    transaction.TransactionDate = DateTime.Now;

                    _context.StockTransactions.Add(transaction);

                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
            }

            ViewBag.Products = await _context.Products
                .OrderBy(p => p.ProductName)
                .ToListAsync();

            return View(transaction);
        }
    }
}
