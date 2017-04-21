using System;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace MusicBeePlugin
{
  public partial class ClDebugPlot : Form
  {
    private readonly Series _series;
    private float _max = 0;

    public ClDebugPlot()
    {
      InitializeComponent();

      this.chart1.Titles.Add("Debug Spectrograph");
      _series = this.chart1.Series.Add("fftdata");

      FormClosing += CL_DebugPlot_FormClosing;
    }

    public void UpdatePlot(float[] data)
    {
      if (!Visible)return;
      Invoke( new MethodInvoker(delegate
      {
        _series.Points.Clear();
        foreach (float item in data)
        {
          if (item == 0) continue;
          if (item > _max) _max = item;
          _series.Points.AddY(item);
        }
        chart1.ChartAreas.First().AxisY.Maximum = _max;
        chart1.Refresh();
      }));
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
