using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using CUE.NET;
using CUE.NET.Devices.Generic.Enums;
using CUE.NET.Exceptions;
using MusicBeePlugin.Settings;
using MusicBeePlugin.UI;

namespace MusicBeePlugin.Devices
{
  public class DeviceController
  {
    private float[] _curbardata;
    private bool _firstinit = true;
    private readonly Plugin _plugin;
    private readonly SettingsManager _settings;
    private bool _initAble = true;
    // ReSharper disable once InconsistentNaming
    private SettingsWindow _settingsUI;
    private readonly List<AbstractEffectDevice> _devices = new List<AbstractEffectDevice>();

    public static bool IsInitialized => CueSDK.IsInitialized;
    public float TrackProgress { get; set; }
    public float[] Curbardata
    {
      get => IsInitialized ? _curbardata : null;
      set => _curbardata = value;
    }
    public AbstractEffectDevice[] Devices => IsInitialized ? _devices.ToArray() : null;

    public DeviceController(Plugin plugin, SettingsManager settingsManager)
    {
      _plugin = plugin;
      CueSDK.PossibleX64NativePaths.Add(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
                                        @"\x64\CUESDK_2015.dll");
      CueSDK.PossibleX86NativePaths.Add(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
                                        @"\x86\CUESDK_2015.dll");
      _settings = settingsManager;
    }

    public void Init(bool force = false)
    {
      if (!CueSDK.IsSDKAvailable(null))
      {
        if (!_initAble)
        {
          _settingsUI?.SetMessage("No SDK available");
        }
        _initAble = false;
        return;
      }

      if (IsInitialized && !force) return;

      try
      {
        if (_firstinit || force)
        {
          CueSDK.Initialize(true);
          _firstinit = false;
        }
        else
        {
          CueSDK.Reinitialize(true);
        }
        CueSDK.UpdateMode = UpdateMode.Continuous;
        CueSDK.UpdateFrequency = 1f / 30f;
        Debug.WriteLine("Initialized with " + CueSDK.LoadedArchitecture + "-SDK");
        _devices.Clear();
        foreach (var initializedDevice in CueSDK.InitializedDevices)
        {
          switch (initializedDevice.DeviceInfo.Type)
          {
            case CorsairDeviceType.Unknown:
              break;
            case CorsairDeviceType.Mouse:
              break;
            case CorsairDeviceType.Keyboard:
              _devices.Add(new KeyboardEffectDevice(CueSDK.KeyboardSDK, _settings, this));
              break;
            case CorsairDeviceType.Headset:
              break;
            case CorsairDeviceType.Mousemat:
              break;
            default:
              throw new ArgumentOutOfRangeException();
          }
        }
      }
      catch (CUEException e)
      {
        Console.WriteLine(e);
        throw;
      }
    }

    internal void UpdateSpectrographData()
    {
      _plugin.UpdateSpectrographData();
    }

    private static void UnInit()
    {
      if (!IsInitialized) return;
      CueSDK.Reinitialize(false);
    }

    public void StartEffect()
    {
      if (!IsInitialized)
      {
        Init();
      }
      if (_devices.Count == 0)
      {
        Init(force: true);
      }
      if (_devices.Count == 0) return;

      foreach (var dev in _devices)
      {
        dev.StartEffect();
      }
    }

    public void StopEffect()
    {
      foreach (var dev in _devices)
      {
        dev.StopEffect();
      }
      if (IsInitialized)
      {
        UnInit();
      }
    }

    public void AddSettings(SettingsWindow settingsUI)
    {
      _settingsUI = settingsUI;
    }

    public int GetDesiredBarCount()
    {
      var ret = 0;
      foreach (var keyboard in _devices.OfType<KeyboardEffectDevice>().ToList())
      {
        if (keyboard.GetDesiredBarCount() > ret)
        {
          ret = keyboard.GetDesiredBarCount();
        }
      }

      return ret;
    }
  }
}