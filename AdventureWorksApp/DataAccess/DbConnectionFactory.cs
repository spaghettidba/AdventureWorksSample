/*
 * ============================================================================
 * DATA ACCESS LAYER: DbConnectionFactory
 * ============================================================================
 * 
 * This class manages the creation of database connections.
 * It implements the Factory pattern to centralize connection creation.
 * 
 * TEACHING NOTE:
 * - By centralizing connection creation, it's easy to change
 *   the database type or connection logic in a single place
 * - The connection string can be modified at runtime
 * - We use Microsoft.Data.SqlClient for SQL Server
 * ============================================================================
 */

using Microsoft.Data.SqlClient;
using System.Data;

namespace AdventureWorksApp.DataAccess
{
    /// <summary>
    /// Factory for creating SQL Server database connections.
    /// Centralizes connection string management and the creation
    /// of connection objects.
    /// </summary>
    public static class DbConnectionFactory
    {
        // Database connection string - modifiable at runtime
        private static string _connectionString = string.Empty;

        /// <summary>
        /// Property to get/set the connection string.
        /// Can be set from the application's main form.
        /// </summary>
        public static string ConnectionString
        {
            get => _connectionString;
            set => _connectionString = value;
        }

        /// <summary>
        /// Checks if the connection string has been configured
        /// </summary>
        public static bool IsConfigured => !string.IsNullOrWhiteSpace(_connectionString);

        /// <summary>
        /// Creates and returns a new database connection.
        /// 
        /// TEACHING NOTE:
        /// - Each repository should use this method to obtain connections
        /// - Connections must always be closed after use (using statement)
        /// - We don't maintain static open connections to avoid concurrency
        ///   and resource issues
        /// </summary>
        /// <returns>A new SqlConnection instance</returns>
        /// <exception cref="InvalidOperationException">
        /// If the connection string has not been configured
        /// </exception>
        public static IDbConnection CreateConnection()
        {
            if (!IsConfigured)
            {
                throw new InvalidOperationException(
                    "The connection string has not been configured. " +
                    "Set DbConnectionFactory.ConnectionString before accessing data.");
            }

            return new SqlConnection(_connectionString);
        }

        /// <summary>
        /// Tests the database connection.
        /// Useful for verifying that the connection string is correct.
        /// </summary>
        /// <returns>True if the connection was successful, false otherwise</returns>
        public static async Task<(bool Success, string Message)> TestConnectionAsync()
        {
            if (!IsConfigured)
            {
                return (false, "The connection string has not been configured.");
            }

            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                
                // Verify that the AdventureWorks database exists by running a simple query
                using var command = new SqlCommand("SELECT DB_NAME()", connection);
                var dbName = await command.ExecuteScalarAsync();
                
                return (true, $"Successfully connected to database: {dbName}");
            }
            catch (SqlException ex)
            {
                return (false, $"SQL connection error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return (false, $"General error: {ex.Message}");
            }
        }
    }
}
