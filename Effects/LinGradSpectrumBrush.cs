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
    private readonly DeviceController _controller;

    public LinGradSpectrumBrush(IGradient gradient, DeviceController controller) : base (gradient)
    {
      _controller = controller ?? throw new ArgumentNullException(nameof(controller));
    }

    protected override CorsairColor GetColorAtPoint(RectangleF rectangle, BrushRenderTarget renderTarget)
    {
      return _controller.IsInSpectrum(rectangle,renderTarget) ? base.GetColorAtPoint(rectangle, renderTarget) : CorsairColor.Transparent;
    }
  }
}