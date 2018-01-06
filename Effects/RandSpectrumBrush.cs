using System;
using System.Drawing;
using CUE.NET.Brushes;
using CUE.NET.Devices.Generic;
using MusicBeePlugin.Devices;

namespace MusicBeePlugin.Effects
{
  class RandSpectrumBrush : RandomColorBrush
  {
    private readonly DeviceController _controller;

    public RandSpectrumBrush(DeviceController controller)
    {
      _controller = controller ?? throw new ArgumentNullException(nameof(controller));
    }

    protected override CorsairColor GetColorAtPoint(RectangleF rectangle, BrushRenderTarget renderTarget)
    {
      return _controller.IsInSpectrum(rectangle,renderTarget) ? base.GetColorAtPoint(rectangle, renderTarget) : CorsairColor.Transparent;
    }
  }
}