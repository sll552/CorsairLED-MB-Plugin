using System;
using System.Drawing;
using CUE.NET.Brushes;
using CUE.NET.Gradients;

namespace MusicBeePlugin
{
  public class ClSpectrumBrushFactory
  {
    public enum ColoringMode
    {
      Solid,
      Gradient,
      Random
    }

    private readonly ClDeviceController _controller;

    public ClSpectrumBrushFactory(ClDeviceController controller)
    {
      _controller = controller ?? throw new ArgumentNullException(nameof(controller));
    }

    public IBrush GetSpectrumBrush(ColoringMode mode, Color primColor)
    {
      switch (mode)
      {
        case ColoringMode.Solid:
          return new ClSolidSpectrumBrush(primColor, _controller);
        case ColoringMode.Gradient:
          return new ClLinGradSpectrumBrush(new RainbowGradient(primColor.GetHue(), primColor.GetHue() + 360f),_controller);
        case ColoringMode.Random:
          return new ClRandSpectrumBrush(_controller);
        default:
          return null;
      }
    }

  }
}
