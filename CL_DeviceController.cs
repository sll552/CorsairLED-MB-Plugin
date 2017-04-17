using System;
using System.Diagnostics;
using CUE.NET;
using CUE.NET.Devices.Keyboard;
using CUE.NET.Exceptions;

namespace MusicBeePlugin
{
  public class ClDeviceController
  {
    private bool _isinitialized = false;
    private CorsairKeyboard _keyboard;

    public void Init()
    {
      try
      {
        CueSDK.Initialize(true);
        Debug.WriteLine("Initialized with " + CueSDK.LoadedArchitecture + "-SDK");
        _keyboard = CueSDK.KeyboardSDK;
        _isinitialized = true;
      }
      catch (CUEException e)
      {
        Console.WriteLine(e);
      }
    }

    public bool IsInitialized()
    {
      return _isinitialized;
    }

    public string GetKeyboardModel()
    {
      return _isinitialized ? _keyboard.KeyboardDeviceInfo.Model : "None";
    }
  }
}
