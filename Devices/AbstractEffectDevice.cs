using System;
using System.Collections.Generic;
using System.Linq;
using CUE.NET.Devices.Generic;
using CUE.NET.Devices.Generic.Enums;
using MusicBeePlugin.Settings;

namespace MusicBeePlugin.Devices
{
  public abstract class AbstractEffectDevice
  {
    public enum Effect
    {
      Spectrograph,
      Beat,
      None
    }

    protected readonly AbstractCueDevice Device;
    protected readonly SettingsManager Settings;
    protected readonly DeviceController Controller;

    public CorsairDeviceType DeviceType => Device.DeviceInfo.Type;
    public string DeviceName => Device.DeviceInfo.Model;

    public bool IsDefaultDevice
    {
      get => Settings.Defaultdev == DeviceName;
      set
      {
        if (value)
        {
          Settings.Defaultdev = DeviceName;
        }
        else if (IsDefaultDevice)
        {
          Settings.Defaultdev = null;
        }
      } 
    } 

    public Effect ActiveEffect
    {
      get => Settings.GetEffect(DeviceName);
      set
      {
        if (GetSupportedEffects().Contains(value))
        {
          Settings.SetEffect(DeviceName,value);
        }
      }
    } 
    public bool Enabled
    {
      get => Settings.GetEnabled(DeviceName);
      set => Settings.SetEnabled(DeviceName, value);
    }

    protected AbstractEffectDevice(AbstractCueDevice device, SettingsManager settings, DeviceController controller)
    {
      Device = device ?? throw new ArgumentNullException(nameof(device));
      Settings = settings ?? throw new ArgumentNullException(nameof(settings));
      Controller = controller ?? throw new ArgumentNullException(nameof(controller));
    }

    public abstract void StartEffect();
    public abstract void StopEffect();
    public abstract IEnumerable<Effect> GetSupportedEffects();
  }
}
