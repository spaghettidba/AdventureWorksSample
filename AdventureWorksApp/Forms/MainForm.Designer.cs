/*
 * ============================================================================
 * FORM: MainForm (Designer)
 * ============================================================================
 * 
 * This file contains the designer-generated code for the main form.
 * 
 * TEACHING NOTE:
 * - In a real project, this code is automatically generated
 *   by the Visual Studio visual editor
 * - Here we write it manually for teaching purposes
 * - InitializeComponent() configures all form controls
 * - Controls are declared as private fields of the partial class
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
        /// Required method for designer support.
        /// Do not modify the contents with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            // Create controls
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

            // Suspend layout for performance
            this.groupBoxConnection.SuspendLayout();
            this.groupBoxNavigation.SuspendLayout();
            this.SuspendLayout();

            // ============================================================================
            // groupBoxConnection - Group for connection configuration
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
            this.groupBoxConnection.Text = "Database Connection Configuration";

            // ============================================================================
            // labelConnectionString - Label for the textbox
            // ============================================================================
            this.labelConnectionString.AutoSize = true;
            this.labelConnectionString.Location = new Point(15, 30);
            this.labelConnectionString.Name = "labelConnectionString";
            this.labelConnectionString.Size = new Size(130, 15);
            this.labelConnectionString.TabIndex = 0;
            this.labelConnectionString.Text = "Connection String:";

            // ============================================================================
            // textBoxConnectionString - Field to enter the connection string
            // ============================================================================
            this.textBoxConnectionString.Location = new Point(15, 50);
            this.textBoxConnectionString.Name = "textBoxConnectionString";
            this.textBoxConnectionString.Size = new Size(630, 23);
            this.textBoxConnectionString.TabIndex = 1;
            this.textBoxConnectionString.PlaceholderText = "Server=localhost;Database=AdventureWorks2019;Integrated Security=True;TrustServerCertificate=True";

            // ============================================================================
            // buttonTestConnection - Button to test the connection
            // ============================================================================
            this.buttonTestConnection.Location = new Point(655, 49);
            this.buttonTestConnection.Name = "buttonTestConnection";
            this.buttonTestConnection.Size = new Size(90, 25);
            this.buttonTestConnection.TabIndex = 2;
            this.buttonTestConnection.Text = "Test";
            this.buttonTestConnection.UseVisualStyleBackColor = true;

            // ============================================================================
            // labelConnectionStatus - Shows the connection status
            // ============================================================================
            this.labelConnectionStatus.AutoSize = true;
            this.labelConnectionStatus.Location = new Point(15, 85);
            this.labelConnectionStatus.Name = "labelConnectionStatus";
            this.labelConnectionStatus.Size = new Size(200, 15);
            this.labelConnectionStatus.TabIndex = 3;
            this.labelConnectionStatus.Text = "Status: Not connected";
            this.labelConnectionStatus.ForeColor = Color.Gray;

            // ============================================================================
            // groupBoxNavigation - Group for navigation between forms
            // ============================================================================
            this.groupBoxNavigation.Controls.Add(this.buttonProducts);
            this.groupBoxNavigation.Controls.Add(this.buttonSalesOrders);
            this.groupBoxNavigation.Controls.Add(this.buttonOrderDetails);
            this.groupBoxNavigation.Location = new Point(12, 220);
            this.groupBoxNavigation.Name = "groupBoxNavigation";
            this.groupBoxNavigation.Size = new Size(760, 180);
            this.groupBoxNavigation.TabIndex = 1;
            this.groupBoxNavigation.TabStop = false;
            this.groupBoxNavigation.Text = "Data Management";

            // ============================================================================
            // buttonProducts - Opens the product management form
            // ============================================================================
            this.buttonProducts.Location = new Point(15, 35);
            this.buttonProducts.Name = "buttonProducts";
            this.buttonProducts.Size = new Size(230, 45);
            this.buttonProducts.TabIndex = 0;
            this.buttonProducts.Text = "📦 Product Management\n(Production.Product)";
            this.buttonProducts.UseVisualStyleBackColor = true;

            // ============================================================================
            // buttonSalesOrders - Opens the order management form
            // ============================================================================
            this.buttonSalesOrders.Location = new Point(265, 35);
            this.buttonSalesOrders.Name = "buttonSalesOrders";
            this.buttonSalesOrders.Size = new Size(230, 45);
            this.buttonSalesOrders.TabIndex = 1;
            this.buttonSalesOrders.Text = "📋 Order Management\n(Sales.SalesOrderHeader)";
            this.buttonSalesOrders.UseVisualStyleBackColor = true;

            // ============================================================================
            // buttonOrderDetails - Opens the order details management form
            // ============================================================================
            this.buttonOrderDetails.Location = new Point(515, 35);
            this.buttonOrderDetails.Name = "buttonOrderDetails";
            this.buttonOrderDetails.Size = new Size(230, 45);
            this.buttonOrderDetails.TabIndex = 2;
            this.buttonOrderDetails.Text = "📝 Order Details\n(Sales.SalesOrderDetail)";
            this.buttonOrderDetails.UseVisualStyleBackColor = true;

            // ============================================================================
            // labelInstructions - Instructions for use
            // ============================================================================
            this.labelInstructions.Location = new Point(15, 100);
            this.labelInstructions.Name = "labelInstructions";
            this.labelInstructions.Size = new Size(730, 70);
            this.labelInstructions.TabIndex = 3;
            this.labelInstructions.Text = @"INSTRUCTIONS:
1. Enter the AdventureWorks database connection string and click 'Test'
2. Once connected, use the buttons above to manage data from various tables
3. Each form allows you to view, insert, modify and delete records";
            this.labelInstructions.ForeColor = Color.DarkBlue;

            // ============================================================================
            // labelWelcome - Welcome title
            // ============================================================================
            this.labelWelcome.AutoSize = true;
            this.labelWelcome.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
            this.labelWelcome.Location = new Point(12, 20);
            this.labelWelcome.Name = "labelWelcome";
            this.labelWelcome.Size = new Size(500, 32);
            this.labelWelcome.TabIndex = 2;
            this.labelWelcome.Text = "AdventureWorks - Teaching Application";
            this.labelWelcome.ForeColor = Color.DarkBlue;

            // ============================================================================
            // MainForm - Main form
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
            this.Text = "AdventureWorks - Main Menu";

            // Resume layout
            this.groupBoxConnection.ResumeLayout(false);
            this.groupBoxConnection.PerformLayout();
            this.groupBoxNavigation.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        // Control declarations
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
