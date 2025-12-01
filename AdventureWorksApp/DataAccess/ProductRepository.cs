/*
 * ============================================================================
 * DATA ACCESS LAYER: ProductRepository
 * ============================================================================
 * 
 * Repository per l'accesso ai dati della tabella Production.Product.
 * Implementa le operazioni CRUD (Create, Read, Update, Delete) usando Dapper.
 * 
 * NOTA DIDATTICA:
 * - Il pattern Repository separa la logica di accesso ai dati dalla logica 
 *   di business e dalla presentazione
 * - Dapper mappa automaticamente i risultati delle query sugli oggetti
 * - Usiamo query parametrizzate per prevenire SQL injection
 * - Tutti i metodi sono async per non bloccare l'interfaccia utente
 * ============================================================================
 */

using Dapper;
using AdventureWorksApp.Models;

namespace AdventureWorksApp.DataAccess
{
    /// <summary>
    /// Repository per la gestione dei prodotti dalla tabella Production.Product.
    /// Fornisce metodi CRUD per interagire con i dati dei prodotti.
    /// </summary>
    public class ProductRepository
    {
        /// <summary>
        /// Ottiene tutti i prodotti dal database.
        /// 
        /// NOTA DIDATTICA:
        /// - Query() restituisce un IEnumerable che viene mappato automaticamente
        /// - Il metodo è asincrono (async/await) per non bloccare l'UI
        /// - ORDER BY assicura un ordinamento consistente
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
        /// Ottiene un prodotto specifico tramite il suo ID.
        /// 
        /// NOTA DIDATTICA:
        /// - QueryFirstOrDefaultAsync restituisce il primo risultato o null
        /// - Usiamo un oggetto anonimo per passare i parametri alla query
        /// - I parametri sono prefissati con @ nella query SQL
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
        /// Cerca prodotti per nome (ricerca parziale).
        /// 
        /// NOTA DIDATTICA:
        /// - Usiamo LIKE per la ricerca parziale
        /// - I caratteri % vengono aggiunti in C#, non nella query
        /// - Questo permette di controllare meglio il tipo di ricerca
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
        /// Inserisce un nuovo prodotto nel database.
        /// 
        /// NOTA DIDATTICA:
        /// - OUTPUT INSERTED.ProductID restituisce l'ID generato
        /// - ExecuteScalarAsync restituisce un singolo valore
        /// - Impostiamo automaticamente ModifiedDate e rowguid
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
        /// Aggiorna un prodotto esistente.
        /// 
        /// NOTA DIDATTICA:
        /// - L'UPDATE modifica solo i campi specificati
        /// - ModifiedDate viene aggiornato automaticamente
        /// - Restituiamo il numero di righe modificate (dovrebbe essere 1)
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
        /// Elimina un prodotto dal database.
        /// 
        /// NOTA DIDATTICA:
        /// - ATTENZIONE: potrebbero esserci vincoli di integrità referenziale
        /// - Se il prodotto è referenziato in altre tabelle, l'eliminazione fallirà
        /// - In produzione, considerare soft delete (flag IsDeleted)
        /// </summary>
        public async Task<int> DeleteAsync(int productId)
        {
            using var connection = DbConnectionFactory.CreateConnection();
            
            const string sql = "DELETE FROM Production.Product WHERE ProductID = @ProductID";
            
            return await connection.ExecuteAsync(sql, new { ProductID = productId });
        }

        /// <summary>
        /// Verifica se un prodotto può essere eliminato.
        /// Controlla se è referenziato in SalesOrderDetail.
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
