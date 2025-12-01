/*
 * ============================================================================
 * FORM: SalesOrderDetailForm (Logic)
 * ============================================================================
 * 
 * Form for managing order details from the Sales.SalesOrderDetail table.
 * 
 * FEATURES:
 * - Can show all details or filter by order
 * - Shows relationships: displays product name from the Product table
 * - Allows adding/modifying/ofeting order lines
 * 
 * TEACHING NOTE:
 * - Example of a form with parametric filter
 * - Loading related data (products) for selection
 * - Composite primary key (SalesOrderID + SalesOrderDetailID)
 * ============================================================================
 */

using AdventureWorksApp.DataAccess;
using AdventureWorksApp.Models;

namespace AdventureWorksApp.Forms
{
    /// <summary>
    /// Form for CRUD management of the Sales.SalesOrderDetail table.
    /// </summary>
    public partial class SalesOrderDetailForm : Form
    {
        // Constant for the default special offer (no discount)
        // TEACHING NOTE: In AdventureWorks, SpecialOfferID = 1 means "No Discount"
        private const int NO_DISCOUNT_SPECIAL_OFFER_ID = 1;
        
        private readonly SalesOrderDetailRepository _detailRepository;
        private readonly ProductRepository _productRepository;
        private readonly SalesOrderHeaderRepository _headerRepository;
        
        private SalesOrderDetail? _currentDetail;
        private bool _isNewDetail;
        private int? _filteredOrderId;
        private List<Product> _products = new();

        /// <summary>
        /// Constructor without parameters - shows all details.
        /// </summary>
        public SalesOrderDetailForm() : this(null)
        {
        }

        /// <summary>
        /// Constructor with order ID - shows only the details of that order.
        /// 
        /// TEACHING NOTE:
        /// - Constructor overload to support different opening modes
        /// - If orderId is set, we automatically filter for that order
        /// </summary>
        public SalesOrderDetailForm(int? orderId)
        {
            InitializeComponent();
            
            _detailRepository = new SalesOrderDetailRepository();
            _productRepository = new ProductRepository();
            _headerRepository = new SalesOrderHeaderRepository();
            _filteredOrderId = orderId;

            // Event handlers
            this.Load += Form_Load;
            this.dataGridViewDetails.SelectionChanged += DataGridView_SelectionChanged;
            this.buttonFilter.Click += ButtonFilter_Click;
            this.buttonShowAll.Click += ButtonShowAll_Click;
            this.buttonNew.Click += ButtonNew_Click;
            this.buttonSave.Click += ButtonSave_Click;
            this.buttonDelete.Click += ButtonDelete_Click;
            this.buttonCancel.Click += ButtonCancel_Click;
            this.comboBoxProduct.SelectedIndexChanged += ComboBoxProduct_SelectedIndexChanged;
            
            this.textBoxFilterOrderId.KeyPress += (s, e) =>
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    ButtonFilter_Click(s, e);
                    e.Handled = true;
                }
            };
        }

        private async void Form_Load(object? sender, EventArgs e)
        {
            // Load products for the combo box
            await LoadProductsAsync();
            
            // Se abbiamo un ID ordine, filtriamo automaticamente
            if (_filteredOrderId.HasValue)
            {
                textBoxFilterOrderId.Text = _filteredOrderId.Value.ToString();
                await LoadDetailsForOrderAsync(_filteredOrderId.Value);
                // Hide filter buttons if we are in filtered mode
                buttonShowAll.Visible = false;
            }
            else
            {
                await LoadAllDetailsAsync();
            }
            
            SetViewMode();
        }

        /// <summary>
        /// Loads all products for the selection combo box.
        /// </summary>
        private async Task LoadProductsAsync()
        {
            try
            {
                _products = (await _productRepository.GetAllAsync()).ToList();
                
                comboBoxProduct.DisplayMember = "DisplayText";
                comboBoxProduct.ValueMember = "ProductID";
                
                // We create an anonymous list with formatted DisplayText
                var items = _products.Select(p => new
                {
                    p.ProductID,
                    DisplayText = $"{p.ProductID} - {p.Name} ({p.ListPrice:C})"
                }).ToList();
                
                comboBoxProduct.DataSource = items;
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error loading products: {ex.Message}", Color.Red);
            }
        }

        /// <summary>
        /// Loads all order details.
        /// </summary>
        private async Task LoadAllDetailsAsync()
        {
            try
            {
                UpdateStatus("Loading details...", Color.Blue);
                var details = await _detailRepository.GetAllAsync();
                dataGridViewDetails.DataSource = details.ToList();
                ConfigureGridColumns();
                labelOrderInfo.Text = "";
                UpdateStatus($"Loaded {details.Count()} details", Color.Green);
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error: {ex.Message}", Color.Red);
            }
        }

        /// <summary>
        /// Carica i details di un ordine specifico.
        /// 
        /// TEACHING NOTE:
        /// - This method shows how to filter data by a foreign key
        /// - We also retrieve order info to show in the header
        /// </summary>
        private async Task LoadDetailsForOrderAsync(int orderId)
        {
            try
            {
                UpdateStatus("Loading details...dine...", Color.Blue);
                
                // Load order info
                var order = await _headerRepository.GetByIdAsync(orderId);
                if (order != null)
                {
                    labelOrderInfo.Text = $"Order: {order.SalesOrderNumber} of {order.OrderDate:dd/MM/yyyy} - Total: {order.TotalDue:C}";
                    this.Text = $"Order Details {order.SalesOrderNumber}";
                }
                
                // Carica details
                var details = await _detailRepository.GetByOrderIdAsync(orderId);
                dataGridViewDetails.DataSource = details.ToList();
                ConfigureGridColumns();
                
                UpdateStatus($"Loaded {details.Count()} lines for the order", Color.Green);
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error: {ex.Message}", Color.Red);
            }
        }

        private void ConfigureGridColumns()
        {
            if (dataGridViewDetails.Columns.Count == 0) return;

            var hidden = new[] { "rowguid", "ModifiedDate", "SpecialOfferID", "CalculatedLineTotal" };
            foreach (var col in hidden)
                if (dataGridViewDetails.Columns.Contains(col))
                    dataGridViewDetails.Columns[col].Visible = false;

            SetHeader("SalesOrderID", "Order ID");
            SetHeader("SalesOrderDetailID", "Line ID");
            SetHeader("SalesOrderNumber", "Order #");
            SetHeader("ProductID", "Product ID");
            SetHeader("ProductName", "Product");
            SetHeader("OrderQty", "Quantity");
            SetHeader("UnitPrice", "Unit Price");
            SetHeader("UnitPriceDiscount", "Discount");
            SetHeader("LineTotal", "Line Total");
            SetHeader("CarrierTrackingNumber", "Tracking");
        }

        private void SetHeader(string name, string header)
        {
            if (dataGridViewDetails.Columns.Contains(name))
                dataGridViewDetails.Columns[name].HeaderText = header;
        }

        private void DataGridView_SelectionChanged(object? sender, EventArgs e)
        {
            if (_isNewDetail) return;

            if (dataGridViewDetails.SelectedRows.Count > 0)
            {
                _currentDetail = dataGridViewDetails.SelectedRows[0].DataBoundItem as SalesOrderDetail;
                if (_currentDetail != null)
                {
                    PopulateDetails(_currentDetail);
                    SetEditMode();
                }
            }
        }

        private void PopulateDetails(SalesOrderDetail detail)
        {
            textBoxOrderID.Text = detail.SalesOrderID.ToString();
            textBoxDetailID.Text = detail.SalesOrderDetailID.ToString();
            textBoxOrderNumber.Text = detail.SalesOrderNumber ?? "";
            
            // Select the product in the combo
            for (int i = 0; i < comboBoxProduct.Items.Count; i++)
            {
                var item = comboBoxProduct.Items[i];
                if (item == null) continue;
                
                var prop = item.GetType().GetProperty("ProductID");
                if (prop != null)
                {
                    var value = prop.GetValue(item);
                    if (value != null && (int)value == detail.ProductID)
                    {
                        comboBoxProduct.SelectedIndex = i;
                        break;
                    }
                }
            }
            
            textBoxProductName.Text = detail.ProductName ?? "";
            numericUpDownQty.Value = detail.OrderQty;
            textBoxUnitPrice.Text = detail.UnitPrice.ToString("F2");
            textBoxDiscount.Text = (detail.UnitPriceDiscount * 100).ToString("F2");
            textBoxLineTotal.Text = detail.LineTotal.ToString("C");
            textBoxCarrierTracking.Text = detail.CarrierTrackingNumber ?? "";
        }

        private void ClearDetails()
        {
            textBoxOrderID.Text = "";
            textBoxDetailID.Text = "";
            textBoxOrderNumber.Text = "";
            if (comboBoxProduct.Items.Count > 0)
                comboBoxProduct.SelectedIndex = 0;
            textBoxProductName.Text = "";
            numericUpDownQty.Value = 1;
            textBoxUnitPrice.Text = "";
            textBoxDiscount.Text = "0";
            textBoxLineTotal.Text = "";
            textBoxCarrierTracking.Text = "";
        }

        /// <summary>
        /// When a product is selected, update name and price.
        /// </summary>
        private void ComboBoxProduct_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (comboBoxProduct.SelectedItem == null) return;

            var item = comboBoxProduct.SelectedItem;
            var propId = item.GetType().GetProperty("ProductID");
            if (propId != null)
            {
                int productId = (int)propId.GetValue(item)!;
                var product = _products.FirstOrDefault(p => p.ProductID == productId);
                if (product != null)
                {
                    textBoxProductName.Text = product.Name;
                    textBoxUnitPrice.Text = product.ListPrice?.ToString("F2") ?? "0.00";
                }
            }
        }

        private async void ButtonFilter_Click(object? sender, EventArgs e)
        {
            if (int.TryParse(textBoxFilterOrderId.Text, out int orderId))
            {
                _filteredOrderId = orderId;
                await LoadDetailsForOrderAsync(orderId);
            }
            else
            {
                MessageBox.Show("Enter a valid order ID.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void ButtonShowAll_Click(object? sender, EventArgs e)
        {
            _filteredOrderId = null;
            textBoxFilterOrderId.Text = "";
            await LoadAllDetailsAsync();
        }

        private void ButtonNew_Click(object? sender, EventArgs e)
        {
            // Per creare un nuovo detailso, dobbiamo avere un ordine selezionato
            if (!_filteredOrderId.HasValue)
            {
                MessageBox.Show("Per inserire un nuovo detailso, filtra prima per un ordine specifico.",
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _isNewDetail = true;
            _currentDetail = new SalesOrderDetail
            {
                SalesOrderID = _filteredOrderId.Value,
                OrderQty = 1,
                SpecialOfferID = NO_DISCOUNT_SPECIAL_OFFER_ID,
                UnitPriceDiscount = 0
            };

            ClearDetails();
            textBoxOrderID.Text = _filteredOrderId.Value.ToString();
            textBoxDetailID.Text = "(Nuovo)";
            SetNewMode();
            comboBoxProduct.Focus();
            UpdateStatus("Inserisci i dati ofla nuova riga ordine", Color.Blue);
        }

        private async void ButtonSave_Click(object? sender, EventArgs e)
        {
            // Validation
            if (comboBoxProduct.SelectedItem == null)
            {
                MessageBox.Show("Select a product.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Get product ID from combo
                var item = comboBoxProduct.SelectedItem;
                var propId = item.GetType().GetProperty("ProductID");
                int productId = (int)propId!.GetValue(item)!;

                // Get the price from the product
                var product = _products.FirstOrDefault(p => p.ProductID == productId);
                decimal unitPrice = product?.ListPrice ?? 0;

                // Discount (converti da percentuale a decimale)
                decimal discount = 0;
                if (decimal.TryParse(textBoxDiscount.Text, out decimal discountPercent))
                {
                    discount = discountPercent / 100;
                }

                var detail = new SalesOrderDetail
                {
                    SalesOrderID = _filteredOrderId ?? _currentDetail?.SalesOrderID ?? 0,
                    ProductID = productId,
                    OrderQty = (short)numericUpDownQty.Value,
                    UnitPrice = unitPrice,
                    UnitPriceDiscount = discount,
                    SpecialOfferID = NO_DISCOUNT_SPECIAL_OFFER_ID,
                    CarrierTrackingNumber = string.IsNullOrWhiteSpace(textBoxCarrierTracking.Text) 
                        ? null : textBoxCarrierTracking.Text
                };

                if (_isNewDetail)
                {
                    UpdateStatus("Inserting...", Color.Blue);
                    int newDetailId = await _detailRepository.InsertAsync(detail);
                    UpdateStatus($"Line inserted with ID: {newDetailId}", Color.Green);
                    MessageBox.Show($"Order line inserted!\nDetail ID: {newDetailId}",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (_currentDetail != null)
                {
                    detail.SalesOrderDetailID = _currentDetail.SalesOrderDetailID;
                    UpdateStatus("Saving...", Color.Blue);
                    await _detailRepository.UpdateAsync(detail);
                    UpdateStatus("Line updated", Color.Green);
                }

                _isNewDetail = false;
                
                // Reload data
                if (_filteredOrderId.HasValue)
                    await LoadDetailsForOrderAsync(_filteredOrderId.Value);
                else
                    await LoadAllDetailsAsync();
                    
                SetViewMode();
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error: {ex.Message}", Color.Red);
                MessageBox.Show($"Error:\n{ex.Message}", "Errore",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void ButtonDelete_Click(object? sender, EventArgs e)
        {
            if (_currentDetail == null || _isNewDetail)
            {
                MessageBox.Show("Select a line to delete.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(
                $"Delete line {_currentDetail.SalesOrderDetailID} " +
                $"(Product: {_currentDetail.ProductName})?",
                "Confirm deletion",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            try
            {
                UpdateStatus("Deleting...", Color.Blue);
                await _detailRepository.DeleteAsync(
                    _currentDetail.SalesOrderID, 
                    _currentDetail.SalesOrderDetailID);
                UpdateStatus("Line deleted", Color.Green);

                _currentDetail = null;
                ClearDetails();
                
                if (_filteredOrderId.HasValue)
                    await LoadDetailsForOrderAsync(_filteredOrderId.Value);
                else
                    await LoadAllDetailsAsync();
                    
                SetViewMode();
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error: {ex.Message}", Color.Red);
                MessageBox.Show($"Error:\n{ex.Message}", "Errore",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void ButtonCancel_Click(object? sender, EventArgs e)
        {
            _isNewDetail = false;
            _currentDetail = null;
            ClearDetails();
            
            if (_filteredOrderId.HasValue)
                await LoadDetailsForOrderAsync(_filteredOrderId.Value);
            else
                await LoadAllDetailsAsync();
                
            SetViewMode();
            UpdateStatus("Operation cancelled", Color.Gray);
        }

        private void SetViewMode()
        {
            SetControlsEnabled(false);
            buttonNew.Enabled = _filteredOrderId.HasValue;
            buttonSave.Enabled = false;
            buttonDelete.Enabled = false;
            buttonCancel.Enabled = false;
            dataGridViewDetails.Enabled = true;
        }

        private void SetEditMode()
        {
            SetControlsEnabled(true);
            buttonNew.Enabled = _filteredOrderId.HasValue;
            buttonSave.Enabled = true;
            buttonDelete.Enabled = true;
            buttonCancel.Enabled = true;
            dataGridViewDetails.Enabled = true;
            // In edit mode, we don't allow changing the product
            comboBoxProduct.Enabled = false;
        }

        private void SetNewMode()
        {
            SetControlsEnabled(true);
            comboBoxProduct.Enabled = true; // In new mode, we can select a product
            buttonNew.Enabled = false;
            buttonSave.Enabled = true;
            buttonDelete.Enabled = false;
            buttonCancel.Enabled = true;
            dataGridViewDetails.Enabled = false;
        }

        private void SetControlsEnabled(bool enabled)
        {
            comboBoxProduct.Enabled = enabled;
            numericUpDownQty.Enabled = enabled;
            textBoxDiscount.Enabled = enabled;
            textBoxCarrierTracking.Enabled = enabled;
        }

        private void UpdateStatus(string message, Color color)
        {
            labelStatusBar.Text = message;
            labelStatusBar.ForeColor = color;
        }
    }
}
