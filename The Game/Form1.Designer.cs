namespace The_Game
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            plTheWorldsWindow = new Panel();
            ctbTheMap = new MyGame.Controls.ColorTextBox();
            label3 = new Label();
            label1 = new Label();
            lblLocalY = new Label();
            lblLocalX = new Label();
            lblGridY = new Label();
            lblGridX = new Label();
            pnlMessageBoard = new Panel();
            rtbMessages = new RichTextBox();
            panel1 = new Panel();
            label5 = new Label();
            lblPlayerHP = new Label();
            lblPlayerMP = new Label();
            lblPlayerRace = new Label();
            lblTileSpeed = new Label();
            lblInit = new Label();
            lblActionCount = new Label();
            lblPlayerLevel = new Label();
            lblPlayerName = new Label();
            label4 = new Label();
            label2 = new Label();
            panel4 = new Panel();
            tbcDynamic = new TabControl();
            Inventory = new TabPage();
            Journal = new TabPage();
            tabPage3 = new TabPage();
            plTheWorldsWindow.SuspendLayout();
            pnlMessageBoard.SuspendLayout();
            panel1.SuspendLayout();
            panel4.SuspendLayout();
            tbcDynamic.SuspendLayout();
            SuspendLayout();
            // 
            // plTheWorldsWindow
            // 
            plTheWorldsWindow.BackColor = Color.Black;
            plTheWorldsWindow.Controls.Add(ctbTheMap);
            plTheWorldsWindow.Controls.Add(label3);
            plTheWorldsWindow.Controls.Add(label1);
            plTheWorldsWindow.Controls.Add(lblLocalY);
            plTheWorldsWindow.Controls.Add(lblLocalX);
            plTheWorldsWindow.Controls.Add(lblGridY);
            plTheWorldsWindow.Controls.Add(lblGridX);
            plTheWorldsWindow.Location = new Point(232, 4);
            plTheWorldsWindow.Margin = new Padding(4);
            plTheWorldsWindow.Name = "plTheWorldsWindow";
            plTheWorldsWindow.Size = new Size(453, 461);
            plTheWorldsWindow.TabIndex = 0;
            // 
            // ctbTheMap
            // 
            ctbTheMap.Anchor = AnchorStyles.None;
            ctbTheMap.BackColor = Color.DarkGray;
            ctbTheMap.DesignTimeLines = new string[]
    {
    "ColorTextBox Ready!",
    "[Design Mode]"
    };
            ctbTheMap.ForeColor = Color.DarkGray;
            ctbTheMap.LetterSpacing = -3;
            ctbTheMap.Location = new Point(4, 53);
            ctbTheMap.Margin = new Padding(3, 3, 0, 3);
            ctbTheMap.Name = "ctbTheMap";
            ctbTheMap.ShadowOffset = new Point(1, 2);
            ctbTheMap.Size = new Size(445, 386);
            ctbTheMap.TabIndex = 7;
            ctbTheMap.Text = "colorTextBox1";
            ctbTheMap.TextAlign = HorizontalAlignment.Center;
            ctbTheMap.VerticalAlign = System.Windows.Forms.VisualStyles.VerticalAlignment.Center;
            ctbTheMap.WidthInChars = 52;
            ctbTheMap.KeyPress += ctbTheMap_KeyPress;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Black;
            label3.Font = new Font("JetBrains Mono", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.ForeColor = SystemColors.ButtonFace;
            label3.Location = new Point(161, 8);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(98, 16);
            label3.TabIndex = 6;
            label3.Text = "Location Name";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(182, 8);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(49, 16);
            label1.TabIndex = 5;
            label1.Text = "label1";
            // 
            // lblLocalY
            // 
            lblLocalY.AutoSize = true;
            lblLocalY.BackColor = Color.DarkGray;
            lblLocalY.Font = new Font("JetBrains Mono", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblLocalY.Location = new Point(372, 34);
            lblLocalY.Margin = new Padding(4, 0, 4, 0);
            lblLocalY.Name = "lblLocalY";
            lblLocalY.Size = new Size(77, 16);
            lblLocalY.TabIndex = 4;
            lblLocalY.Text = "localY: 25";
            // 
            // lblLocalX
            // 
            lblLocalX.AutoSize = true;
            lblLocalX.BackColor = Color.DarkGray;
            lblLocalX.Font = new Font("JetBrains Mono", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblLocalX.Location = new Point(287, 34);
            lblLocalX.Margin = new Padding(4, 0, 4, 0);
            lblLocalX.Name = "lblLocalX";
            lblLocalX.Size = new Size(77, 16);
            lblLocalX.TabIndex = 3;
            lblLocalX.Text = "localX: 50";
            // 
            // lblGridY
            // 
            lblGridY.AutoSize = true;
            lblGridY.BackColor = Color.DarkGray;
            lblGridY.Font = new Font("JetBrains Mono", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblGridY.Location = new Point(96, 34);
            lblGridY.Margin = new Padding(4, 0, 4, 0);
            lblGridY.Name = "lblGridY";
            lblGridY.Size = new Size(84, 16);
            lblGridY.TabIndex = 2;
            lblGridY.Text = "GridY: 1000";
            // 
            // lblGridX
            // 
            lblGridX.AutoSize = true;
            lblGridX.BackColor = Color.DarkGray;
            lblGridX.Font = new Font("JetBrains Mono", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblGridX.Location = new Point(4, 34);
            lblGridX.Margin = new Padding(4, 0, 4, 0);
            lblGridX.Name = "lblGridX";
            lblGridX.Size = new Size(84, 16);
            lblGridX.TabIndex = 1;
            lblGridX.Text = "GridX: 1000";
            // 
            // pnlMessageBoard
            // 
            pnlMessageBoard.BackColor = Color.Black;
            pnlMessageBoard.Controls.Add(rtbMessages);
            pnlMessageBoard.Location = new Point(7, 446);
            pnlMessageBoard.Margin = new Padding(4);
            pnlMessageBoard.Name = "pnlMessageBoard";
            pnlMessageBoard.Size = new Size(850, 139);
            pnlMessageBoard.TabIndex = 1;
            // 
            // rtbMessages
            // 
            rtbMessages.Location = new Point(2, 4);
            rtbMessages.Margin = new Padding(4);
            rtbMessages.Name = "rtbMessages";
            rtbMessages.Size = new Size(844, 132);
            rtbMessages.TabIndex = 0;
            rtbMessages.Text = "";
            // 
            // panel1
            // 
            panel1.BackColor = Color.Black;
            panel1.Controls.Add(label5);
            panel1.Controls.Add(lblPlayerHP);
            panel1.Controls.Add(lblPlayerMP);
            panel1.Controls.Add(lblPlayerRace);
            panel1.Controls.Add(lblTileSpeed);
            panel1.Controls.Add(lblInit);
            panel1.Controls.Add(lblActionCount);
            panel1.Controls.Add(lblPlayerLevel);
            panel1.Controls.Add(lblPlayerName);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(label2);
            panel1.Location = new Point(7, 4);
            panel1.Margin = new Padding(4);
            panel1.Name = "panel1";
            panel1.Size = new Size(217, 401);
            panel1.TabIndex = 2;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.DarkGray;
            label5.Location = new Point(10, 191);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(49, 16);
            label5.TabIndex = 10;
            label5.Text = "label5";
            // 
            // lblPlayerHP
            // 
            lblPlayerHP.AutoSize = true;
            lblPlayerHP.BackColor = Color.DarkGray;
            lblPlayerHP.Location = new Point(10, 79);
            lblPlayerHP.Margin = new Padding(4, 0, 4, 0);
            lblPlayerHP.Name = "lblPlayerHP";
            lblPlayerHP.Size = new Size(77, 16);
            lblPlayerHP.TabIndex = 9;
            lblPlayerHP.Text = "HP 100/100";
            lblPlayerHP.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblPlayerMP
            // 
            lblPlayerMP.AutoSize = true;
            lblPlayerMP.BackColor = Color.DarkGray;
            lblPlayerMP.Location = new Point(10, 105);
            lblPlayerMP.Margin = new Padding(4, 0, 4, 0);
            lblPlayerMP.Name = "lblPlayerMP";
            lblPlayerMP.Size = new Size(77, 16);
            lblPlayerMP.TabIndex = 8;
            lblPlayerMP.Text = "MP 100/100";
            lblPlayerMP.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblPlayerRace
            // 
            lblPlayerRace.AutoSize = true;
            lblPlayerRace.BackColor = Color.DarkGray;
            lblPlayerRace.Location = new Point(14, 30);
            lblPlayerRace.Margin = new Padding(4, 0, 4, 0);
            lblPlayerRace.Name = "lblPlayerRace";
            lblPlayerRace.Size = new Size(56, 16);
            lblPlayerRace.TabIndex = 7;
            lblPlayerRace.Text = "Level: ";
            // 
            // lblTileSpeed
            // 
            lblTileSpeed.AutoSize = true;
            lblTileSpeed.BackColor = Color.DarkGray;
            lblTileSpeed.Location = new Point(10, 135);
            lblTileSpeed.Margin = new Padding(4, 0, 4, 0);
            lblTileSpeed.Name = "lblTileSpeed";
            lblTileSpeed.Size = new Size(56, 16);
            lblTileSpeed.TabIndex = 6;
            lblTileSpeed.Text = "Speed: ";
            // 
            // lblInit
            // 
            lblInit.AutoSize = true;
            lblInit.BackColor = Color.DarkGray;
            lblInit.Location = new Point(10, 221);
            lblInit.Margin = new Padding(4, 0, 4, 0);
            lblInit.Name = "lblInit";
            lblInit.Size = new Size(70, 16);
            lblInit.TabIndex = 5;
            lblInit.Text = "Inititive";
            // 
            // lblActionCount
            // 
            lblActionCount.AutoSize = true;
            lblActionCount.BackColor = Color.DarkGray;
            lblActionCount.FlatStyle = FlatStyle.Flat;
            lblActionCount.Font = new Font("JetBrains Mono", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblActionCount.Location = new Point(10, 160);
            lblActionCount.Margin = new Padding(4, 0, 4, 0);
            lblActionCount.Name = "lblActionCount";
            lblActionCount.Size = new Size(105, 16);
            lblActionCount.TabIndex = 4;
            lblActionCount.Text = "ActionPoints: ";
            lblActionCount.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblPlayerLevel
            // 
            lblPlayerLevel.AutoSize = true;
            lblPlayerLevel.BackColor = Color.DarkGray;
            lblPlayerLevel.Font = new Font("JetBrains Mono", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblPlayerLevel.Location = new Point(154, 30);
            lblPlayerLevel.Margin = new Padding(4, 0, 4, 0);
            lblPlayerLevel.Name = "lblPlayerLevel";
            lblPlayerLevel.Size = new Size(35, 16);
            lblPlayerLevel.TabIndex = 3;
            lblPlayerLevel.Text = "race";
            // 
            // lblPlayerName
            // 
            lblPlayerName.AutoSize = true;
            lblPlayerName.BackColor = Color.DarkGray;
            lblPlayerName.Font = new Font("JetBrains Mono", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblPlayerName.Location = new Point(70, 8);
            lblPlayerName.Margin = new Padding(4, 0, 4, 0);
            lblPlayerName.Name = "lblPlayerName";
            lblPlayerName.Size = new Size(77, 16);
            lblPlayerName.TabIndex = 2;
            lblPlayerName.Text = "PlayerName";
            lblPlayerName.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(88, 8);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(49, 16);
            label4.TabIndex = 1;
            label4.Text = "label4";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(52, 15);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(49, 16);
            label2.TabIndex = 0;
            label2.Text = "label2";
            // 
            // panel4
            // 
            panel4.BackColor = Color.Black;
            panel4.Controls.Add(tbcDynamic);
            panel4.Location = new Point(693, 4);
            panel4.Margin = new Padding(4);
            panel4.Name = "panel4";
            panel4.Size = new Size(217, 401);
            panel4.TabIndex = 4;
            // 
            // tbcDynamic
            // 
            tbcDynamic.Controls.Add(Inventory);
            tbcDynamic.Controls.Add(Journal);
            tbcDynamic.Controls.Add(tabPage3);
            tbcDynamic.Font = new Font("JetBrains Mono", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tbcDynamic.ItemSize = new Size(50, 20);
            tbcDynamic.Location = new Point(14, 15);
            tbcDynamic.Margin = new Padding(4);
            tbcDynamic.Name = "tbcDynamic";
            tbcDynamic.SelectedIndex = 0;
            tbcDynamic.Size = new Size(203, 386);
            tbcDynamic.TabIndex = 0;
            tbcDynamic.Tag = "";
            // 
            // Inventory
            // 
            Inventory.AccessibleDescription = "Inventory";
            Inventory.AccessibleName = "Inventory";
            Inventory.Location = new Point(4, 24);
            Inventory.Margin = new Padding(4);
            Inventory.Name = "Inventory";
            Inventory.Padding = new Padding(4);
            Inventory.Size = new Size(195, 358);
            Inventory.TabIndex = 0;
            Inventory.Text = "tabPage1";
            Inventory.UseVisualStyleBackColor = true;
            // 
            // Journal
            // 
            Journal.AccessibleDescription = "The players journal";
            Journal.AccessibleName = "PlayerJournal";
            Journal.Location = new Point(4, 24);
            Journal.Margin = new Padding(4);
            Journal.Name = "Journal";
            Journal.Padding = new Padding(4);
            Journal.Size = new Size(195, 358);
            Journal.TabIndex = 1;
            Journal.Text = "tabPage2";
            Journal.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            tabPage3.Location = new Point(4, 24);
            tabPage3.Margin = new Padding(4);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(4);
            tabPage3.Size = new Size(195, 358);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "tabPage3";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.AppWorkspace;
            ClientSize = new Size(914, 601);
            Controls.Add(panel4);
            Controls.Add(panel1);
            Controls.Add(pnlMessageBoard);
            Controls.Add(plTheWorldsWindow);
            Font = new Font("JetBrains Mono", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 1, true);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(4);
            Name = "Form1";
            Text = "Form1";
            plTheWorldsWindow.ResumeLayout(false);
            plTheWorldsWindow.PerformLayout();
            pnlMessageBoard.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel4.ResumeLayout(false);
            tbcDynamic.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel plTheWorldsWindow;
        private Panel pnlMessageBoard;
        private Label lblLocalY;
        private Label lblLocalX;
        private Label lblGridY;
        private Label lblGridX;
        private Panel panel1;
        private Panel panel4;
        private Label label1;
        private Label label2;
        private TabControl tbcDynamic;
        private TabPage Inventory;
        private TabPage Journal;
        private TabPage tabPage3;
        private Label label3;
        private Label label4;
        private Label lblPlayerHP;
        private Label lblPlayerMP;
        private Label lblPlayerRace;
        private Label lblTileSpeed;
        private Label lblInit;
        private Label lblActionCount;
        private Label lblPlayerLevel;
        private Label lblPlayerName;
        private Label label5;
        private RichTextBox rtbMessages;
        private MyGame.Controls.ColorTextBox ctbTheMap;
    }
}
