/*
 * ============================================================================
 * DATA ACCESS LAYER: SalesOrderHeaderRepository
 * ============================================================================
 * 
 * Repository per l'accesso ai dati della tabella Sales.SalesOrderHeader.
 * Gestisce le operazioni CRUD per le intestazioni degli ordini di vendita.
 * 
 * NOTA DIDATTICA:
 * - Gli ordini hanno relazioni con molte altre tabelle (Customer, Territory, etc.)
 * - Per semplicità didattica, non tutti i campi sono modificabili dall'utente
 * - Alcuni campi come TotalDue sono calcolati automaticamente dal database
 * ============================================================================
 */

using Dapper;
using AdventureWorksApp.Models;

namespace AdventureWorksApp.DataAccess
{
    /// <summary>
    /// Repository per la gestione degli ordini dalla tabella Sales.SalesOrderHeader.
    /// Fornisce metodi CRUD per interagire con le intestazioni ordine.
    /// </summary>
    public class SalesOrderHeaderRepository
    {
        /// <summary>
        /// Ottiene tutti gli ordini dal database.
        /// 
        /// NOTA DIDATTICA:
        /// - Limitiamo a 1000 record per non sovraccaricare l'UI
        /// - In produzione si userebbe la paginazione
        /// - ORDER BY DESC mostra prima gli ordini più recenti
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
        /// Ottiene un ordine specifico tramite il suo ID.
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
        /// Cerca ordini per numero ordine o per periodo.
        /// 
        /// NOTA DIDATTICA:
        /// - Esempio di query con condizioni multiple opzionali
        /// - Ogni parametro può essere null per non filtrare
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
        /// Inserisce un nuovo ordine nel database.
        /// 
        /// NOTA DIDATTICA:
        /// - TotalDue e altri campi calcolati NON sono inseriti qui
        /// - Il database li calcola tramite trigger o computed columns
        /// - In un sistema reale, l'inserimento ordini è molto più complesso
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
        /// Aggiorna un ordine esistente.
        /// 
        /// NOTA DIDATTICA:
        /// - Solo alcuni campi sono aggiornabili dopo la creazione
        /// - Status, Comment e date di spedizione sono i più comuni
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
        /// Elimina un ordine dal database.
        /// 
        /// NOTA DIDATTICA:
        /// - L'eliminazione deve prima rimuovere i dettagli dell'ordine
        /// - In produzione si usa una transazione per garantire consistenza
        /// - Meglio usare soft delete (Status = Cancelled) in produzione
        /// </summary>
        public async Task<int> DeleteAsync(int salesOrderId)
        {
            using var connection = DbConnectionFactory.CreateConnection();
            connection.Open();
            
            // Usiamo una transazione per eliminare prima i dettagli e poi l'header
            using var transaction = connection.BeginTransaction();
            
            try
            {
                // Prima eliminiamo i dettagli
                const string deleteDetailsSql = 
                    "DELETE FROM Sales.SalesOrderDetail WHERE SalesOrderID = @SalesOrderID";
                await connection.ExecuteAsync(deleteDetailsSql, 
                    new { SalesOrderID = salesOrderId }, transaction);
                
                // Poi eliminiamo l'header
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
        /// Ottiene gli ordini di un cliente specifico.
        /// Utile per visualizzare lo storico ordini di un cliente.
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
