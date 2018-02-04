using System;
using MusicBeePlugin.Devices;
using SharpConfig;

namespace MusicBeePlugin.Settings
{
  class EffectStringConverter : TypeStringConverter<AbstractEffectDevice.Effect>
  {
    public override string ConvertToString(object value)
    {
      return Enum.GetName(typeof(AbstractEffectDevice.Effect), value);
    }

    public override object ConvertFromString(string value, Type hint)
    {
      Enum.TryParse<AbstractEffectDevice.Effect>(value, out var tmpColoringMode);
      return tmpColoringMode;
    }
  }
}
