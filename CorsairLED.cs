using System;
using System.Diagnostics;
using System.Timers;
using CUE.NET.Exceptions;
using CUE.NET.Devices.Generic.Enums;

namespace MusicBeePlugin
{
  public partial class Plugin
  {
    private MusicBeeApiInterface _mbApiInterface;
    private readonly PluginInfo _about = new PluginInfo();
    private ClSettings _settings;
    private readonly ClDeviceController _devcontroller = new ClDeviceController();
    private readonly ClDebugPlot _debugplot = new ClDebugPlot();
    private Timer _timer = new Timer();
    private readonly int _barcount = 22;

    public PluginInfo Initialise(IntPtr apiInterfacePtr)
    {
      _mbApiInterface = new MusicBeeApiInterface();
      _mbApiInterface.Initialise(apiInterfacePtr);
      _about.PluginInfoVersion = PluginInfoVersion;
      _about.Name = "CorsairLED";
      _about.Description = "Adds Support for Corsair CUE Devices";
      _about.Author = "Stefan Lengauer";
      _about.TargetApplication = "";   // current only applies to artwork, lyrics or instant messenger name that appears in the provider drop down selector or target Instant Messenger
      _about.Type = PluginType.General;
      _about.VersionMajor = 0;  // your plugin version
      _about.VersionMinor = 1;
      _about.Revision = 1;
      _about.MinInterfaceVersion = MinInterfaceVersion;
      _about.MinApiRevision = MinApiRevision;
      _about.ReceiveNotifications = (ReceiveNotificationFlags.PlayerEvents | ReceiveNotificationFlags.TagEvents);
      _about.ConfigurationPanelHeight = 0;   // height in pixels that musicbee should reserve in a panel for config settings. When set, a handle to an empty panel will be passed to the Configure function

      _mbApiInterface.MB_AddMenuItem("mnuTools/CL_Show Debug Plot", "HotKey For CL Debug Plot", ShowDebugPlot);

      try
      {
        _devcontroller.Init();
        if (_devcontroller.IsInitialized())
        {
          _settings = new ClSettings(_devcontroller);
        }
      }
      catch (CUEException ex)
      {
        Debug.WriteLine("CUE Exception! ErrorCode: " + Enum.GetName(typeof(CorsairError), ex.Error));
        return null;
      }

      _timer.Elapsed += TimerOnElapsed;
      _timer.Interval = 15;

      Debug.WriteLine(_about.Name + " loaded");
      return _about;
    }

    private void ShowDebugPlot(object sender, EventArgs e)
    {
      _debugplot.Show();
    }

    public bool Configure(IntPtr panelHandle)
    {
      // save any persistent settings in a sub-folder of this path
      string dataPath = _mbApiInterface.Setting_GetPersistentStoragePath();

      Debug.WriteLine(_about.Name + " Configure called");
      _settings.Show();
      // This prevents showing the About Box by MusicBee
      return true;
    }

    // called by MusicBee when the user clicks Apply or Save in the MusicBee Preferences screen.
    // its up to you to figure out whether anything has changed and needs updating
    public void SaveSettings()
    {
      // save any persistent settings in a sub-folder of this path
      string dataPath = _mbApiInterface.Setting_GetPersistentStoragePath();
    }

    // MusicBee is closing the plugin (plugin is being disabled by user or MusicBee is shutting down)
    public void Close(PluginCloseReason reason)
    {
    }

    // uninstall this plugin - clean up any persisted files
    public void Uninstall()
    {
    }

    // receive event notifications from MusicBee
    // you need to set about.ReceiveNotificationFlags = PlayerEvents to receive all notifications, and not just the startup event
    public void ReceiveNotification(string sourceFileUrl, NotificationType type)
    {
      // perform some action depending on the notification type
      switch (type)
      {
        case NotificationType.PluginStartup:
          // perform startup initialisation
          switch (_mbApiInterface.Player_GetPlayState())
          {
            case PlayState.Playing:
            case PlayState.Paused:
              break;
          }
          break;
        case NotificationType.TrackChanged:
          break;
        case NotificationType.PlayStateChanged:
          switch (_mbApiInterface.Player_GetPlayState())
          {
            case PlayState.Playing:
              Debug.WriteLine("Playing event");
              _timer.Start();
              break;
            case PlayState.Stopped:
            case PlayState.Paused:
              _timer.Stop();
              break;
          }
          break;
      }
    }

    private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
    {
      float[] data = new float[4096];
      var ret = _mbApiInterface.NowPlaying_GetSpectrumData(data);
      //Debug.WriteLine("ret = " + ret);
      if (ret > 0)
      {
        _debugplot.UpdatePlot(CalcBarData(_barcount,data));
      }
    }

    private float[] CalcBarData(int barcount, float[] fftdata)
    {
      float[] bardata = new float[barcount];
      int jumpwidth = (fftdata.Length/2) / barcount;
      int bar = 0;
      float avg;

      for (int i = 0; i < fftdata.Length/2; i+=jumpwidth)
      {
        avg = 0;
        for (int j = i; j < i+jumpwidth && j < fftdata.Length/2; j++)
        {
          avg += fftdata[j];
        }
        avg /= jumpwidth;

        if (bar < bardata.Length-1)
        {
          //bardata[bar] = (float) Math.Sqrt(avg) * 1000f;
          bardata[bar] = (float) Math.Sqrt(avg) * 10f;
          //bardata[bar] = (float)Math.Sqrt(avg) * 15f * (1 + (float)Math.Sqrt((double)bar/ (double)_barcount) * 1.25f);
          //bardata[bar] = (float)Math.Log10(1/(avg*avg)) * 10f;
        }
        bar++;
      }
      //foreach (var bard in bardata)
      //{
      //  Debug.WriteLine(bard);
      //}
      bardata[0] *= 0.7f;
      return bardata;
    }
  }
}