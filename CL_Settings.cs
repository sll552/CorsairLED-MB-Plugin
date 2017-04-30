using System;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace MusicBeePlugin
{
  public partial class ClSettings : Form
  {
    private readonly ClDeviceController _deviceController;
    private readonly ClSetting<Color> _effectSettingPrimaryColor = new ClSetting<Color>(new Color(), "ESprimColor");
    private readonly ClSetting<Color> _effectSettingBackgroundColor = new ClSetting<Color>(new Color(), "ESbackColor");
    private readonly ClSetting<ClSpectrumBrushFactory.ColoringMode> _effectSettingColorMode = new ClSetting<ClSpectrumBrushFactory.ColoringMode>(ClSpectrumBrushFactory.ColoringMode.Solid, "EScolorMode");
    private readonly ClSetting<bool> _effectSettingLightbarProgress = new ClSetting<bool>(false, "ESlbProg");
    private readonly string _configFile;
    private readonly Plugin.PluginInfo _about;

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
    public ClSpectrumBrushFactory.ColoringMode EffectSettingColorMode
    {
      get => _effectSettingColorMode.Value;
      private set => _effectSettingColorMode.Value = value;
    }
    public bool EffectSettingLightbarProgress
    {
      get => _effectSettingLightbarProgress.Value;
      private set => _effectSettingLightbarProgress.Value = value;
    }

    public ClSettings(Plugin.PluginInfo about, ClDeviceController dc, string configFileLocation)
    {
      _deviceController = dc ?? throw new ArgumentNullException(nameof(dc));
      _about = about ?? throw new ArgumentNullException(nameof(about));
      string configFileDir = Path.GetFullPath(configFileLocation ?? throw new ArgumentNullException(nameof(configFileLocation))) + "\\CorsairLED";

      if (!Directory.Exists(configFileDir))
      {
        Directory.CreateDirectory(configFileDir);
      }
      _configFile = configFileDir + "\\CorsairLED.config";

      InitializeComponent();

      if (ClDeviceController.IsInitialized)
      {
        UpdateValues();
        OneTimeInit();
      }

      FormClosing += CL_Settings_FormClosing;
      Shown += CL_Settings_OnShown;
    }

    private void OneTimeInit()
    {
      colorModeComboBox.DataSource = Enum.GetValues(typeof(ClSpectrumBrushFactory.ColoringMode));
      colorModeComboBox.SelectedIndexChanged += ColorModeComboBoxOnSelectedIndexChanged;
      detectedKeyboardLabel.Text = _deviceController.GetKeyboardModel();
      lightbarProgCheckBox.CheckStateChanged += LightbarProgCheckBoxOnCheckStateChanged;
    }

    private void LightbarProgCheckBoxOnCheckStateChanged(object sender, EventArgs eventArgs)
    {
      EffectSettingLightbarProgress = lightbarProgCheckBox.Checked;
    }

    private void ColorModeComboBoxOnSelectedIndexChanged(object sender, EventArgs eventArgs)
    {
      ClSpectrumBrushFactory.ColoringMode tmp;
      Enum.TryParse<ClSpectrumBrushFactory.ColoringMode>(colorModeComboBox.SelectedValue.ToString(), out tmp);
      EffectSettingColorMode = tmp;
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

      primaryColorPicker.BackColor = ColorTranslator.FromHtml(ReadKey(_effectSettingPrimaryColor.Key));
      EffectSettingPrimaryColor = primaryColorPicker.BackColor;
      backColorPicker.BackColor = ColorTranslator.FromHtml(ReadKey(_effectSettingBackgroundColor.Key));
      EffectSettingBackgroundColor = backColorPicker.BackColor;

      ClSpectrumBrushFactory.ColoringMode tmpColoringMode;
      Enum.TryParse<ClSpectrumBrushFactory.ColoringMode>(ReadKey(_effectSettingColorMode.Key), out tmpColoringMode);
      EffectSettingColorMode = tmpColoringMode;
      colorModeComboBox.SelectedItem = EffectSettingColorMode;

      bool tmpLightbarProg;
      Boolean.TryParse(ReadKey(_effectSettingLightbarProgress.Key), out tmpLightbarProg);
      EffectSettingLightbarProgress = tmpLightbarProg;
      lightbarProgCheckBox.Checked = tmpLightbarProg;

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
      PersistKey(_effectSettingColorMode.Key,
        Enum.GetName(typeof(ClSpectrumBrushFactory.ColoringMode), EffectSettingColorMode));
      PersistKey(_effectSettingLightbarProgress.Key, EffectSettingLightbarProgress.ToString());

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

    public void Delete()
    {
      if (File.Exists(_configFile))
      {
        File.Delete(_configFile);
      }
      if (Path.GetDirectoryName(_configFile) != null && Directory.Exists(Path.GetDirectoryName(_configFile)))
      {
        Directory.Delete(Path.GetDirectoryName(_configFile));
      }
    }

    private void aboutButton_Click(object sender, EventArgs e)
    {
      Debug.Assert(_about != null, "_about != null");
      MessageBox.Show(this,
        _about.Name + " v" + _about.VersionMajor + "." + _about.VersionMinor + "." + _about.Revision + "\nAuthor: " +
        _about.Author, "About CorsairLED", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    public void SetMessage(string msg)
    {
      messageLabel.Text = msg;
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