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
        panel1 = new Panel();
        label1 = new Label();
        label3 = new Label();
        panel1.SuspendLayout();
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
        // panel1
        // 
        panel1.BorderStyle = BorderStyle.FixedSingle;
        panel1.Controls.Add(label1);
        panel1.Location = new Point(42, 50);
        panel1.Name = "panel1";
        panel1.Size = new Size(492, 519);
        panel1.TabIndex = 6;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new Point(237, 251);
        label1.Name = "label1";
        label1.Size = new Size(57, 15);
        label1.TabIndex = 6;
        label1.Text = "LabelOne";
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.BorderStyle = BorderStyle.FixedSingle;
        label3.Location = new Point(42, 30);
        label3.Name = "label3";
        label3.Size = new Size(70, 17);
        label3.TabIndex = 8;
        label3.Text = "   Tab One   ";
        // 
        // BookPrices
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(584, 636);
        Controls.Add(label3);
        Controls.Add(panel1);
        Controls.Add(version_lbl);
        MaximizeBox = false;
        MaximumSize = new Size(600, 675);
        MinimumSize = new Size(600, 675);
        Name = "BookPrices";
        Text = "Book Prices";
        panel1.ResumeLayout(false);
        panel1.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
    private Label version_lbl;
    private Panel panel1;
    private Label label1;
    private Label label3;
}
