/*
 * ============================================================================
 * FORM: ProductForm (Logica)
 * ============================================================================
 * 
 * Form per la gestione dei prodotti dalla tabella Production.Product.
 * Implementa le operazioni CRUD complete con interfaccia user-friendly.
 * 
 * NOTA DIDATTICA:
 * - Separazione tra UI e accesso ai dati tramite Repository
 * - Uso di async/await per operazioni non bloccanti
 * - Validazione dati prima del salvataggio
 * - Gestione degli stati (visualizzazione/modifica/nuovo)
 * ============================================================================
 */

using AdventureWorksApp.DataAccess;
using AdventureWorksApp.Models;

namespace AdventureWorksApp.Forms
{
    /// <summary>
    /// Form per la gestione CRUD della tabella Production.Product.
    /// </summary>
    public partial class ProductForm : Form
    {
        // Costanti per valori di default dei prodotti
        // NOTA DIDATTICA: Questi valori rappresentano livelli di stock standard
        private const short DEFAULT_SAFETY_STOCK_LEVEL = 100;
        private const short DEFAULT_REORDER_POINT = 75;
        
        // Repository per l'accesso ai dati
        private readonly ProductRepository _repository;
        
        // Prodotto attualmente selezionato/in modifica
        private Product? _currentProduct;
        
        // Flag per indicare se stiamo inserendo un nuovo prodotto
        private bool _isNewProduct;

        /// <summary>
        /// Costruttore del form.
        /// </summary>
        public ProductForm()
        {
            InitializeComponent();
            
            // Inizializza il repository
            _repository = new ProductRepository();
            
            // Collega gli event handler
            this.Load += ProductForm_Load;
            this.dataGridViewProducts.SelectionChanged += DataGridView_SelectionChanged;
            this.buttonSearch.Click += ButtonSearch_Click;
            this.buttonRefresh.Click += ButtonRefresh_Click;
            this.buttonNew.Click += ButtonNew_Click;
            this.buttonSave.Click += ButtonSave_Click;
            this.buttonDelete.Click += ButtonDelete_Click;
            this.buttonCancel.Click += ButtonCancel_Click;
            
            // Permetti ricerca con Enter
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
        /// Caricamento iniziale del form.
        /// </summary>
        private async void ProductForm_Load(object? sender, EventArgs e)
        {
            await LoadProductsAsync();
            SetViewMode();
        }

        /// <summary>
        /// Carica tutti i prodotti nella griglia.
        /// 
        /// NOTA DIDATTICA:
        /// - DataSource permette di collegare direttamente una collection
        /// - ToList() materializza la query per il binding
        /// </summary>
        private async Task LoadProductsAsync()
        {
            try
            {
                UpdateStatus("Caricamento prodotti...", Color.Blue);
                
                var products = await _repository.GetAllAsync();
                
                // Usiamo BindingSource per un binding più flessibile
                dataGridViewProducts.DataSource = products.ToList();
                
                // Nascondiamo le colonne non necessarie per la visualizzazione
                ConfigureGridColumns();
                
                UpdateStatus($"Caricati {products.Count()} prodotti", Color.Green);
            }
            catch (Exception ex)
            {
                UpdateStatus($"Errore: {ex.Message}", Color.Red);
                MessageBox.Show($"Errore nel caricamento dei prodotti:\n{ex.Message}",
                    "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Configura le colonne della griglia per una migliore visualizzazione.
        /// </summary>
        private void ConfigureGridColumns()
        {
            if (dataGridViewProducts.Columns.Count == 0) return;

            // Nascondi colonne tecniche
            var hiddenColumns = new[] { "rowguid", "ModifiedDate", "SizeUnitMeasureCode", 
                "WeightUnitMeasureCode", "ProductSubcategoryID", "ProductModelID",
                "DiscontinuedDate", "DaysToManufacture", "ProductLine", "Class", "Style" };

            foreach (var colName in hiddenColumns)
            {
                if (dataGridViewProducts.Columns.Contains(colName))
                    dataGridViewProducts.Columns[colName].Visible = false;
            }

            // Rinomina colonne per l'utente
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
        /// Gestisce la selezione di una riga nella griglia.
        /// Popola i campi dettaglio con i dati del prodotto selezionato.
        /// </summary>
        private void DataGridView_SelectionChanged(object? sender, EventArgs e)
        {
            if (_isNewProduct) return; // Non interferire se stiamo inserendo

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
        /// Popola i campi dettaglio con i dati del prodotto.
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
        /// Pulisce tutti i campi dettaglio.
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
        /// Cerca prodotti per nome.
        /// </summary>
        private async void ButtonSearch_Click(object? sender, EventArgs e)
        {
            string searchTerm = textBoxSearch.Text.Trim();
            
            try
            {
                UpdateStatus("Ricerca in corso...", Color.Blue);
                
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
                
                UpdateStatus($"Trovati {products.Count()} prodotti", Color.Green);
            }
            catch (Exception ex)
            {
                UpdateStatus($"Errore: {ex.Message}", Color.Red);
            }
        }

        /// <summary>
        /// Aggiorna la lista dei prodotti.
        /// </summary>
        private async void ButtonRefresh_Click(object? sender, EventArgs e)
        {
            textBoxSearch.Text = "";
            await LoadProductsAsync();
        }

        /// <summary>
        /// Inizia l'inserimento di un nuovo prodotto.
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
            
            UpdateStatus("Inserisci i dati del nuovo prodotto", Color.Blue);
        }

        /// <summary>
        /// Salva il prodotto (nuovo o modificato).
        /// 
        /// NOTA DIDATTICA:
        /// - Prima validiamo i dati
        /// - Poi costruiamo l'oggetto Product
        /// - Infine chiamiamo il repository appropriato (Insert o Update)
        /// </summary>
        private async void ButtonSave_Click(object? sender, EventArgs e)
        {
            // Validazione
            if (string.IsNullOrWhiteSpace(textBoxName.Text))
            {
                MessageBox.Show("Il nome del prodotto è obbligatorio.", "Validazione",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxProductNumber.Text))
            {
                MessageBox.Show("Il numero prodotto è obbligatorio.", "Validazione",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxProductNumber.Focus();
                return;
            }

            try
            {
                // Costruisci l'oggetto Product dai campi UI
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
                    // Inserimento nuovo prodotto
                    UpdateStatus("Inserimento in corso...", Color.Blue);
                    int newId = await _repository.InsertAsync(product);
                    UpdateStatus($"Prodotto inserito con ID: {newId}", Color.Green);
                    MessageBox.Show($"Prodotto inserito con successo!\nID: {newId}",
                        "Successo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (_currentProduct != null)
                {
                    // Modifica prodotto esistente
                    product.ProductID = _currentProduct.ProductID;
                    UpdateStatus("Salvataggio in corso...", Color.Blue);
                    await _repository.UpdateAsync(product);
                    UpdateStatus("Prodotto aggiornato con successo", Color.Green);
                }

                // Ricarica la lista e torna in modalità visualizzazione
                _isNewProduct = false;
                await LoadProductsAsync();
                SetViewMode();
            }
            catch (Exception ex)
            {
                UpdateStatus($"Errore: {ex.Message}", Color.Red);
                MessageBox.Show($"Errore durante il salvataggio:\n{ex.Message}",
                    "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Elimina il prodotto selezionato.
        /// </summary>
        private async void ButtonDelete_Click(object? sender, EventArgs e)
        {
            if (_currentProduct == null || _isNewProduct)
            {
                MessageBox.Show("Seleziona un prodotto da eliminare.",
                    "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Conferma eliminazione
            var result = MessageBox.Show(
                $"Sei sicuro di voler eliminare il prodotto:\n\n{_currentProduct.Name}?",
                "Conferma eliminazione",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            try
            {
                // Verifica se può essere eliminato
                bool canDelete = await _repository.CanDeleteAsync(_currentProduct.ProductID);
                if (!canDelete)
                {
                    MessageBox.Show(
                        "Impossibile eliminare questo prodotto perché è referenziato in ordini esistenti.",
                        "Eliminazione non consentita",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                UpdateStatus("Eliminazione in corso...", Color.Blue);
                await _repository.DeleteAsync(_currentProduct.ProductID);
                UpdateStatus("Prodotto eliminato", Color.Green);

                // Ricarica la lista
                _currentProduct = null;
                ClearDetails();
                await LoadProductsAsync();
                SetViewMode();
            }
            catch (Exception ex)
            {
                UpdateStatus($"Errore: {ex.Message}", Color.Red);
                MessageBox.Show($"Errore durante l'eliminazione:\n{ex.Message}",
                    "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Annulla l'operazione corrente.
        /// </summary>
        private async void ButtonCancel_Click(object? sender, EventArgs e)
        {
            _isNewProduct = false;
            _currentProduct = null;
            ClearDetails();
            await LoadProductsAsync();
            SetViewMode();
            UpdateStatus("Operazione annullata", Color.Gray);
        }

        /// <summary>
        /// Imposta la form in modalità visualizzazione.
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
        /// Imposta la form in modalità modifica.
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
        /// Imposta la form in modalità nuovo inserimento.
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
        /// Abilita/disabilita i controlli di input.
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
        /// Aggiorna il messaggio di stato.
        /// </summary>
        private void UpdateStatus(string message, Color color)
        {
            labelStatus.Text = message;
            labelStatus.ForeColor = color;
        }
    }
}
