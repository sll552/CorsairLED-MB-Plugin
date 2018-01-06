using System;
using System.Drawing;
using CUE.NET.Brushes;
using CUE.NET.Gradients;
using MusicBeePlugin.Devices;

namespace MusicBeePlugin.Effects
{
  public class SpectrumBrushFactory
  {
    public enum ColoringMode
    {
      Solid,
      Gradient,
      Random
    }

    private readonly DeviceController _controller;

    public SpectrumBrushFactory(DeviceController controller)
    {
      _controller = controller ?? throw new ArgumentNullException(nameof(controller));
    }

    public IBrush GetSpectrumBrush(ColoringMode mode, Color primColor)
    {
      switch (mode)
      {
        case ColoringMode.Solid:
          return new SolidSpectrumBrush(primColor, _controller);
        case ColoringMode.Gradient:
          return new LinGradSpectrumBrush(new RainbowGradient(primColor.GetHue(), primColor.GetHue() + 360f),_controller);
        case ColoringMode.Random:
          return new RandSpectrumBrush(_controller);
        default:
          return null;
      }
    }

  }
}
