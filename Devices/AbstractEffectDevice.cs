using System;
using System.Runtime.InteropServices;
using CUE.NET.Devices.Generic;
using CUE.NET.Devices.Generic.Enums;
using CUE.NET.Devices.Keyboard;
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

    protected readonly AbstractCueDevice _device;
    protected readonly SettingsManager _settings;
    protected readonly DeviceController _controller;

    public CorsairDeviceType DeviceType => _device.DeviceInfo.Type;
    public string DeviceName => _device.DeviceInfo.Model;

    protected AbstractEffectDevice(AbstractCueDevice device, SettingsManager settings, DeviceController controller)
    {
      _device = device ?? throw new ArgumentNullException(nameof(device));
      _settings = settings ?? throw new ArgumentNullException(nameof(settings));
      _controller = controller ?? throw new ArgumentNullException(nameof(controller));
    }

    public abstract void StartEffect();
    public abstract void StopEffect();
    public abstract Effect[] GetSupportedEffects();
  }
}
