using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using CUE.NET.Brushes;
using CUE.NET.Devices.Generic.EventArgs;
using CUE.NET.Devices.Mousemat;
using CUE.NET.Groups;
using CUE.NET.Groups.Extensions;
using MusicBeePlugin.Effects;
using MusicBeePlugin.Settings;

namespace MusicBeePlugin.Devices
{
  class MousematEffectDevice : AbstractEffectDevice
  {
    private BeatBrush _beatBrush = null;
    private ListLedGroup _beatGroup = null;

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

    public override void StartEffect()
    {
      if (Settings == null || ActiveEffect != Effect.Beat || !Enabled) return;
      Device.Brush = new SolidColorBrush(Settings.GetBackgroundColor(DeviceName));

      if (_beatGroup != null)
      {
        _beatGroup.Detach();
        Device.DetachLedGroup(_beatGroup);
      }

      if (_beatBrush == null)
      {
        _beatBrush = new BeatBrush(Settings.GetPrimaryColor(DeviceName));
      }

      _beatGroup = new ListLedGroup(Device, false, Device)
      {
        Brush = _beatBrush
      };
      _beatGroup.Attach();
    }

    public override void StopEffect()
    {
      void RemoveGroup(ILedGroup group)
      {
        if (group == null) return;
        group.Brush = new SolidColorBrush(Color.Transparent);
        Device.Update(true);
        Device.DetachLedGroup(group);
        Device.Update(true);
      }

      if (Device == null) return;

      RemoveGroup(_beatGroup);
      _beatGroup = null;

      Device.Brush = null;
      Device.Update(true);
    }

    public override IEnumerable<Effect> GetSupportedEffects()
    {
      return new[] { Effect.None, Effect.Beat };
    }
  }
}
