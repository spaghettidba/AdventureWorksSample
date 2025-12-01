/*
 * ============================================================================
 * DATA ACCESS LAYER: SalesOrderDetailRepository
 * ============================================================================
 * 
 * Repository for accessing data from the Sales.SalesOrderDetail table.
 * Manages CRUD operations for order detail lines.
 * 
 * TEACHING NOTE:
 * - The table has a composite primary key (SalesOrderID, SalesOrderDetailID)
 * - Each detail is linked to a product and an order
 * - LineTotal is a computed column calculated by the database
 * - We use JOIN to get related information (product name, etc.)
 * ============================================================================
 */

using Dapper;
using AdventureWorksApp.Models;

namespace AdventureWorksApp.DataAccess
{
    /// <summary>
    /// Repository for managing order details from the Sales.SalesOrderDetail table.
    /// Provides CRUD methods to interact with order detail lines.
    /// </summary>
    public class SalesOrderDetailRepository
    {
        /// <summary>
        /// Gets all detail lines with product information.
        /// 
        /// TEACHING NOTE:
        /// - We use JOIN to get the product name from the Product table
        /// - We limit results to avoid overloading the UI
        /// - Dapper's automatic mapping includes columns from the JOIN
        /// </summary>
        public async Task<IEnumerable<SalesOrderDetail>> GetAllAsync(int maxRecords = 500)
        {
            using var connection = DbConnectionFactory.CreateConnection();
            
            string sql = $@"
                SELECT TOP (@MaxRecords)
                    d.SalesOrderID, d.SalesOrderDetailID, d.CarrierTrackingNumber,
                    d.OrderQty, d.ProductID, d.SpecialOfferID, d.UnitPrice,
                    d.UnitPriceDiscount, d.LineTotal, d.rowguid, d.ModifiedDate,
                    p.Name AS ProductName,
                    h.SalesOrderNumber
                FROM Sales.SalesOrderDetail d
                INNER JOIN Production.Product p ON d.ProductID = p.ProductID
                INNER JOIN Sales.SalesOrderHeader h ON d.SalesOrderID = h.SalesOrderID
                ORDER BY d.SalesOrderID DESC, d.SalesOrderDetailID";

            return await connection.QueryAsync<SalesOrderDetail>(sql, new { MaxRecords = maxRecords });
        }

        /// <summary>
        /// Gets all detail lines for a specific order.
        /// 
        /// TEACHING NOTE:
        /// - This is the most common query: given an order, show its details
        /// - Includes JOIN with Product to have the product name
        /// </summary>
        public async Task<IEnumerable<SalesOrderDetail>> GetByOrderIdAsync(int salesOrderId)
        {
            using var connection = DbConnectionFactory.CreateConnection();
            
            const string sql = @"
                SELECT 
                    d.SalesOrderID, d.SalesOrderDetailID, d.CarrierTrackingNumber,
                    d.OrderQty, d.ProductID, d.SpecialOfferID, d.UnitPrice,
                    d.UnitPriceDiscount, d.LineTotal, d.rowguid, d.ModifiedDate,
                    p.Name AS ProductName,
                    h.SalesOrderNumber
                FROM Sales.SalesOrderDetail d
                INNER JOIN Production.Product p ON d.ProductID = p.ProductID
                INNER JOIN Sales.SalesOrderHeader h ON d.SalesOrderID = h.SalesOrderID
                WHERE d.SalesOrderID = @SalesOrderID
                ORDER BY d.SalesOrderDetailID";

            return await connection.QueryAsync<SalesOrderDetail>(sql, new { SalesOrderID = salesOrderId });
        }

        /// <summary>
        /// Gets a single detail line by the composite primary key.
        /// 
        /// TEACHING NOTE:
        /// - The primary key is composite: SalesOrderID + SalesOrderDetailID
        /// - We must pass both values to uniquely identify the row
        /// </summary>
        public async Task<SalesOrderDetail?> GetByIdAsync(int salesOrderId, int salesOrderDetailId)
        {
            using var connection = DbConnectionFactory.CreateConnection();
            
            const string sql = @"
                SELECT 
                    d.SalesOrderID, d.SalesOrderDetailID, d.CarrierTrackingNumber,
                    d.OrderQty, d.ProductID, d.SpecialOfferID, d.UnitPrice,
                    d.UnitPriceDiscount, d.LineTotal, d.rowguid, d.ModifiedDate,
                    p.Name AS ProductName,
                    h.SalesOrderNumber
                FROM Sales.SalesOrderDetail d
                INNER JOIN Production.Product p ON d.ProductID = p.ProductID
                INNER JOIN Sales.SalesOrderHeader h ON d.SalesOrderID = h.SalesOrderID
                WHERE d.SalesOrderID = @SalesOrderID 
                  AND d.SalesOrderDetailID = @SalesOrderDetailID";

            return await connection.QueryFirstOrDefaultAsync<SalesOrderDetail>(sql, new
            {
                SalesOrderID = salesOrderId,
                SalesOrderDetailID = salesOrderDetailId
            });
        }

        /// <summary>
        /// Gets all detail lines containing a specific product.
        /// Useful for seeing where a product has been sold.
        /// </summary>
        public async Task<IEnumerable<SalesOrderDetail>> GetByProductIdAsync(int productId)
        {
            using var connection = DbConnectionFactory.CreateConnection();
            
            const string sql = @"
                SELECT TOP 100
                    d.SalesOrderID, d.SalesOrderDetailID, d.CarrierTrackingNumber,
                    d.OrderQty, d.ProductID, d.SpecialOfferID, d.UnitPrice,
                    d.UnitPriceDiscount, d.LineTotal, d.rowguid, d.ModifiedDate,
                    p.Name AS ProductName,
                    h.SalesOrderNumber
                FROM Sales.SalesOrderDetail d
                INNER JOIN Production.Product p ON d.ProductID = p.ProductID
                INNER JOIN Sales.SalesOrderHeader h ON d.SalesOrderID = h.SalesOrderID
                WHERE d.ProductID = @ProductID
                ORDER BY h.OrderDate DESC";

            return await connection.QueryAsync<SalesOrderDetail>(sql, new { ProductID = productId });
        }

        /// <summary>
        /// Inserts a new order detail line.
        /// 
        /// TEACHING NOTE:
        /// - SalesOrderDetailID is an IDENTITY, generated by the database
        /// - LineTotal is a computed column, should not be inserted
        /// - SpecialOfferID = 1 is "No Discount" (always present in AdventureWorks)
        /// </summary>
        public async Task<int> InsertAsync(SalesOrderDetail detail)
        {
            using var connection = DbConnectionFactory.CreateConnection();
            
            const string sql = @"
                INSERT INTO Sales.SalesOrderDetail (
                    SalesOrderID, CarrierTrackingNumber, OrderQty, ProductID,
                    SpecialOfferID, UnitPrice, UnitPriceDiscount, rowguid, ModifiedDate
                )
                OUTPUT INSERTED.SalesOrderDetailID
                VALUES (
                    @SalesOrderID, @CarrierTrackingNumber, @OrderQty, @ProductID,
                    @SpecialOfferID, @UnitPrice, @UnitPriceDiscount, NEWID(), GETDATE()
                )";

            return await connection.ExecuteScalarAsync<int>(sql, detail);
        }

        /// <summary>
        /// Updates an existing order detail line.
        /// 
        /// TEACHING NOTE:
        /// - Only quantity, discount and tracking number are modifiable
        /// - Changing the product requires deleting and reinserting the line
        /// - The composite primary key must be used in the WHERE clause
        /// </summary>
        public async Task<int> UpdateAsync(SalesOrderDetail detail)
        {
            using var connection = DbConnectionFactory.CreateConnection();
            
            const string sql = @"
                UPDATE Sales.SalesOrderDetail SET
                    CarrierTrackingNumber = @CarrierTrackingNumber,
                    OrderQty = @OrderQty,
                    UnitPriceDiscount = @UnitPriceDiscount,
                    ModifiedDate = GETDATE()
                WHERE SalesOrderID = @SalesOrderID 
                  AND SalesOrderDetailID = @SalesOrderDetailID";

            return await connection.ExecuteAsync(sql, detail);
        }

        /// <summary>
        /// Deletes an order detail line.
        /// 
        /// TEACHING NOTE:
        /// - Requires both values of the composite primary key
        /// - Deleting a line should update the header's SubTotal
        /// - In production, a transaction is used
        /// </summary>
        public async Task<int> DeleteAsync(int salesOrderId, int salesOrderDetailId)
        {
            using var connection = DbConnectionFactory.CreateConnection();
            
            const string sql = @"
                DELETE FROM Sales.SalesOrderDetail 
                WHERE SalesOrderID = @SalesOrderID 
                  AND SalesOrderDetailID = @SalesOrderDetailID";
            
            return await connection.ExecuteAsync(sql, new
            {
                SalesOrderID = salesOrderId,
                SalesOrderDetailID = salesOrderDetailId
            });
        }

        /// <summary>
        /// Calculates the total of an order by summing the LineTotal of its details.
        /// Useful for verifying data consistency.
        /// </summary>
        public async Task<decimal> GetOrderTotalAsync(int salesOrderId)
        {
            using var connection = DbConnectionFactory.CreateConnection();
            
            const string sql = @"
                SELECT ISNULL(SUM(LineTotal), 0) 
                FROM Sales.SalesOrderDetail 
                WHERE SalesOrderID = @SalesOrderID";
            
            return await connection.ExecuteScalarAsync<decimal>(sql, new { SalesOrderID = salesOrderId });
        }
    }
}
