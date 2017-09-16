using System;
using System.Diagnostics;
using System.Threading;
using System.Timers;
using CUE.NET.Exceptions;
using CUE.NET.Devices.Generic.Enums;
using Timer = System.Timers.Timer;

namespace MusicBeePlugin
{
  public partial class Plugin
  {
    private MusicBeeApiInterface _mbApiInterface;
    private readonly PluginInfo _about = new PluginInfo();
    private ClSettings _settings;
    private ClDeviceController _devcontroller;
    private readonly ClDebugPlot _debugplot = new ClDebugPlot();
    private readonly Timer _pauseTimer = new Timer();
    private int _barcount = 22;

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
      _about.Revision = 3;
      _about.MinInterfaceVersion = MinInterfaceVersion;
      _about.MinApiRevision = MinApiRevision;
      _about.ReceiveNotifications = (ReceiveNotificationFlags.PlayerEvents | ReceiveNotificationFlags.TagEvents);
      _about.ConfigurationPanelHeight = 0;   // height in pixels that musicbee should reserve in a panel for config settings. When set, a handle to an empty panel will be passed to the Configure function

      try
      {
        _devcontroller = new ClDeviceController(this);
        _devcontroller.Init();
        _settings = new ClSettings(_about, _devcontroller, _mbApiInterface.Setting_GetPersistentStoragePath());
        if (ClDeviceController.IsInitialized)
        {
          _mbApiInterface.MB_AddMenuItem("mnuTools/CL_Show Debug Plot", "HotKey For CL Debug Plot", ShowDebugPlot);
          _devcontroller.AddSettings(_settings);
          _barcount = _devcontroller.GetDesiredBarCount();
        }
      }
      catch (CUEException ex)
      {
        Console.WriteLine("CUE Exception! ErrorCode: " + Enum.GetName(typeof(CorsairError), ex.Error));
        throw;
      }
      if (!ClDeviceController.IsInitialized) return null;

      _pauseTimer.AutoReset = false;
      _pauseTimer.Interval = 5000;
      _pauseTimer.Elapsed += PauseTimerOnElapsed;

      Debug.WriteLine(_about.Name + " loaded");
      Debug.WriteLine("MusicBee Version" + _mbApiInterface.MusicBeeVersion);
      return _about;
    }

    private void ShowDebugPlot(object sender, EventArgs e)
    {
      _debugplot?.Show();
    }

    public bool Configure(IntPtr panelHandle)
    {
      Debug.WriteLine(_about.Name + " Configure called with path" + _mbApiInterface.Setting_GetPersistentStoragePath());
      _settings?.Show();

      // This prevents showing the About Box by MusicBee
      return true;
    }

    // called by MusicBee when the user clicks Apply or Save in the MusicBee Preferences screen.
    // its up to you to figure out whether anything has changed and needs updating
    public void SaveSettings()
    {
      _settings?.PersistValues();
    }

    // MusicBee is closing the plugin (plugin is being disabled by user or MusicBee is shutting down)
    public void Close(PluginCloseReason reason)
    {
      _devcontroller.StopEffect();
      // Stop is async so give it some extra time (hopefully prevents unclean shutdown)
      Thread.Sleep(500);
    }

    // uninstall this plugin - clean up any persisted files
    public void Uninstall()
    {
      _settings?.Delete();
    }

    // receive event notifications from MusicBee
    // you need to set about.ReceiveNotificationFlags = PlayerEvents to receive all notifications, and not just the startup event
    public void ReceiveNotification(string sourceFileUrl, NotificationType type)
    {
      // perform some action depending on the notification type
      switch (type)
      {
        case NotificationType.PluginStartup:
          _pauseTimer.Start();
          break;
        case NotificationType.TrackChanged:
          break;
        case NotificationType.PlayStateChanged:
          switch (_mbApiInterface.Player_GetPlayState())
          {
            case PlayState.Playing:
              _pauseTimer.Stop();
              _devcontroller.StartEffect();
              break;
            case PlayState.Stopped:
            case PlayState.Paused:
            case PlayState.Undefined:
            case PlayState.Loading:
              _pauseTimer.Start();
              break;
            default:
              throw new ArgumentOutOfRangeException();
          }
          break;
        case NotificationType.TrackChanging:
          break;
        case NotificationType.AutoDjStarted:
          break;
        case NotificationType.AutoDjStopped:
          break;
        case NotificationType.VolumeMuteChanged:
          break;
        case NotificationType.VolumeLevelChanged:
          break;
        case NotificationType.NowPlayingListChanged:
          break;
        case NotificationType.NowPlayingListEnded:
          break;
        case NotificationType.NowPlayingArtworkReady:
          break;
        case NotificationType.NowPlayingLyricsReady:
          break;
        case NotificationType.TagsChanging:
          break;
        case NotificationType.TagsChanged:
          break;
        case NotificationType.RatingChanging:
          break;
        case NotificationType.RatingChanged:
          break;
        case NotificationType.PlayCountersChanged:
          break;
        case NotificationType.ScreenSaverActivating:
          break;
        case NotificationType.ShutdownStarted:
          break;
        case NotificationType.EmbedInPanel:
          break;
        case NotificationType.PlayerRepeatChanged:
          break;
        case NotificationType.PlayerShuffleChanged:
          break;
        case NotificationType.PlayerEqualiserOnOffChanged:
          break;
        case NotificationType.PlayerScrobbleChanged:
          break;
        case NotificationType.ReplayGainChanged:
          break;
        case NotificationType.FileDeleting:
          break;
        case NotificationType.FileDeleted:
          break;
        case NotificationType.ApplicationWindowChanged:
          break;
        case NotificationType.StopAfterCurrentChanged:
          break;
        case NotificationType.LibrarySwitched:
          break;
        case NotificationType.FileAddedToLibrary:
          break;
        case NotificationType.FileAddedToInbox:
          break;
        case NotificationType.SynchCompleted:
          break;
        case NotificationType.DownloadCompleted:
          break;
        case NotificationType.MusicBeeStarted:
          break;
      }
    }

    private void PauseTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
    {
      _devcontroller.StopEffect();
    }

    public void UpdateSpectrographData()
    {
      float[] bardata = CalcBarData(_barcount);
      _devcontroller.Curbardata = bardata;
      _devcontroller.TrackProgress = _mbApiInterface.Player_GetPosition() * 1.0f / _mbApiInterface.NowPlaying_GetDuration();
      _debugplot?.UpdatePlot(bardata);
    }

    private float[] CalcBarData(int barcount)
    {
      float[] bardata = new float[barcount];
      float[] fftdata = new float[2048];
      var ret = _mbApiInterface.NowPlaying_GetSpectrumData(fftdata);
      int jumpwidth = (fftdata.Length/2) / barcount;
      int bar = 0;

      if (ret > 0)
      {
        for (int i = 0; i < fftdata.Length / 2; i += jumpwidth)
        {
          float avg = 0;
          for (int j = i; j < i + jumpwidth && j < fftdata.Length / 2; j++)
          {
            avg += fftdata[j];
          }
          avg /= jumpwidth;

          if (bar < bardata.Length - 1)
          {
            //bardata[bar] = (float) Math.Sqrt(avg) * 1000f;
            bardata[bar] = (float)Math.Sqrt(avg) * 10f;
            //bardata[bar] = (float)Math.Sqrt(avg) * 15f * (1 + (float)Math.Sqrt((double)bar/ (double)_barcount) * 1.25f);
            //bardata[bar] = (float)Math.Log10(1/(avg*avg)) * 10f;
          }
          bar++;
        }
        bardata[0] *= 0.6f; 
      }
      return bardata;
    }
  }
}