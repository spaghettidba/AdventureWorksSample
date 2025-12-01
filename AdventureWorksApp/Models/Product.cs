/*
 * ============================================================================
 * MODELLO: Product
 * ============================================================================
 * 
 * Questo modello rappresenta la tabella Production.Product del database 
 * AdventureWorks. Contiene le proprietà principali di un prodotto.
 * 
 * NOTA DIDATTICA:
 * - Le proprietà corrispondono alle colonne della tabella nel database
 * - Usiamo nullable types (?) per le colonne che possono contenere NULL
 * - I nomi delle proprietà devono corrispondere ai nomi delle colonne per
 *   permettere a Dapper di mappare automaticamente i risultati
 * ============================================================================
 */

namespace AdventureWorksApp.Models
{
    /// <summary>
    /// Rappresenta un prodotto dalla tabella Production.Product.
    /// Questa classe viene usata per il mapping dei dati con Dapper.
    /// </summary>
    public class Product
    {
        // Chiave primaria della tabella
        public int ProductID { get; set; }

        // Nome del prodotto (richiesto)
        public string Name { get; set; } = string.Empty;

        // Numero identificativo del prodotto
        public string ProductNumber { get; set; } = string.Empty;

        // Indica se il prodotto può essere fabbricato internamente
        public bool MakeFlag { get; set; }

        // Indica se il prodotto è finito
        public bool FinishedGoodsFlag { get; set; }

        // Colore del prodotto (può essere NULL)
        public string? Color { get; set; }

        // Livello minimo di sicurezza dello stock
        public short SafetyStockLevel { get; set; }

        // Punto di riordino
        public short ReorderPoint { get; set; }

        // Costo standard del prodotto
        public decimal StandardCost { get; set; }

        // Prezzo di listino (può essere NULL per prodotti non venduti)
        public decimal? ListPrice { get; set; }

        // Taglia del prodotto (può essere NULL)
        public string? Size { get; set; }

        // Unità di misura della taglia
        public string? SizeUnitMeasureCode { get; set; }

        // Unità di misura del peso
        public string? WeightUnitMeasureCode { get; set; }

        // Peso del prodotto (può essere NULL)
        public decimal? Weight { get; set; }

        // Giorni necessari per la produzione
        public int DaysToManufacture { get; set; }

        // Linea di prodotto (R = Road, M = Mountain, T = Touring, S = Standard)
        public string? ProductLine { get; set; }

        // Classe del prodotto (H = High, M = Medium, L = Low)
        public string? Class { get; set; }

        // Stile del prodotto (W = Women, M = Men, U = Universal)
        public string? Style { get; set; }

        // Riferimento alla sottocategoria del prodotto
        public int? ProductSubcategoryID { get; set; }

        // Riferimento al modello del prodotto
        public int? ProductModelID { get; set; }

        // Data di inizio vendita
        public DateTime SellStartDate { get; set; }

        // Data di fine vendita (NULL se ancora in vendita)
        public DateTime? SellEndDate { get; set; }

        // Data di discontinuazione (NULL se non discontinuato)
        public DateTime? DiscontinuedDate { get; set; }

        // GUID di riga per tracking modifiche
        public Guid rowguid { get; set; }

        // Data di modifica del record
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Override di ToString per visualizzazione user-friendly
        /// </summary>
        public override string ToString()
        {
            return $"{ProductID} - {Name}";
        }
    }
}
