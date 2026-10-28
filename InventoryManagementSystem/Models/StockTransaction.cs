using InventoryManagementSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Models
{
    public class StockTransaction
    {
        [Key]
        public int TransactionId { get; set; }

        // Product involved in the transaction
        public int ProductId { get; set; }

        public Product? Product { get; set; }

        // IN = stock received, OUT = stock sold/removed
        [Required]
        [StringLength(10)]
        public string TransactionType { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        public DateTime TransactionDate { get; set; } = DateTime.Now;

        [StringLength(250)]
        public string? Remarks { get; set; }
    }
}