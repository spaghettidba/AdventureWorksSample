/*
 * ============================================================================
 * FORM: MainForm (Logica)
 * ============================================================================
 * 
 * Form principale dell'applicazione AdventureWorks.
 * Permette di:
 * - Configurare la stringa di connessione al database
 * - Testare la connessione
 * - Navigare verso le form di gestione dati
 * 
 * NOTA DIDATTICA:
 * - Questo è il pattern tipico di Windows Forms: separazione tra
 *   design (*.Designer.cs) e logica (*.cs)
 * - La classe è partial, condivisa tra i due file
 * - Gli event handler collegano eventi UI alle azioni
 * ============================================================================
 */

using AdventureWorksApp.DataAccess;

namespace AdventureWorksApp.Forms
{
    /// <summary>
    /// Form principale dell'applicazione.
    /// Gestisce la connessione al database e la navigazione.
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// Costruttore del form principale.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            
            // Collegamento degli event handler
            // NOTA: In un progetto reale, questi vengono generati dal designer
            this.Load += MainForm_Load;
            this.buttonTestConnection.Click += ButtonTestConnection_Click;
            this.buttonProducts.Click += ButtonProducts_Click;
            this.buttonSalesOrders.Click += ButtonSalesOrders_Click;
            this.buttonOrderDetails.Click += ButtonOrderDetails_Click;
        }

        /// <summary>
        /// Evento caricamento del form.
        /// Inizializza la UI con i valori correnti.
        /// </summary>
        private void MainForm_Load(object? sender, EventArgs e)
        {
            // Se la stringa di connessione è già configurata (da appsettings.json),
            // la mostriamo nella textbox
            if (DbConnectionFactory.IsConfigured)
            {
                textBoxConnectionString.Text = DbConnectionFactory.ConnectionString;
                UpdateConnectionStatus("Stringa caricata da configurazione", Color.Blue);
            }
            
            // Abilitiamo i pulsanti di navigazione solo se connessi
            UpdateNavigationButtons(DbConnectionFactory.IsConfigured);
        }

        /// <summary>
        /// Testa la connessione al database.
        /// 
        /// NOTA DIDATTICA:
        /// - Usiamo async/await per non bloccare l'UI durante il test
        /// - Il metodo è async void perché è un event handler
        /// - Disabilitiamo il pulsante durante il test per evitare click multipli
        /// </summary>
        private async void ButtonTestConnection_Click(object? sender, EventArgs e)
        {
            // Ottieni la stringa di connessione dalla textbox
            string connectionString = textBoxConnectionString.Text.Trim();
            
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                UpdateConnectionStatus("Inserisci una stringa di connessione valida", Color.Red);
                return;
            }

            // Imposta la stringa di connessione
            DbConnectionFactory.ConnectionString = connectionString;

            // Disabilita il pulsante durante il test
            buttonTestConnection.Enabled = false;
            UpdateConnectionStatus("Test in corso...", Color.Orange);

            try
            {
                // Esegui il test asincrono
                var (success, message) = await DbConnectionFactory.TestConnectionAsync();

                if (success)
                {
                    UpdateConnectionStatus($"✓ {message}", Color.Green);
                    UpdateNavigationButtons(true);
                }
                else
                {
                    UpdateConnectionStatus($"✗ {message}", Color.Red);
                    UpdateNavigationButtons(false);
                }
            }
            catch (Exception ex)
            {
                UpdateConnectionStatus($"Errore: {ex.Message}", Color.Red);
                UpdateNavigationButtons(false);
            }
            finally
            {
                // Riabilita il pulsante
                buttonTestConnection.Enabled = true;
            }
        }

        /// <summary>
        /// Aggiorna lo stato della connessione visualizzato.
        /// </summary>
        private void UpdateConnectionStatus(string message, Color color)
        {
            labelConnectionStatus.Text = $"Stato: {message}";
            labelConnectionStatus.ForeColor = color;
        }

        /// <summary>
        /// Abilita o disabilita i pulsanti di navigazione.
        /// </summary>
        private void UpdateNavigationButtons(bool enabled)
        {
            buttonProducts.Enabled = enabled;
            buttonSalesOrders.Enabled = enabled;
            buttonOrderDetails.Enabled = enabled;
        }

        /// <summary>
        /// Apre la form di gestione prodotti.
        /// 
        /// NOTA DIDATTICA:
        /// - ShowDialog() apre la form in modo modale (blocca il form padre)
        /// - Show() aprirebbe la form in modo non modale
        /// - Per un'app didattica, modale è più semplice da gestire
        /// </summary>
        private void ButtonProducts_Click(object? sender, EventArgs e)
        {
            using var productForm = new ProductForm();
            productForm.ShowDialog(this);
        }

        /// <summary>
        /// Apre la form di gestione ordini.
        /// </summary>
        private void ButtonSalesOrders_Click(object? sender, EventArgs e)
        {
            using var ordersForm = new SalesOrderHeaderForm();
            ordersForm.ShowDialog(this);
        }

        /// <summary>
        /// Apre la form di gestione dettagli ordine.
        /// </summary>
        private void ButtonOrderDetails_Click(object? sender, EventArgs e)
        {
            using var detailsForm = new SalesOrderDetailForm();
            detailsForm.ShowDialog(this);
        }
    }
}
