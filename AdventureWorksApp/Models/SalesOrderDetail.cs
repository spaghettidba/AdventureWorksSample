/*
 * ============================================================================
 * MODEL: SalesOrderDetail
 * ============================================================================
 * 
 * This model represents the Sales.SalesOrderDetail table from the AdventureWorks
 * database. It contains the detail lines of a sales order.
 * 
 * TEACHING NOTE:
 * - This table has a composite primary key (SalesOrderID, SalesOrderDetailID)
 * - It has a N:1 relationship with SalesOrderHeader via SalesOrderID
 * - It has a N:1 relationship with Product via ProductID
 * - Each row represents an ordered product with quantity and price
 * ============================================================================
 */

namespace AdventureWorksApp.Models
{
    /// <summary>
    /// Represents an order detail line from the Sales.SalesOrderDetail table.
    /// Contains information about the individual ordered product: quantity,
    /// price, discount, etc.
    /// </summary>
    public class SalesOrderDetail
    {
        // Part of the primary key - reference to the order
        public int SalesOrderID { get; set; }

        // Part of the primary key - sequential line identifier
        public int SalesOrderDetailID { get; set; }

        // Carrier tracking number for the shipment
        public string? CarrierTrackingNumber { get; set; }

        // Ordered quantity
        public short OrderQty { get; set; }

        // Ordered product ID - foreign key to Production.Product
        public int ProductID { get; set; }

        // Applied special offer ID
        public int SpecialOfferID { get; set; }

        // Unit price of the product
        public decimal UnitPrice { get; set; }

        // Applied discount percentage (0.00 - 1.00)
        public decimal UnitPriceDiscount { get; set; }

        // Line total: (UnitPrice * OrderQty) - (UnitPrice * OrderQty * UnitPriceDiscount)
        public decimal LineTotal { get; set; }

        // Row GUID for change tracking
        public Guid rowguid { get; set; }

        // Record modification date
        public DateTime ModifiedDate { get; set; }

        // ============================================================================
        // NAVIGATION PROPERTIES (not in database, populated by JOIN queries)
        // ============================================================================

        /// <summary>
        /// Product name - populated via JOIN with Production.Product
        /// This property is used to display product information without
        /// needing additional queries
        /// </summary>
        public string? ProductName { get; set; }

        /// <summary>
        /// Order number - populated via JOIN with Sales.SalesOrderHeader
        /// </summary>
        public string? SalesOrderNumber { get; set; }

        /// <summary>
        /// Calculates the line total programmatically
        /// Useful for verifying the database value
        /// </summary>
        public decimal CalculatedLineTotal => 
            (UnitPrice * OrderQty) * (1 - UnitPriceDiscount);

        /// <summary>
        /// Override of ToString for user-friendly display
        /// </summary>
        public override string ToString()
        {
            return $"Order {SalesOrderID}, Line {SalesOrderDetailID}: {ProductName ?? $"Product {ProductID}"} x {OrderQty} = {LineTotal:C}";
        }
    }
}
