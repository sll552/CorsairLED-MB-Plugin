using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Diagnostics;
using CUE.NET.Exceptions;
using CUE.NET.Devices.Generic.Enums;

namespace MusicBeePlugin
{
  public partial class Plugin
  {
    private MusicBeeApiInterface _mbApiInterface;
    private PluginInfo _about = new PluginInfo();
    private ClSettings _settings;
    private ClDeviceController _devcontroller = new ClDeviceController();
    private ClDebugPlot _debugplot = new ClDebugPlot();

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

      Debug.WriteLine(_about.Name + " loaded");
      return _about;
    }

    private void ShowDebugPlot(object sender, EventArgs e)
    {
      _debugplot.Show();
      float[] data = new float[100]; 
      for (int i = 0; i < 100; i++)
      {
        data[i] = i;
        if (i > data.Length/2)
        {
          data[i] = data.Length - i;
        }
      }
      _debugplot.UpdatePlot(data);
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
              // ...
              break;
          }
          break;
        case NotificationType.TrackChanged:
          string artist = _mbApiInterface.NowPlaying_GetFileTag(MetaDataType.Artist);
          // ...
          break;
      }
    }

    // return an array of lyric or artwork provider names this plugin supports
    // the providers will be iterated through one by one and passed to the RetrieveLyrics/ RetrieveArtwork function in order set by the user in the MusicBee Tags(2) preferences screen until a match is found
    public string[] GetProviders()
    {
      return null;
    }

    // return lyrics for the requested artist/title from the requested provider
    // only required if PluginType = LyricsRetrieval
    // return null if no lyrics are found
    public string RetrieveLyrics(string sourceFileUrl, string artist, string trackTitle, string album, bool synchronisedPreferred, string provider)
    {
      return null;
    }

    // return Base64 string representation of the artwork binary data from the requested provider
    // only required if PluginType = ArtworkRetrieval
    // return null if no artwork is found
    public string RetrieveArtwork(string sourceFileUrl, string albumArtist, string album, string provider)
    {
      //Return Convert.ToBase64String(artworkBinaryData)
      return null;
    }
  }
}