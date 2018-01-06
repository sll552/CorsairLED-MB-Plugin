using System;
using System.Drawing;
using CUE.NET.Brushes;
using CUE.NET.Devices.Generic;
using MusicBeePlugin.Devices;

namespace MusicBeePlugin.Effects
{
  class SolidSpectrumBrush : SolidColorBrush
  {
    private readonly DeviceController _controller;

    public SolidSpectrumBrush(CorsairColor color, DeviceController controller) : base (color)
    {
      _controller = controller ?? throw new ArgumentNullException(nameof(controller));
    }

    protected override CorsairColor GetColorAtPoint(RectangleF rectangle, BrushRenderTarget renderTarget)
    {
      return _controller.IsInSpectrum(rectangle,renderTarget) ? base.GetColorAtPoint(rectangle, renderTarget) : CorsairColor.Transparent;
    }
  }
}
