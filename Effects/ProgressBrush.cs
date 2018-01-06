using System.Drawing;
using CUE.NET.Brushes;
using CUE.NET.Devices.Generic;

namespace MusicBeePlugin.Effects
{
  class ProgressBrush : SolidColorBrush
  {

    public float Progress { get; set; }

    public ProgressBrush(CorsairColor color) : base(color)
    {
      Progress = 0;
    }

    protected override CorsairColor GetColorAtPoint(RectangleF rectangle, BrushRenderTarget renderTarget)
    {
      return (Progress > renderTarget.Point.X/rectangle.Width) ? base.GetColorAtPoint(rectangle, renderTarget) : CorsairColor.Transparent;
    }
  }
}
