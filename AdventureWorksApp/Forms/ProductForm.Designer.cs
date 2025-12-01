/*
 * ============================================================================
 * FORM: ProductForm (Designer)
 * ============================================================================
 * 
 * Form for managing products from the Production.Product table.
 * Contains a DataGridView to display products and controls for
 * modification/insertion/deletion.
 * 
 * TEACHING NOTE:
 * - The UI is divided into three sections: data grid, details and action buttons
 * - We use SplitContainer to divide the display
 * ============================================================================
 */

namespace AdventureWorksApp.Forms
{
    partial class ProductForm
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
            // Create main controls
            this.splitContainer = new SplitContainer();
            this.dataGridViewProducts = new DataGridView();
            this.panelSearch = new Panel();
            this.textBoxSearch = new TextBox();
            this.buttonSearch = new Button();
            this.buttonRefresh = new Button();

            // Create details panel
            this.groupBoxDetails = new GroupBox();
            this.labelProductID = new Label();
            this.textBoxProductID = new TextBox();
            this.labelName = new Label();
            this.textBoxName = new TextBox();
            this.labelProductNumber = new Label();
            this.textBoxProductNumber = new TextBox();
            this.labelColor = new Label();
            this.textBoxColor = new TextBox();
            this.labelListPrice = new Label();
            this.textBoxListPrice = new TextBox();
            this.labelStandardCost = new Label();
            this.textBoxStandardCost = new TextBox();
            this.labelSafetyStockLevel = new Label();
            this.textBoxSafetyStockLevel = new TextBox();
            this.labelReorderPoint = new Label();
            this.textBoxReorderPoint = new TextBox();
            this.checkBoxMakeFlag = new CheckBox();
            this.checkBoxFinishedGoods = new CheckBox();
            this.labelSellStartDate = new Label();
            this.dateTimePickerSellStart = new DateTimePicker();

            // Action buttons
            this.panelActions = new Panel();
            this.buttonNew = new Button();
            this.buttonSave = new Button();
            this.buttonDelete = new Button();
            this.buttonCancel = new Button();
            this.labelStatus = new Label();

            // Suspend layout
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProducts)).BeginInit();
            this.panelSearch.SuspendLayout();
            this.groupBoxDetails.SuspendLayout();
            this.panelActions.SuspendLayout();
            this.SuspendLayout();

            // ============================================================================
            // splitContainer - Divides the form into grid and details
            // ============================================================================
            this.splitContainer.Dock = DockStyle.Fill;
            this.splitContainer.Location = new Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = Orientation.Horizontal;
            this.splitContainer.Size = new Size(900, 650);
            this.splitContainer.SplitterDistance = 350;
            this.splitContainer.TabIndex = 0;

            // ============================================================================
            // Panel1 - Contains the grid and search
            // ============================================================================
            this.splitContainer.Panel1.Controls.Add(this.dataGridViewProducts);
            this.splitContainer.Panel1.Controls.Add(this.panelSearch);

            // ============================================================================
            // panelSearch - Search bar
            // ============================================================================
            this.panelSearch.Controls.Add(this.textBoxSearch);
            this.panelSearch.Controls.Add(this.buttonSearch);
            this.panelSearch.Controls.Add(this.buttonRefresh);
            this.panelSearch.Dock = DockStyle.Top;
            this.panelSearch.Height = 40;
            this.panelSearch.Padding = new Padding(5);

            this.textBoxSearch.Location = new Point(10, 8);
            this.textBoxSearch.Size = new Size(300, 23);
            this.textBoxSearch.PlaceholderText = "Search by product name...";

            this.buttonSearch.Location = new Point(320, 7);
            this.buttonSearch.Size = new Size(80, 25);
            this.buttonSearch.Text = "🔍 Search";

            this.buttonRefresh.Location = new Point(410, 7);
            this.buttonRefresh.Size = new Size(100, 25);
            this.buttonRefresh.Text = "🔄 Refresh";

            // ============================================================================
            // dataGridViewProducts - Grid to display products
            // ============================================================================
            this.dataGridViewProducts.AllowUserToAddRows = false;
            this.dataGridViewProducts.AllowUserToDeleteRows = false;
            this.dataGridViewProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewProducts.Dock = DockStyle.Fill;
            this.dataGridViewProducts.Location = new Point(0, 40);
            this.dataGridViewProducts.MultiSelect = false;
            this.dataGridViewProducts.Name = "dataGridViewProducts";
            this.dataGridViewProducts.ReadOnly = true;
            this.dataGridViewProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewProducts.TabIndex = 0;

            // ============================================================================
            // Panel2 - Contains details and actions
            // ============================================================================
            this.splitContainer.Panel2.Controls.Add(this.groupBoxDetails);
            this.splitContainer.Panel2.Controls.Add(this.panelActions);

            // ============================================================================
            // groupBoxDetails - Product details panel
            // ============================================================================
            this.groupBoxDetails.Dock = DockStyle.Fill;
            this.groupBoxDetails.Text = "Product Details";
            this.groupBoxDetails.Padding = new Padding(10);

            // Row 1: ID and Name
            int y = 25;
            this.labelProductID.Text = "ID:";
            this.labelProductID.Location = new Point(15, y);
            this.labelProductID.AutoSize = true;
            this.textBoxProductID.Location = new Point(120, y - 3);
            this.textBoxProductID.Size = new Size(80, 23);
            this.textBoxProductID.ReadOnly = true;
            this.textBoxProductID.BackColor = Color.LightGray;

            this.labelName.Text = "Name:";
            this.labelName.Location = new Point(220, y);
            this.labelName.AutoSize = true;
            this.textBoxName.Location = new Point(280, y - 3);
            this.textBoxName.Size = new Size(250, 23);

            this.labelProductNumber.Text = "Number:";
            this.labelProductNumber.Location = new Point(550, y);
            this.labelProductNumber.AutoSize = true;
            this.textBoxProductNumber.Location = new Point(620, y - 3);
            this.textBoxProductNumber.Size = new Size(150, 23);

            // Row 2: Color, Price, Cost
            y = 55;
            this.labelColor.Text = "Color:";
            this.labelColor.Location = new Point(15, y);
            this.labelColor.AutoSize = true;
            this.textBoxColor.Location = new Point(120, y - 3);
            this.textBoxColor.Size = new Size(100, 23);

            this.labelListPrice.Text = "Price:";
            this.labelListPrice.Location = new Point(240, y);
            this.labelListPrice.AutoSize = true;
            this.textBoxListPrice.Location = new Point(300, y - 3);
            this.textBoxListPrice.Size = new Size(100, 23);

            this.labelStandardCost.Text = "Cost:";
            this.labelStandardCost.Location = new Point(420, y);
            this.labelStandardCost.AutoSize = true;
            this.textBoxStandardCost.Location = new Point(470, y - 3);
            this.textBoxStandardCost.Size = new Size(100, 23);

            // Row 3: Stock and Reorder
            y = 85;
            this.labelSafetyStockLevel.Text = "Safety Stock:";
            this.labelSafetyStockLevel.Location = new Point(15, y);
            this.labelSafetyStockLevel.AutoSize = true;
            this.textBoxSafetyStockLevel.Location = new Point(120, y - 3);
            this.textBoxSafetyStockLevel.Size = new Size(80, 23);

            this.labelReorderPoint.Text = "Reorder Point:";
            this.labelReorderPoint.Location = new Point(220, y);
            this.labelReorderPoint.AutoSize = true;
            this.textBoxReorderPoint.Location = new Point(320, y - 3);
            this.textBoxReorderPoint.Size = new Size(80, 23);

            // Row 4: Flags and date
            y = 115;
            this.checkBoxMakeFlag.Text = "Internal Manufacturing";
            this.checkBoxMakeFlag.Location = new Point(15, y);
            this.checkBoxMakeFlag.AutoSize = true;

            this.checkBoxFinishedGoods.Text = "Finished Good";
            this.checkBoxFinishedGoods.Location = new Point(170, y);
            this.checkBoxFinishedGoods.AutoSize = true;

            this.labelSellStartDate.Text = "Sell Start:";
            this.labelSellStartDate.Location = new Point(320, y);
            this.labelSellStartDate.AutoSize = true;
            this.dateTimePickerSellStart.Location = new Point(420, y - 3);
            this.dateTimePickerSellStart.Size = new Size(200, 23);
            this.dateTimePickerSellStart.Format = DateTimePickerFormat.Short;

            // Add controls to groupbox
            this.groupBoxDetails.Controls.AddRange(new Control[]
            {
                this.labelProductID, this.textBoxProductID,
                this.labelName, this.textBoxName,
                this.labelProductNumber, this.textBoxProductNumber,
                this.labelColor, this.textBoxColor,
                this.labelListPrice, this.textBoxListPrice,
                this.labelStandardCost, this.textBoxStandardCost,
                this.labelSafetyStockLevel, this.textBoxSafetyStockLevel,
                this.labelReorderPoint, this.textBoxReorderPoint,
                this.checkBoxMakeFlag, this.checkBoxFinishedGoods,
                this.labelSellStartDate, this.dateTimePickerSellStart
            });

            // ============================================================================
            // panelActions - Action buttons
            // ============================================================================
            this.panelActions.Dock = DockStyle.Bottom;
            this.panelActions.Height = 50;
            this.panelActions.Controls.Add(this.buttonNew);
            this.panelActions.Controls.Add(this.buttonSave);
            this.panelActions.Controls.Add(this.buttonDelete);
            this.panelActions.Controls.Add(this.buttonCancel);
            this.panelActions.Controls.Add(this.labelStatus);

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

            this.labelStatus.Location = new Point(460, 17);
            this.labelStatus.Size = new Size(400, 20);
            this.labelStatus.Text = "";
            this.labelStatus.ForeColor = Color.Blue;

            // ============================================================================
            // ProductForm - Form configuration
            // ============================================================================
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(900, 650);
            this.Controls.Add(this.splitContainer);
            this.Name = "ProductForm";
            this.Text = "Product Management - Production.Product";
            this.StartPosition = FormStartPosition.CenterParent;

            // Resume layout
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProducts)).EndInit();
            this.panelSearch.ResumeLayout(false);
            this.panelSearch.PerformLayout();
            this.groupBoxDetails.ResumeLayout(false);
            this.groupBoxDetails.PerformLayout();
            this.panelActions.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        // Main controls
        private SplitContainer splitContainer;
        private DataGridView dataGridViewProducts;
        private Panel panelSearch;
        private TextBox textBoxSearch;
        private Button buttonSearch;
        private Button buttonRefresh;

        // Detail controls
        private GroupBox groupBoxDetails;
        private Label labelProductID;
        private TextBox textBoxProductID;
        private Label labelName;
        private TextBox textBoxName;
        private Label labelProductNumber;
        private TextBox textBoxProductNumber;
        private Label labelColor;
        private TextBox textBoxColor;
        private Label labelListPrice;
        private TextBox textBoxListPrice;
        private Label labelStandardCost;
        private TextBox textBoxStandardCost;
        private Label labelSafetyStockLevel;
        private TextBox textBoxSafetyStockLevel;
        private Label labelReorderPoint;
        private TextBox textBoxReorderPoint;
        private CheckBox checkBoxMakeFlag;
        private CheckBox checkBoxFinishedGoods;
        private Label labelSellStartDate;
        private DateTimePicker dateTimePickerSellStart;

        // Action controls
        private Panel panelActions;
        private Button buttonNew;
        private Button buttonSave;
        private Button buttonDelete;
        private Button buttonCancel;
        private Label labelStatus;
    }
}
