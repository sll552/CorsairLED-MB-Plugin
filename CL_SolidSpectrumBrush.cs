using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using CUE.NET.Brushes;
using CUE.NET.Devices.Generic;

namespace MusicBeePlugin
{
  class ClSolidSpectrumBrush : SolidColorBrush
  {
    private readonly ClDeviceController _controller;
    private float _scale = 1.0f;
    private float _max = 1.5f;
    private float _min = 0.0f;

    public ClSolidSpectrumBrush(CorsairColor color, ClDeviceController controller) : base(color)
    {
      _controller = controller ?? throw new ArgumentNullException(nameof(controller));
    }

    protected override CorsairColor GetColorAtPoint(RectangleF rectangle, BrushRenderTarget renderTarget)
    {
      float[] bardata = _controller.Curbardata;
      if (bardata == null) return CorsairColor.Transparent;
      int barwidth = (int) Math.Floor(rectangle.Width / bardata.Length);

      // Scale everything to fit the rectangle
      var f = (rectangle.Height / bardata.Max());
      if (f > _scale)
      {
        _scale = f;
      }
      for (int i = 0; i < bardata.Length; i++)
      {
        // make sure that  _min > bardata > _max
        bardata[i] = Math.Min(Math.Max(bardata[i],_min),_max);
        // scale
        //bardata[i] = bardata[i] * 100;
      }

      int baridx = (int) Math.Floor((renderTarget.Point.X - rectangle.Left) / barwidth);
      
      return (_max - bardata[baridx]) / _max < renderTarget.Point.Y / rectangle.Height ? base.GetColorAtPoint(rectangle, renderTarget) : CorsairColor.Transparent;
    }
  }
}
