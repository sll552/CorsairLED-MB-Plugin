namespace MusicBeePlugin.UI
{
  partial class DeviceSettings
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.lightbarProgCheckBox = new System.Windows.Forms.CheckBox();
      this.colorModeComboBox = new System.Windows.Forms.ComboBox();
      this.label4 = new System.Windows.Forms.Label();
      this.backColorPicker = new System.Windows.Forms.Button();
      this.label3 = new System.Windows.Forms.Label();
      this.primaryColorPicker = new System.Windows.Forms.Button();
      this.label2 = new System.Windows.Forms.Label();
      this.colorDialog1 = new System.Windows.Forms.ColorDialog();
      this.groupBox2.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBox2
      // 
      this.groupBox2.AutoSize = true;
      this.groupBox2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.groupBox2.Controls.Add(this.lightbarProgCheckBox);
      this.groupBox2.Controls.Add(this.colorModeComboBox);
      this.groupBox2.Controls.Add(this.label4);
      this.groupBox2.Controls.Add(this.backColorPicker);
      this.groupBox2.Controls.Add(this.label3);
      this.groupBox2.Controls.Add(this.primaryColorPicker);
      this.groupBox2.Controls.Add(this.label2);
      this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.groupBox2.Location = new System.Drawing.Point(0, 0);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(311, 110);
      this.groupBox2.TabIndex = 3;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Device Settings";
      // 
      // lightbarProgCheckBox
      // 
      this.lightbarProgCheckBox.AutoSize = true;
      this.lightbarProgCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lightbarProgCheckBox.Location = new System.Drawing.Point(10, 74);
      this.lightbarProgCheckBox.Name = "lightbarProgCheckBox";
      this.lightbarProgCheckBox.Size = new System.Drawing.Size(248, 17);
      this.lightbarProgCheckBox.TabIndex = 8;
      this.lightbarProgCheckBox.Text = "Use K95P lightbar as track progressbar";
      this.lightbarProgCheckBox.UseVisualStyleBackColor = true;
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
      // DeviceSettings
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.AutoSize = true;
      this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.Controls.Add(this.groupBox2);
      this.Name = "DeviceSettings";
      this.Size = new System.Drawing.Size(311, 110);
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.CheckBox lightbarProgCheckBox;
    private System.Windows.Forms.ComboBox colorModeComboBox;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Button backColorPicker;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Button primaryColorPicker;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.ColorDialog colorDialog1;
  }
}
