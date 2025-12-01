/*
 * ============================================================================
 * PUNTO DI INGRESSO DELL'APPLICAZIONE
 * ============================================================================
 * 
 * Questo file contiene il metodo Main che avvia l'applicazione Windows Forms.
 * Configura anche il caricamento delle impostazioni da appsettings.json.
 * 
 * NOTA DIDATTICA:
 * - STAThread è necessario per Windows Forms (Single Thread Apartment)
 * - ApplicationConfiguration.Initialize() configura DPI e font
 * - La configurazione viene caricata da appsettings.json all'avvio
 * ============================================================================
 */

using Microsoft.Extensions.Configuration;
using AdventureWorksApp.DataAccess;
using AdventureWorksApp.Forms;

namespace AdventureWorksApp;

static class Program
{
    /// <summary>
    /// Punto di ingresso principale dell'applicazione.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // Inizializza la configurazione dell'applicazione (DPI, font, etc.)
        ApplicationConfiguration.Initialize();
        
        // Carica la configurazione da appsettings.json
        LoadConfiguration();
        
        // Avvia l'applicazione con il form principale
        Application.Run(new MainForm());
    }

    /// <summary>
    /// Carica la configurazione da appsettings.json.
    /// 
    /// NOTA DIDATTICA:
    /// - IConfigurationBuilder permette di caricare configurazioni da varie fonti
    /// - AddJsonFile cerca il file nella directory dell'applicazione
    /// - optional: true significa che l'app funziona anche senza il file
    /// - reloadOnChange: true ricarica automaticamente se il file cambia
    /// </summary>
    private static void LoadConfiguration()
    {
        try
        {
            // Costruiamo il builder della configurazione
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            // Leggiamo la stringa di connessione dalla configurazione
            string? connectionString = configuration.GetConnectionString("AdventureWorks");
            
            // Se esiste e non è il valore di default, la usiamo
            if (!string.IsNullOrWhiteSpace(connectionString) && 
                !connectionString.Contains("YOUR_SERVER"))
            {
                DbConnectionFactory.ConnectionString = connectionString;
            }
        }
        catch (Exception ex)
        {
            // Se c'è un errore nella lettura del file, lo ignoriamo
            // L'utente potrà configurare la connessione manualmente
            System.Diagnostics.Debug.WriteLine($"Errore caricamento configurazione: {ex.Message}");
        }
    }
}