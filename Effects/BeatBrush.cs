using System;
using CUE.NET.Brushes;
using CUE.NET.Devices.Generic;

namespace MusicBeePlugin.Effects
{
  class BeatBrush : SolidColorBrush
  {
    private float _minbeat = 0;
    private float _beat = 0;

    public float Beat
    {
      get => _beat;
      set
      {
        if (value < _minbeat)
        {
          _minbeat = value;
        }
        else
        {
          _minbeat += Math.Max(_minbeat / 10, 0.005f);
        }
        _beat = value;
      }
    }

    public BeatBrush(CorsairColor color) : base(color)
    {
    }

    protected override CorsairColor FinalizeColor(CorsairColor color)
    {
      var ret = new CorsairColor(color);
      ret.A = (byte)(Math.Round((ret.A * 1.0f) / 100 * ((_minbeat - Beat) / (_minbeat / 100))));
      return Beat > _minbeat ? ret : CorsairColor.Transparent;
    }
  }
}
