/*
 * ============================================================================
 * MODELLO: SalesOrderDetail
 * ============================================================================
 * 
 * Questo modello rappresenta la tabella Sales.SalesOrderDetail del database 
 * AdventureWorks. Contiene le righe di dettaglio di un ordine di vendita.
 * 
 * NOTA DIDATTICA:
 * - Questa tabella ha una chiave primaria composita (SalesOrderID, SalesOrderDetailID)
 * - Ha una relazione N:1 con SalesOrderHeader tramite SalesOrderID
 * - Ha una relazione N:1 con Product tramite ProductID
 * - Ogni riga rappresenta un prodotto ordinato con quantità e prezzo
 * ============================================================================
 */

namespace AdventureWorksApp.Models
{
    /// <summary>
    /// Rappresenta una riga di dettaglio di un ordine dalla tabella 
    /// Sales.SalesOrderDetail. Contiene informazioni sul singolo prodotto 
    /// ordinato: quantità, prezzo, sconto, ecc.
    /// </summary>
    public class SalesOrderDetail
    {
        // Parte della chiave primaria - riferimento all'ordine
        public int SalesOrderID { get; set; }

        // Parte della chiave primaria - identificativo progressivo della riga
        public int SalesOrderDetailID { get; set; }

        // Posizione della riga nel carrello (per ordini online)
        public string? CarrierTrackingNumber { get; set; }

        // Quantità ordinata
        public short OrderQty { get; set; }

        // ID del prodotto ordinato - chiave esterna verso Production.Product
        public int ProductID { get; set; }

        // ID dell'offerta speciale applicata
        public int SpecialOfferID { get; set; }

        // Prezzo unitario del prodotto
        public decimal UnitPrice { get; set; }

        // Percentuale di sconto applicata (0.00 - 1.00)
        public decimal UnitPriceDiscount { get; set; }

        // Totale della riga: (UnitPrice * OrderQty) - (UnitPrice * OrderQty * UnitPriceDiscount)
        public decimal LineTotal { get; set; }

        // GUID di riga per tracking modifiche
        public Guid rowguid { get; set; }

        // Data di modifica del record
        public DateTime ModifiedDate { get; set; }

        // ============================================================================
        // PROPRIETÀ DI NAVIGAZIONE (non nel database, popolate da query JOIN)
        // ============================================================================

        /// <summary>
        /// Nome del prodotto - popolato tramite JOIN con Production.Product
        /// Questa proprietà viene usata per visualizzare informazioni del prodotto
        /// senza dover fare query aggiuntive
        /// </summary>
        public string? ProductName { get; set; }

        /// <summary>
        /// Numero ordine - popolato tramite JOIN con Sales.SalesOrderHeader
        /// </summary>
        public string? SalesOrderNumber { get; set; }

        /// <summary>
        /// Calcola il totale della riga in modo programmatico
        /// Utile per verificare il valore del database
        /// </summary>
        public decimal CalculatedLineTotal => 
            (UnitPrice * OrderQty) * (1 - UnitPriceDiscount);

        /// <summary>
        /// Override di ToString per visualizzazione user-friendly
        /// </summary>
        public override string ToString()
        {
            return $"Ordine {SalesOrderID}, Riga {SalesOrderDetailID}: {ProductName ?? $"Prodotto {ProductID}"} x {OrderQty} = {LineTotal:C}";
        }
    }
}
