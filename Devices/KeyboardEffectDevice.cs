using System;
using System.Drawing;
using System.Linq;
using CUE.NET.Brushes;
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
    private float _max = 1.2f;
    private float _min = 0.0f;
    private readonly SpectrumBrushFactory _brushFactory;
    private ProgressBrush _progressBrush;
    private readonly CorsairLedId[] _lightbarLeds = new CorsairLedId[]
    {
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
      _device.Updated += KeyboardOnUpdated;
    }

    private void KeyboardOnUpdated(object sender, UpdatedEventArgs args)
    {
      _controller.UpdateSpectrographData();
      if (_progressBrush != null)
      {
        _progressBrush.Progress = _controller.TrackProgress;
      }
    }

    public override void StartEffect()
    {
      if (_settings == null) return;

      _device.Brush = new SolidColorBrush(_settings.GetBackgroundColor(DeviceName));

      if (_spectrumGroup != null)
      {
        _spectrumGroup.Detach();
        _device.DetachLedGroup(_spectrumGroup);
      }

      _spectrumGroup = new ListLedGroup(_device, false, _device)
      {
        Brush = _brushFactory.GetSpectrumBrush(_settings.GetColoringMode(DeviceName), _settings.GetPrimaryColor(DeviceName))
      };


      if (_settings.GetLightbarProgress(DeviceName))
      {
        _spectrumGroup = _spectrumGroup.Exclude(_lightbarLeds);
        if (_progressBrush == null)
        {
          _progressBrush = new ProgressBrush(_settings.GetPrimaryColor(DeviceName));
        }

        if (_progressGroup != null)
        {
          _progressGroup.Detach();
          _device.DetachLedGroup(_progressGroup);
        }

        _progressGroup = new ListLedGroup(_device, _lightbarLeds)
        {
          Brush = _progressBrush
        };
      }
      else if (_progressGroup != null)
      {
        _progressGroup.Brush = new SolidColorBrush(Color.Transparent);
        _device.Update(true);
        _device.DetachLedGroup(_progressGroup);
        _device.Update(true);
      }
      _spectrumGroup.Attach();
    }

    public override void StopEffect()
    {
      void RemoveGroup(ILedGroup group)
      {
        if (group == null) return;
        group.Brush = new SolidColorBrush(Color.Transparent);
        _device.Update(true);
        _device.DetachLedGroup(group);
        _device.Update(true);
      }

      if (_device == null) return;

      RemoveGroup(_spectrumGroup);
      _spectrumGroup = null;
      RemoveGroup(_progressGroup);
      _progressGroup = null;

      _device.Brush = null;
      _device.Update(true);
    }

    public override Effect[] GetSupportedEffects()
    {
      return new[] { Effect.Spectrograph };
    }

    public bool IsInSpectrum(RectangleF rectangle, BrushRenderTarget renderTarget)
    {
      var bardata = _controller.Curbardata;
      if (bardata == null) return false;

      var barwidth = (int)Math.Floor(rectangle.Width / bardata.Length);

      for (var i = 0; i < bardata.Length; i++)
      {
        // make sure that  min < bardata < max
        bardata[i] = Math.Min(Math.Max(bardata[i], _min), _max);
      }

      var baridx = (int)Math.Min(Math.Floor((renderTarget.Point.X - rectangle.Left) / barwidth), bardata.Length - 1);
      return (_max - bardata[baridx]) / _max < renderTarget.Point.Y / rectangle.Height;
    }

    public int GetDesiredBarCount()
    {
      // This spans a ledgroup over the number row of the keyboard which is most likely the row with the most keys
      var rectangleLedGroup = new RectangleLedGroup(_device,
        new PointF(0f, _device[CorsairKeyboardLedId.D1].LedRectangle.Location.Y),
        new PointF(_device.DeviceRectangle.Width + 10,
          _device[CorsairKeyboardLedId.Backspace].LedRectangle.Location.Y * 1.1f), 0.1f, false);

      return rectangleLedGroup.GetLeds().Count();
    }

  }
}
