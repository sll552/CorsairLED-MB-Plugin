﻿using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MusicBeePlugin
{
  public partial class ClSettings : Form
  {
    private readonly ClDeviceController _deviceController;
    private readonly ClSetting<Color> _effectSettingPrimaryColor = new ClSetting<Color>(new Color(), "ESprimColor");
    private readonly ClSetting<Color> _effectSettingBackgroundColor = new ClSetting<Color>(new Color(), "ESbackColor");
    private readonly string _configFile;

    public Color EffectSettingPrimaryColor
    {
      get => _effectSettingPrimaryColor.Value;
      private set => _effectSettingPrimaryColor.Value = value;
    }
    public Color EffectSettingBackgroundColor
    {
      get => _effectSettingBackgroundColor.Value;
      private set => _effectSettingBackgroundColor.Value = value;
    }

    public ClSettings(ClDeviceController dc, string configFileLocation)
    {
      _deviceController = dc ?? throw new ArgumentNullException(nameof(dc));
      string configFileDir = Path.GetFullPath(configFileLocation ?? throw new ArgumentNullException(nameof(configFileLocation))) + "\\CorsairLED";

      if (!Directory.Exists(configFileDir))
      {
        Directory.CreateDirectory(configFileDir);
      }
      _configFile = configFileDir + "\\CorsairLED.config";

      InitializeComponent();

      if (dc.IsInitialized)
      {
        UpdateValues();
      }

      FormClosing += CL_Settings_FormClosing;
      Shown += CL_Settings_OnShown;
    }

    private void CL_Settings_OnShown(object sender, EventArgs eventArgs)
    {
      UpdateValues();
    }

    private void CL_Settings_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (e.CloseReason != CloseReason.UserClosing) return;
      Hide();
      e.Cancel = true;
    }

    private void saveCloseButton_Click(object sender, EventArgs e)
    {
      PersistValues();
      Hide();
    }

    private void UpdateValues()
    {
      string ReadKey(string key)
      {
        ExeConfigurationFileMap map = new ExeConfigurationFileMap { ExeConfigFilename = Path.GetFullPath(_configFile) };
        var configFile = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
        var appSettings = configFile.AppSettings.Settings;

        return appSettings[key]?.Value;
      }

      detectedKeyboardLabel.Text = _deviceController.GetKeyboardModel();

      primaryColorPicker.BackColor = ColorTranslator.FromHtml(ReadKey(_effectSettingPrimaryColor.Key));
      EffectSettingPrimaryColor = primaryColorPicker.BackColor;
      backColorPicker.BackColor = ColorTranslator.FromHtml(ReadKey(_effectSettingBackgroundColor.Key));
      EffectSettingBackgroundColor = backColorPicker.BackColor;

    }

    public void PersistValues()
    {
      void PersistKey(string key, string value)
      {
        ExeConfigurationFileMap map = new ExeConfigurationFileMap { ExeConfigFilename = Path.GetFullPath(_configFile) };
        var configFile = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
        var settings = configFile.AppSettings.Settings;

        if (settings[key] == null)
        {
          settings.Add(key, value);
        }
        else
        {
          settings[key].Value = value;
        }
        configFile.Save(ConfigurationSaveMode.Modified);
        ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
      }

      PersistKey(_effectSettingPrimaryColor.Key, ColorTranslator.ToHtml(EffectSettingPrimaryColor));
      PersistKey(_effectSettingBackgroundColor.Key, ColorTranslator.ToHtml(EffectSettingBackgroundColor));

    }

    private void primaryColorPicker_Click(object sender, EventArgs e)
    {
      if (colorDialog1.ShowDialog() == DialogResult.OK)
      {
        EffectSettingPrimaryColor = colorDialog1.Color;
        primaryColorPicker.BackColor = colorDialog1.Color;
      }
    }

    private void backColorPicker_Click(object sender, EventArgs e)
    {
      if (colorDialog1.ShowDialog() == DialogResult.OK)
      {
        EffectSettingBackgroundColor = colorDialog1.Color;
        backColorPicker.BackColor = colorDialog1.Color;
      }
    }
  }

  internal class ClSetting<TValue>
  {
    public TValue Value { get; set; }
    public string Key { get; }

    public ClSetting(TValue value, string key)
    {
      if (value == null) throw new ArgumentNullException(nameof(value));
      Key = key ?? throw new ArgumentNullException(nameof(key));
      Value = value;
    }
  }
}