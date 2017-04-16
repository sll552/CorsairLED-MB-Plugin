using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using CUE.NET;
using CUE.NET.Devices;
using CUE.NET.Devices.Keyboard;
using CUE.NET.Exceptions;
using CUE.NET.Devices.Generic.Enums;
using CUE.NET.Devices.Keyboard.Enums;

namespace MusicBeePlugin
{
  public class ClDeviceController
  {
    private bool _isinitialized;
    private CorsairKeyboard _keyboard;

    public void Init()
    {
      CueSDK.Initialize(true);
      Debug.WriteLine("Initialized with " + CueSDK.LoadedArchitecture + "-SDK");
      _keyboard = CueSDK.KeyboardSDK;
      _isinitialized = true;
    }

    public bool IsInitialized()
    {
      return _isinitialized;
    }

    public string GetKeyboardModel()
    {
      return _keyboard?.KeyboardDeviceInfo.Model;
    }
  }
}
