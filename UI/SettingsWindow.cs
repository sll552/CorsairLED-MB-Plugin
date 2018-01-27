using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using MusicBeePlugin.Devices;
using MusicBeePlugin.Settings;

namespace MusicBeePlugin.UI
{
  public partial class SettingsWindow : Form
  {
    // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
    private readonly DeviceController _deviceController;
    private readonly Plugin.PluginInfo _about;
    private readonly SettingsManager _settingsManager;
    private readonly List<AbstractEffectDevice> _devices;
    // ReSharper disable once CollectionNeverQueried.Local
    private readonly List<TabPage> _tabPages = new List<TabPage>();
    private BindingSource _binding = new BindingSource();

    public SettingsWindow(Plugin.PluginInfo about, DeviceController dc, SettingsManager settingsManager)
    {
      _deviceController = dc ?? throw new ArgumentNullException(nameof(dc));
      _about = about ?? throw new ArgumentNullException(nameof(about));
      _settingsManager = settingsManager ?? throw new ArgumentNullException(nameof(settingsManager));

      InitializeComponent();

      if (DeviceController.IsInitialized)
      {
        _devices = new List<AbstractEffectDevice>(_deviceController.Devices);
        UpdateDeviceTable();
        CreateTabs();
        UpdateValues();
      }

      FormClosing += CL_Settings_FormClosing;
      Shown += CL_Settings_OnShown;
    }
    
    private void CL_Settings_OnShown(object sender, EventArgs eventArgs)
    {
      UpdateValues();
      dataGridView1.AutoResizeColumns();
    }

    private void CL_Settings_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (e.CloseReason != CloseReason.UserClosing) return;
      Hide();
      e.Cancel = true;
    }

    private void SaveCloseButton_Click(object sender, EventArgs e)
    {
      PersistValues();
      Hide();
    }

    private void UpdateDeviceTable()
    {
      dataGridView1.Columns.Clear();
      dataGridView1.Rows.Clear();
      _binding.Clear();
      _binding.DataSource = typeof(AbstractEffectDevice);

      foreach (AbstractEffectDevice dev in _devices)
      {
        _binding.Add(dev);
      }

      dataGridView1.AutoGenerateColumns = false;
      dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
      dataGridView1.DataSource = _binding;

      dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
      {
        ReadOnly = true,
        AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
        DataPropertyName = "DeviceName",
        Name = "Name"
      });

      dataGridView1.Columns.Add(new DataGridViewCheckBoxColumn
      {
        DataPropertyName = "IsDefaultDevice",
        Name = "Default"
      });

      dataGridView1.Columns.Add(new DataGridViewComboBoxColumn
      {
        DataSource = Enum.GetValues(typeof(AbstractEffectDevice.Effect)),
        DataPropertyName = "ActiveEffect",
        Name = "Effect"
      });

      dataGridView1.Columns.Add(new DataGridViewCheckBoxColumn
      {
        DataPropertyName = "Enabled",
        Name = "Enabled"
      });
    }

    private void CreateTabs()
    {
      tabControl1.Controls.Clear();
      foreach (var dev in _devices)
      {
        var deviceSettings = new DeviceSettings(_settingsManager, dev);
        var tabpage = new TabPage(dev.DeviceName);
        deviceSettings.Dock = DockStyle.Fill;
        tabpage.Controls.Add(deviceSettings);
        _tabPages.Add(tabpage);
        tabControl1.Controls.Add(tabpage);
      }
    }

    private void UpdateValues()
    {
      foreach (var tab in tabControl1.Controls)
      {
        if (tab is DeviceSettings deviceSettings)
        {
          deviceSettings.UpdateValues();
        }
      }
    }

    public void PersistValues()
    {
      _settingsManager.Save();
    }

    public void Delete()
    {
      _settingsManager.Delete();
    }

    private void aboutButton_Click(object sender, EventArgs e)
    {
      Debug.Assert(_about != null, "_about != null");
      MessageBox.Show(this,
        $@"{_about.Name} v{_about.VersionMajor}.{_about.VersionMinor}.{_about.Revision}
Author: {_about.Author}", @"About CorsairLED", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    public void SetMessage(string msg)
    {
      messageLabel.Text = msg;
    }
  }
}