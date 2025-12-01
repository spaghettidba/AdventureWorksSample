/*
 * ============================================================================
 * MODEL: SalesOrderHeader
 * ============================================================================
 * 
 * This model represents the Sales.SalesOrderHeader table from the AdventureWorks
 * database. It contains the main information of a sales order.
 * 
 * TEACHING NOTE:
 * - This table has a 1:N relationship with SalesOrderDetail
 * - Each order can have multiple detail lines (ordered products)
 * - The primary key is SalesOrderID
 * - Contains references to Customer, SalesPerson, Territory, etc.
 * ============================================================================
 */

namespace AdventureWorksApp.Models
{
    /// <summary>
    /// Represents a sales order header from the Sales.SalesOrderHeader table.
    /// Contains general order data such as customer, dates, totals, etc.
    /// </summary>
    public class SalesOrderHeader
    {
        // Order primary key
        public int SalesOrderID { get; set; }

        // Order revision number
        public byte RevisionNumber { get; set; }

        // Order date
        public DateTime OrderDate { get; set; }

        // Requested delivery date
        public DateTime DueDate { get; set; }

        // Actual ship date (NULL if not yet shipped)
        public DateTime? ShipDate { get; set; }

        // Order status: 1=In process, 2=Approved, 3=Backordered, 
        // 4=Rejected, 5=Shipped, 6=Cancelled
        public byte Status { get; set; }

        // Online order flag (true = web order)
        public bool OnlineOrderFlag { get; set; }

        // Order number for display (format: SO + number)
        public string SalesOrderNumber { get; set; } = string.Empty;

        // Customer purchase order number
        public string? PurchaseOrderNumber { get; set; }

        // Customer account number
        public string? AccountNumber { get; set; }

        // Customer ID
        public int CustomerID { get; set; }

        // Salesperson ID (can be NULL for online orders)
        public int? SalesPersonID { get; set; }

        // Sales territory ID
        public int? TerritoryID { get; set; }

        // Billing address ID
        public int BillToAddressID { get; set; }

        // Shipping address ID
        public int ShipToAddressID { get; set; }

        // Shipping method ID
        public int ShipMethodID { get; set; }

        // Credit card ID (NULL if other payment method)
        public int? CreditCardID { get; set; }

        // Credit card approval code
        public string? CreditCardApprovalCode { get; set; }

        // Currency rate ID
        public int? CurrencyRateID { get; set; }

        // Order subtotal (sum of details)
        public decimal SubTotal { get; set; }

        // Tax amount
        public decimal TaxAmt { get; set; }

        // Shipping cost
        public decimal Freight { get; set; }

        // Order total (SubTotal + TaxAmt + Freight)
        public decimal TotalDue { get; set; }

        // Order comment
        public string? Comment { get; set; }

        // Row GUID for change tracking
        public Guid rowguid { get; set; }

        // Record modification date
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Calculated property to display the status in a readable format
        /// </summary>
        public string StatusDescription => Status switch
        {
            1 => "In Process",
            2 => "Approved",
            3 => "Backordered",
            4 => "Rejected",
            5 => "Shipped",
            6 => "Cancelled",
            _ => "Unknown"
        };

        /// <summary>
        /// Override of ToString for user-friendly display
        /// </summary>
        public override string ToString()
        {
            return $"{SalesOrderNumber} - {OrderDate:dd/MM/yyyy} - {TotalDue:C}";
        }
    }
}
