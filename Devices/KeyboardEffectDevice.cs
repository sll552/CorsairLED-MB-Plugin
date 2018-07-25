using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CUE.NET.Brushes;
using CUE.NET.Devices.Generic;
using CUE.NET.Devices.Generic.Enums;
using CUE.NET.Devices.Generic.EventArgs;
using CUE.NET.Devices.Keyboard;
using CUE.NET.Devices.Keyboard.Enums;
using CUE.NET.Groups;
using CUE.NET.Groups.Extensions;
using MusicBeePlugin.Effects;
using MusicBeePlugin.Settings;

namespace MusicBeePlugin.Devices
{
  class KeyboardEffectDevice :  AbstractEffectDevice, ISpectrumDevice
  {
    private ListLedGroup _spectrumGroup;
    private ListLedGroup _progressGroup;
    private ListLedGroup _beatGroup;
    private const float Max = 1.2f;
    private const float Min = 0.0f;
    private readonly SpectrumBrushFactory _brushFactory;
    private ProgressBrush _progressBrush;
    private BeatBrush _beatBrush;
    private readonly CorsairLedId[] _lightbarLeds = {
      CorsairLedId.Lightbar1,
      CorsairLedId.Lightbar2,
      CorsairLedId.Lightbar3,
      CorsairLedId.Lightbar4,
      CorsairLedId.Lightbar5,
      CorsairLedId.Lightbar6,
      CorsairLedId.Lightbar7,
      CorsairLedId.Lightbar8,
      CorsairLedId.Lightbar9,
      CorsairLedId.Lightbar10,
      CorsairLedId.Lightbar11,
      CorsairLedId.Lightbar12,
      CorsairLedId.Lightbar13,
      CorsairLedId.Lightbar14,
      CorsairLedId.Lightbar15,
      CorsairLedId.Lightbar16,
      CorsairLedId.Lightbar17,
      CorsairLedId.Lightbar18,
      CorsairLedId.Lightbar19
    };

    public KeyboardEffectDevice(CorsairKeyboard device, SettingsManager settings, DeviceController controller) : base(device, settings, controller)
    {
      _brushFactory = new SpectrumBrushFactory(this);
      Device.Updated += KeyboardOnUpdated;
    }

    private void KeyboardOnUpdated(object sender, UpdatedEventArgs args)
    {
      Controller.UpdateSpectrographData();
      if (_progressBrush != null)
      {
        _progressBrush.Progress = Controller.TrackProgress;
      }
      if (_beatBrush != null)
      {
        _beatBrush.Beat = Controller.Beat;
      }
    }

    protected override void SpectrographEffectImpl()
    {
      Device.Brush = new SolidColorBrush(Settings.GetBackgroundColor(DeviceName));
      if (_spectrumGroup != null)
      {
        _spectrumGroup.Detach();
        Device.DetachLedGroup(_spectrumGroup);
      }

      _spectrumGroup = new ListLedGroup(Device, false, Device)
      {
        Brush = _brushFactory.GetSpectrumBrush(Settings.GetColoringMode(DeviceName), Settings.GetPrimaryColor(DeviceName))
      };


      if (Settings.GetLightbarProgress(DeviceName))
      {
        _spectrumGroup = _spectrumGroup.Exclude(_lightbarLeds);
        if (_progressBrush == null)
        {
          _progressBrush = new ProgressBrush(Settings.GetPrimaryColor(DeviceName));
        }

        if (_progressGroup != null)
        {
          _progressGroup.Detach();
          Device.DetachLedGroup(_progressGroup);
        }

        _progressGroup = new ListLedGroup(Device, _lightbarLeds)
        {
          Brush = _progressBrush
        };
      }
      else if (_progressGroup != null)
      {
        _progressGroup.Brush = new SolidColorBrush(Color.Transparent);
        Device.Update(true);
        Device.DetachLedGroup(_progressGroup);
        Device.Update(true);
      }
      _spectrumGroup.Attach();
    }

    protected override void BeatEffectImpl()
    {
      if (_beatBrush == null)
      {
        _beatBrush = new BeatBrush(Settings.GetPrimaryColor(DeviceName));
      }

      if (_beatGroup == null)
      {
        _beatGroup = new ListLedGroup(Device, false, Device);
      }

      GenericBeatImpl(_beatBrush, _beatGroup);
    }

    public override IEnumerable<Effect> GetSupportedEffects()
    {
      return new[] { Effect.Spectrograph, Effect.Beat, Effect.None };
    }

    public bool IsInSpectrum(RectangleF rectangle, BrushRenderTarget renderTarget)
    {
      var bardata = Controller.Curbardata;
      if (bardata == null) return false;

      var barwidth = (int)Math.Floor(rectangle.Width / bardata.Length);

      for (var i = 0; i < bardata.Length; i++)
      {
        // make sure that  min < bardata < max
        bardata[i] = Math.Min(Math.Max(bardata[i], Min), Max);
      }

      var baridx = (int)Math.Min(Math.Floor((renderTarget.Point.X - rectangle.Left) / barwidth), bardata.Length - 1);
      return (Max - bardata[baridx]) / Max < renderTarget.Point.Y / rectangle.Height;
    }

    public int GetDesiredBarCount()
    {
      // go over each row on the keybaord to determine the max leds in a row
      CorsairLed[] ledlist =
      {
        Device[CorsairLedId.Escape],
        Device[CorsairLedId.D1],
        Device[CorsairLedId.Tab],
        Device[CorsairLedId.CapsLock],
        Device[CorsairLedId.LeftShift],
        Device[CorsairLedId.LeftCtrl]
      };
      int barcnt = 0;

      foreach (var led in ledlist)
      {
        var lg = new RectangleLedGroup(Device, new PointF(0f, led.LedRectangle.Location.Y),
          new PointF(Device.DeviceRectangle.Width + 10, led.LedRectangle.Bottom), 0.1f, false);
        var cnt = lg.GetLeds().Count();
        if (cnt > barcnt)
        {
          barcnt = cnt;
        }
      }
      return barcnt > 0 ? barcnt : 21;
    }
  }
}
