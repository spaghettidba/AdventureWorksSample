/*
 * ============================================================================
 * FORM: SalesOrderDetailForm (Logica)
 * ============================================================================
 * 
 * Form per la gestione dei dettagli ordine dalla tabella Sales.SalesOrderDetail.
 * 
 * CARATTERISTICHE:
 * - Può mostrare tutti i dettagli o filtrarli per ordine
 * - Mostra le relazioni: visualizza nome prodotto dalla tabella Product
 * - Permette di aggiungere/modificare/eliminare righe ordine
 * 
 * NOTA DIDATTICA:
 * - Esempio di form con filtro parametrico
 * - Caricamento dati correlati (prodotti) per selezione
 * - Chiave primaria composita (SalesOrderID + SalesOrderDetailID)
 * ============================================================================
 */

using AdventureWorksApp.DataAccess;
using AdventureWorksApp.Models;

namespace AdventureWorksApp.Forms
{
    /// <summary>
    /// Form per la gestione CRUD della tabella Sales.SalesOrderDetail.
    /// </summary>
    public partial class SalesOrderDetailForm : Form
    {
        private readonly SalesOrderDetailRepository _detailRepository;
        private readonly ProductRepository _productRepository;
        private readonly SalesOrderHeaderRepository _headerRepository;
        
        private SalesOrderDetail? _currentDetail;
        private bool _isNewDetail;
        private int? _filteredOrderId;
        private List<Product> _products = new();

        /// <summary>
        /// Costruttore senza parametri - mostra tutti i dettagli.
        /// </summary>
        public SalesOrderDetailForm() : this(null)
        {
        }

        /// <summary>
        /// Costruttore con ID ordine - mostra solo i dettagli di quell'ordine.
        /// 
        /// NOTA DIDATTICA:
        /// - Overload del costruttore per supportare diverse modalità di apertura
        /// - Se orderId è valorizzato, filtriamo automaticamente per quell'ordine
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
            // Carica i prodotti per la combo box
            await LoadProductsAsync();
            
            // Se abbiamo un ID ordine, filtriamo automaticamente
            if (_filteredOrderId.HasValue)
            {
                textBoxFilterOrderId.Text = _filteredOrderId.Value.ToString();
                await LoadDetailsForOrderAsync(_filteredOrderId.Value);
                // Nascondi i pulsanti di filtro se siamo in modalità filtrata
                buttonShowAll.Visible = false;
            }
            else
            {
                await LoadAllDetailsAsync();
            }
            
            SetViewMode();
        }

        /// <summary>
        /// Carica tutti i prodotti per la combo box di selezione.
        /// </summary>
        private async Task LoadProductsAsync()
        {
            try
            {
                _products = (await _productRepository.GetAllAsync()).ToList();
                
                comboBoxProduct.DisplayMember = "DisplayText";
                comboBoxProduct.ValueMember = "ProductID";
                
                // Creiamo una lista anonima con DisplayText formattato
                var items = _products.Select(p => new
                {
                    p.ProductID,
                    DisplayText = $"{p.ProductID} - {p.Name} ({p.ListPrice:C})"
                }).ToList();
                
                comboBoxProduct.DataSource = items;
            }
            catch (Exception ex)
            {
                UpdateStatus($"Errore caricamento prodotti: {ex.Message}", Color.Red);
            }
        }

        /// <summary>
        /// Carica tutti i dettagli ordine.
        /// </summary>
        private async Task LoadAllDetailsAsync()
        {
            try
            {
                UpdateStatus("Caricamento dettagli...", Color.Blue);
                var details = await _detailRepository.GetAllAsync();
                dataGridViewDetails.DataSource = details.ToList();
                ConfigureGridColumns();
                labelOrderInfo.Text = "";
                UpdateStatus($"Caricati {details.Count()} dettagli", Color.Green);
            }
            catch (Exception ex)
            {
                UpdateStatus($"Errore: {ex.Message}", Color.Red);
            }
        }

        /// <summary>
        /// Carica i dettagli di un ordine specifico.
        /// 
        /// NOTA DIDATTICA:
        /// - Questo metodo mostra come filtrare i dati per una chiave esterna
        /// - Recuperiamo anche info dell'ordine per mostrarle nell'header
        /// </summary>
        private async Task LoadDetailsForOrderAsync(int orderId)
        {
            try
            {
                UpdateStatus("Caricamento dettagli ordine...", Color.Blue);
                
                // Carica info ordine
                var order = await _headerRepository.GetByIdAsync(orderId);
                if (order != null)
                {
                    labelOrderInfo.Text = $"Ordine: {order.SalesOrderNumber} del {order.OrderDate:dd/MM/yyyy} - Totale: {order.TotalDue:C}";
                    this.Text = $"Dettagli Ordine {order.SalesOrderNumber}";
                }
                
                // Carica dettagli
                var details = await _detailRepository.GetByOrderIdAsync(orderId);
                dataGridViewDetails.DataSource = details.ToList();
                ConfigureGridColumns();
                
                UpdateStatus($"Caricati {details.Count()} righe per l'ordine", Color.Green);
            }
            catch (Exception ex)
            {
                UpdateStatus($"Errore: {ex.Message}", Color.Red);
            }
        }

        private void ConfigureGridColumns()
        {
            if (dataGridViewDetails.Columns.Count == 0) return;

            var hidden = new[] { "rowguid", "ModifiedDate", "SpecialOfferID", "CalculatedLineTotal" };
            foreach (var col in hidden)
                if (dataGridViewDetails.Columns.Contains(col))
                    dataGridViewDetails.Columns[col].Visible = false;

            SetHeader("SalesOrderID", "ID Ordine");
            SetHeader("SalesOrderDetailID", "ID Riga");
            SetHeader("SalesOrderNumber", "N° Ordine");
            SetHeader("ProductID", "ID Prodotto");
            SetHeader("ProductName", "Prodotto");
            SetHeader("OrderQty", "Quantità");
            SetHeader("UnitPrice", "Prezzo Unit.");
            SetHeader("UnitPriceDiscount", "Sconto");
            SetHeader("LineTotal", "Totale Riga");
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
            
            // Seleziona il prodotto nella combo
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
        /// Quando si seleziona un prodotto, aggiorna nome e prezzo.
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
                MessageBox.Show("Inserisci un ID ordine valido.", "Validazione",
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
            // Per creare un nuovo dettaglio, dobbiamo avere un ordine selezionato
            if (!_filteredOrderId.HasValue)
            {
                MessageBox.Show("Per inserire un nuovo dettaglio, filtra prima per un ordine specifico.",
                    "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _isNewDetail = true;
            _currentDetail = new SalesOrderDetail
            {
                SalesOrderID = _filteredOrderId.Value,
                OrderQty = 1,
                SpecialOfferID = 1, // "No Discount" - default in AdventureWorks
                UnitPriceDiscount = 0
            };

            ClearDetails();
            textBoxOrderID.Text = _filteredOrderId.Value.ToString();
            textBoxDetailID.Text = "(Nuovo)";
            SetNewMode();
            comboBoxProduct.Focus();
            UpdateStatus("Inserisci i dati della nuova riga ordine", Color.Blue);
        }

        private async void ButtonSave_Click(object? sender, EventArgs e)
        {
            // Validazione
            if (comboBoxProduct.SelectedItem == null)
            {
                MessageBox.Show("Seleziona un prodotto.", "Validazione",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Ottieni ID prodotto dalla combo
                var item = comboBoxProduct.SelectedItem;
                var propId = item.GetType().GetProperty("ProductID");
                int productId = (int)propId!.GetValue(item)!;

                // Ottieni il prezzo dal prodotto
                var product = _products.FirstOrDefault(p => p.ProductID == productId);
                decimal unitPrice = product?.ListPrice ?? 0;

                // Sconto (converti da percentuale a decimale)
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
                    SpecialOfferID = 1, // Default
                    CarrierTrackingNumber = string.IsNullOrWhiteSpace(textBoxCarrierTracking.Text) 
                        ? null : textBoxCarrierTracking.Text
                };

                if (_isNewDetail)
                {
                    UpdateStatus("Inserimento...", Color.Blue);
                    int newDetailId = await _detailRepository.InsertAsync(detail);
                    UpdateStatus($"Riga inserita con ID: {newDetailId}", Color.Green);
                    MessageBox.Show($"Riga ordine inserita!\nID Dettaglio: {newDetailId}",
                        "Successo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (_currentDetail != null)
                {
                    detail.SalesOrderDetailID = _currentDetail.SalesOrderDetailID;
                    UpdateStatus("Salvataggio...", Color.Blue);
                    await _detailRepository.UpdateAsync(detail);
                    UpdateStatus("Riga aggiornata", Color.Green);
                }

                _isNewDetail = false;
                
                // Ricarica i dati
                if (_filteredOrderId.HasValue)
                    await LoadDetailsForOrderAsync(_filteredOrderId.Value);
                else
                    await LoadAllDetailsAsync();
                    
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
            if (_currentDetail == null || _isNewDetail)
            {
                MessageBox.Show("Seleziona una riga da eliminare.", "Attenzione",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(
                $"Eliminare la riga {_currentDetail.SalesOrderDetailID} " +
                $"(Prodotto: {_currentDetail.ProductName})?",
                "Conferma eliminazione",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            try
            {
                UpdateStatus("Eliminazione...", Color.Blue);
                await _detailRepository.DeleteAsync(
                    _currentDetail.SalesOrderID, 
                    _currentDetail.SalesOrderDetailID);
                UpdateStatus("Riga eliminata", Color.Green);

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
                UpdateStatus($"Errore: {ex.Message}", Color.Red);
                MessageBox.Show($"Errore:\n{ex.Message}", "Errore",
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
            UpdateStatus("Operazione annullata", Color.Gray);
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
            // In modifica, non permettiamo di cambiare prodotto
            comboBoxProduct.Enabled = false;
        }

        private void SetNewMode()
        {
            SetControlsEnabled(true);
            comboBoxProduct.Enabled = true; // In nuovo, possiamo selezionare prodotto
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
