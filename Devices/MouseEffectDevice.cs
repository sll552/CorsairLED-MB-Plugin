using System.Collections.Generic;
using System.Diagnostics;
using CUE.NET.Devices.Generic.EventArgs;
using CUE.NET.Devices.Mouse;
using CUE.NET.Groups;
using MusicBeePlugin.Effects;
using MusicBeePlugin.Settings;

namespace MusicBeePlugin.Devices
{
  class MouseEffectDevice : AbstractEffectDevice
  {
    private BeatBrush _beatBrush = null;
    private ListLedGroup _beatGroup = null;

    public MouseEffectDevice(CorsairMouse device, SettingsManager settings, DeviceController controller) : base(device, settings, controller)
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
      return new[] {Effect.None};
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
