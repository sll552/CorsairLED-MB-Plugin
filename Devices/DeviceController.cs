using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using CUE.NET;
using CUE.NET.Brushes;
using CUE.NET.Devices.Generic.Enums;
using CUE.NET.Devices.Generic.EventArgs;
using CUE.NET.Devices.Keyboard;
using CUE.NET.Devices.Keyboard.Enums;
using CUE.NET.Exceptions;
using CUE.NET.Groups;
using CUE.NET.Groups.Extensions;
using MusicBeePlugin.Effects;
using MusicBeePlugin.Settings;
using MusicBeePlugin.UI;

namespace MusicBeePlugin.Devices
{
  public class DeviceController
  {
    private CorsairKeyboard _keyboard;
    private float[] _curbardata;
    private ListLedGroup _spectrumGroup;
    private ListLedGroup _progressGroup;
    private bool _firstinit = true;
    private readonly Plugin _plugin;
    private float _max = 1.2f;
    private float _min = 0.0f;
    private readonly SpectrumBrushFactory _brushFactory;
    private ProgressBrush _progressBrush;
    private SettingsManager _settings;
    private bool _initAble = true;

    private readonly CorsairLedId[] _lightbarLeds = new CorsairLedId[]
    {
      CorsairLedId.Lightbar1,
      CorsairLedId.Lightbar2,
      CorsairLedId.Lightbar3,
      CorsairLedId.Lightbar4,
      CorsairLedId.Lightbar5,
      CorsairLedId.Lightbar6,
      CorsairLedId.Lightbar7,
      CorsairLedId.Lightbar8,
      CorsairLedId.Lightbar9,
      CorsairLedId.Lightbar10,
      CorsairLedId.Lightbar11,
      CorsairLedId.Lightbar12,
      CorsairLedId.Lightbar13,
      CorsairLedId.Lightbar14,
      CorsairLedId.Lightbar15,
      CorsairLedId.Lightbar16,
      CorsairLedId.Lightbar17,
      CorsairLedId.Lightbar18,
      CorsairLedId.Lightbar19
    };

    private SettingsWindow _settingsUI;

    public static bool IsInitialized => CueSDK.IsInitialized;
    public float TrackProgress { get; set; }

    public DeviceController(Plugin plugin)
    {
      _plugin = plugin;
      _brushFactory = new SpectrumBrushFactory(this);
      CueSDK.PossibleX64NativePaths.Add(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
                                        @"\x64\CUESDK_2015.dll");
      CueSDK.PossibleX86NativePaths.Add(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
                                        @"\x86\CUESDK_2015.dll");
    }

    public float[] Curbardata
    {
      private get => IsInitialized ? _curbardata : null;
      set => _curbardata = value;
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
        _keyboard = CueSDK.KeyboardSDK;
        _keyboard.Updated += KeyboardOnUpdated;
      }
      catch (CUEException e)
      {
        Console.WriteLine(e);
        throw;
      }
    }

    private void KeyboardOnUpdated(object sender, UpdatedEventArgs args)
    {
      _plugin.UpdateSpectrographData();
      if (_progressBrush != null)
      {
        _progressBrush.Progress = TrackProgress;
      }
    }

    private static void UnInit()
    {
      if (!IsInitialized) return;
      CueSDK.Reinitialize(false);
    }

    public string GetKeyboardModel()
    {
      return IsInitialized ? _keyboard.KeyboardDeviceInfo.Model : "None";
    }

    public int GetDesiredBarCount()
    {
      // This spans a ledgroup over the number row of the keyboard which is most likely the row with the most keys
      var rectangleLedGroup = new RectangleLedGroup(_keyboard,
        new PointF(0f, _keyboard[CorsairKeyboardLedId.D1].LedRectangle.Location.Y),
        new PointF(_keyboard.DeviceRectangle.Width + 10,
          _keyboard[CorsairKeyboardLedId.Backspace].LedRectangle.Location.Y * 1.1f), 0.1f, false);

      return rectangleLedGroup.GetLeds().Count();
    }

    public void StartEffect()
    {
      if (!IsInitialized)
      {
        Init();
      }
      if (_keyboard == null)
      {
        Init(force: true);
      }
      if (_keyboard == null) return;

      _keyboard.Brush = new SolidColorBrush(_settings?.GetBackgroundColor(GetKeyboardModel()) ?? Color.Black);

      if (_spectrumGroup != null)
      {
        _spectrumGroup.Detach();
        _keyboard.DetachLedGroup(_spectrumGroup);
      }
      _spectrumGroup = new ListLedGroup(_keyboard, false, _keyboard)
      {
        Brush = _brushFactory.GetSpectrumBrush(
          _settings?.GetColoringMode(GetKeyboardModel()) ?? SpectrumBrushFactory.ColoringMode.Solid,
          _settings?.GetPrimaryColor(GetKeyboardModel()) ?? Color.Red)
      };


      if (_settings?.GetLightbarProgress(GetKeyboardModel()) ?? false)
      {
        _spectrumGroup = _spectrumGroup.Exclude(_lightbarLeds);
        if (_progressBrush == null)
        {
          _progressBrush = new ProgressBrush(_settings?.GetPrimaryColor(GetKeyboardModel()) ?? Color.Red);
        }

        if (_progressGroup != null)
        {
          _progressGroup.Detach();
          _keyboard.DetachLedGroup(_progressGroup);
        }

        _progressGroup = new ListLedGroup(_keyboard, _lightbarLeds)
        {
          Brush = _progressBrush
        };
      }
      else if (_progressGroup != null)
      {
        _progressGroup.Brush = new SolidColorBrush(Color.Transparent);
        _keyboard.Update(true);
        _keyboard.DetachLedGroup(_progressGroup);
        _keyboard.Update(true);
      }

      _spectrumGroup.Attach();
    }

    public void StopEffect()
    {
      void RemoveGroup(ILedGroup group)
      {
        if (group == null) return;
        group.Brush = new SolidColorBrush(Color.Transparent);
        _keyboard.Update(true);
        _keyboard.DetachLedGroup(group);
        _keyboard.Update(true);
      }

      if (_keyboard == null) return;

      RemoveGroup(_spectrumGroup);
      _spectrumGroup = null;
      RemoveGroup(_progressGroup);
      _progressGroup = null;

      _keyboard.Brush = null;
      _keyboard.Update(true);

      if (IsInitialized)
      {
        UnInit();
      }
    }

    public bool IsInSpectrum(RectangleF rectangle, BrushRenderTarget renderTarget)
    {
      float[] bardata = Curbardata;
      if (bardata == null) return false;

      int barwidth = (int) Math.Floor(rectangle.Width / bardata.Length);

      for (int i = 0; i < bardata.Length; i++)
      {
        // make sure that  min < bardata < max
        bardata[i] = Math.Min(Math.Max(bardata[i], _min), _max);
      }

      int baridx = (int) Math.Min(Math.Floor((renderTarget.Point.X - rectangle.Left) / barwidth), bardata.Length - 1);
      return (_max - bardata[baridx]) / _max < renderTarget.Point.Y / rectangle.Height;
    }

    public void AddSettings(SettingsManager settings, SettingsWindow settingsUI)
    {
      _settings = settings;
      _settingsUI = settingsUI;
    }
  }
}