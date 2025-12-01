/*
 * ============================================================================
 * FORM: SalesOrderDetailForm (Designer)
 * ============================================================================
 * 
 * Form for managing order details from the Sales.SalesOrderDetail table.
 * Can be opened in two modes:
 * 1. General view of all details (with order filter)
 * 2. Specific view for an order (passed from constructor)
 * 
 * TEACHING NOTE:
 * - This form shows relationships between tables
 * - Each detail is linked to an order (SalesOrderID) and a product (ProductID)
 * ============================================================================
 */

namespace AdventureWorksApp.Forms
{
    partial class SalesOrderDetailForm
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
            // Main controls
            this.splitContainer = new SplitContainer();
            this.dataGridViewDetails = new DataGridView();
            this.panelFilter = new Panel();
            this.labelFilterOrder = new Label();
            this.textBoxFilterOrderId = new TextBox();
            this.buttonFilter = new Button();
            this.buttonShowAll = new Button();
            this.labelOrderInfo = new Label();

            // Details panel
            this.groupBoxDetails = new GroupBox();
            this.labelOrderID = new Label();
            this.textBoxOrderID = new TextBox();
            this.labelDetailID = new Label();
            this.textBoxDetailID = new TextBox();
            this.labelOrderNumber = new Label();
            this.textBoxOrderNumber = new TextBox();
            this.labelProductID = new Label();
            this.comboBoxProduct = new ComboBox();
            this.labelProductName = new Label();
            this.textBoxProductName = new TextBox();
            this.labelOrderQty = new Label();
            this.numericUpDownQty = new NumericUpDown();
            this.labelUnitPrice = new Label();
            this.textBoxUnitPrice = new TextBox();
            this.labelDiscount = new Label();
            this.textBoxDiscount = new TextBox();
            this.labelLineTotal = new Label();
            this.textBoxLineTotal = new TextBox();
            this.labelCarrierTracking = new Label();
            this.textBoxCarrierTracking = new TextBox();

            // Action buttons
            this.panelActions = new Panel();
            this.buttonNew = new Button();
            this.buttonSave = new Button();
            this.buttonDelete = new Button();
            this.buttonCancel = new Button();
            this.labelStatusBar = new Label();

            // Suspend layout
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownQty)).BeginInit();
            this.panelFilter.SuspendLayout();
            this.groupBoxDetails.SuspendLayout();
            this.panelActions.SuspendLayout();
            this.SuspendLayout();

            // ============================================================================
            // splitContainer
            // ============================================================================
            this.splitContainer.Dock = DockStyle.Fill;
            this.splitContainer.Orientation = Orientation.Horizontal;
            this.splitContainer.Size = new Size(950, 650);
            this.splitContainer.SplitterDistance = 350;

            this.splitContainer.Panel1.Controls.Add(this.dataGridViewDetails);
            this.splitContainer.Panel1.Controls.Add(this.panelFilter);

            // ============================================================================
            // panelFilter
            // ============================================================================
            this.panelFilter.Dock = DockStyle.Top;
            this.panelFilter.Height = 50;
            this.panelFilter.Padding = new Padding(5);

            this.labelFilterOrder.Text = "Filter by Order ID:";
            this.labelFilterOrder.Location = new Point(10, 15);
            this.labelFilterOrder.AutoSize = true;

            this.textBoxFilterOrderId.Location = new Point(130, 12);
            this.textBoxFilterOrderId.Size = new Size(100, 23);

            this.buttonFilter.Location = new Point(240, 11);
            this.buttonFilter.Size = new Size(80, 25);
            this.buttonFilter.Text = "🔍 Filter";

            this.buttonShowAll.Location = new Point(330, 11);
            this.buttonShowAll.Size = new Size(100, 25);
            this.buttonShowAll.Text = "📋 Show All";

            this.labelOrderInfo.Location = new Point(450, 15);
            this.labelOrderInfo.Size = new Size(450, 20);
            this.labelOrderInfo.ForeColor = Color.DarkBlue;
            this.labelOrderInfo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            this.panelFilter.Controls.AddRange(new Control[] {
                this.labelFilterOrder, this.textBoxFilterOrderId,
                this.buttonFilter, this.buttonShowAll, this.labelOrderInfo
            });

            // ============================================================================
            // dataGridViewDetails
            // ============================================================================
            this.dataGridViewDetails.AllowUserToAddRows = false;
            this.dataGridViewDetails.AllowUserToDeleteRows = false;
            this.dataGridViewDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewDetails.Dock = DockStyle.Fill;
            this.dataGridViewDetails.MultiSelect = false;
            this.dataGridViewDetails.ReadOnly = true;
            this.dataGridViewDetails.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Panel2
            this.splitContainer.Panel2.Controls.Add(this.groupBoxDetails);
            this.splitContainer.Panel2.Controls.Add(this.panelActions);

            // ============================================================================
            // groupBoxDetails
            // ============================================================================
            this.groupBoxDetails.Dock = DockStyle.Fill;
            this.groupBoxDetails.Text = "Order Line Detail";
            this.groupBoxDetails.Padding = new Padding(10);

            // Row 1: Order ID and Detail
            int y = 25;
            this.labelOrderID.Text = "Order ID:";
            this.labelOrderID.Location = new Point(15, y);
            this.labelOrderID.AutoSize = true;
            this.textBoxOrderID.Location = new Point(100, y - 3);
            this.textBoxOrderID.Size = new Size(80, 23);
            this.textBoxOrderID.ReadOnly = true;
            this.textBoxOrderID.BackColor = Color.LightGray;

            this.labelDetailID.Text = "Line ID:";
            this.labelDetailID.Location = new Point(200, y);
            this.labelDetailID.AutoSize = true;
            this.textBoxDetailID.Location = new Point(260, y - 3);
            this.textBoxDetailID.Size = new Size(80, 23);
            this.textBoxDetailID.ReadOnly = true;
            this.textBoxDetailID.BackColor = Color.LightGray;

            this.labelOrderNumber.Text = "Order Number:";
            this.labelOrderNumber.Location = new Point(360, y);
            this.labelOrderNumber.AutoSize = true;
            this.textBoxOrderNumber.Location = new Point(465, y - 3);
            this.textBoxOrderNumber.Size = new Size(120, 23);
            this.textBoxOrderNumber.ReadOnly = true;
            this.textBoxOrderNumber.BackColor = Color.LightGray;

            // Row 2: Product
            y = 60;
            this.labelProductID.Text = "Product:";
            this.labelProductID.Location = new Point(15, y);
            this.labelProductID.AutoSize = true;
            this.comboBoxProduct.Location = new Point(100, y - 3);
            this.comboBoxProduct.Size = new Size(350, 23);
            this.comboBoxProduct.DropDownStyle = ComboBoxStyle.DropDownList;

            this.labelProductName.Text = "Name:";
            this.labelProductName.Location = new Point(470, y);
            this.labelProductName.AutoSize = true;
            this.textBoxProductName.Location = new Point(520, y - 3);
            this.textBoxProductName.Size = new Size(300, 23);
            this.textBoxProductName.ReadOnly = true;
            this.textBoxProductName.BackColor = Color.LightYellow;

            // Row 3: Quantity and price
            y = 100;
            this.labelOrderQty.Text = "Quantity:";
            this.labelOrderQty.Location = new Point(15, y);
            this.labelOrderQty.AutoSize = true;
            this.numericUpDownQty.Location = new Point(100, y - 3);
            this.numericUpDownQty.Size = new Size(80, 23);
            this.numericUpDownQty.Minimum = 1;
            this.numericUpDownQty.Maximum = 9999;

            this.labelUnitPrice.Text = "Unit Price:";
            this.labelUnitPrice.Location = new Point(200, y);
            this.labelUnitPrice.AutoSize = true;
            this.textBoxUnitPrice.Location = new Point(305, y - 3);
            this.textBoxUnitPrice.Size = new Size(100, 23);
            this.textBoxUnitPrice.ReadOnly = true;
            this.textBoxUnitPrice.BackColor = Color.LightYellow;

            this.labelDiscount.Text = "Discount %:";
            this.labelDiscount.Location = new Point(420, y);
            this.labelDiscount.AutoSize = true;
            this.textBoxDiscount.Location = new Point(490, y - 3);
            this.textBoxDiscount.Size = new Size(60, 23);

            this.labelLineTotal.Text = "Line Total:";
            this.labelLineTotal.Location = new Point(570, y);
            this.labelLineTotal.AutoSize = true;
            this.labelLineTotal.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.textBoxLineTotal.Location = new Point(650, y - 3);
            this.textBoxLineTotal.Size = new Size(120, 23);
            this.textBoxLineTotal.ReadOnly = true;
            this.textBoxLineTotal.BackColor = Color.LightGreen;
            this.textBoxLineTotal.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            // Row 4: Tracking
            y = 140;
            this.labelCarrierTracking.Text = "Shipping Tracking:";
            this.labelCarrierTracking.Location = new Point(15, y);
            this.labelCarrierTracking.AutoSize = true;
            this.textBoxCarrierTracking.Location = new Point(140, y - 3);
            this.textBoxCarrierTracking.Size = new Size(200, 23);

            // Add controls
            this.groupBoxDetails.Controls.AddRange(new Control[] {
                this.labelOrderID, this.textBoxOrderID,
                this.labelDetailID, this.textBoxDetailID,
                this.labelOrderNumber, this.textBoxOrderNumber,
                this.labelProductID, this.comboBoxProduct,
                this.labelProductName, this.textBoxProductName,
                this.labelOrderQty, this.numericUpDownQty,
                this.labelUnitPrice, this.textBoxUnitPrice,
                this.labelDiscount, this.textBoxDiscount,
                this.labelLineTotal, this.textBoxLineTotal,
                this.labelCarrierTracking, this.textBoxCarrierTracking
            });

            // ============================================================================
            // panelActions
            // ============================================================================
            this.panelActions.Dock = DockStyle.Bottom;
            this.panelActions.Height = 50;

            this.buttonNew.Location = new Point(10, 12);
            this.buttonNew.Size = new Size(100, 28);
            this.buttonNew.Text = "➕ New";

            this.buttonSave.Location = new Point(120, 12);
            this.buttonSave.Size = new Size(100, 28);
            this.buttonSave.Text = "💾 Save";

            this.buttonDelete.Location = new Point(230, 12);
            this.buttonDelete.Size = new Size(100, 28);
            this.buttonDelete.Text = "🗑️ Delete";

            this.buttonCancel.Location = new Point(340, 12);
            this.buttonCancel.Size = new Size(100, 28);
            this.buttonCancel.Text = "❌ Cancel";

            this.labelStatusBar.Location = new Point(460, 17);
            this.labelStatusBar.Size = new Size(450, 20);
            this.labelStatusBar.ForeColor = Color.Blue;

            this.panelActions.Controls.AddRange(new Control[] {
                this.buttonNew, this.buttonSave, this.buttonDelete,
                this.buttonCancel, this.labelStatusBar
            });

            // ============================================================================
            // SalesOrderDetailForm
            // ============================================================================
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(950, 650);
            this.Controls.Add(this.splitContainer);
            this.Name = "SalesOrderDetailForm";
            this.Text = "Order Details - Sales.SalesOrderDetail";
            this.StartPosition = FormStartPosition.CenterParent;

            // Resume layout
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDetails)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownQty)).EndInit();
            this.panelFilter.ResumeLayout(false);
            this.panelFilter.PerformLayout();
            this.groupBoxDetails.ResumeLayout(false);
            this.groupBoxDetails.PerformLayout();
            this.panelActions.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        // Main controls
        private SplitContainer splitContainer;
        private DataGridView dataGridViewDetails;
        private Panel panelFilter;
        private Label labelFilterOrder;
        private TextBox textBoxFilterOrderId;
        private Button buttonFilter;
        private Button buttonShowAll;
        private Label labelOrderInfo;

        // Detail controls
        private GroupBox groupBoxDetails;
        private Label labelOrderID;
        private TextBox textBoxOrderID;
        private Label labelDetailID;
        private TextBox textBoxDetailID;
        private Label labelOrderNumber;
        private TextBox textBoxOrderNumber;
        private Label labelProductID;
        private ComboBox comboBoxProduct;
        private Label labelProductName;
        private TextBox textBoxProductName;
        private Label labelOrderQty;
        private NumericUpDown numericUpDownQty;
        private Label labelUnitPrice;
        private TextBox textBoxUnitPrice;
        private Label labelDiscount;
        private TextBox textBoxDiscount;
        private Label labelLineTotal;
        private TextBox textBoxLineTotal;
        private Label labelCarrierTracking;
        private TextBox textBoxCarrierTracking;

        // Action buttons
        private Panel panelActions;
        private Button buttonNew;
        private Button buttonSave;
        private Button buttonDelete;
        private Button buttonCancel;
        private Label labelStatusBar;
    }
}
