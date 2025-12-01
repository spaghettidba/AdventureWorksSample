/*
 * ============================================================================
 * MODELLO: SalesOrderHeader
 * ============================================================================
 * 
 * Questo modello rappresenta la tabella Sales.SalesOrderHeader del database 
 * AdventureWorks. Contiene le informazioni principali di un ordine di vendita.
 * 
 * NOTA DIDATTICA:
 * - Questa tabella ha una relazione 1:N con SalesOrderDetail
 * - Ogni ordine può avere più righe di dettaglio (prodotti ordinati)
 * - La chiave primaria è SalesOrderID
 * - Contiene riferimenti a Customer, SalesPerson, Territory, ecc.
 * ============================================================================
 */

namespace AdventureWorksApp.Models
{
    /// <summary>
    /// Rappresenta l'intestazione di un ordine di vendita dalla tabella 
    /// Sales.SalesOrderHeader. Contiene dati generali dell'ordine come 
    /// cliente, date, totali, ecc.
    /// </summary>
    public class SalesOrderHeader
    {
        // Chiave primaria dell'ordine
        public int SalesOrderID { get; set; }

        // Numero di revisione dell'ordine
        public byte RevisionNumber { get; set; }

        // Data dell'ordine
        public DateTime OrderDate { get; set; }

        // Data di consegna richiesta
        public DateTime DueDate { get; set; }

        // Data di spedizione effettiva (NULL se non ancora spedito)
        public DateTime? ShipDate { get; set; }

        // Stato dell'ordine: 1=In process, 2=Approved, 3=Backordered, 
        // 4=Rejected, 5=Shipped, 6=Cancelled
        public byte Status { get; set; }

        // Flag per ordine online (true = ordine web)
        public bool OnlineOrderFlag { get; set; }

        // Numero ordine per visualizzazione (formato: SO + numero)
        public string SalesOrderNumber { get; set; } = string.Empty;

        // Numero ordine di acquisto del cliente
        public string? PurchaseOrderNumber { get; set; }

        // Numero di conto del cliente
        public string? AccountNumber { get; set; }

        // ID del cliente
        public int CustomerID { get; set; }

        // ID del venditore (può essere NULL per ordini online)
        public int? SalesPersonID { get; set; }

        // ID del territorio di vendita
        public int? TerritoryID { get; set; }

        // ID dell'indirizzo di fatturazione
        public int BillToAddressID { get; set; }

        // ID dell'indirizzo di spedizione
        public int ShipToAddressID { get; set; }

        // ID del metodo di spedizione
        public int ShipMethodID { get; set; }

        // ID della carta di credito (NULL se altro metodo)
        public int? CreditCardID { get; set; }

        // Numero di approvazione carta di credito
        public string? CreditCardApprovalCode { get; set; }

        // ID della valuta
        public int? CurrencyRateID { get; set; }

        // Subtotale dell'ordine (somma dei dettagli)
        public decimal SubTotal { get; set; }

        // Importo tasse
        public decimal TaxAmt { get; set; }

        // Costo spedizione
        public decimal Freight { get; set; }

        // Totale ordine (SubTotal + TaxAmt + Freight)
        public decimal TotalDue { get; set; }

        // Commento sull'ordine
        public string? Comment { get; set; }

        // GUID di riga per tracking modifiche
        public Guid rowguid { get; set; }

        // Data di modifica del record
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Proprietà calcolata per visualizzare lo stato in modo leggibile
        /// </summary>
        public string StatusDescription => Status switch
        {
            1 => "In elaborazione",
            2 => "Approvato",
            3 => "In attesa merce",
            4 => "Rifiutato",
            5 => "Spedito",
            6 => "Annullato",
            _ => "Sconosciuto"
        };

        /// <summary>
        /// Override di ToString per visualizzazione user-friendly
        /// </summary>
        public override string ToString()
        {
            return $"{SalesOrderNumber} - {OrderDate:dd/MM/yyyy} - {TotalDue:C}";
        }
    }
}
