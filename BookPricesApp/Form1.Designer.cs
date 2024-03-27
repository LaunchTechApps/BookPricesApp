namespace BookPrices;

partial class BookPrices
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BookPrices));
        version_lbl = new Label();
        amazon_pnl = new Panel();
        progressBar1 = new ProgressBar();
        amazon_title = new Label();
        amazon_tab = new Label();
        ebay_tab = new Label();
        ebay_title = new Label();
        ebay_pnl = new Panel();
        ebay_progress = new ProgressBar();
        label2 = new Label();
        amazon_pnl.SuspendLayout();
        ebay_pnl.SuspendLayout();
        SuspendLayout();
        // 
        // version_lbl
        // 
        version_lbl.AutoSize = true;
        version_lbl.Location = new Point(518, 572);
        version_lbl.Name = "version_lbl";
        version_lbl.Size = new Size(16, 15);
        version_lbl.TabIndex = 5;
        version_lbl.Text = "...";
        // 
        // amazon_pnl
        // 
        amazon_pnl.BorderStyle = BorderStyle.FixedSingle;
        amazon_pnl.Controls.Add(progressBar1);
        amazon_pnl.Controls.Add(amazon_title);
        amazon_pnl.Location = new Point(42, 50);
        amazon_pnl.Name = "amazon_pnl";
        amazon_pnl.Size = new Size(492, 519);
        amazon_pnl.TabIndex = 6;
        // 
        // progressBar1
        // 
        progressBar1.Location = new Point(124, 45);
        progressBar1.Name = "progressBar1";
        progressBar1.Size = new Size(225, 29);
        progressBar1.TabIndex = 2;
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
        amazon_tab.Click += amazon_tb_Click;
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
        ebay_tab.Text = "     Ebay     ";
        ebay_tab.Click += ebay_tb_Click;
        // 
        // ebay_title
        // 
        ebay_title.AutoSize = true;
        ebay_title.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
        ebay_title.Location = new Point(212, 17);
        ebay_title.Name = "ebay_title";
        ebay_title.Size = new Size(54, 25);
        ebay_title.TabIndex = 0;
        ebay_title.Text = "Ebay";
        // 
        // ebay_pnl
        // 
        ebay_pnl.BorderStyle = BorderStyle.FixedSingle;
        ebay_pnl.Controls.Add(ebay_progress);
        ebay_pnl.Controls.Add(ebay_title);
        ebay_pnl.Location = new Point(42, 50);
        ebay_pnl.Name = "ebay_pnl";
        ebay_pnl.Size = new Size(492, 519);
        ebay_pnl.TabIndex = 11;
        // 
        // ebay_progress
        // 
        ebay_progress.Location = new Point(124, 45);
        ebay_progress.Name = "ebay_progress";
        ebay_progress.Size = new Size(225, 29);
        ebay_progress.TabIndex = 1;
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new Point(522, 572);
        label2.Name = "label2";
        label2.Size = new Size(16, 15);
        label2.TabIndex = 10;
        label2.Text = "...";
        // 
        // BookPrices
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(584, 636);
        Controls.Add(label2);
        Controls.Add(ebay_tab);
        Controls.Add(amazon_tab);
        Controls.Add(version_lbl);
        Controls.Add(amazon_pnl);
        Controls.Add(ebay_pnl);
        Icon = (Icon)resources.GetObject("$this.Icon");
        MaximizeBox = false;
        MaximumSize = new Size(600, 675);
        MinimumSize = new Size(600, 675);
        Name = "BookPrices";
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
    private Label label3;
    private Label ebay_tab;
    private Label amazon_title;
    private Label ebay_title;
    private Panel ebay_pnl;
    private Label label2;
    private ProgressBar ebay_progress;
    private ProgressBar progressBar1;
}
