
using InventoryManagementSystem.Models;

namespace InventoryManagementSystem.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalProducts { get; set; }

        public int TotalStockUnits { get; set; }

        public int LowStockProducts { get; set; }

        public List<StockTransaction> RecentTransactions { get; set; }
            = new List<StockTransaction>();
    }
}
