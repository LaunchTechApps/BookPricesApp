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
        version_lbl = new Label();
        amazon_pnl = new Panel();
        amazon_lbl = new Label();
        amazon_tb = new Label();
        ebay_tb = new Label();
        ebay_lbl = new Label();
        ebay_pnl = new Panel();
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
        amazon_pnl.Controls.Add(amazon_lbl);
        amazon_pnl.Location = new Point(42, 50);
        amazon_pnl.Name = "amazon_pnl";
        amazon_pnl.Size = new Size(492, 519);
        amazon_pnl.TabIndex = 6;
        // 
        // amazon_lbl
        // 
        amazon_lbl.AutoSize = true;
        amazon_lbl.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
        amazon_lbl.Location = new Point(181, 17);
        amazon_lbl.Name = "amazon_lbl";
        amazon_lbl.Size = new Size(85, 25);
        amazon_lbl.TabIndex = 0;
        amazon_lbl.Text = "Amazon";
        // 
        // amazon_tb
        // 
        amazon_tb.AutoSize = true;
        amazon_tb.BorderStyle = BorderStyle.FixedSingle;
        amazon_tb.Location = new Point(42, 24);
        amazon_tb.Name = "amazon_tb";
        amazon_tb.Padding = new Padding(3);
        amazon_tb.Size = new Size(77, 23);
        amazon_tb.TabIndex = 8;
        amazon_tb.Text = "   Amazon   ";
        amazon_tb.Click += amazon_tb_Click;
        // 
        // ebay_tb
        // 
        ebay_tb.AutoSize = true;
        ebay_tb.BorderStyle = BorderStyle.FixedSingle;
        ebay_tb.Location = new Point(125, 24);
        ebay_tb.Name = "ebay_tb";
        ebay_tb.Padding = new Padding(3);
        ebay_tb.Size = new Size(70, 23);
        ebay_tb.TabIndex = 9;
        ebay_tb.Text = "     Ebay     ";
        ebay_tb.Click += ebay_tb_Click;
        // 
        // ebay_lbl
        // 
        ebay_lbl.AutoSize = true;
        ebay_lbl.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
        ebay_lbl.Location = new Point(212, 17);
        ebay_lbl.Name = "ebay_lbl";
        ebay_lbl.Size = new Size(54, 25);
        ebay_lbl.TabIndex = 0;
        ebay_lbl.Text = "Ebay";
        // 
        // ebay_pnl
        // 
        ebay_pnl.BorderStyle = BorderStyle.FixedSingle;
        ebay_pnl.Controls.Add(ebay_lbl);
        ebay_pnl.Location = new Point(42, 50);
        ebay_pnl.Name = "ebay_pnl";
        ebay_pnl.Size = new Size(492, 519);
        ebay_pnl.TabIndex = 11;
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
        Controls.Add(ebay_pnl);
        Controls.Add(label2);
        Controls.Add(ebay_tb);
        Controls.Add(amazon_tb);
        Controls.Add(amazon_pnl);
        Controls.Add(version_lbl);
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
    private Label amazon_tb;
    private Panel amazon_pnl;
    private Label label3;
    private Label ebay_tb;
    private Label amazon_lbl;
    private Label ebay_lbl;
    private Panel ebay_pnl;
    private Label label2;
}
