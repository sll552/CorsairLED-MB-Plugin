using System;
using System.Drawing;
using CUE.NET.Brushes;
using CUE.NET.Devices.Generic;
using CUE.NET.Gradients;
using MusicBeePlugin.Devices;

namespace MusicBeePlugin.Effects
{
  class LinGradSpectrumBrush : LinearGradientBrush
  {
    private readonly ISpectrumDevice _device;

    public LinGradSpectrumBrush(IGradient gradient, ISpectrumDevice device) : base (gradient)
    {
      _device = device ?? throw new ArgumentNullException(nameof(device));
    }

    protected override CorsairColor GetColorAtPoint(RectangleF rectangle, BrushRenderTarget renderTarget)
    {
      return _device.IsInSpectrum(rectangle,renderTarget) ? base.GetColorAtPoint(rectangle, renderTarget) : CorsairColor.Transparent;
    }
  }
}