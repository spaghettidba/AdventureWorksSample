/*
 * ============================================================================
 * DATA ACCESS LAYER: ProductRepository
 * ============================================================================
 * 
 * Repository for accessing data from the Production.Product table.
 * Implements CRUD operations (Create, Read, Update, Delete) using Dapper.
 * 
 * TEACHING NOTE:
 * - The Repository pattern separates data access logic from business logic
 *   and presentation
 * - Dapper automatically maps query results to objects
 * - We use parameterized queries to prevent SQL injection
 * - All methods are async to avoid blocking the user interface
 * ============================================================================
 */

using Dapper;
using AdventureWorksApp.Models;

namespace AdventureWorksApp.DataAccess
{
    /// <summary>
    /// Repository for managing products from the Production.Product table.
    /// Provides CRUD methods to interact with product data.
    /// </summary>
    public class ProductRepository
    {
        /// <summary>
        /// Gets all products from the database.
        /// 
        /// TEACHING NOTE:
        /// - Query() returns an IEnumerable that is automatically mapped
        /// - The method is asynchronous (async/await) to avoid blocking the UI
        /// - ORDER BY ensures consistent ordering
        /// </summary>
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            using var connection = DbConnectionFactory.CreateConnection();
            
            const string sql = @"
                SELECT 
                    ProductID, Name, ProductNumber, MakeFlag, FinishedGoodsFlag,
                    Color, SafetyStockLevel, ReorderPoint, StandardCost, ListPrice,
                    Size, SizeUnitMeasureCode, WeightUnitMeasureCode, Weight,
                    DaysToManufacture, ProductLine, Class, Style,
                    ProductSubcategoryID, ProductModelID, SellStartDate,
                    SellEndDate, DiscontinuedDate, rowguid, ModifiedDate
                FROM Production.Product
                ORDER BY Name";

            return await connection.QueryAsync<Product>(sql);
        }

        /// <summary>
        /// Gets a specific product by its ID.
        /// 
        /// TEACHING NOTE:
        /// - QueryFirstOrDefaultAsync returns the first result or null
        /// - We use an anonymous object to pass parameters to the query
        /// - Parameters are prefixed with @ in the SQL query
        /// </summary>
        public async Task<Product?> GetByIdAsync(int productId)
        {
            using var connection = DbConnectionFactory.CreateConnection();
            
            const string sql = @"
                SELECT 
                    ProductID, Name, ProductNumber, MakeFlag, FinishedGoodsFlag,
                    Color, SafetyStockLevel, ReorderPoint, StandardCost, ListPrice,
                    Size, SizeUnitMeasureCode, WeightUnitMeasureCode, Weight,
                    DaysToManufacture, ProductLine, Class, Style,
                    ProductSubcategoryID, ProductModelID, SellStartDate,
                    SellEndDate, DiscontinuedDate, rowguid, ModifiedDate
                FROM Production.Product
                WHERE ProductID = @ProductID";

            return await connection.QueryFirstOrDefaultAsync<Product>(sql, new { ProductID = productId });
        }

        /// <summary>
        /// Searches products by name (partial search).
        /// 
        /// TEACHING NOTE:
        /// - We use LIKE for partial matching
        /// - The % characters are added in C#, not in the query
        /// - This allows better control over the search type
        /// </summary>
        public async Task<IEnumerable<Product>> SearchByNameAsync(string searchTerm)
        {
            using var connection = DbConnectionFactory.CreateConnection();
            
            const string sql = @"
                SELECT 
                    ProductID, Name, ProductNumber, MakeFlag, FinishedGoodsFlag,
                    Color, SafetyStockLevel, ReorderPoint, StandardCost, ListPrice,
                    Size, SizeUnitMeasureCode, WeightUnitMeasureCode, Weight,
                    DaysToManufacture, ProductLine, Class, Style,
                    ProductSubcategoryID, ProductModelID, SellStartDate,
                    SellEndDate, DiscontinuedDate, rowguid, ModifiedDate
                FROM Production.Product
                WHERE Name LIKE @SearchTerm
                ORDER BY Name";

            return await connection.QueryAsync<Product>(sql, new { SearchTerm = $"%{searchTerm}%" });
        }

        /// <summary>
        /// Inserts a new product into the database.
        /// 
        /// TEACHING NOTE:
        /// - OUTPUT INSERTED.ProductID returns the generated ID
        /// - ExecuteScalarAsync returns a single value
        /// - We automatically set ModifiedDate and rowguid
        /// </summary>
        public async Task<int> InsertAsync(Product product)
        {
            using var connection = DbConnectionFactory.CreateConnection();
            
            const string sql = @"
                INSERT INTO Production.Product (
                    Name, ProductNumber, MakeFlag, FinishedGoodsFlag,
                    Color, SafetyStockLevel, ReorderPoint, StandardCost, ListPrice,
                    Size, SizeUnitMeasureCode, WeightUnitMeasureCode, Weight,
                    DaysToManufacture, ProductLine, Class, Style,
                    ProductSubcategoryID, ProductModelID, SellStartDate,
                    SellEndDate, DiscontinuedDate, rowguid, ModifiedDate
                )
                OUTPUT INSERTED.ProductID
                VALUES (
                    @Name, @ProductNumber, @MakeFlag, @FinishedGoodsFlag,
                    @Color, @SafetyStockLevel, @ReorderPoint, @StandardCost, @ListPrice,
                    @Size, @SizeUnitMeasureCode, @WeightUnitMeasureCode, @Weight,
                    @DaysToManufacture, @ProductLine, @Class, @Style,
                    @ProductSubcategoryID, @ProductModelID, @SellStartDate,
                    @SellEndDate, @DiscontinuedDate, NEWID(), GETDATE()
                )";

            return await connection.ExecuteScalarAsync<int>(sql, product);
        }

        /// <summary>
        /// Updates an existing product.
        /// 
        /// TEACHING NOTE:
        /// - UPDATE only modifies the specified fields
        /// - ModifiedDate is updated automatically
        /// - We return the number of modified rows (should be 1)
        /// </summary>
        public async Task<int> UpdateAsync(Product product)
        {
            using var connection = DbConnectionFactory.CreateConnection();
            
            const string sql = @"
                UPDATE Production.Product SET
                    Name = @Name,
                    ProductNumber = @ProductNumber,
                    MakeFlag = @MakeFlag,
                    FinishedGoodsFlag = @FinishedGoodsFlag,
                    Color = @Color,
                    SafetyStockLevel = @SafetyStockLevel,
                    ReorderPoint = @ReorderPoint,
                    StandardCost = @StandardCost,
                    ListPrice = @ListPrice,
                    Size = @Size,
                    SizeUnitMeasureCode = @SizeUnitMeasureCode,
                    WeightUnitMeasureCode = @WeightUnitMeasureCode,
                    Weight = @Weight,
                    DaysToManufacture = @DaysToManufacture,
                    ProductLine = @ProductLine,
                    Class = @Class,
                    Style = @Style,
                    ProductSubcategoryID = @ProductSubcategoryID,
                    ProductModelID = @ProductModelID,
                    SellStartDate = @SellStartDate,
                    SellEndDate = @SellEndDate,
                    DiscontinuedDate = @DiscontinuedDate,
                    ModifiedDate = GETDATE()
                WHERE ProductID = @ProductID";

            return await connection.ExecuteAsync(sql, product);
        }

        /// <summary>
        /// Deletes a product from the database.
        /// 
        /// TEACHING NOTE:
        /// - WARNING: there may be referential integrity constraints
        /// - If the product is referenced in other tables, deletion will fail
        /// - In production, consider soft delete (IsDeleted flag)
        /// </summary>
        public async Task<int> DeleteAsync(int productId)
        {
            using var connection = DbConnectionFactory.CreateConnection();
            
            const string sql = "DELETE FROM Production.Product WHERE ProductID = @ProductID";
            
            return await connection.ExecuteAsync(sql, new { ProductID = productId });
        }

        /// <summary>
        /// Checks if a product can be deleted.
        /// Verifies if it is referenced in SalesOrderDetail.
        /// </summary>
        public async Task<bool> CanDeleteAsync(int productId)
        {
            using var connection = DbConnectionFactory.CreateConnection();
            
            const string sql = @"
                SELECT CASE 
                    WHEN EXISTS (SELECT 1 FROM Sales.SalesOrderDetail WHERE ProductID = @ProductID)
                    THEN 0 ELSE 1 
                END";
            
            return await connection.ExecuteScalarAsync<bool>(sql, new { ProductID = productId });
        }
    }
}
