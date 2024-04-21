namespace BookPrices;

partial class MainForm
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
        amazon_pnl = new Panel();
        amazon_status_lbl = new Label();
        amazon_main_btn = new Button();
        amazon_text_box = new TextBox();
        amazon_select_btn = new Button();
        amazon_progress = new ProgressBar();
        amazon_title = new Label();
        amazon_tab = new Label();
        ebay_tab = new Label();
        ebay_title = new Label();
        ebay_pnl = new Panel();
        ebay_main_btn = new Button();
        ebay_text_box = new TextBox();
        ebay_select_btn = new Button();
        ebay_progress = new ProgressBar();
        version_lbl = new Label();
        filePicker = new OpenFileDialog();
        ebay_status_lbl = new Label();
        amazon_pnl.SuspendLayout();
        ebay_pnl.SuspendLayout();
        SuspendLayout();
        // 
        // amazon_pnl
        // 
        amazon_pnl.BorderStyle = BorderStyle.FixedSingle;
        amazon_pnl.Controls.Add(amazon_status_lbl);
        amazon_pnl.Controls.Add(amazon_main_btn);
        amazon_pnl.Controls.Add(amazon_text_box);
        amazon_pnl.Controls.Add(amazon_select_btn);
        amazon_pnl.Controls.Add(amazon_progress);
        amazon_pnl.Controls.Add(amazon_title);
        amazon_pnl.Location = new Point(42, 50);
        amazon_pnl.Name = "amazon_pnl";
        amazon_pnl.Size = new Size(492, 519);
        amazon_pnl.TabIndex = 6;
        // 
        // amazon_status_lbl
        // 
        amazon_status_lbl.AutoSize = true;
        amazon_status_lbl.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
        amazon_status_lbl.Location = new Point(124, 77);
        amazon_status_lbl.Name = "amazon_status_lbl";
        amazon_status_lbl.Size = new Size(17, 17);
        amazon_status_lbl.TabIndex = 15;
        amazon_status_lbl.Text = "...";
        // 
        // amazon_main_btn
        // 
        amazon_main_btn.BackColor = Color.FromArgb(192, 255, 192);
        amazon_main_btn.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point);
        amazon_main_btn.Location = new Point(159, 414);
        amazon_main_btn.Name = "amazon_main_btn";
        amazon_main_btn.Size = new Size(167, 53);
        amazon_main_btn.TabIndex = 14;
        amazon_main_btn.Text = "Start";
        amazon_main_btn.UseVisualStyleBackColor = false;
        amazon_main_btn.Click += amazon_main_btn_Click;
        // 
        // amazon_text_box
        // 
        amazon_text_box.Location = new Point(159, 233);
        amazon_text_box.Name = "amazon_text_box";
        amazon_text_box.Size = new Size(250, 23);
        amazon_text_box.TabIndex = 13;
        // 
        // amazon_select_btn
        // 
        amazon_select_btn.Location = new Point(82, 233);
        amazon_select_btn.Name = "amazon_select_btn";
        amazon_select_btn.Size = new Size(75, 23);
        amazon_select_btn.TabIndex = 12;
        amazon_select_btn.Text = "Select";
        amazon_select_btn.UseVisualStyleBackColor = true;
        amazon_select_btn.Click += amazon_select_btn_Click;
        // 
        // amazon_progress
        // 
        amazon_progress.Location = new Point(124, 45);
        amazon_progress.Name = "amazon_progress";
        amazon_progress.Size = new Size(238, 29);
        amazon_progress.TabIndex = 2;
        // 
        // amazon_title
        // 
        amazon_title.AutoSize = true;
        amazon_title.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
        amazon_title.Location = new Point(202, 17);
        amazon_title.Name = "amazon_title";
        amazon_title.Size = new Size(85, 25);
        amazon_title.TabIndex = 0;
        amazon_title.Text = "Amazon";
        // 
        // amazon_tab
        // 
        amazon_tab.AutoSize = true;
        amazon_tab.BorderStyle = BorderStyle.FixedSingle;
        amazon_tab.Location = new Point(42, 24);
        amazon_tab.Name = "amazon_tab";
        amazon_tab.Padding = new Padding(3);
        amazon_tab.Size = new Size(77, 23);
        amazon_tab.TabIndex = 8;
        amazon_tab.Text = "   Amazon   ";
        amazon_tab.Click += amazon_tab_Click;
        // 
        // ebay_tab
        // 
        ebay_tab.AutoSize = true;
        ebay_tab.BorderStyle = BorderStyle.FixedSingle;
        ebay_tab.Location = new Point(125, 24);
        ebay_tab.Name = "ebay_tab";
        ebay_tab.Padding = new Padding(3);
        ebay_tab.Size = new Size(70, 23);
        ebay_tab.TabIndex = 9;
        ebay_tab.Text = "     eBay     ";
        ebay_tab.Click += ebay_tab_Click;
        // 
        // ebay_title
        // 
        ebay_title.AutoSize = true;
        ebay_title.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
        ebay_title.Location = new Point(212, 17);
        ebay_title.Name = "ebay_title";
        ebay_title.Size = new Size(54, 25);
        ebay_title.TabIndex = 0;
        ebay_title.Text = "eBay";
        // 
        // ebay_pnl
        // 
        ebay_pnl.BorderStyle = BorderStyle.FixedSingle;
        ebay_pnl.Controls.Add(ebay_status_lbl);
        ebay_pnl.Controls.Add(ebay_main_btn);
        ebay_pnl.Controls.Add(ebay_text_box);
        ebay_pnl.Controls.Add(ebay_select_btn);
        ebay_pnl.Controls.Add(ebay_progress);
        ebay_pnl.Controls.Add(ebay_title);
        ebay_pnl.Location = new Point(42, 50);
        ebay_pnl.Name = "ebay_pnl";
        ebay_pnl.Size = new Size(492, 519);
        ebay_pnl.TabIndex = 11;
        // 
        // ebay_main_btn
        // 
        ebay_main_btn.BackColor = Color.FromArgb(192, 255, 192);
        ebay_main_btn.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point);
        ebay_main_btn.Location = new Point(159, 414);
        ebay_main_btn.Name = "ebay_main_btn";
        ebay_main_btn.Size = new Size(167, 53);
        ebay_main_btn.TabIndex = 18;
        ebay_main_btn.Text = "Start";
        ebay_main_btn.UseVisualStyleBackColor = false;
        ebay_main_btn.Click += ebay_main_btn_Click;
        // 
        // ebay_text_box
        // 
        ebay_text_box.Location = new Point(159, 233);
        ebay_text_box.Name = "ebay_text_box";
        ebay_text_box.Size = new Size(250, 23);
        ebay_text_box.TabIndex = 17;
        // 
        // ebay_select_btn
        // 
        ebay_select_btn.Location = new Point(82, 233);
        ebay_select_btn.Name = "ebay_select_btn";
        ebay_select_btn.Size = new Size(75, 23);
        ebay_select_btn.TabIndex = 16;
        ebay_select_btn.Text = "Select";
        ebay_select_btn.UseVisualStyleBackColor = true;
        ebay_select_btn.Click += ebay_select_btn_Click;
        // 
        // ebay_progress
        // 
        ebay_progress.Location = new Point(124, 45);
        ebay_progress.Name = "ebay_progress";
        ebay_progress.Size = new Size(238, 29);
        ebay_progress.TabIndex = 1;
        // 
        // version_lbl
        // 
        version_lbl.AutoSize = true;
        version_lbl.Location = new Point(12, 612);
        version_lbl.Name = "version_lbl";
        version_lbl.Size = new Size(16, 15);
        version_lbl.TabIndex = 10;
        version_lbl.Text = "...";
        // 
        // ebay_status_lbl
        // 
        ebay_status_lbl.AutoSize = true;
        ebay_status_lbl.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
        ebay_status_lbl.Location = new Point(124, 77);
        ebay_status_lbl.Name = "ebay_status_lbl";
        ebay_status_lbl.Size = new Size(17, 17);
        ebay_status_lbl.TabIndex = 19;
        ebay_status_lbl.Text = "...";
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(584, 636);
        Controls.Add(version_lbl);
        Controls.Add(ebay_tab);
        Controls.Add(amazon_tab);
        Controls.Add(ebay_pnl);
        Controls.Add(amazon_pnl);
        Icon = (Icon)resources.GetObject("$this.Icon");
        MaximizeBox = false;
        MaximumSize = new Size(600, 675);
        MinimumSize = new Size(600, 675);
        Name = "MainForm";
        Text = "Book Prices";
        amazon_pnl.ResumeLayout(false);
        amazon_pnl.PerformLayout();
        ebay_pnl.ResumeLayout(false);
        ebay_pnl.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
    private Label version_lbl;
    private Label amazon_tab;
    private Panel amazon_pnl;
    private Label ebay_tab;
    private Label amazon_title;
    private Label ebay_title;
    private Panel ebay_pnl;
    private ProgressBar ebay_progress;
    private ProgressBar amazon_progress;
    private OpenFileDialog filePicker;
    private Button amazon_select_btn;
    private TextBox amazon_text_box;
    private Button amazon_main_btn;
    private Button ebay_main_btn;
    private TextBox ebay_text_box;
    private Button ebay_select_btn;
    private Label amazon_status_lbl;
    private Label ebay_status_lbl;
}
