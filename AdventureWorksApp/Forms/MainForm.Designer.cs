/*
 * ============================================================================
 * FORM: MainForm (Designer)
 * ============================================================================
 * 
 * Questo file contiene il codice generato dal designer per il form principale.
 * 
 * NOTA DIDATTICA:
 * - In un progetto reale, questo codice viene generato automaticamente
 *   dall'editor visuale di Visual Studio
 * - Qui lo scriviamo manualmente per scopi didattici
 * - InitializeComponent() configura tutti i controlli del form
 * - I controlli sono dichiarati come campi privati della partial class
 * ============================================================================
 */

namespace AdventureWorksApp.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Metodo richiesto per il supporto del designer.
        /// Non modificare il contenuto con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            // Creazione dei controlli
            this.groupBoxConnection = new GroupBox();
            this.labelConnectionStatus = new Label();
            this.buttonTestConnection = new Button();
            this.textBoxConnectionString = new TextBox();
            this.labelConnectionString = new Label();
            this.groupBoxNavigation = new GroupBox();
            this.buttonProducts = new Button();
            this.buttonSalesOrders = new Button();
            this.buttonOrderDetails = new Button();
            this.labelWelcome = new Label();
            this.labelInstructions = new Label();

            // Sospendi layout per performance
            this.groupBoxConnection.SuspendLayout();
            this.groupBoxNavigation.SuspendLayout();
            this.SuspendLayout();

            // ============================================================================
            // groupBoxConnection - Gruppo per la configurazione della connessione
            // ============================================================================
            this.groupBoxConnection.Controls.Add(this.labelConnectionStatus);
            this.groupBoxConnection.Controls.Add(this.buttonTestConnection);
            this.groupBoxConnection.Controls.Add(this.textBoxConnectionString);
            this.groupBoxConnection.Controls.Add(this.labelConnectionString);
            this.groupBoxConnection.Location = new Point(12, 80);
            this.groupBoxConnection.Name = "groupBoxConnection";
            this.groupBoxConnection.Size = new Size(760, 120);
            this.groupBoxConnection.TabIndex = 0;
            this.groupBoxConnection.TabStop = false;
            this.groupBoxConnection.Text = "Configurazione Connessione Database";

            // ============================================================================
            // labelConnectionString - Etichetta per la textbox
            // ============================================================================
            this.labelConnectionString.AutoSize = true;
            this.labelConnectionString.Location = new Point(15, 30);
            this.labelConnectionString.Name = "labelConnectionString";
            this.labelConnectionString.Size = new Size(130, 15);
            this.labelConnectionString.TabIndex = 0;
            this.labelConnectionString.Text = "Stringa di connessione:";

            // ============================================================================
            // textBoxConnectionString - Campo per inserire la stringa di connessione
            // ============================================================================
            this.textBoxConnectionString.Location = new Point(15, 50);
            this.textBoxConnectionString.Name = "textBoxConnectionString";
            this.textBoxConnectionString.Size = new Size(630, 23);
            this.textBoxConnectionString.TabIndex = 1;
            this.textBoxConnectionString.PlaceholderText = "Server=localhost;Database=AdventureWorks2019;Integrated Security=True;TrustServerCertificate=True";

            // ============================================================================
            // buttonTestConnection - Pulsante per testare la connessione
            // ============================================================================
            this.buttonTestConnection.Location = new Point(655, 49);
            this.buttonTestConnection.Name = "buttonTestConnection";
            this.buttonTestConnection.Size = new Size(90, 25);
            this.buttonTestConnection.TabIndex = 2;
            this.buttonTestConnection.Text = "Testa";
            this.buttonTestConnection.UseVisualStyleBackColor = true;

            // ============================================================================
            // labelConnectionStatus - Mostra lo stato della connessione
            // ============================================================================
            this.labelConnectionStatus.AutoSize = true;
            this.labelConnectionStatus.Location = new Point(15, 85);
            this.labelConnectionStatus.Name = "labelConnectionStatus";
            this.labelConnectionStatus.Size = new Size(200, 15);
            this.labelConnectionStatus.TabIndex = 3;
            this.labelConnectionStatus.Text = "Stato: Non connesso";
            this.labelConnectionStatus.ForeColor = Color.Gray;

            // ============================================================================
            // groupBoxNavigation - Gruppo per la navigazione tra le form
            // ============================================================================
            this.groupBoxNavigation.Controls.Add(this.buttonProducts);
            this.groupBoxNavigation.Controls.Add(this.buttonSalesOrders);
            this.groupBoxNavigation.Controls.Add(this.buttonOrderDetails);
            this.groupBoxNavigation.Location = new Point(12, 220);
            this.groupBoxNavigation.Name = "groupBoxNavigation";
            this.groupBoxNavigation.Size = new Size(760, 180);
            this.groupBoxNavigation.TabIndex = 1;
            this.groupBoxNavigation.TabStop = false;
            this.groupBoxNavigation.Text = "Gestione Dati";

            // ============================================================================
            // buttonProducts - Apre la form di gestione prodotti
            // ============================================================================
            this.buttonProducts.Location = new Point(15, 35);
            this.buttonProducts.Name = "buttonProducts";
            this.buttonProducts.Size = new Size(230, 45);
            this.buttonProducts.TabIndex = 0;
            this.buttonProducts.Text = "📦 Gestione Prodotti\n(Production.Product)";
            this.buttonProducts.UseVisualStyleBackColor = true;

            // ============================================================================
            // buttonSalesOrders - Apre la form di gestione ordini
            // ============================================================================
            this.buttonSalesOrders.Location = new Point(265, 35);
            this.buttonSalesOrders.Name = "buttonSalesOrders";
            this.buttonSalesOrders.Size = new Size(230, 45);
            this.buttonSalesOrders.TabIndex = 1;
            this.buttonSalesOrders.Text = "📋 Gestione Ordini\n(Sales.SalesOrderHeader)";
            this.buttonSalesOrders.UseVisualStyleBackColor = true;

            // ============================================================================
            // buttonOrderDetails - Apre la form di gestione dettagli ordine
            // ============================================================================
            this.buttonOrderDetails.Location = new Point(515, 35);
            this.buttonOrderDetails.Name = "buttonOrderDetails";
            this.buttonOrderDetails.Size = new Size(230, 45);
            this.buttonOrderDetails.TabIndex = 2;
            this.buttonOrderDetails.Text = "📝 Dettagli Ordini\n(Sales.SalesOrderDetail)";
            this.buttonOrderDetails.UseVisualStyleBackColor = true;

            // ============================================================================
            // labelInstructions - Istruzioni per l'uso
            // ============================================================================
            this.labelInstructions.Location = new Point(15, 100);
            this.labelInstructions.Name = "labelInstructions";
            this.labelInstructions.Size = new Size(730, 70);
            this.labelInstructions.TabIndex = 3;
            this.labelInstructions.Text = @"ISTRUZIONI:
1. Inserisci la stringa di connessione al database AdventureWorks e clicca 'Testa'
2. Una volta connesso, usa i pulsanti sopra per gestire i dati delle varie tabelle
3. Ogni form permette di visualizzare, inserire, modificare ed eliminare i record";
            this.labelInstructions.ForeColor = Color.DarkBlue;

            // ============================================================================
            // labelWelcome - Titolo di benvenuto
            // ============================================================================
            this.labelWelcome.AutoSize = true;
            this.labelWelcome.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
            this.labelWelcome.Location = new Point(12, 20);
            this.labelWelcome.Name = "labelWelcome";
            this.labelWelcome.Size = new Size(500, 32);
            this.labelWelcome.TabIndex = 2;
            this.labelWelcome.Text = "AdventureWorks - Applicazione Didattica";
            this.labelWelcome.ForeColor = Color.DarkBlue;

            // ============================================================================
            // MainForm - Form principale
            // ============================================================================
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(784, 420);
            this.Controls.Add(this.labelWelcome);
            this.Controls.Add(this.groupBoxConnection);
            this.Controls.Add(this.groupBoxNavigation);
            this.Controls.Add(this.labelInstructions);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "AdventureWorks - Menu Principale";

            // Riprendi layout
            this.groupBoxConnection.ResumeLayout(false);
            this.groupBoxConnection.PerformLayout();
            this.groupBoxNavigation.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        // Dichiarazione dei controlli
        private GroupBox groupBoxConnection;
        private Label labelConnectionString;
        private TextBox textBoxConnectionString;
        private Button buttonTestConnection;
        private Label labelConnectionStatus;
        private GroupBox groupBoxNavigation;
        private Button buttonProducts;
        private Button buttonSalesOrders;
        private Button buttonOrderDetails;
        private Label labelWelcome;
        private Label labelInstructions;
    }
}
