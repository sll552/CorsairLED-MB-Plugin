using System;
using System.Drawing;
using CUE.NET.Brushes;
using CUE.NET.Devices.Generic;
using CUE.NET.Gradients;

namespace MusicBeePlugin
{
  class ClLinGradSpectrumBrush : LinearGradientBrush
  {
    private readonly ClDeviceController _controller;

    public ClLinGradSpectrumBrush(IGradient gradient, ClDeviceController controller) : base (gradient)
    {
      _controller = controller ?? throw new ArgumentNullException(nameof(controller));
    }

    protected override CorsairColor GetColorAtPoint(RectangleF rectangle, BrushRenderTarget renderTarget)
    {
      return _controller.IsInSpectrum(rectangle,renderTarget) ? base.GetColorAtPoint(rectangle, renderTarget) : CorsairColor.Transparent;
    }
  }
}