using System;
using System.Collections.Generic;
using CUE.NET.Devices.Generic.EventArgs;
using CUE.NET.Devices.Mousemat;
using CUE.NET.Groups;
using MusicBeePlugin.Effects;
using MusicBeePlugin.Settings;

namespace MusicBeePlugin.Devices
{
  class MousematEffectDevice : AbstractEffectDevice
  {
    private BeatBrush _beatBrush;
    private ListLedGroup _beatGroup;

    public MousematEffectDevice(CorsairMousemat device, SettingsManager settings, DeviceController controller) : base(device, settings, controller)
    {
      Device.Updated += DeviceOnUpdated;
    }

    private void DeviceOnUpdated(object sender, UpdatedEventArgs args)
    {
      Controller.UpdateSpectrographData();
      if (_beatBrush != null)
      {
        _beatBrush.Beat = Controller.Beat;
      }
    }
    
    public override IEnumerable<Effect> GetSupportedEffects()
    {
      return new[] { Effect.None, Effect.Beat };
    }

    protected override void SpectrographEffectImpl()
    {
      throw new NotImplementedException();
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
  }
}
