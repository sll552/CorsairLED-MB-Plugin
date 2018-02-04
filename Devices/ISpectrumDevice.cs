using System.Drawing;
using CUE.NET.Brushes;

namespace MusicBeePlugin.Devices
{
  public interface ISpectrumDevice
  {
    bool IsInSpectrum(RectangleF rectangle, BrushRenderTarget renderTarget);
  }
}
