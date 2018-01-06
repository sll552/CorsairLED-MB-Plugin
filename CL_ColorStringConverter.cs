using System;
using System.Drawing;
using SharpConfig;

namespace MusicBeePlugin
{
  class ClColorStringConverter: TypeStringConverter<Color>
  {
    public override string ConvertToString(object value)
    {
      return ColorTranslator.ToHtml((Color)value);
    }

    public override object ConvertFromString(string value, Type hint)
    {
      return ColorTranslator.FromHtml(value);
    }
  }
}
