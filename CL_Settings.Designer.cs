﻿namespace MusicBeePlugin
{
    partial class CL_Settings
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
      this.label2 = new System.Windows.Forms.Label();
      this.detectedKeyboardLabel = new System.Windows.Forms.Label();
      this.saveCloseButton = new System.Windows.Forms.Button();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.detectedKeyboardLabel);
      this.groupBox1.Controls.Add(this.label2);
      this.groupBox1.Location = new System.Drawing.Point(13, 13);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(411, 45);
      this.groupBox1.TabIndex = 0;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Detected Devices";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label2.Location = new System.Drawing.Point(7, 20);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(64, 13);
      this.label2.TabIndex = 0;
      this.label2.Text = "Keyboard:";
      // 
      // detectedKeyboardLabel
      // 
      this.detectedKeyboardLabel.AllowDrop = true;
      this.detectedKeyboardLabel.AutoSize = true;
      this.detectedKeyboardLabel.BackColor = System.Drawing.SystemColors.ControlDarkDark;
      this.detectedKeyboardLabel.Location = new System.Drawing.Point(77, 20);
      this.detectedKeyboardLabel.Name = "detectedKeyboardLabel";
      this.detectedKeyboardLabel.Size = new System.Drawing.Size(33, 13);
      this.detectedKeyboardLabel.TabIndex = 1;
      this.detectedKeyboardLabel.Text = "None";
      // 
      // saveCloseButton
      // 
      this.saveCloseButton.Location = new System.Drawing.Point(320, 311);
      this.saveCloseButton.Name = "saveCloseButton";
      this.saveCloseButton.Size = new System.Drawing.Size(104, 28);
      this.saveCloseButton.TabIndex = 1;
      this.saveCloseButton.Text = "Save and Close";
      this.saveCloseButton.UseVisualStyleBackColor = true;
      this.saveCloseButton.Click += new System.EventHandler(this.button1_Click);
      // 
      // CL_Settings
      // 
      this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
      this.ClientSize = new System.Drawing.Size(436, 351);
      this.Controls.Add(this.saveCloseButton);
      this.Controls.Add(this.groupBox1);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "CL_Settings";
      this.ShowIcon = false;
      this.Text = "Corsair LED Plugin Settings";
      this.TopMost = true;
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label detectedKeyboardLabel;
        private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Button saveCloseButton;
  }
}