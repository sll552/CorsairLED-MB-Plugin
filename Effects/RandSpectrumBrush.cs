using System;
using System.Drawing;
using CUE.NET.Brushes;
using CUE.NET.Devices.Generic;
using MusicBeePlugin.Devices;

namespace MusicBeePlugin.Effects
{
  class RandSpectrumBrush : RandomColorBrush
  {
    private readonly ISpectrumDevice _device;

    public RandSpectrumBrush(ISpectrumDevice device)
    {
      _device = device ?? throw new ArgumentNullException(nameof(device));
    }

    protected override CorsairColor GetColorAtPoint(RectangleF rectangle, BrushRenderTarget renderTarget)
    {
      return _device.IsInSpectrum(rectangle,renderTarget) ? base.GetColorAtPoint(rectangle, renderTarget) : CorsairColor.Transparent;
    }
  }
}