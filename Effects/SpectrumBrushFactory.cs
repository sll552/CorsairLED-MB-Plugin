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

    private readonly ISpectrumDevice _device;

    public SpectrumBrushFactory(ISpectrumDevice device)
    {
      _device = device ?? throw new ArgumentNullException(nameof(device));
    }

    public IBrush GetSpectrumBrush(ColoringMode mode, Color primColor)
    {
      switch (mode)
      {
        case ColoringMode.Solid:
          return new SolidSpectrumBrush(primColor, _device);
        case ColoringMode.Gradient:
          return new LinGradSpectrumBrush(new RainbowGradient(primColor.GetHue(), primColor.GetHue() + 360f), _device);
        case ColoringMode.Random:
          return new RandSpectrumBrush(_device);
        default:
          return null;
      }
    }

  }
}
