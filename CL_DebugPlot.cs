using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OxyPlot;
using OxyPlot.WindowsForms;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace MusicBeePlugin
{
  public partial class ClDebugPlot : Form
  {
    private PlotModel _model = new PlotModel { Title = "Debug Spectrograph" };
    private ColumnSeries _series = new ColumnSeries();

    public ClDebugPlot()
    {
      InitializeComponent();
      _model.Axes.Add(new CategoryAxis());
      _model.Series.Add(_series);
      plotView1.Model = _model;
      this.FormClosing += CL_DebugPlot_FormClosing;
    }

    public void UpdatePlot(float[] data)
    {
      _series.Items.Clear();
      foreach (float item in data)
      {
        _series.Items.Add(new ColumnItem(item));
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
