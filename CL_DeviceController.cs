using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using CUE.NET;
using CUE.NET.Brushes;
using CUE.NET.Devices.Generic.Enums;
using CUE.NET.Devices.Generic.EventArgs;
using CUE.NET.Devices.Keyboard;
using CUE.NET.Devices.Keyboard.Enums;
using CUE.NET.Exceptions;
using CUE.NET.Groups;
using CUE.NET.Groups.Extensions;

namespace MusicBeePlugin
{
  public class ClDeviceController
  {
    private CorsairKeyboard _keyboard;
    private float[] _curbardata;
    private ListLedGroup _spectrumGroup;
    private bool _firstinit = true;
    private readonly Plugin _plugin;
    private float _max = 1.2f;
    private float _min = 0.0f;
    private ClSpectrumBrushFactory _brushFactory;

    public ClDeviceController(Plugin plugin)
    {
      _plugin = plugin;
      _brushFactory = new ClSpectrumBrushFactory(this);
    }

    public float[] Curbardata { get => IsInitialized ? _curbardata : null; set => _curbardata = value; }

    public static bool IsInitialized => CueSDK.IsInitialized;

    public void Init()
    {
      if (IsInitialized) return;
      try
      {
        if (_firstinit)
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
    }

    public static void UnInit()
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
      _keyboard.Brush = new SolidColorBrush(Color.Black);
      _spectrumGroup = new ListLedGroup(_keyboard, _keyboard) {Brush = _brushFactory.GetSpectrumBrush(ClSpectrumBrushFactory.ColoringMode.Random,Color.Red)};

    }

    public void StopEffect()
    {
      if (_keyboard == null || _spectrumGroup == null) return;

      _keyboard.DetachLedGroup(_spectrumGroup);
      _keyboard.Brush = null;
      _keyboard.Update(true);
      _spectrumGroup.RemoveLeds(_keyboard);
      _spectrumGroup.Detach();
      _spectrumGroup = null;

      if (IsInitialized)
      {
        UnInit();
      }
    }

    public bool IsInSpectrum(RectangleF rectangle, BrushRenderTarget renderTarget)
    {
      float[] bardata = Curbardata;
      if (bardata == null) return false;

      int barwidth = (int)Math.Floor(rectangle.Width / bardata.Length);
      
      for (int i = 0; i < bardata.Length; i++)
      {
        // make sure that  min > bardata > max
        bardata[i] = Math.Min(Math.Max(bardata[i], _min), _max);
      }

      int baridx = (int)Math.Floor((renderTarget.Point.X - rectangle.Left) / barwidth);
      return (_max - bardata[baridx]) / _max < renderTarget.Point.Y / rectangle.Height;
    }
  }
}
