/*
 * ============================================================================
 * MODEL: Product
 * ============================================================================
 * 
 * This model represents the Production.Product table from the AdventureWorks
 * database. It contains the main properties of a product.
 * 
 * TEACHING NOTE:
 * - Properties correspond to columns in the database table
 * - We use nullable types (?) for columns that can contain NULL
 * - Property names must match column names to allow Dapper to
 *   automatically map results
 * ============================================================================
 */

namespace AdventureWorksApp.Models
{
    /// <summary>
    /// Represents a product from the Production.Product table.
    /// This class is used for data mapping with Dapper.
    /// </summary>
    public class Product
    {
        // Primary key of the table
        public int ProductID { get; set; }

        // Product name (required)
        public string Name { get; set; } = string.Empty;

        // Product identifier number
        public string ProductNumber { get; set; } = string.Empty;

        // Indicates if the product can be manufactured internally
        public bool MakeFlag { get; set; }

        // Indicates if the product is finished
        public bool FinishedGoodsFlag { get; set; }

        // Product color (can be NULL)
        public string? Color { get; set; }

        // Minimum safety stock level
        public short SafetyStockLevel { get; set; }

        // Reorder point
        public short ReorderPoint { get; set; }

        // Standard cost of the product
        public decimal StandardCost { get; set; }

        // List price (can be NULL for products not sold)
        public decimal? ListPrice { get; set; }

        // Product size (can be NULL)
        public string? Size { get; set; }

        // Size unit of measure
        public string? SizeUnitMeasureCode { get; set; }

        // Weight unit of measure
        public string? WeightUnitMeasureCode { get; set; }

        // Product weight (can be NULL)
        public decimal? Weight { get; set; }

        // Days required for manufacturing
        public int DaysToManufacture { get; set; }

        // Product line (R = Road, M = Mountain, T = Touring, S = Standard)
        public string? ProductLine { get; set; }

        // Product class (H = High, M = Medium, L = Low)
        public string? Class { get; set; }

        // Product style (W = Women, M = Men, U = Universal)
        public string? Style { get; set; }

        // Reference to product subcategory
        public int? ProductSubcategoryID { get; set; }

        // Reference to product model
        public int? ProductModelID { get; set; }

        // Sale start date
        public DateTime SellStartDate { get; set; }

        // Sale end date (NULL if still on sale)
        public DateTime? SellEndDate { get; set; }

        // Discontinuation date (NULL if not discontinued)
        public DateTime? DiscontinuedDate { get; set; }

        // Row GUID for change tracking
        public Guid rowguid { get; set; }

        // Record modification date
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Override of ToString for user-friendly display
        /// </summary>
        public override string ToString()
        {
            return $"{ProductID} - {Name}";
        }
    }
}
