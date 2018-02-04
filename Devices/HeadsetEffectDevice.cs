﻿using System.Collections.Generic;
using CUE.NET.Devices.Generic.EventArgs;
using CUE.NET.Devices.Headset;
using CUE.NET.Groups;
using MusicBeePlugin.Effects;
using MusicBeePlugin.Settings;

namespace MusicBeePlugin.Devices
{
  class HeadsetEffectDevice : AbstractEffectDevice
  {
    private BeatBrush _beatBrush = null;
    private ListLedGroup _beatGroup = null;

    public HeadsetEffectDevice(CorsairHeadset device, SettingsManager settings, DeviceController controller) : base(device, settings, controller)
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
      return new[] { Effect.Beat, Effect.None };
    }

    protected override void SpectrographEffectImpl()
    {
      throw new System.NotImplementedException();
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
