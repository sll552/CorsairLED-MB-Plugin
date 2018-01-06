using System;
using SharpConfig;

namespace MusicBeePlugin
{
  class ClColoringModeStringConverter : TypeStringConverter<ClSpectrumBrushFactory.ColoringMode>
  {
    public override string ConvertToString(object value)
    {
      return Enum.GetName(typeof(ClSpectrumBrushFactory.ColoringMode), value);
    }

    public override object ConvertFromString(string value, Type hint)
    {
      Enum.TryParse<ClSpectrumBrushFactory.ColoringMode>(value, out var tmpColoringMode);
      return tmpColoringMode;
    }
  }
}
