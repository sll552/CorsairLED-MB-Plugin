using System;
using System.Collections.Generic;
using System.Linq;
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
  public class CL_DeviceController
  {
    private bool isinitialized = false;
    private CorsairKeyboard keyboard;

    public void Init()
    {
      CueSDK.Initialize(true);
      Debug.WriteLine("Initialized with " + CueSDK.LoadedArchitecture + "-SDK");
      keyboard = CueSDK.KeyboardSDK;
      isinitialized = true;
    }

    public bool IsInitialized()
    {
      return isinitialized;
    }

    public string getKeyboardModel()
    {
      return keyboard?.KeyboardDeviceInfo.Model;
    }
  }
}
