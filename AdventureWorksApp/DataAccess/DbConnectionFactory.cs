/*
 * ============================================================================
 * DATA ACCESS LAYER: DbConnectionFactory
 * ============================================================================
 * 
 * Questa classe gestisce la creazione delle connessioni al database.
 * Implementa il pattern Factory per centralizzare la creazione delle connessioni.
 * 
 * NOTA DIDATTICA:
 * - Centralizzando la creazione delle connessioni, è facile cambiare
 *   il tipo di database o la logica di connessione in un unico punto
 * - La stringa di connessione può essere modificata a runtime
 * - Usiamo Microsoft.Data.SqlClient per SQL Server
 * ============================================================================
 */

using Microsoft.Data.SqlClient;
using System.Data;

namespace AdventureWorksApp.DataAccess
{
    /// <summary>
    /// Factory per la creazione di connessioni al database SQL Server.
    /// Centralizza la gestione della stringa di connessione e la creazione
    /// degli oggetti connessione.
    /// </summary>
    public static class DbConnectionFactory
    {
        // Stringa di connessione al database - modificabile a runtime
        private static string _connectionString = string.Empty;

        /// <summary>
        /// Proprietà per ottenere/impostare la stringa di connessione.
        /// Può essere impostata dalla form principale dell'applicazione.
        /// </summary>
        public static string ConnectionString
        {
            get => _connectionString;
            set => _connectionString = value;
        }

        /// <summary>
        /// Verifica se la stringa di connessione è stata configurata
        /// </summary>
        public static bool IsConfigured => !string.IsNullOrWhiteSpace(_connectionString);

        /// <summary>
        /// Crea e restituisce una nuova connessione al database.
        /// 
        /// NOTA DIDATTICA:
        /// - Ogni repository dovrebbe usare questo metodo per ottenere connessioni
        /// - Le connessioni devono essere sempre chiuse dopo l'uso (using statement)
        /// - Non manteniamo connessioni aperte statiche per evitare problemi
        ///   di concorrenza e risorse
        /// </summary>
        /// <returns>Una nuova istanza di SqlConnection</returns>
        /// <exception cref="InvalidOperationException">
        /// Se la stringa di connessione non è stata configurata
        /// </exception>
        public static IDbConnection CreateConnection()
        {
            if (!IsConfigured)
            {
                throw new InvalidOperationException(
                    "La stringa di connessione non è stata configurata. " +
                    "Impostare DbConnectionFactory.ConnectionString prima di accedere ai dati.");
            }

            return new SqlConnection(_connectionString);
        }

        /// <summary>
        /// Testa la connessione al database.
        /// Utile per verificare che la stringa di connessione sia corretta.
        /// </summary>
        /// <returns>True se la connessione è riuscita, false altrimenti</returns>
        public static async Task<(bool Success, string Message)> TestConnectionAsync()
        {
            if (!IsConfigured)
            {
                return (false, "La stringa di connessione non è stata configurata.");
            }

            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                
                // Verifica che il database AdventureWorks esista eseguendo una query semplice
                using var command = new SqlCommand("SELECT DB_NAME()", connection);
                var dbName = await command.ExecuteScalarAsync();
                
                return (true, $"Connessione riuscita al database: {dbName}");
            }
            catch (SqlException ex)
            {
                return (false, $"Errore di connessione SQL: {ex.Message}");
            }
            catch (Exception ex)
            {
                return (false, $"Errore generico: {ex.Message}");
            }
        }
    }
}
