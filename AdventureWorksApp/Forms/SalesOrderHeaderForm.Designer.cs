/*
 * ============================================================================
 * FORM: SalesOrderHeaderForm (Designer)
 * ============================================================================
 * 
 * Form per la gestione degli ordini dalla tabella Sales.SalesOrderHeader.
 * Mostra la lista degli ordini e permette di visualizzarne i dettagli.
 * 
 * NOTA DIDATTICA:
 * - Questa form mostra le relazioni: da qui si può navigare ai dettagli ordine
 * - Alcuni campi sono readonly perché calcolati dal database
 * ============================================================================
 */

namespace AdventureWorksApp.Forms
{
    partial class SalesOrderHeaderForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            // Controlli principali
            this.splitContainer = new SplitContainer();
            this.dataGridViewOrders = new DataGridView();
            this.panelSearch = new Panel();
            this.textBoxSearch = new TextBox();
            this.buttonSearch = new Button();
            this.buttonRefresh = new Button();
            this.dateTimePickerFrom = new DateTimePicker();
            this.dateTimePickerTo = new DateTimePicker();
            this.labelFrom = new Label();
            this.labelTo = new Label();

            // Pannello dettagli
            this.groupBoxDetails = new GroupBox();
            this.labelOrderID = new Label();
            this.textBoxOrderID = new TextBox();
            this.labelOrderNumber = new Label();
            this.textBoxOrderNumber = new TextBox();
            this.labelOrderDate = new Label();
            this.dateTimePickerOrderDate = new DateTimePicker();
            this.labelDueDate = new Label();
            this.dateTimePickerDueDate = new DateTimePicker();
            this.labelShipDate = new Label();
            this.dateTimePickerShipDate = new DateTimePicker();
            this.checkBoxShipped = new CheckBox();
            this.labelStatus = new Label();
            this.comboBoxStatus = new ComboBox();
            this.labelCustomerID = new Label();
            this.textBoxCustomerID = new TextBox();
            this.labelSubTotal = new Label();
            this.textBoxSubTotal = new TextBox();
            this.labelTaxAmt = new Label();
            this.textBoxTaxAmt = new TextBox();
            this.labelFreight = new Label();
            this.textBoxFreight = new TextBox();
            this.labelTotalDue = new Label();
            this.textBoxTotalDue = new TextBox();
            this.labelComment = new Label();
            this.textBoxComment = new TextBox();
            this.checkBoxOnlineOrder = new CheckBox();
            this.buttonViewDetails = new Button();

            // Pulsanti azione
            this.panelActions = new Panel();
            this.buttonNew = new Button();
            this.buttonSave = new Button();
            this.buttonDelete = new Button();
            this.buttonCancel = new Button();
            this.labelStatusBar = new Label();

            // Sospendi layout
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOrders)).BeginInit();
            this.panelSearch.SuspendLayout();
            this.groupBoxDetails.SuspendLayout();
            this.panelActions.SuspendLayout();
            this.SuspendLayout();

            // ============================================================================
            // splitContainer
            // ============================================================================
            this.splitContainer.Dock = DockStyle.Fill;
            this.splitContainer.Orientation = Orientation.Horizontal;
            this.splitContainer.Size = new Size(1000, 700);
            this.splitContainer.SplitterDistance = 380;

            // Panel1 - Griglia
            this.splitContainer.Panel1.Controls.Add(this.dataGridViewOrders);
            this.splitContainer.Panel1.Controls.Add(this.panelSearch);

            // ============================================================================
            // panelSearch - Ricerca e filtri
            // ============================================================================
            this.panelSearch.Dock = DockStyle.Top;
            this.panelSearch.Height = 45;
            this.panelSearch.Padding = new Padding(5);

            this.textBoxSearch.Location = new Point(10, 12);
            this.textBoxSearch.Size = new Size(150, 23);
            this.textBoxSearch.PlaceholderText = "Numero ordine...";

            this.labelFrom.Text = "Da:";
            this.labelFrom.Location = new Point(170, 15);
            this.labelFrom.AutoSize = true;

            this.dateTimePickerFrom.Location = new Point(195, 11);
            this.dateTimePickerFrom.Size = new Size(120, 23);
            this.dateTimePickerFrom.Format = DateTimePickerFormat.Short;
            this.dateTimePickerFrom.Value = DateTime.Today.AddYears(-1);

            this.labelTo.Text = "A:";
            this.labelTo.Location = new Point(325, 15);
            this.labelTo.AutoSize = true;

            this.dateTimePickerTo.Location = new Point(345, 11);
            this.dateTimePickerTo.Size = new Size(120, 23);
            this.dateTimePickerTo.Format = DateTimePickerFormat.Short;

            this.buttonSearch.Location = new Point(480, 10);
            this.buttonSearch.Size = new Size(80, 25);
            this.buttonSearch.Text = "🔍 Cerca";

            this.buttonRefresh.Location = new Point(570, 10);
            this.buttonRefresh.Size = new Size(100, 25);
            this.buttonRefresh.Text = "🔄 Aggiorna";

            this.panelSearch.Controls.AddRange(new Control[] {
                this.textBoxSearch, this.labelFrom, this.dateTimePickerFrom,
                this.labelTo, this.dateTimePickerTo, this.buttonSearch, this.buttonRefresh
            });

            // ============================================================================
            // dataGridViewOrders
            // ============================================================================
            this.dataGridViewOrders.AllowUserToAddRows = false;
            this.dataGridViewOrders.AllowUserToDeleteRows = false;
            this.dataGridViewOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewOrders.Dock = DockStyle.Fill;
            this.dataGridViewOrders.MultiSelect = false;
            this.dataGridViewOrders.ReadOnly = true;
            this.dataGridViewOrders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Panel2 - Dettagli e azioni
            this.splitContainer.Panel2.Controls.Add(this.groupBoxDetails);
            this.splitContainer.Panel2.Controls.Add(this.panelActions);

            // ============================================================================
            // groupBoxDetails - Dettagli ordine
            // ============================================================================
            this.groupBoxDetails.Dock = DockStyle.Fill;
            this.groupBoxDetails.Text = "Dettagli Ordine";
            this.groupBoxDetails.Padding = new Padding(10);

            // Riga 1
            int y = 25;
            this.labelOrderID.Text = "ID Ordine:";
            this.labelOrderID.Location = new Point(15, y);
            this.labelOrderID.AutoSize = true;
            this.textBoxOrderID.Location = new Point(100, y - 3);
            this.textBoxOrderID.Size = new Size(80, 23);
            this.textBoxOrderID.ReadOnly = true;
            this.textBoxOrderID.BackColor = Color.LightGray;

            this.labelOrderNumber.Text = "Numero:";
            this.labelOrderNumber.Location = new Point(200, y);
            this.labelOrderNumber.AutoSize = true;
            this.textBoxOrderNumber.Location = new Point(260, y - 3);
            this.textBoxOrderNumber.Size = new Size(120, 23);
            this.textBoxOrderNumber.ReadOnly = true;
            this.textBoxOrderNumber.BackColor = Color.LightGray;

            this.labelCustomerID.Text = "Cliente ID:";
            this.labelCustomerID.Location = new Point(400, y);
            this.labelCustomerID.AutoSize = true;
            this.textBoxCustomerID.Location = new Point(470, y - 3);
            this.textBoxCustomerID.Size = new Size(80, 23);

            this.checkBoxOnlineOrder.Text = "Ordine Online";
            this.checkBoxOnlineOrder.Location = new Point(570, y - 3);
            this.checkBoxOnlineOrder.AutoSize = true;

            // Riga 2 - Date
            y = 55;
            this.labelOrderDate.Text = "Data Ordine:";
            this.labelOrderDate.Location = new Point(15, y);
            this.labelOrderDate.AutoSize = true;
            this.dateTimePickerOrderDate.Location = new Point(100, y - 3);
            this.dateTimePickerOrderDate.Size = new Size(130, 23);
            this.dateTimePickerOrderDate.Format = DateTimePickerFormat.Short;

            this.labelDueDate.Text = "Data Consegna:";
            this.labelDueDate.Location = new Point(250, y);
            this.labelDueDate.AutoSize = true;
            this.dateTimePickerDueDate.Location = new Point(350, y - 3);
            this.dateTimePickerDueDate.Size = new Size(130, 23);
            this.dateTimePickerDueDate.Format = DateTimePickerFormat.Short;

            this.checkBoxShipped.Text = "Spedito";
            this.checkBoxShipped.Location = new Point(500, y - 3);
            this.checkBoxShipped.AutoSize = true;

            this.labelShipDate.Text = "Data Spedizione:";
            this.labelShipDate.Location = new Point(580, y);
            this.labelShipDate.AutoSize = true;
            this.dateTimePickerShipDate.Location = new Point(690, y - 3);
            this.dateTimePickerShipDate.Size = new Size(130, 23);
            this.dateTimePickerShipDate.Format = DateTimePickerFormat.Short;
            this.dateTimePickerShipDate.Enabled = false;

            // Riga 3 - Stato
            y = 85;
            this.labelStatus.Text = "Stato:";
            this.labelStatus.Location = new Point(15, y);
            this.labelStatus.AutoSize = true;
            this.comboBoxStatus.Location = new Point(100, y - 3);
            this.comboBoxStatus.Size = new Size(150, 23);
            this.comboBoxStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxStatus.Items.AddRange(new object[] {
                "1 - In elaborazione",
                "2 - Approvato",
                "3 - In attesa merce",
                "4 - Rifiutato",
                "5 - Spedito",
                "6 - Annullato"
            });

            // Riga 4 - Importi
            y = 120;
            this.labelSubTotal.Text = "Subtotale:";
            this.labelSubTotal.Location = new Point(15, y);
            this.labelSubTotal.AutoSize = true;
            this.textBoxSubTotal.Location = new Point(100, y - 3);
            this.textBoxSubTotal.Size = new Size(100, 23);
            this.textBoxSubTotal.ReadOnly = true;
            this.textBoxSubTotal.BackColor = Color.LightYellow;

            this.labelTaxAmt.Text = "Tasse:";
            this.labelTaxAmt.Location = new Point(220, y);
            this.labelTaxAmt.AutoSize = true;
            this.textBoxTaxAmt.Location = new Point(270, y - 3);
            this.textBoxTaxAmt.Size = new Size(100, 23);
            this.textBoxTaxAmt.ReadOnly = true;
            this.textBoxTaxAmt.BackColor = Color.LightYellow;

            this.labelFreight.Text = "Spedizione:";
            this.labelFreight.Location = new Point(390, y);
            this.labelFreight.AutoSize = true;
            this.textBoxFreight.Location = new Point(460, y - 3);
            this.textBoxFreight.Size = new Size(100, 23);
            this.textBoxFreight.ReadOnly = true;
            this.textBoxFreight.BackColor = Color.LightYellow;

            this.labelTotalDue.Text = "TOTALE:";
            this.labelTotalDue.Location = new Point(580, y);
            this.labelTotalDue.AutoSize = true;
            this.labelTotalDue.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.textBoxTotalDue.Location = new Point(650, y - 3);
            this.textBoxTotalDue.Size = new Size(120, 23);
            this.textBoxTotalDue.ReadOnly = true;
            this.textBoxTotalDue.BackColor = Color.LightGreen;
            this.textBoxTotalDue.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            // Riga 5 - Commento
            y = 155;
            this.labelComment.Text = "Commento:";
            this.labelComment.Location = new Point(15, y);
            this.labelComment.AutoSize = true;
            this.textBoxComment.Location = new Point(100, y - 3);
            this.textBoxComment.Size = new Size(500, 23);

            // Pulsante dettagli
            this.buttonViewDetails.Location = new Point(620, y - 5);
            this.buttonViewDetails.Size = new Size(150, 28);
            this.buttonViewDetails.Text = "📋 Vedi Dettagli Ordine";

            // Aggiungi controlli al groupbox
            this.groupBoxDetails.Controls.AddRange(new Control[] {
                this.labelOrderID, this.textBoxOrderID,
                this.labelOrderNumber, this.textBoxOrderNumber,
                this.labelCustomerID, this.textBoxCustomerID,
                this.checkBoxOnlineOrder,
                this.labelOrderDate, this.dateTimePickerOrderDate,
                this.labelDueDate, this.dateTimePickerDueDate,
                this.checkBoxShipped, this.labelShipDate, this.dateTimePickerShipDate,
                this.labelStatus, this.comboBoxStatus,
                this.labelSubTotal, this.textBoxSubTotal,
                this.labelTaxAmt, this.textBoxTaxAmt,
                this.labelFreight, this.textBoxFreight,
                this.labelTotalDue, this.textBoxTotalDue,
                this.labelComment, this.textBoxComment,
                this.buttonViewDetails
            });

            // ============================================================================
            // panelActions
            // ============================================================================
            this.panelActions.Dock = DockStyle.Bottom;
            this.panelActions.Height = 50;

            this.buttonNew.Location = new Point(10, 12);
            this.buttonNew.Size = new Size(100, 28);
            this.buttonNew.Text = "➕ Nuovo";

            this.buttonSave.Location = new Point(120, 12);
            this.buttonSave.Size = new Size(100, 28);
            this.buttonSave.Text = "💾 Salva";

            this.buttonDelete.Location = new Point(230, 12);
            this.buttonDelete.Size = new Size(100, 28);
            this.buttonDelete.Text = "🗑️ Elimina";

            this.buttonCancel.Location = new Point(340, 12);
            this.buttonCancel.Size = new Size(100, 28);
            this.buttonCancel.Text = "❌ Annulla";

            this.labelStatusBar.Location = new Point(460, 17);
            this.labelStatusBar.Size = new Size(500, 20);
            this.labelStatusBar.ForeColor = Color.Blue;

            this.panelActions.Controls.AddRange(new Control[] {
                this.buttonNew, this.buttonSave, this.buttonDelete,
                this.buttonCancel, this.labelStatusBar
            });

            // ============================================================================
            // SalesOrderHeaderForm
            // ============================================================================
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1000, 700);
            this.Controls.Add(this.splitContainer);
            this.Name = "SalesOrderHeaderForm";
            this.Text = "Gestione Ordini - Sales.SalesOrderHeader";
            this.StartPosition = FormStartPosition.CenterParent;

            // Riprendi layout
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOrders)).EndInit();
            this.panelSearch.ResumeLayout(false);
            this.panelSearch.PerformLayout();
            this.groupBoxDetails.ResumeLayout(false);
            this.groupBoxDetails.PerformLayout();
            this.panelActions.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        // Controlli principali
        private SplitContainer splitContainer;
        private DataGridView dataGridViewOrders;
        private Panel panelSearch;
        private TextBox textBoxSearch;
        private Button buttonSearch;
        private Button buttonRefresh;
        private DateTimePicker dateTimePickerFrom;
        private DateTimePicker dateTimePickerTo;
        private Label labelFrom;
        private Label labelTo;

        // Controlli dettagli
        private GroupBox groupBoxDetails;
        private Label labelOrderID;
        private TextBox textBoxOrderID;
        private Label labelOrderNumber;
        private TextBox textBoxOrderNumber;
        private Label labelOrderDate;
        private DateTimePicker dateTimePickerOrderDate;
        private Label labelDueDate;
        private DateTimePicker dateTimePickerDueDate;
        private Label labelShipDate;
        private DateTimePicker dateTimePickerShipDate;
        private CheckBox checkBoxShipped;
        private Label labelStatus;
        private ComboBox comboBoxStatus;
        private Label labelCustomerID;
        private TextBox textBoxCustomerID;
        private Label labelSubTotal;
        private TextBox textBoxSubTotal;
        private Label labelTaxAmt;
        private TextBox textBoxTaxAmt;
        private Label labelFreight;
        private TextBox textBoxFreight;
        private Label labelTotalDue;
        private TextBox textBoxTotalDue;
        private Label labelComment;
        private TextBox textBoxComment;
        private CheckBox checkBoxOnlineOrder;
        private Button buttonViewDetails;

        // Controlli azione
        private Panel panelActions;
        private Button buttonNew;
        private Button buttonSave;
        private Button buttonDelete;
        private Button buttonCancel;
        private Label labelStatusBar;
    }
}
