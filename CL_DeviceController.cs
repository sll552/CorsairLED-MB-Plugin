using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using CUE.NET;
using CUE.NET.Brushes;
using CUE.NET.Devices.Generic;
using CUE.NET.Devices.Generic.Enums;
using CUE.NET.Devices.Keyboard;
using CUE.NET.Devices.Keyboard.Enums;
using CUE.NET.Exceptions;
using CUE.NET.Groups;

namespace MusicBeePlugin
{
  public class ClDeviceController
  {
    private CorsairKeyboard _keyboard;
    private float[] _curbardata;
    private ListLedGroup spectrumGroup;

    public float[] Curbardata { get => IsInitialized ? _curbardata : null; set => _curbardata = value; }
    public bool IsInitialized { get; private set; }

    public void Init()
    {
      try
      {
        CueSDK.Initialize(true);
        CueSDK.UpdateMode = UpdateMode.Continuous;
        Debug.WriteLine("Initialized with " + CueSDK.LoadedArchitecture + "-SDK");
        _keyboard = CueSDK.KeyboardSDK;
        IsInitialized = true;
      }
      catch (CUEException e)
      {
        Console.WriteLine(e);
        throw;
      }
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
      if (!IsInitialized)return;
      spectrumGroup = new ListLedGroup(_keyboard, _keyboard) {Brush = new ClSolidSpectrumBrush(Color.Red, this)};

    }

    public void StopEffect()
    {
      _keyboard.DetachLedGroup(spectrumGroup);
    }
  }
}
