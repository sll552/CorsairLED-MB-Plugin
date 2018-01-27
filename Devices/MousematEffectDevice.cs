using System.Collections.Generic;
using System.Diagnostics;
using CUE.NET.Devices.Mousemat;
using MusicBeePlugin.Settings;

namespace MusicBeePlugin.Devices
{
  class MousematEffectDevice : AbstractEffectDevice
  {
    public MousematEffectDevice(CorsairMousemat device, SettingsManager settings, DeviceController controller) : base(device, settings, controller)
    {
    }

    public override void StartEffect()
    {
      Debug.WriteLine("Mousemat Start Effect");
    }

    public override void StopEffect()
    {
      Debug.WriteLine("Mousemat Stop Effect");
    }

    public override IEnumerable<Effect> GetSupportedEffects()
    {
      return new[] { Effect.None };
    }
  }
}
