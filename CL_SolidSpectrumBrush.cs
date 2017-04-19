using System;
using System.Drawing;
using System.Linq;
using CUE.NET.Brushes;
using CUE.NET.Devices.Generic;

namespace MusicBeePlugin
{
  class ClSolidSpectrumBrush : SolidColorBrush
  {
    private readonly ClDeviceController _controller;

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
      for (int i = 0; i < bardata.Length; i++)
      {
        bardata[i] = bardata[i] * (rectangle.Height / bardata.Max());
      }
      int baridx = (int) Math.Floor((renderTarget.Point.X - rectangle.Left) / barwidth);
      
      return (bardata[baridx] - rectangle.Bottom) > renderTarget.Point.Y ? base.GetColorAtPoint(rectangle, renderTarget) : CorsairColor.Transparent;
    }
  }
}
