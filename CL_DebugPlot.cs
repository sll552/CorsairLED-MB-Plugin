using System;
using System.Windows.Forms;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace MusicBeePlugin
{
  public partial class ClDebugPlot : Form
  {
    private readonly PlotModel _model = new PlotModel { Title = "Debug Spectrograph" };
    private readonly ColumnSeries _series = new ColumnSeries();

    public ClDebugPlot()
    {
      InitializeComponent();
      _model.Axes.Add(new CategoryAxis());
      _model.Series.Add(_series);
      plotView1.Model = _model;
      FormClosing += CL_DebugPlot_FormClosing;
    }

    public void UpdatePlot(float[] data)
    {
      float max = 0;
      _series.Items.Clear();
      foreach (float item in data)
      {
        if (item == 0) continue;
        if (item > max) max = item;
        _series.Items.Add(new ColumnItem(item));
      }
      foreach (Axis axis in _model.Axes)
      {
        if (axis.Position != AxisPosition.Bottom && axis.Maximum <= max)
        {
          axis.Maximum = max;
          axis.Minimum = 0;
        }
      }
      _model.InvalidatePlot(true);
      plotView1.InvalidatePlot(true);
      plotView1.Invalidate();
    }

    private void CL_DebugPlot_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (e.CloseReason != CloseReason.UserClosing) return;
      Hide();
      e.Cancel=true;
    }

    private void button1_Click(object sender, EventArgs e)
    {
      Random rnd = new Random();
      float[] data = new float[100];
      for (int i = 0; i < 100; i++)
      {
        data[i] = rnd.Next(100);
        if (i > data.Length / 2)
        {
          data[i] = rnd.Next(data.Length - i);
        }
      }
      UpdatePlot(data);
    }
  }
}
