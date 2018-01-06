using System;
using MusicBeePlugin.Effects;
using SharpConfig;

namespace MusicBeePlugin.Settings
{
  class ColoringModeStringConverter : TypeStringConverter<SpectrumBrushFactory.ColoringMode>
  {
    public override string ConvertToString(object value)
    {
      return Enum.GetName(typeof(SpectrumBrushFactory.ColoringMode), value);
    }

    public override object ConvertFromString(string value, Type hint)
    {
      Enum.TryParse<SpectrumBrushFactory.ColoringMode>(value, out var tmpColoringMode);
      return tmpColoringMode;
    }
  }
}
