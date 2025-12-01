/*
 * ============================================================================
 * FORM: SalesOrderHeaderForm (Logica)
 * ============================================================================
 * 
 * Form per la gestione degli ordini dalla tabella Sales.SalesOrderHeader.
 * Permette visualizzazione, modifica e navigazione verso i dettagli ordine.
 * 
 * NOTA DIDATTICA:
 * - Gli ordini hanno molti campi calcolati (TotalDue, SubTotal)
 * - Alcuni campi sono readonly perché gestiti dal database
 * - La navigazione verso SalesOrderDetail mostra le relazioni tra tabelle
 * ============================================================================
 */

using AdventureWorksApp.DataAccess;
using AdventureWorksApp.Models;

namespace AdventureWorksApp.Forms
{
    /// <summary>
    /// Form per la gestione CRUD della tabella Sales.SalesOrderHeader.
    /// </summary>
    public partial class SalesOrderHeaderForm : Form
    {
        private readonly SalesOrderHeaderRepository _repository;
        private SalesOrderHeader? _currentOrder;
        private bool _isNewOrder;

        public SalesOrderHeaderForm()
        {
            InitializeComponent();
            _repository = new SalesOrderHeaderRepository();

            // Event handlers
            this.Load += Form_Load;
            this.dataGridViewOrders.SelectionChanged += DataGridView_SelectionChanged;
            this.buttonSearch.Click += ButtonSearch_Click;
            this.buttonRefresh.Click += ButtonRefresh_Click;
            this.buttonNew.Click += ButtonNew_Click;
            this.buttonSave.Click += ButtonSave_Click;
            this.buttonDelete.Click += ButtonDelete_Click;
            this.buttonCancel.Click += ButtonCancel_Click;
            this.buttonViewDetails.Click += ButtonViewDetails_Click;
            this.checkBoxShipped.CheckedChanged += CheckBoxShipped_CheckedChanged;
            
            this.textBoxSearch.KeyPress += (s, e) =>
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    ButtonSearch_Click(s, e);
                    e.Handled = true;
                }
            };
        }

        private async void Form_Load(object? sender, EventArgs e)
        {
            await LoadOrdersAsync();
            SetViewMode();
        }

        /// <summary>
        /// Carica gli ordini nella griglia.
        /// </summary>
        private async Task LoadOrdersAsync()
        {
            try
            {
                UpdateStatus("Caricamento ordini...", Color.Blue);
                var orders = await _repository.GetAllAsync();
                dataGridViewOrders.DataSource = orders.ToList();
                ConfigureGridColumns();
                UpdateStatus($"Caricati {orders.Count()} ordini", Color.Green);
            }
            catch (Exception ex)
            {
                UpdateStatus($"Errore: {ex.Message}", Color.Red);
                MessageBox.Show($"Errore nel caricamento:\n{ex.Message}",
                    "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigureGridColumns()
        {
            if (dataGridViewOrders.Columns.Count == 0) return;

            // Nascondi colonne tecniche
            var hidden = new[] { "rowguid", "ModifiedDate", "RevisionNumber", 
                "BillToAddressID", "ShipToAddressID", "ShipMethodID", "CreditCardID",
                "CreditCardApprovalCode", "CurrencyRateID", "SalesPersonID", "TerritoryID",
                "PurchaseOrderNumber", "AccountNumber" };

            foreach (var col in hidden)
                if (dataGridViewOrders.Columns.Contains(col))
                    dataGridViewOrders.Columns[col].Visible = false;

            // Rinomina
            SetHeader("SalesOrderID", "ID");
            SetHeader("SalesOrderNumber", "Numero Ordine");
            SetHeader("OrderDate", "Data Ordine");
            SetHeader("DueDate", "Data Consegna");
            SetHeader("ShipDate", "Data Spedizione");
            SetHeader("Status", "Stato");
            SetHeader("OnlineOrderFlag", "Online");
            SetHeader("CustomerID", "Cliente");
            SetHeader("SubTotal", "Subtotale");
            SetHeader("TaxAmt", "Tasse");
            SetHeader("Freight", "Spedizione");
            SetHeader("TotalDue", "Totale");
            SetHeader("Comment", "Commento");
        }

        private void SetHeader(string name, string header)
        {
            if (dataGridViewOrders.Columns.Contains(name))
                dataGridViewOrders.Columns[name].HeaderText = header;
        }

        private void DataGridView_SelectionChanged(object? sender, EventArgs e)
        {
            if (_isNewOrder) return;

            if (dataGridViewOrders.SelectedRows.Count > 0)
            {
                _currentOrder = dataGridViewOrders.SelectedRows[0].DataBoundItem as SalesOrderHeader;
                if (_currentOrder != null)
                {
                    PopulateDetails(_currentOrder);
                    SetEditMode();
                }
            }
        }

        private void PopulateDetails(SalesOrderHeader order)
        {
            textBoxOrderID.Text = order.SalesOrderID.ToString();
            textBoxOrderNumber.Text = order.SalesOrderNumber;
            textBoxCustomerID.Text = order.CustomerID.ToString();
            checkBoxOnlineOrder.Checked = order.OnlineOrderFlag;
            dateTimePickerOrderDate.Value = order.OrderDate;
            dateTimePickerDueDate.Value = order.DueDate;
            
            checkBoxShipped.Checked = order.ShipDate.HasValue;
            if (order.ShipDate.HasValue)
            {
                dateTimePickerShipDate.Value = order.ShipDate.Value;
                dateTimePickerShipDate.Enabled = true;
            }
            else
            {
                dateTimePickerShipDate.Value = DateTime.Today;
                dateTimePickerShipDate.Enabled = false;
            }

            // Stato (0-based index)
            if (order.Status >= 1 && order.Status <= 6)
                comboBoxStatus.SelectedIndex = order.Status - 1;

            textBoxSubTotal.Text = order.SubTotal.ToString("C");
            textBoxTaxAmt.Text = order.TaxAmt.ToString("C");
            textBoxFreight.Text = order.Freight.ToString("C");
            textBoxTotalDue.Text = order.TotalDue.ToString("C");
            textBoxComment.Text = order.Comment ?? "";
        }

        private void ClearDetails()
        {
            textBoxOrderID.Text = "";
            textBoxOrderNumber.Text = "";
            textBoxCustomerID.Text = "";
            checkBoxOnlineOrder.Checked = false;
            dateTimePickerOrderDate.Value = DateTime.Today;
            dateTimePickerDueDate.Value = DateTime.Today.AddDays(7);
            checkBoxShipped.Checked = false;
            dateTimePickerShipDate.Value = DateTime.Today;
            dateTimePickerShipDate.Enabled = false;
            comboBoxStatus.SelectedIndex = 0;
            textBoxSubTotal.Text = "";
            textBoxTaxAmt.Text = "";
            textBoxFreight.Text = "";
            textBoxTotalDue.Text = "";
            textBoxComment.Text = "";
        }

        private void CheckBoxShipped_CheckedChanged(object? sender, EventArgs e)
        {
            dateTimePickerShipDate.Enabled = checkBoxShipped.Checked;
            if (checkBoxShipped.Checked && dateTimePickerShipDate.Value < dateTimePickerOrderDate.Value)
            {
                dateTimePickerShipDate.Value = DateTime.Today;
            }
        }

        private async void ButtonSearch_Click(object? sender, EventArgs e)
        {
            try
            {
                UpdateStatus("Ricerca...", Color.Blue);
                
                var orders = await _repository.SearchAsync(
                    string.IsNullOrWhiteSpace(textBoxSearch.Text) ? null : textBoxSearch.Text,
                    dateTimePickerFrom.Value,
                    dateTimePickerTo.Value);

                dataGridViewOrders.DataSource = orders.ToList();
                ConfigureGridColumns();
                UpdateStatus($"Trovati {orders.Count()} ordini", Color.Green);
            }
            catch (Exception ex)
            {
                UpdateStatus($"Errore: {ex.Message}", Color.Red);
            }
        }

        private async void ButtonRefresh_Click(object? sender, EventArgs e)
        {
            textBoxSearch.Text = "";
            await LoadOrdersAsync();
        }

        private void ButtonNew_Click(object? sender, EventArgs e)
        {
            _isNewOrder = true;
            _currentOrder = new SalesOrderHeader
            {
                OrderDate = DateTime.Today,
                DueDate = DateTime.Today.AddDays(7),
                Status = 1,
                OnlineOrderFlag = false
            };

            ClearDetails();
            textBoxOrderID.Text = "(Nuovo)";
            textBoxOrderNumber.Text = "(Generato automaticamente)";
            SetNewMode();
            textBoxCustomerID.Focus();
            UpdateStatus("Inserisci i dati del nuovo ordine", Color.Blue);
        }

        private async void ButtonSave_Click(object? sender, EventArgs e)
        {
            // Validazione base
            if (!int.TryParse(textBoxCustomerID.Text, out int customerId))
            {
                MessageBox.Show("ID Cliente non valido.", "Validazione",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxCustomerID.Focus();
                return;
            }

            try
            {
                var order = new SalesOrderHeader
                {
                    CustomerID = customerId,
                    OnlineOrderFlag = checkBoxOnlineOrder.Checked,
                    OrderDate = dateTimePickerOrderDate.Value,
                    DueDate = dateTimePickerDueDate.Value,
                    ShipDate = checkBoxShipped.Checked ? dateTimePickerShipDate.Value : null,
                    Status = (byte)(comboBoxStatus.SelectedIndex + 1),
                    Comment = string.IsNullOrWhiteSpace(textBoxComment.Text) ? null : textBoxComment.Text,
                    // Per un nuovo ordine, dobbiamo impostare valori di default per campi obbligatori
                    BillToAddressID = 1, // Valore di default - in produzione andrebbe selezionato
                    ShipToAddressID = 1,
                    ShipMethodID = 1,
                    SubTotal = 0,
                    TaxAmt = 0,
                    Freight = 0
                };

                if (_isNewOrder)
                {
                    UpdateStatus("Inserimento...", Color.Blue);
                    int newId = await _repository.InsertAsync(order);
                    UpdateStatus($"Ordine inserito con ID: {newId}", Color.Green);
                    MessageBox.Show($"Ordine inserito!\nID: {newId}\n\nNOTA: Aggiungi dettagli ordine per completare.",
                        "Successo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (_currentOrder != null)
                {
                    order.SalesOrderID = _currentOrder.SalesOrderID;
                    UpdateStatus("Salvataggio...", Color.Blue);
                    await _repository.UpdateAsync(order);
                    UpdateStatus("Ordine aggiornato", Color.Green);
                }

                _isNewOrder = false;
                await LoadOrdersAsync();
                SetViewMode();
            }
            catch (Exception ex)
            {
                UpdateStatus($"Errore: {ex.Message}", Color.Red);
                MessageBox.Show($"Errore:\n{ex.Message}", "Errore", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void ButtonDelete_Click(object? sender, EventArgs e)
        {
            if (_currentOrder == null || _isNewOrder)
            {
                MessageBox.Show("Seleziona un ordine da eliminare.", "Attenzione",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(
                $"Eliminare l'ordine {_currentOrder.SalesOrderNumber}?\n\n" +
                "ATTENZIONE: Verranno eliminati anche tutti i dettagli dell'ordine!",
                "Conferma eliminazione",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result != DialogResult.Yes) return;

            try
            {
                UpdateStatus("Eliminazione...", Color.Blue);
                await _repository.DeleteAsync(_currentOrder.SalesOrderID);
                UpdateStatus("Ordine eliminato", Color.Green);

                _currentOrder = null;
                ClearDetails();
                await LoadOrdersAsync();
                SetViewMode();
            }
            catch (Exception ex)
            {
                UpdateStatus($"Errore: {ex.Message}", Color.Red);
                MessageBox.Show($"Errore:\n{ex.Message}", "Errore",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void ButtonCancel_Click(object? sender, EventArgs e)
        {
            _isNewOrder = false;
            _currentOrder = null;
            ClearDetails();
            await LoadOrdersAsync();
            SetViewMode();
            UpdateStatus("Operazione annullata", Color.Gray);
        }

        /// <summary>
        /// Apre la form dei dettagli ordine per l'ordine selezionato.
        /// 
        /// NOTA DIDATTICA:
        /// - Questo mostra la navigazione tra tabelle correlate
        /// - Passiamo l'ID dell'ordine alla form dei dettagli
        /// </summary>
        private void ButtonViewDetails_Click(object? sender, EventArgs e)
        {
            if (_currentOrder == null)
            {
                MessageBox.Show("Seleziona un ordine per visualizzarne i dettagli.",
                    "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var detailForm = new SalesOrderDetailForm(_currentOrder.SalesOrderID);
            detailForm.ShowDialog(this);
        }

        private void SetViewMode()
        {
            SetControlsEnabled(false);
            buttonNew.Enabled = true;
            buttonSave.Enabled = false;
            buttonDelete.Enabled = false;
            buttonCancel.Enabled = false;
            buttonViewDetails.Enabled = false;
            dataGridViewOrders.Enabled = true;
        }

        private void SetEditMode()
        {
            SetControlsEnabled(true);
            buttonNew.Enabled = true;
            buttonSave.Enabled = true;
            buttonDelete.Enabled = true;
            buttonCancel.Enabled = true;
            buttonViewDetails.Enabled = true;
            dataGridViewOrders.Enabled = true;
        }

        private void SetNewMode()
        {
            SetControlsEnabled(true);
            buttonNew.Enabled = false;
            buttonSave.Enabled = true;
            buttonDelete.Enabled = false;
            buttonCancel.Enabled = true;
            buttonViewDetails.Enabled = false;
            dataGridViewOrders.Enabled = false;
        }

        private void SetControlsEnabled(bool enabled)
        {
            textBoxCustomerID.Enabled = enabled;
            checkBoxOnlineOrder.Enabled = enabled;
            dateTimePickerOrderDate.Enabled = enabled;
            dateTimePickerDueDate.Enabled = enabled;
            checkBoxShipped.Enabled = enabled;
            comboBoxStatus.Enabled = enabled;
            textBoxComment.Enabled = enabled;
        }

        private void UpdateStatus(string message, Color color)
        {
            labelStatusBar.Text = message;
            labelStatusBar.ForeColor = color;
        }
    }
}
