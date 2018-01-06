using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace MusicBeePlugin
{
  public partial class ClSettings : Form
  {
    private readonly ClDeviceController _deviceController;
    private readonly Plugin.PluginInfo _about;
    private readonly ClSettingsManager _settingsManager;

    public ClSettings(Plugin.PluginInfo about, ClDeviceController dc, ClSettingsManager settingsManager)
    {
      _deviceController = dc ?? throw new ArgumentNullException(nameof(dc));
      _about = about ?? throw new ArgumentNullException(nameof(about));
      _settingsManager = settingsManager ?? throw new ArgumentNullException(nameof(settingsManager));

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
      _settingsManager.SetLightbarProgress(_deviceController.GetKeyboardModel(), lightbarProgCheckBox.Checked);
    }

    private void ColorModeComboBoxOnSelectedIndexChanged(object sender, EventArgs eventArgs)
    {
      Enum.TryParse<ClSpectrumBrushFactory.ColoringMode>(colorModeComboBox.SelectedValue.ToString(), out var tmp);
      _settingsManager.SetColoringMode(_deviceController.GetKeyboardModel(), tmp);
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
      primaryColorPicker.BackColor = _settingsManager.GetPrimaryColor(_deviceController.GetKeyboardModel());
      backColorPicker.BackColor = _settingsManager.GetBackgroundColor(_deviceController.GetKeyboardModel());
      colorModeComboBox.SelectedItem = _settingsManager.GetColoringMode(_deviceController.GetKeyboardModel());
      lightbarProgCheckBox.Checked = _settingsManager.GetLightbarProgress(_deviceController.GetKeyboardModel());

    }

    public void PersistValues()
    {
      _settingsManager.Save();
    }

    private void primaryColorPicker_Click(object sender, EventArgs e)
    {
      if (colorDialog1.ShowDialog() != DialogResult.OK) return;
      _settingsManager.SetPrimaryColor(_deviceController.GetKeyboardModel(),colorDialog1.Color);
      primaryColorPicker.BackColor = colorDialog1.Color;
    }

    private void backColorPicker_Click(object sender, EventArgs e)
    {
      if (colorDialog1.ShowDialog() != DialogResult.OK) return;
      _settingsManager.SetBackgroundColor(_deviceController.GetKeyboardModel(), colorDialog1.Color);
      backColorPicker.BackColor = colorDialog1.Color;
    }

    public void Delete()
    {
      _settingsManager.Delete();
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
}