using System;
using System.Linq;
using System.Windows.Forms;
using MusicBeePlugin.Devices;
using MusicBeePlugin.Effects;
using MusicBeePlugin.Settings;

namespace MusicBeePlugin.UI
{
  public partial class DeviceSettings : UserControl
  {
    private readonly SettingsManager _settings;
    private readonly AbstractEffectDevice _device;

    public DeviceSettings(SettingsManager manager, AbstractEffectDevice device)
    {
      _settings = manager ?? throw new ArgumentNullException(nameof(manager));
      _device = device ?? throw new ArgumentNullException(nameof(device));
      InitializeComponent();
      OneTimeInit();
    }

    private void OneTimeInit()
    {
      if (_device.GetSupportedEffects().Contains(AbstractEffectDevice.Effect.Spectrograph))
      {
        colorModeComboBox.DataSource = Enum.GetValues(typeof(SpectrumBrushFactory.ColoringMode));
        colorModeComboBox.SelectedIndexChanged += ColorModeComboBoxOnSelectedIndexChanged;
      }
      else
      {
        colorModeComboBox.Visible = false;
        label4.Visible = false;
      }

      if (_device.DeviceName.Equals("K95 RGB PLATINUM"))
      {
        lightbarProgCheckBox.CheckStateChanged += LightbarProgCheckBoxOnCheckStateChanged;
      }
      else
      {
        lightbarProgCheckBox.Visible = false;
      }
      UpdateValues();
    }

    public void UpdateValues()
    {
      // We basically don't care about updating values that are not visible as SettingsManager is expected to always return a valid value
      primaryColorPicker.BackColor = _settings.GetPrimaryColor(_device.DeviceName);
      backColorPicker.BackColor = _settings.GetBackgroundColor(_device.DeviceName);
      colorModeComboBox.SelectedItem = _settings.GetColoringMode(_device.DeviceName);
      lightbarProgCheckBox.Checked = _settings.GetLightbarProgress(_device.DeviceName);
    }

    private void LightbarProgCheckBoxOnCheckStateChanged(object sender, EventArgs eventArgs)
    {
      _settings.SetLightbarProgress(_device.DeviceName, lightbarProgCheckBox.Checked);
    }

    private void ColorModeComboBoxOnSelectedIndexChanged(object sender, EventArgs eventArgs)
    {
      Enum.TryParse<SpectrumBrushFactory.ColoringMode>(colorModeComboBox.SelectedValue.ToString(), out var tmp);
      _settings.SetColoringMode(_device.DeviceName, tmp);
    }

    private void primaryColorPicker_Click(object sender, EventArgs e)
    {
      if (colorDialog1.ShowDialog() != DialogResult.OK) return;
      _settings.SetPrimaryColor(_device.DeviceName, colorDialog1.Color);
      primaryColorPicker.BackColor = colorDialog1.Color;
    }

    private void backColorPicker_Click(object sender, EventArgs e)
    {
      if (colorDialog1.ShowDialog() != DialogResult.OK) return;
      _settings.SetBackgroundColor(_device.DeviceName, colorDialog1.Color);
      backColorPicker.BackColor = colorDialog1.Color;
    }
  }
}
