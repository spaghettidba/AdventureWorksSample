/*
 * ============================================================================
 * FORM: ProductForm (Logic)
 * ============================================================================
 * 
 * Form for managing products from the Production.Product table.
 * Implements complete CRUD operations with a user-friendly interface.
 * 
 * TEACHING NOTE:
 * - Separation between UI and data access via Repository
 * - Use of async/await for non-blocking operations
 * - Data validation before saving
 * - State management (view/edit/new)
 * ============================================================================
 */

using AdventureWorksApp.DataAccess;
using AdventureWorksApp.Models;

namespace AdventureWorksApp.Forms
{
    /// <summary>
    /// Form for CRUD management of the Production.Product table.
    /// </summary>
    public partial class ProductForm : Form
    {
        // Constants for default product values
        // TEACHING NOTE: These values represent standard stock levels
        private const short DEFAULT_SAFETY_STOCK_LEVEL = 100;
        private const short DEFAULT_REORDER_POINT = 75;
        
        // Repository for data access
        private readonly ProductRepository _repository;
        
        // Currently selected/editing product
        private Product? _currentProduct;
        
        // Flag to indicate if we are inserting a new product
        private bool _isNewProduct;

        /// <summary>
        /// Form constructor.
        /// </summary>
        public ProductForm()
        {
            InitializeComponent();
            
            // Initialize the repository
            _repository = new ProductRepository();
            
            // Connect event handlers
            this.Load += ProductForm_Load;
            this.dataGridViewProducts.SelectionChanged += DataGridView_SelectionChanged;
            this.buttonSearch.Click += ButtonSearch_Click;
            this.buttonRefresh.Click += ButtonRefresh_Click;
            this.buttonNew.Click += ButtonNew_Click;
            this.buttonSave.Click += ButtonSave_Click;
            this.buttonDelete.Click += ButtonDelete_Click;
            this.buttonCancel.Click += ButtonCancel_Click;
            
            // Allow search with Enter
            this.textBoxSearch.KeyPress += (s, e) =>
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    ButtonSearch_Click(s, e);
                    e.Handled = true;
                }
            };
        }

        /// <summary>
        /// Initial form loading.
        /// </summary>
        private async void ProductForm_Load(object? sender, EventArgs e)
        {
            await LoadProductsAsync();
            SetViewMode();
        }

        /// <summary>
        /// Loads all products into the grid.
        /// 
        /// TEACHING NOTE:
        /// - DataSource allows directly binding a collection
        /// - ToList() materializes the query for binding
        /// </summary>
        private async Task LoadProductsAsync()
        {
            try
            {
                UpdateStatus("Loading products...", Color.Blue);
                
                var products = await _repository.GetAllAsync();
                
                // We use BindingSource for more flexible binding
                dataGridViewProducts.DataSource = products.ToList();
                
                // Hide columns not needed for display
                ConfigureGridColumns();
                
                UpdateStatus($"Loaded {products.Count()} products", Color.Green);
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error: {ex.Message}", Color.Red);
                MessageBox.Show($"Error nel caricamento dei products:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Configures grid columns for better display.
        /// </summary>
        private void ConfigureGridColumns()
        {
            if (dataGridViewProducts.Columns.Count == 0) return;

            // Hide technical columns
            var hiddenColumns = new[] { "rowguid", "ModifiedDate", "SizeUnitMeasureCode", 
                "WeightUnitMeasureCode", "ProductSubcategoryID", "ProductModelID",
                "DiscontinuedDate", "DaysToManufacture", "ProductLine", "Class", "Style" };

            foreach (var colName in hiddenColumns)
            {
                if (dataGridViewProducts.Columns.Contains(colName))
                    dataGridViewProducts.Columns[colName].Visible = false;
            }

            // Rename columns for the user
            SetColumnHeader("ProductID", "ID");
            SetColumnHeader("Name", "Nome");
            SetColumnHeader("ProductNumber", "Numero");
            SetColumnHeader("Color", "Colore");
            SetColumnHeader("ListPrice", "Prezzo");
            SetColumnHeader("StandardCost", "Costo");
            SetColumnHeader("SafetyStockLevel", "Stock Min.");
            SetColumnHeader("ReorderPoint", "Riordino");
            SetColumnHeader("MakeFlag", "Prod. Interna");
            SetColumnHeader("FinishedGoodsFlag", "Finito");
            SetColumnHeader("Size", "Taglia");
            SetColumnHeader("Weight", "Peso");
            SetColumnHeader("SellStartDate", "Inizio Vendita");
            SetColumnHeader("SellEndDate", "Fine Vendita");
        }

        private void SetColumnHeader(string columnName, string headerText)
        {
            if (dataGridViewProducts.Columns.Contains(columnName))
                dataGridViewProducts.Columns[columnName].HeaderText = headerText;
        }

        /// <summary>
        /// Handles row selection in the grid.
        /// Populates detail fields with the selected product data.
        /// </summary>
        private void DataGridView_SelectionChanged(object? sender, EventArgs e)
        {
            if (_isNewProduct) return; // Don't interfere if we're inserting

            if (dataGridViewProducts.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewProducts.SelectedRows[0];
                _currentProduct = selectedRow.DataBoundItem as Product;
                
                if (_currentProduct != null)
                {
                    PopulateDetails(_currentProduct);
                    SetEditMode();
                }
            }
        }

        /// <summary>
        /// Populates detail fields with product data.
        /// </summary>
        private void PopulateDetails(Product product)
        {
            textBoxProductID.Text = product.ProductID.ToString();
            textBoxName.Text = product.Name;
            textBoxProductNumber.Text = product.ProductNumber;
            textBoxColor.Text = product.Color ?? "";
            textBoxListPrice.Text = product.ListPrice?.ToString("F2") ?? "";
            textBoxStandardCost.Text = product.StandardCost.ToString("F2");
            textBoxSafetyStockLevel.Text = product.SafetyStockLevel.ToString();
            textBoxReorderPoint.Text = product.ReorderPoint.ToString();
            checkBoxMakeFlag.Checked = product.MakeFlag;
            checkBoxFinishedGoods.Checked = product.FinishedGoodsFlag;
            dateTimePickerSellStart.Value = product.SellStartDate;
        }

        /// <summary>
        /// Clears all detail fields.
        /// </summary>
        private void ClearDetails()
        {
            textBoxProductID.Text = "";
            textBoxName.Text = "";
            textBoxProductNumber.Text = "";
            textBoxColor.Text = "";
            textBoxListPrice.Text = "";
            textBoxStandardCost.Text = "";
            textBoxSafetyStockLevel.Text = "";
            textBoxReorderPoint.Text = "";
            checkBoxMakeFlag.Checked = false;
            checkBoxFinishedGoods.Checked = false;
            dateTimePickerSellStart.Value = DateTime.Today;
        }

        /// <summary>
        /// Searches products by name.
        /// </summary>
        private async void ButtonSearch_Click(object? sender, EventArgs e)
        {
            string searchTerm = textBoxSearch.Text.Trim();
            
            try
            {
                UpdateStatus("Searching...", Color.Blue);
                
                IEnumerable<Product> products;
                if (string.IsNullOrEmpty(searchTerm))
                {
                    products = await _repository.GetAllAsync();
                }
                else
                {
                    products = await _repository.SearchByNameAsync(searchTerm);
                }
                
                dataGridViewProducts.DataSource = products.ToList();
                ConfigureGridColumns();
                
                UpdateStatus($"Found {products.Count()} products", Color.Green);
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error: {ex.Message}", Color.Red);
            }
        }

        /// <summary>
        /// Refreshes the product list.
        /// </summary>
        private async void ButtonRefresh_Click(object? sender, EventArgs e)
        {
            textBoxSearch.Text = "";
            await LoadProductsAsync();
        }

        /// <summary>
        /// Starts inserting a new product.
        /// </summary>
        private void ButtonNew_Click(object? sender, EventArgs e)
        {
            _isNewProduct = true;
            _currentProduct = new Product
            {
                SellStartDate = DateTime.Today,
                SafetyStockLevel = DEFAULT_SAFETY_STOCK_LEVEL,
                ReorderPoint = DEFAULT_REORDER_POINT,
                MakeFlag = false,
                FinishedGoodsFlag = true
            };
            
            ClearDetails();
            textBoxProductID.Text = "(Nuovo)";
            SetNewMode();
            textBoxName.Focus();
            
            UpdateStatus("Enter data for the new product", Color.Blue);
        }

        /// <summary>
        /// Saves the product (new or modified).
        /// 
        /// TEACHING NOTE:
        /// - First we validate the data
        /// - Then we build the Product object
        /// - Finally we call the appropriate repository (Insert or Update)
        /// </summary>
        private async void ButtonSave_Click(object? sender, EventArgs e)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(textBoxName.Text))
            {
                MessageBox.Show("Product name is required.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxProductNumber.Text))
            {
                MessageBox.Show("Product number is required.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxProductNumber.Focus();
                return;
            }

            try
            {
                // Build the Product object from UI fields
                var product = new Product
                {
                    Name = textBoxName.Text.Trim(),
                    ProductNumber = textBoxProductNumber.Text.Trim(),
                    Color = string.IsNullOrWhiteSpace(textBoxColor.Text) ? null : textBoxColor.Text.Trim(),
                    ListPrice = decimal.TryParse(textBoxListPrice.Text, out var price) ? price : null,
                    StandardCost = decimal.TryParse(textBoxStandardCost.Text, out var cost) ? cost : 0,
                    SafetyStockLevel = short.TryParse(textBoxSafetyStockLevel.Text, out var stock) ? stock : DEFAULT_SAFETY_STOCK_LEVEL,
                    ReorderPoint = short.TryParse(textBoxReorderPoint.Text, out var reorder) ? reorder : DEFAULT_REORDER_POINT,
                    MakeFlag = checkBoxMakeFlag.Checked,
                    FinishedGoodsFlag = checkBoxFinishedGoods.Checked,
                    SellStartDate = dateTimePickerSellStart.Value
                };

                if (_isNewProduct)
                {
                    // Inserting new product
                    UpdateStatus("Inserting...", Color.Blue);
                    int newId = await _repository.InsertAsync(product);
                    UpdateStatus($"Product inserted with ID: {newId}", Color.Green);
                    MessageBox.Show($"Product inserted successfully!\nID: {newId}",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (_currentProduct != null)
                {
                    // Modifying existing product
                    product.ProductID = _currentProduct.ProductID;
                    UpdateStatus("Saving...", Color.Blue);
                    await _repository.UpdateAsync(product);
                    UpdateStatus("Product updated successfully", Color.Green);
                }

                // Reload the list and return to view mode
                _isNewProduct = false;
                await LoadProductsAsync();
                SetViewMode();
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error: {ex.Message}", Color.Red);
                MessageBox.Show($"Error during save:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Deletes the selected product.
        /// </summary>
        private async void ButtonDelete_Click(object? sender, EventArgs e)
        {
            if (_currentProduct == null || _isNewProduct)
            {
                MessageBox.Show("Select a product to delete.",
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Confirm deletion
            var result = MessageBox.Show(
                $"Are you sure you want to delete the product:\n\n{_currentProduct.Name}?",
                "Confirm deletion",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            try
            {
                // Check if it can be deleted
                bool canDelete = await _repository.CanDeleteAsync(_currentProduct.ProductID);
                if (!canDelete)
                {
                    MessageBox.Show(
                        "Cannot delete this product because it is referenced in existing orders.",
                        "Deletion not allowed",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                UpdateStatus("Deleting...", Color.Blue);
                await _repository.DeleteAsync(_currentProduct.ProductID);
                UpdateStatus("Product deleted", Color.Green);

                // Reload the list
                _currentProduct = null;
                ClearDetails();
                await LoadProductsAsync();
                SetViewMode();
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error: {ex.Message}", Color.Red);
                MessageBox.Show($"Error durante l'eliminazione:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Cancels the current operation.
        /// </summary>
        private async void ButtonCancel_Click(object? sender, EventArgs e)
        {
            _isNewProduct = false;
            _currentProduct = null;
            ClearDetails();
            await LoadProductsAsync();
            SetViewMode();
            UpdateStatus("Operation cancelled", Color.Gray);
        }

        /// <summary>
        /// Sets the form to view mode.
        /// </summary>
        private void SetViewMode()
        {
            SetControlsEnabled(false);
            buttonNew.Enabled = true;
            buttonSave.Enabled = false;
            buttonDelete.Enabled = false;
            buttonCancel.Enabled = false;
            dataGridViewProducts.Enabled = true;
        }

        /// <summary>
        /// Sets the form to edit mode.
        /// </summary>
        private void SetEditMode()
        {
            SetControlsEnabled(true);
            buttonNew.Enabled = true;
            buttonSave.Enabled = true;
            buttonDelete.Enabled = true;
            buttonCancel.Enabled = true;
            dataGridViewProducts.Enabled = true;
        }

        /// <summary>
        /// Sets the form to new entry mode.
        /// </summary>
        private void SetNewMode()
        {
            SetControlsEnabled(true);
            buttonNew.Enabled = false;
            buttonSave.Enabled = true;
            buttonDelete.Enabled = false;
            buttonCancel.Enabled = true;
            dataGridViewProducts.Enabled = false;
        }

        /// <summary>
        /// Enables/disables input controls.
        /// </summary>
        private void SetControlsEnabled(bool enabled)
        {
            textBoxName.Enabled = enabled;
            textBoxProductNumber.Enabled = enabled;
            textBoxColor.Enabled = enabled;
            textBoxListPrice.Enabled = enabled;
            textBoxStandardCost.Enabled = enabled;
            textBoxSafetyStockLevel.Enabled = enabled;
            textBoxReorderPoint.Enabled = enabled;
            checkBoxMakeFlag.Enabled = enabled;
            checkBoxFinishedGoods.Enabled = enabled;
            dateTimePickerSellStart.Enabled = enabled;
        }

        /// <summary>
        /// Updates the status message.
        /// </summary>
        private void UpdateStatus(string message, Color color)
        {
            labelStatus.Text = message;
            labelStatus.ForeColor = color;
        }
    }
}
