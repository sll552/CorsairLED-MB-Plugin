﻿using System;
using System.Drawing;
using CUE.NET.Brushes;
using CUE.NET.Devices.Generic;

namespace MusicBeePlugin
{
  class ClSolidSpectrumBrush : SolidColorBrush
  {
    private readonly ClDeviceController _controller;

    public ClSolidSpectrumBrush(CorsairColor color, ClDeviceController controller) : base (color)
    {
      _controller = controller ?? throw new ArgumentNullException(nameof(controller));
    }

    protected override CorsairColor GetColorAtPoint(RectangleF rectangle, BrushRenderTarget renderTarget)
    {
      return _controller.IsInSpectrum(rectangle,renderTarget) ? base.GetColorAtPoint(rectangle, renderTarget) : CorsairColor.Transparent;
    }
  }
}
