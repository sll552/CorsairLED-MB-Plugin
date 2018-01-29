using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using CUE.NET.Brushes;
using CUE.NET.Devices.Generic;
using CUE.NET.Devices.Generic.Enums;
using CUE.NET.Groups;
using CUE.NET.Groups.Extensions;
using MusicBeePlugin.Effects;
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

    /// <summary>
    /// Expect all used LedGroups to be null when this is called
    /// </summary>
    public void StartEffect()
    {
      if (Settings == null || !GetSupportedEffects().Contains(ActiveEffect) || !Enabled) return;
      switch (ActiveEffect)
      {
        case Effect.Spectrograph:
          SpectrographEffectImpl();
          break;
        case Effect.Beat:
          BeatEffectImpl();
          break;
        case Effect.None:
          return;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    /// <summary>
    /// After this Method is called all LedGroups are set to null
    /// </summary>
    public void StopEffect()
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

      var fields = this.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
      foreach (var field in fields)
      {
        if (field.FieldType != typeof(ListLedGroup)) continue;
        RemoveGroup((ListLedGroup)field.GetValue(this));
        field.SetValue(this, null);
      }

      Device.Brush = null;
      Device.Update(true);
    }

    public abstract IEnumerable<Effect> GetSupportedEffects();

    #region Effect implementations

    protected abstract void SpectrographEffectImpl();
    protected abstract void BeatEffectImpl();

    protected void GenericBeatImpl(BeatBrush brush, ListLedGroup beatGroup)
    {
      Device.Brush = new SolidColorBrush(Settings.GetBackgroundColor(DeviceName));

      if (beatGroup != null)
      {
        beatGroup.Detach();
        Device.DetachLedGroup(beatGroup);
      }

      beatGroup = new ListLedGroup(Device, false, Device)
      {
        Brush = brush
      };

      beatGroup.Attach();
    }

    #endregion
  }
}
