using System.Collections.Generic;
using System.Diagnostics;
using CUE.NET.Devices.Headset;
using MusicBeePlugin.Settings;

namespace MusicBeePlugin.Devices
{
  class HeadsetEffectDevice : AbstractEffectDevice
  {
    public HeadsetEffectDevice(CorsairHeadset device, SettingsManager settings, DeviceController controller) : base(device, settings, controller)
    {
    }

    public override IEnumerable<Effect> GetSupportedEffects()
    {
      return new[] { Effect.None };
    }

    protected override void SpectrographEffectImpl()
    {
      throw new System.NotImplementedException();
    }

    protected override void BeatEffectImpl()
    {
      throw new System.NotImplementedException();
    }
  }
}
