using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using MusicBeePlugin.Effects;
using SharpConfig;
using Configuration = SharpConfig.Configuration;

namespace MusicBeePlugin.Settings
{
  /// <summary>
  /// Manages the Settings for the Plugin
  /// </summary>
  public class SettingsManager
  {
    private const string GlobalSection = "Global";
    private readonly Configuration _config;
    private string _defaultDev;

    /// <summary>
    /// The device to use as provider for default values for colors
    /// </summary>
    public string Defaultdev
    {
      get => _defaultDev;
      set
      {
        _defaultDev = value;
        var section = _config[GlobalSection];
        section["DefaultDevice"].StringValue = _defaultDev;
      }
    }

    public string ConfigFile { get; }

    /// <summary>
    /// Create a new Configuration Manager for the given config file. If the file exists the config is read, otherwise a new config will be created.
    /// </summary>
    /// <param name="config">Path to the config file (can't be null)</param>
    public SettingsManager(string config)
    {
      ConfigFile = config ?? throw new ArgumentNullException();
      Configuration.RegisterTypeStringConverter(new ColorStringConverter());
      Configuration.RegisterTypeStringConverter(new ColoringModeStringConverter());
      if (Path.GetDirectoryName(ConfigFile) != null && !Directory.Exists(Path.GetDirectoryName(ConfigFile)))
      {
        Directory.CreateDirectory(Path.GetDirectoryName(ConfigFile) ?? throw new InvalidOperationException());
      }
      _config = File.Exists(ConfigFile) ? Configuration.LoadFromFile(ConfigFile) : new Configuration();
    }

    /// <summary>
    /// Save the current <see cref="Configuration"/> to disk
    /// </summary>
    public void Save()
    {
      _config.SaveToFile(ConfigFile);
    }

    /// <summary>
    /// Delete the config file and clear the current <see cref="Configuration"/>
    /// </summary>
    public void Delete()
    {
      if (File.Exists(ConfigFile))
      {
        File.Delete(ConfigFile);
      }
      if (Path.GetDirectoryName(ConfigFile) != null && Directory.Exists(Path.GetDirectoryName(ConfigFile)))
      {
        Directory.Delete(Path.GetDirectoryName(ConfigFile) ?? throw new InvalidOperationException());
      }
      _config.Clear();
    }

    /// <summary>
    /// Get the primary effect color for the device
    /// </summary>
    /// <param name="device">The name of the device</param>
    /// <returns>The configured color, or RED if none can be found</returns>
    public Color GetPrimaryColor(string device)
    {
      const string key = "PrimaryColor";
      var section = GetSectionForDev(device, key);
      return section?[key].GetValue<Color>() ?? Color.Red;
    }

    /// <summary>
    /// Set the primary effect color for the device
    /// </summary>
    /// <param name="device">The name of the device</param>
    /// <param name="color">The color to set</param>
    public void SetPrimaryColor(string device, Color color)
    {
      var section =_config[device];
      section["PrimaryColor"].SetValue(color);
    }

    /// <summary>
    /// Get the background color for the device
    /// </summary>
    /// <param name="device">The name of the device</param>
    /// <returns>The configured color, or BLACK if none can be found</returns>
    public Color GetBackgroundColor(string device)
    {
      const string key = "BackgroundColor";
      var section = GetSectionForDev(device, key);
      return section?[key].GetValue<Color>() ?? Color.Black;
    }

    /// <summary>
    /// Set the background color for the device
    /// </summary>
    /// <param name="device">The name of the device</param>
    /// <param name="color">The color to set</param>
    public void SetBackgroundColor(string device, Color color)
    {
      var section = _config[device];
      section["BackgroundColor"].SetValue(color);
    }

    /// <summary>
    /// Get the coloring mode for the device
    /// </summary>
    /// <param name="device">The name of the device</param>
    /// <returns>The configured coloring mode, or ColoringMode.Solid if none can be found</returns>
    public SpectrumBrushFactory.ColoringMode GetColoringMode(string device)
    {
      const string key = "ColoringMode";
      var section = GetSectionForDev(device, key);
      return section?[key].GetValue<SpectrumBrushFactory.ColoringMode>() ?? SpectrumBrushFactory.ColoringMode.Solid;
    }

    /// <summary>
    /// Set the coloring mode for the device
    /// </summary>
    /// <param name="device">The name of the device</param>
    /// <param name="cm">The coloring mode to set</param>
    public void SetColoringMode(string device, SpectrumBrushFactory.ColoringMode cm)
    {
      var section = _config[device];
      section["ColoringMode"].SetValue(cm);
    }

    /// <summary>
    /// Get the setting for the k95 platinum lightbar
    /// </summary>
    /// <param name="device">The name of the device</param>
    /// <returns>The configured lightbar setting, if the device is not a k95 platinum false is returned</returns>
    public bool GetLightbarProgress(string device)
    {
      if (device == "K95 RGB PLATINUM")
      {
         return _config[device].Contains("LightbarProgress") && _config[device]["LightbarProgress"].BoolValue;
      }

      return false;
    }

    /// <summary>
    /// Set the setting for the k95 platinum lightbar
    /// </summary>
    /// <param name="device">The name of the device</param>
    /// <param name="lbprog">The lightbar setting</param>
    public void SetLightbarProgress(string device, bool lbprog)
    {
      if (device == "K95 RGB PLATINUM")
      {
        _config[device]["LightbarProgress"].BoolValue = lbprog;
      }
    }

    /// <summary>
    /// Gets the corresponding <see cref="Section"/> for the given device name, if there is no Section for the specific device, the configured <see cref="Defaultdev"/> Section is used.
    /// If none of these Sections exist null is returned
    /// </summary>
    /// <param name="device">The name of the requested device</param>
    /// <param name="key">The requested config key</param>
    /// <returns>The <see cref="Section"/> to use for the device or null if no default device is configured</returns>
    private Section GetSectionForDev(string device, string key)
    {
      if (_config.Contains(device) && _config[device].Contains(key))
      {
        return _config[device];
      }

      return Defaultdev != null &&_config.Contains(Defaultdev) ? _config[Defaultdev] : null;
    }

    /// <summary>
    /// Migrates settings from the old configuration done in app.config
    /// </summary>
    /// <param name="oldConfigFile">Path to the old config file</param>
    /// <param name="device">The device to use for all values</param>
    public void MigrateFromOld(string oldConfigFile, string device)
    {
      string ReadKey(string key)
      {
        var map = new ExeConfigurationFileMap { ExeConfigFilename = Path.GetFullPath(oldConfigFile) };
        var configFile = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
        var appSettings = configFile.AppSettings.Settings;

        return appSettings[key]?.Value;
      }

      SetPrimaryColor(device, ColorTranslator.FromHtml(ReadKey("ESprimColor")));
      SetBackgroundColor(device, ColorTranslator.FromHtml(ReadKey("ESbackColor")));

      Enum.TryParse<SpectrumBrushFactory.ColoringMode>(ReadKey("EScolorMode"), out var tmpColoringMode);
      SetColoringMode(device, tmpColoringMode);

      bool.TryParse(ReadKey("ESlbProg"), out var tmpLightbarProg);
      SetLightbarProgress(device, tmpLightbarProg);
    }
  }
}
