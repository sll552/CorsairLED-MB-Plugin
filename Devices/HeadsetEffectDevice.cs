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

    public override void StartEffect()
    {
      Debug.WriteLine("Headset Start Effect");
    }

    public override void StopEffect()
    {
      Debug.WriteLine("Headset Stop Effect");
    }

    public override IEnumerable<Effect> GetSupportedEffects()
    {
      return new[] { Effect.None };
    }
  }
}
