using OxyPlot;
using OxyPlot.WindowsForms;

namespace MusicBeePlugin
{
  partial class ClDebugPlot
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
      this.plotView1 = new OxyPlot.WindowsForms.PlotView();
      this.button1 = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // plotView1
      // 
      this.plotView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.plotView1.Location = new System.Drawing.Point(0, 0);
      this.plotView1.Name = "plotView1";
      this.plotView1.PanCursor = System.Windows.Forms.Cursors.Hand;
      this.plotView1.Size = new System.Drawing.Size(604, 374);
      this.plotView1.TabIndex = 0;
      this.plotView1.Text = "plotView1";
      this.plotView1.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
      this.plotView1.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
      this.plotView1.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
      // 
      // button1
      // 
      this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.button1.Location = new System.Drawing.Point(0, 380);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(604, 23);
      this.button1.TabIndex = 1;
      this.button1.Text = "Random";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // ClDebugPlot
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(604, 403);
      this.Controls.Add(this.button1);
      this.Controls.Add(this.plotView1);
      this.Name = "ClDebugPlot";
      this.Text = "CL_DebugPlot";
      this.ResumeLayout(false);

    }

    #endregion

    private PlotView plotView1;
    private System.Windows.Forms.Button button1;
  }
}