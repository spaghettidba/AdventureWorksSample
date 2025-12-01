/*
 * ============================================================================
 * FORM: SalesOrderHeaderForm (Logic)
 * ============================================================================
 * 
 * Form for managing orders from the Sales.SalesOrderHeader table.
 * Allows viewing, modification and navigation to order details.
 * 
 * TEACHING NOTE:
 * - Orders have many calculated fields (TotalDue, SubTotal)
 * - Some fields are readonly because they are managed by the database
 * - Navigation to SalesOrderDetail shows relationships between tables
 * ============================================================================
 */

using AdventureWorksApp.DataAccess;
using AdventureWorksApp.Models;

namespace AdventureWorksApp.Forms
{
    /// <summary>
    /// Form for CRUD management of the Sales.SalesOrderHeader table.
    /// </summary>
    public partial class SalesOrderHeaderForm : Form
    {
        // Constants for default values (used when creating a new order)
        // TEACHING NOTE: In production these values should be selected by the user
        private const int DEFAULT_ADDRESS_ID = 1;
        private const int DEFAULT_SHIP_METHOD_ID = 1;
        
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
        /// Loads orders into the grid.
        /// </summary>
        private async Task LoadOrdersAsync()
        {
            try
            {
                UpdateStatus("Loading orders...", Color.Blue);
                var orders = await _repository.GetAllAsync();
                dataGridViewOrders.DataSource = orders.ToList();
                ConfigureGridColumns();
                UpdateStatus($"Loaded {orders.Count()} orders", Color.Green);
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error: {ex.Message}", Color.Red);
                MessageBox.Show($"Error loading:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigureGridColumns()
        {
            if (dataGridViewOrders.Columns.Count == 0) return;

            // Hide technical columns
            var hidden = new[] { "rowguid", "ModifiedDate", "RevisionNumber", 
                "BillToAddressID", "ShipToAddressID", "ShipMethodID", "CreditCardID",
                "CreditCardApprovalCode", "CurrencyRateID", "SalesPersonID", "TerritoryID",
                "PurchaseOrderNumber", "AccountNumber" };

            foreach (var col in hidden)
                if (dataGridViewOrders.Columns.Contains(col))
                    dataGridViewOrders.Columns[col].Visible = false;

            // Rename
            SetHeader("SalesOrderID", "ID");
            SetHeader("SalesOrderNumber", "Order Number");
            SetHeader("OrderDate", "Order Date");
            SetHeader("DueDate", "Due Date");
            SetHeader("ShipDate", "Ship Date");
            SetHeader("Status", "Status");
            SetHeader("OnlineOrderFlag", "Online");
            SetHeader("CustomerID", "Customer");
            SetHeader("SubTotal", "Subtotal");
            SetHeader("TaxAmt", "Taxes");
            SetHeader("Freight", "Shipping");
            SetHeader("TotalDue", "Total");
            SetHeader("Comment", "Comment");
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

            // Status (0-based index)
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
                UpdateStatus("Searching...", Color.Blue);
                
                var orders = await _repository.SearchAsync(
                    string.IsNullOrWhiteSpace(textBoxSearch.Text) ? null : textBoxSearch.Text,
                    dateTimePickerFrom.Value,
                    dateTimePickerTo.Value);

                dataGridViewOrders.DataSource = orders.ToList();
                ConfigureGridColumns();
                UpdateStatus($"Found {orders.Count()} orders", Color.Green);
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error: {ex.Message}", Color.Red);
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
            textBoxOrderNumber.Text = "(Auto-generated)";
            SetNewMode();
            textBoxCustomerID.Focus();
            UpdateStatus("Enter data for the new order", Color.Blue);
        }

        private async void ButtonSave_Click(object? sender, EventArgs e)
        {
            // Validation base
            if (!int.TryParse(textBoxCustomerID.Text, out int customerId))
            {
                MessageBox.Show("ID Customer non valido.", "Validation",
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
                    // For a new order, we must set default values for required fields
                    BillToAddressID = DEFAULT_ADDRESS_ID,
                    ShipToAddressID = DEFAULT_ADDRESS_ID,
                    ShipMethodID = DEFAULT_SHIP_METHOD_ID,
                    SubTotal = 0,
                    TaxAmt = 0,
                    Freight = 0
                };

                if (_isNewOrder)
                {
                    UpdateStatus("Inserting...", Color.Blue);
                    int newId = await _repository.InsertAsync(order);
                    UpdateStatus($"Order inserted with ID: {newId}", Color.Green);
                    MessageBox.Show($"Order inserted!\nID: {newId}\n\nNOTA: Add order details to complete.",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (_currentOrder != null)
                {
                    order.SalesOrderID = _currentOrder.SalesOrderID;
                    UpdateStatus("Saving...", Color.Blue);
                    await _repository.UpdateAsync(order);
                    UpdateStatus("Order updated", Color.Green);
                }

                _isNewOrder = false;
                await LoadOrdersAsync();
                SetViewMode();
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error: {ex.Message}", Color.Red);
                MessageBox.Show($"Error:\n{ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void ButtonDelete_Click(object? sender, EventArgs e)
        {
            if (_currentOrder == null || _isNewOrder)
            {
                MessageBox.Show("Select an order to delete.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(
                $"Delete order {_currentOrder.SalesOrderNumber}?\n\n" +
                "ATTENZIONE: All order details will also be deleted!",
                "Confirm deletion",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result != DialogResult.Yes) return;

            try
            {
                UpdateStatus("Deleting...", Color.Blue);
                await _repository.DeleteAsync(_currentOrder.SalesOrderID);
                UpdateStatus("Order deleted", Color.Green);

                _currentOrder = null;
                ClearDetails();
                await LoadOrdersAsync();
                SetViewMode();
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error: {ex.Message}", Color.Red);
                MessageBox.Show($"Error:\n{ex.Message}", "Error",
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
            UpdateStatus("Operation cancelled", Color.Gray);
        }

        /// <summary>
        /// Opens the order details form for the selected order.
        /// 
        /// TEACHING NOTE:
        /// - This shows navigation between related tables
        /// - We pass the order ID to the details form
        /// </summary>
        private void ButtonViewDetails_Click(object? sender, EventArgs e)
        {
            if (_currentOrder == null)
            {
                MessageBox.Show("Select an order to view its details.",
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
