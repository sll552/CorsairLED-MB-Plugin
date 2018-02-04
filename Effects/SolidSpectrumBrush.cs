using System;
using System.Drawing;
using CUE.NET.Brushes;
using CUE.NET.Devices.Generic;
using MusicBeePlugin.Devices;

namespace MusicBeePlugin.Effects
{
  class SolidSpectrumBrush : SolidColorBrush
  {
    private readonly ISpectrumDevice _device;

    public SolidSpectrumBrush(CorsairColor color, ISpectrumDevice device) : base (color)
    {
      _device = device ?? throw new ArgumentNullException(nameof(device));
    }

    protected override CorsairColor GetColorAtPoint(RectangleF rectangle, BrushRenderTarget renderTarget)
    {
      return _device.IsInSpectrum(rectangle,renderTarget) ? base.GetColorAtPoint(rectangle, renderTarget) : CorsairColor.Transparent;
    }
  }
}
