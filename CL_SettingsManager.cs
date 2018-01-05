using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using SharpConfig;

namespace MusicBeePlugin
{
  /// <summary>
  /// Manages the Settings for the Plugin
  /// </summary>
  class ClSettingsManager
  {
    private const string GlobalSection = "Global";
    private readonly string _configFile;
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

    /// <summary>
    /// Create a new Configuration Manager for the given config file. If the file exists the config is read, otherwise a new config will be created.
    /// </summary>
    /// <param name="config">Path to the config file (can't be null)</param>
    public ClSettingsManager(string config)
    {
      _configFile = config ?? throw new ArgumentNullException();
      Configuration.RegisterTypeStringConverter(new ClColorStringConverter());
      Configuration.RegisterTypeStringConverter(new ClColoringModeStringConverter());
      _config = File.Exists(_configFile) ? Configuration.LoadFromFile(_configFile) : new Configuration();
    }

    /// <summary>
    /// Save the current <see cref="Configuration"/> to disk
    /// </summary>
    public void Save()
    {
      _config.SaveToFile(_configFile);
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
    public ClSpectrumBrushFactory.ColoringMode GetColoringMode(string device)
    {
      const string key = "ColoringMode";
      var section = GetSectionForDev(device, key);
      return section?[key].GetValue<ClSpectrumBrushFactory.ColoringMode>() ?? ClSpectrumBrushFactory.ColoringMode.Solid;
    }

    /// <summary>
    /// Set the coloring mode for the device
    /// </summary>
    /// <param name="device">The name of the device</param>
    /// <param name="cm">The coloring mode to set</param>
    public void SetColoringMode(string device, ClSpectrumBrushFactory.ColoringMode cm)
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
      return device == "K95 RGB PLATINUM" && _config[device]["LightbarProgress"].BoolValue;
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

      return _config.Contains(Defaultdev) ? _config[Defaultdev] : null;
    }

  }
}
