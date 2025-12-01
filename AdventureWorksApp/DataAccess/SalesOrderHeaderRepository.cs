/*
 * ============================================================================
 * DATA ACCESS LAYER: SalesOrderHeaderRepository
 * ============================================================================
 * 
 * Repository for accessing data from the Sales.SalesOrderHeader table.
 * Manages CRUD operations for sales order headers.
 * 
 * TEACHING NOTE:
 * - Orders have relationships with many other tables (Customer, Territory, etc.)
 * - For teaching simplicity, not all fields are user-modifiable
 * - Some fields like TotalDue are automatically calculated by the database
 * ============================================================================
 */

using Dapper;
using AdventureWorksApp.Models;

namespace AdventureWorksApp.DataAccess
{
    /// <summary>
    /// Repository for managing orders from the Sales.SalesOrderHeader table.
    /// Provides CRUD methods to interact with order headers.
    /// </summary>
    public class SalesOrderHeaderRepository
    {
        /// <summary>
        /// Gets all orders from the database.
        /// 
        /// TEACHING NOTE:
        /// - We limit to 1000 records to avoid overloading the UI
        /// - In production, pagination would be used
        /// - ORDER BY DESC shows most recent orders first
        /// </summary>
        public async Task<IEnumerable<SalesOrderHeader>> GetAllAsync(int maxRecords = 1000)
        {
            using var connection = DbConnectionFactory.CreateConnection();
            
            string sql = $@"
                SELECT TOP (@MaxRecords)
                    SalesOrderID, RevisionNumber, OrderDate, DueDate, ShipDate,
                    Status, OnlineOrderFlag, SalesOrderNumber, PurchaseOrderNumber,
                    AccountNumber, CustomerID, SalesPersonID, TerritoryID,
                    BillToAddressID, ShipToAddressID, ShipMethodID, CreditCardID,
                    CreditCardApprovalCode, CurrencyRateID, SubTotal, TaxAmt,
                    Freight, TotalDue, Comment, rowguid, ModifiedDate
                FROM Sales.SalesOrderHeader
                ORDER BY OrderDate DESC";

            return await connection.QueryAsync<SalesOrderHeader>(sql, new { MaxRecords = maxRecords });
        }

        /// <summary>
        /// Gets a specific order by its ID.
        /// </summary>
        public async Task<SalesOrderHeader?> GetByIdAsync(int salesOrderId)
        {
            using var connection = DbConnectionFactory.CreateConnection();
            
            const string sql = @"
                SELECT 
                    SalesOrderID, RevisionNumber, OrderDate, DueDate, ShipDate,
                    Status, OnlineOrderFlag, SalesOrderNumber, PurchaseOrderNumber,
                    AccountNumber, CustomerID, SalesPersonID, TerritoryID,
                    BillToAddressID, ShipToAddressID, ShipMethodID, CreditCardID,
                    CreditCardApprovalCode, CurrencyRateID, SubTotal, TaxAmt,
                    Freight, TotalDue, Comment, rowguid, ModifiedDate
                FROM Sales.SalesOrderHeader
                WHERE SalesOrderID = @SalesOrderID";

            return await connection.QueryFirstOrDefaultAsync<SalesOrderHeader>(
                sql, new { SalesOrderID = salesOrderId });
        }

        /// <summary>
        /// Searches orders by order number or by period.
        /// 
        /// TEACHING NOTE:
        /// - Example of a query with multiple optional conditions
        /// - Each parameter can be null to not filter
        /// </summary>
        public async Task<IEnumerable<SalesOrderHeader>> SearchAsync(
            string? orderNumber = null,
            DateTime? fromDate = null,
            DateTime? toDate = null,
            int? customerId = null)
        {
            using var connection = DbConnectionFactory.CreateConnection();
            
            const string sql = @"
                SELECT TOP 500
                    SalesOrderID, RevisionNumber, OrderDate, DueDate, ShipDate,
                    Status, OnlineOrderFlag, SalesOrderNumber, PurchaseOrderNumber,
                    AccountNumber, CustomerID, SalesPersonID, TerritoryID,
                    BillToAddressID, ShipToAddressID, ShipMethodID, CreditCardID,
                    CreditCardApprovalCode, CurrencyRateID, SubTotal, TaxAmt,
                    Freight, TotalDue, Comment, rowguid, ModifiedDate
                FROM Sales.SalesOrderHeader
                WHERE (@OrderNumber IS NULL OR SalesOrderNumber LIKE @OrderNumber)
                  AND (@FromDate IS NULL OR OrderDate >= @FromDate)
                  AND (@ToDate IS NULL OR OrderDate <= @ToDate)
                  AND (@CustomerID IS NULL OR CustomerID = @CustomerID)
                ORDER BY OrderDate DESC";

            return await connection.QueryAsync<SalesOrderHeader>(sql, new
            {
                OrderNumber = orderNumber != null ? $"%{orderNumber}%" : null,
                FromDate = fromDate,
                ToDate = toDate,
                CustomerID = customerId
            });
        }

        /// <summary>
        /// Inserts a new order into the database.
        /// 
        /// TEACHING NOTE:
        /// - TotalDue and other calculated fields are NOT inserted here
        /// - The database calculates them via triggers or computed columns
        /// - In a real system, order insertion is much more complex
        /// </summary>
        public async Task<int> InsertAsync(SalesOrderHeader order)
        {
            using var connection = DbConnectionFactory.CreateConnection();
            
            const string sql = @"
                INSERT INTO Sales.SalesOrderHeader (
                    RevisionNumber, OrderDate, DueDate, ShipDate,
                    Status, OnlineOrderFlag, PurchaseOrderNumber,
                    AccountNumber, CustomerID, SalesPersonID, TerritoryID,
                    BillToAddressID, ShipToAddressID, ShipMethodID, CreditCardID,
                    CreditCardApprovalCode, CurrencyRateID, SubTotal, TaxAmt,
                    Freight, Comment, rowguid, ModifiedDate
                )
                OUTPUT INSERTED.SalesOrderID
                VALUES (
                    @RevisionNumber, @OrderDate, @DueDate, @ShipDate,
                    @Status, @OnlineOrderFlag, @PurchaseOrderNumber,
                    @AccountNumber, @CustomerID, @SalesPersonID, @TerritoryID,
                    @BillToAddressID, @ShipToAddressID, @ShipMethodID, @CreditCardID,
                    @CreditCardApprovalCode, @CurrencyRateID, @SubTotal, @TaxAmt,
                    @Freight, @Comment, NEWID(), GETDATE()
                )";

            return await connection.ExecuteScalarAsync<int>(sql, order);
        }

        /// <summary>
        /// Updates an existing order.
        /// 
        /// TEACHING NOTE:
        /// - Only some fields are updatable after creation
        /// - Status, Comment and shipping dates are the most common
        /// </summary>
        public async Task<int> UpdateAsync(SalesOrderHeader order)
        {
            using var connection = DbConnectionFactory.CreateConnection();
            
            const string sql = @"
                UPDATE Sales.SalesOrderHeader SET
                    RevisionNumber = RevisionNumber + 1,
                    DueDate = @DueDate,
                    ShipDate = @ShipDate,
                    Status = @Status,
                    PurchaseOrderNumber = @PurchaseOrderNumber,
                    Comment = @Comment,
                    ModifiedDate = GETDATE()
                WHERE SalesOrderID = @SalesOrderID";

            return await connection.ExecuteAsync(sql, order);
        }

        /// <summary>
        /// Deletes an order from the database.
        /// 
        /// TEACHING NOTE:
        /// - Deletion must first remove the order details
        /// - In production, a transaction is used to ensure consistency
        /// - Better to use soft delete (Status = Cancelled) in production
        /// </summary>
        public async Task<int> DeleteAsync(int salesOrderId)
        {
            using var connection = DbConnectionFactory.CreateConnection();
            await ((System.Data.Common.DbConnection)connection).OpenAsync();
            
            // We use a transaction to delete details first and then the header
            using var transaction = connection.BeginTransaction();
            
            try
            {
                // First delete the details
                const string deleteDetailsSql = 
                    "DELETE FROM Sales.SalesOrderDetail WHERE SalesOrderID = @SalesOrderID";
                await connection.ExecuteAsync(deleteDetailsSql, 
                    new { SalesOrderID = salesOrderId }, transaction);
                
                // Then delete the header
                const string deleteHeaderSql = 
                    "DELETE FROM Sales.SalesOrderHeader WHERE SalesOrderID = @SalesOrderID";
                int result = await connection.ExecuteAsync(deleteHeaderSql, 
                    new { SalesOrderID = salesOrderId }, transaction);
                
                transaction.Commit();
                return result;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Gets orders for a specific customer.
        /// Useful for displaying a customer's order history.
        /// </summary>
        public async Task<IEnumerable<SalesOrderHeader>> GetByCustomerIdAsync(int customerId)
        {
            using var connection = DbConnectionFactory.CreateConnection();
            
            const string sql = @"
                SELECT 
                    SalesOrderID, RevisionNumber, OrderDate, DueDate, ShipDate,
                    Status, OnlineOrderFlag, SalesOrderNumber, PurchaseOrderNumber,
                    AccountNumber, CustomerID, SalesPersonID, TerritoryID,
                    BillToAddressID, ShipToAddressID, ShipMethodID, CreditCardID,
                    CreditCardApprovalCode, CurrencyRateID, SubTotal, TaxAmt,
                    Freight, TotalDue, Comment, rowguid, ModifiedDate
                FROM Sales.SalesOrderHeader
                WHERE CustomerID = @CustomerID
                ORDER BY OrderDate DESC";

            return await connection.QueryAsync<SalesOrderHeader>(sql, new { CustomerID = customerId });
        }
    }
}
