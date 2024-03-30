namespace BookPricesApp.GUI;

partial class ErrorModal
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
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        error_rtb = new RichTextBox();
        error_title = new Label();
        SuspendLayout();
        // 
        // error_rtb
        // 
        error_rtb.Location = new Point(34, 49);
        error_rtb.Name = "error_rtb";
        error_rtb.Size = new Size(364, 241);
        error_rtb.TabIndex = 0;
        error_rtb.Text = "";
        // 
        // error_title
        // 
        error_title.AutoSize = true;
        error_title.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
        error_title.Location = new Point(187, 9);
        error_title.Name = "error_title";
        error_title.Size = new Size(58, 25);
        error_title.TabIndex = 1;
        error_title.Text = "Error";
        // 
        // ErrorModal
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = SystemColors.Control;
        ClientSize = new Size(434, 326);
        Controls.Add(error_title);
        Controls.Add(error_rtb);
        FormBorderStyle = FormBorderStyle.FixedToolWindow;
        Name = "ErrorModal";
        ShowIcon = false;
        ShowInTaskbar = false;
        SizeGripStyle = SizeGripStyle.Hide;
        StartPosition = FormStartPosition.CenterParent;
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private RichTextBox error_rtb;
    private Label error_title;
}