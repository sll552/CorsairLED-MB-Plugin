namespace MusicBeePlugin
{
  partial class ClSettings
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
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.detectedKeyboardLabel = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.saveCloseButton = new System.Windows.Forms.Button();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.backColorPicker = new System.Windows.Forms.Button();
      this.label3 = new System.Windows.Forms.Label();
      this.primaryColorPicker = new System.Windows.Forms.Button();
      this.label2 = new System.Windows.Forms.Label();
      this.colorDialog1 = new System.Windows.Forms.ColorDialog();
      this.label4 = new System.Windows.Forms.Label();
      this.colorModeComboBox = new System.Windows.Forms.ComboBox();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.detectedKeyboardLabel);
      this.groupBox1.Controls.Add(this.label1);
      this.groupBox1.ForeColor = System.Drawing.SystemColors.ControlText;
      this.groupBox1.Location = new System.Drawing.Point(13, 13);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(411, 45);
      this.groupBox1.TabIndex = 0;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Detected Devices";
      // 
      // detectedKeyboardLabel
      // 
      this.detectedKeyboardLabel.AllowDrop = true;
      this.detectedKeyboardLabel.AutoSize = true;
      this.detectedKeyboardLabel.BackColor = System.Drawing.SystemColors.ControlDark;
      this.detectedKeyboardLabel.Location = new System.Drawing.Point(77, 20);
      this.detectedKeyboardLabel.Name = "detectedKeyboardLabel";
      this.detectedKeyboardLabel.Size = new System.Drawing.Size(33, 13);
      this.detectedKeyboardLabel.TabIndex = 1;
      this.detectedKeyboardLabel.Text = "None";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label1.Location = new System.Drawing.Point(7, 20);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(64, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Keyboard:";
      // 
      // saveCloseButton
      // 
      this.saveCloseButton.Location = new System.Drawing.Point(320, 368);
      this.saveCloseButton.Name = "saveCloseButton";
      this.saveCloseButton.Size = new System.Drawing.Size(104, 28);
      this.saveCloseButton.TabIndex = 1;
      this.saveCloseButton.Text = "Save and Close";
      this.saveCloseButton.UseVisualStyleBackColor = true;
      this.saveCloseButton.Click += new System.EventHandler(this.saveCloseButton_Click);
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.colorModeComboBox);
      this.groupBox2.Controls.Add(this.label4);
      this.groupBox2.Controls.Add(this.backColorPicker);
      this.groupBox2.Controls.Add(this.label3);
      this.groupBox2.Controls.Add(this.primaryColorPicker);
      this.groupBox2.Controls.Add(this.label2);
      this.groupBox2.Location = new System.Drawing.Point(13, 65);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(411, 102);
      this.groupBox2.TabIndex = 2;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Effect Settings";
      // 
      // backColorPicker
      // 
      this.backColorPicker.Location = new System.Drawing.Point(265, 15);
      this.backColorPicker.Name = "backColorPicker";
      this.backColorPicker.Size = new System.Drawing.Size(40, 23);
      this.backColorPicker.TabIndex = 4;
      this.backColorPicker.UseVisualStyleBackColor = true;
      this.backColorPicker.Click += new System.EventHandler(this.backColorPicker_Click);
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label3.Location = new System.Drawing.Point(147, 20);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(112, 13);
      this.label3.TabIndex = 3;
      this.label3.Text = "Background Color:";
      // 
      // primaryColorPicker
      // 
      this.primaryColorPicker.Location = new System.Drawing.Point(101, 15);
      this.primaryColorPicker.Name = "primaryColorPicker";
      this.primaryColorPicker.Size = new System.Drawing.Size(40, 23);
      this.primaryColorPicker.TabIndex = 2;
      this.primaryColorPicker.UseVisualStyleBackColor = true;
      this.primaryColorPicker.Click += new System.EventHandler(this.primaryColorPicker_Click);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label2.Location = new System.Drawing.Point(7, 20);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(85, 13);
      this.label2.TabIndex = 1;
      this.label2.Text = "Primary Color:";
      // 
      // colorDialog1
      // 
      this.colorDialog1.AnyColor = true;
      this.colorDialog1.Color = System.Drawing.Color.Red;
      this.colorDialog1.ShowHelp = true;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label4.Location = new System.Drawing.Point(7, 47);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(92, 13);
      this.label4.TabIndex = 6;
      this.label4.Text = "Coloring Mode:";
      // 
      // colorModeComboBox
      // 
      this.colorModeComboBox.BackColor = System.Drawing.SystemColors.ControlDark;
      this.colorModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.colorModeComboBox.FormattingEnabled = true;
      this.colorModeComboBox.Location = new System.Drawing.Point(101, 45);
      this.colorModeComboBox.Name = "colorModeComboBox";
      this.colorModeComboBox.Size = new System.Drawing.Size(121, 21);
      this.colorModeComboBox.TabIndex = 7;
      // 
      // ClSettings
      // 
      this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
      this.ClientSize = new System.Drawing.Size(436, 408);
      this.Controls.Add(this.groupBox2);
      this.Controls.Add(this.saveCloseButton);
      this.Controls.Add(this.groupBox1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ClSettings";
      this.ShowIcon = false;
      this.Text = "Corsair LED Plugin Settings";
      this.TopMost = true;
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.Label detectedKeyboardLabel;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button saveCloseButton;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.Button primaryColorPicker;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.ColorDialog colorDialog1;
    private System.Windows.Forms.Button backColorPicker;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.ComboBox colorModeComboBox;
  }
}