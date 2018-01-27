using System.Collections.Generic;
using System.Diagnostics;
using CUE.NET.Devices.Mouse;
using MusicBeePlugin.Settings;

namespace MusicBeePlugin.Devices
{
  class MouseEffectDevice : AbstractEffectDevice
  {
    public MouseEffectDevice(CorsairMouse device, SettingsManager settings, DeviceController controller) : base(device, settings, controller)
    {
    }

    public override void StartEffect()
    {
      Debug.WriteLine("Mouse Start Effect");
    }

    public override void StopEffect()
    {
      Debug.WriteLine("Mouse Stop Effect");
    }

    public override IEnumerable<Effect> GetSupportedEffects()
    {
      return new[] {Effect.None};
    }
  }
}
