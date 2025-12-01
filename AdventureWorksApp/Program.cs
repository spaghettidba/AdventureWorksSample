/*
 * ============================================================================
 * APPLICATION ENTRY POINT
 * ============================================================================
 * 
 * This file contains the Main method that starts the Windows Forms application.
 * It also configures the loading of settings from appsettings.json.
 * 
 * TEACHING NOTE:
 * - STAThread is required for Windows Forms (Single Thread Apartment)
 * - ApplicationConfiguration.Initialize() configures DPI and fonts
 * - Configuration is loaded from appsettings.json at startup
 * ============================================================================
 */

using Microsoft.Extensions.Configuration;
using AdventureWorksApp.DataAccess;
using AdventureWorksApp.Forms;

namespace AdventureWorksApp;

static class Program
{
    /// <summary>
    /// Main entry point of the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // Initialize application configuration (DPI, fonts, etc.)
        ApplicationConfiguration.Initialize();
        
        // Load configuration from appsettings.json
        LoadConfiguration();
        
        // Start the application with the main form
        Application.Run(new MainForm());
    }

    /// <summary>
    /// Loads configuration from appsettings.json.
    /// 
    /// TEACHING NOTE:
    /// - IConfigurationBuilder allows loading configurations from various sources
    /// - AddJsonFile looks for the file in the application directory
    /// - optional: true means the app works even without the file
    /// - reloadOnChange: true automatically reloads if the file changes
    /// </summary>
    private static void LoadConfiguration()
    {
        try
        {
            // Build the configuration builder
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            // Read the connection string from configuration
            string? connectionString = configuration.GetConnectionString("AdventureWorks");
            
            // If it exists and is not the default value, use it
            if (!string.IsNullOrWhiteSpace(connectionString) && 
                !connectionString.Contains("YOUR_SERVER"))
            {
                DbConnectionFactory.ConnectionString = connectionString;
            }
        }
        catch (Exception ex)
        {
            // If there's an error reading the file, ignore it
            // The user can configure the connection manually
            System.Diagnostics.Debug.WriteLine($"Error loading configuration: {ex.Message}");
        }
    }
}