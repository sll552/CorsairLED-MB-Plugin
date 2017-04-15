using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OxyPlot;
using OxyPlot.WindowsForms;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace MusicBeePlugin
{
  public partial class CL_DebugPlot : Form
  {
    private PlotModel model = new PlotModel { Title = "Debug Spectrograph" };
    private ColumnSeries series = new ColumnSeries();

    public CL_DebugPlot()
    {
      InitializeComponent();
      model.Axes.Add(new CategoryAxis());
      model.Series.Add(series);
      plotView1.Model = model;
      this.FormClosing += CL_DebugPlot_FormClosing;
    }

    public void UpdatePlot(float[] data)
    {
      series.Items.Clear();
      foreach (float item in data)
      {
        series.Items.Add(new ColumnItem(item));
      }
      plotView1.Invalidate();
    }

    private void CL_DebugPlot_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (e.CloseReason == CloseReason.UserClosing) 
      {
        this.Hide();
        e.Cancel=true;
      }
    }

    
  }
}
