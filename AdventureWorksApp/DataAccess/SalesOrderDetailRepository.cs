/*
 * ============================================================================
 * DATA ACCESS LAYER: SalesOrderDetailRepository
 * ============================================================================
 * 
 * Repository per l'accesso ai dati della tabella Sales.SalesOrderDetail.
 * Gestisce le operazioni CRUD per le righe di dettaglio degli ordini.
 * 
 * NOTA DIDATTICA:
 * - La tabella ha una chiave primaria composita (SalesOrderID, SalesOrderDetailID)
 * - Ogni dettaglio è legato a un prodotto e a un ordine
 * - LineTotal è una computed column calcolata dal database
 * - Usiamo JOIN per ottenere informazioni correlate (nome prodotto, etc.)
 * ============================================================================
 */

using Dapper;
using AdventureWorksApp.Models;

namespace AdventureWorksApp.DataAccess
{
    /// <summary>
    /// Repository per la gestione dei dettagli ordine dalla tabella Sales.SalesOrderDetail.
    /// Fornisce metodi CRUD per interagire con le righe di dettaglio degli ordini.
    /// </summary>
    public class SalesOrderDetailRepository
    {
        /// <summary>
        /// Ottiene tutte le righe di dettaglio con informazioni del prodotto.
        /// 
        /// NOTA DIDATTICA:
        /// - Usiamo JOIN per ottenere il nome prodotto dalla tabella Product
        /// - Limitiamo i risultati per non sovraccaricare l'UI
        /// - Il mapping automatico di Dapper include anche le colonne dalla JOIN
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
        /// Ottiene tutte le righe di dettaglio di un ordine specifico.
        /// 
        /// NOTA DIDATTICA:
        /// - Questa è la query più comune: dato un ordine, mostra i suoi dettagli
        /// - Include JOIN con Product per avere il nome prodotto
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
        /// Ottiene una singola riga di dettaglio tramite la chiave primaria composita.
        /// 
        /// NOTA DIDATTICA:
        /// - La chiave primaria è composita: SalesOrderID + SalesOrderDetailID
        /// - Dobbiamo passare entrambi i valori per identificare univocamente la riga
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
        /// Ottiene tutte le righe di dettaglio contenenti un prodotto specifico.
        /// Utile per vedere dove un prodotto è stato venduto.
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
        /// Inserisce una nuova riga di dettaglio ordine.
        /// 
        /// NOTA DIDATTICA:
        /// - SalesOrderDetailID è un IDENTITY, viene generato dal database
        /// - LineTotal è una computed column, non va inserita
        /// - SpecialOfferID = 1 è "No Discount" (sempre presente in AdventureWorks)
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
        /// Aggiorna una riga di dettaglio ordine esistente.
        /// 
        /// NOTA DIDATTICA:
        /// - Solo quantità, sconto e tracking number sono modificabili
        /// - Cambiare prodotto richiede eliminare e reinserire la riga
        /// - La chiave primaria composita va usata nel WHERE
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
        /// Elimina una riga di dettaglio ordine.
        /// 
        /// NOTA DIDATTICA:
        /// - Richiede entrambi i valori della chiave primaria composita
        /// - L'eliminazione di una riga dovrebbe aggiornare SubTotal dell'header
        /// - In produzione si usa una transazione
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
        /// Calcola il totale di un ordine sommando i LineTotal dei suoi dettagli.
        /// Utile per verificare la consistenza dei dati.
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
