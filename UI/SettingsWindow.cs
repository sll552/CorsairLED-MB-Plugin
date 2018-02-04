using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
    private readonly BindingSource _binding = new BindingSource();

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
      _settingsManager.Discard();
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

      foreach (var dev in _devices)
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

      dataGridView1.CellValueChanged += DataGridView1OnCellValueChanged;
      dataGridView1.CellMouseUp += DataGridView1OnCellMouseUp;
      dataGridView1.DataBindingComplete += DataGridView1OnDataBindingComplete;

    }

    // Set the combobox datasource for the supported effects for each device
    private void DataGridView1OnDataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs dataGridViewBindingCompleteEventArgs)
    {
      foreach (DataGridViewRow row in dataGridView1.Rows)
      {
        if (row.Cells.Count <= dataGridView1.Columns["Effect"]?.Index) continue;
        if (row.Cells.Count <= dataGridView1.Columns["Name"]?.Index) continue;

        var tmp = (DataGridViewComboBoxCell)row.Cells[dataGridView1.Columns["Effect"].Index];
        var devname = row.Cells[dataGridView1.Columns["Name"].Index].Value.ToString();
        var effects = _devices.First(device => device.DeviceName == devname).GetSupportedEffects();
        var enumerable = effects as AbstractEffectDevice.Effect[] ?? effects.ToArray();
        tmp.DataSource = enumerable.ToArray();
      }
    }

    // Cancel Edit as soon as MouseUp so that the CellValueChangedEvent is fired
    private void DataGridView1OnCellMouseUp(object sender, DataGridViewCellMouseEventArgs dataGridViewCellMouseEventArgs)
    {
      if (dataGridViewCellMouseEventArgs.ColumnIndex == dataGridView1.Columns["Default"]?.Index)
      {
        dataGridView1.EndEdit();
      }
    }

    private void DataGridView1OnCellValueChanged(object sender, DataGridViewCellEventArgs dataGridViewCellEventArgs)
    {
      if (dataGridViewCellEventArgs.ColumnIndex != dataGridView1.CurrentCell.ColumnIndex ||
          dataGridViewCellEventArgs.RowIndex != dataGridView1.CurrentCell.RowIndex) return;
      DataGridViewColumn defaultcolumn = null;
      foreach (DataGridViewColumn column in dataGridView1.Columns)
      {
        if (column.Name != "Default") continue;
        defaultcolumn = column;
        break;
      }

      if (defaultcolumn == null || dataGridViewCellEventArgs.ColumnIndex != defaultcolumn.Index) return;

      _binding.ResetBindings(false);
      for (var i = 0; i < dataGridView1.RowCount; i++)
      {
        if (i == dataGridViewCellEventArgs.RowIndex) continue;
        dataGridView1.UpdateCellValue(defaultcolumn.Index, i);
      }
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
        if (dev.IsDefaultDevice)
        {
          _tabPages.Insert(0, tabpage);
        }
        else
        {
          _tabPages.Add(tabpage);
        }
      }

      foreach (var page in _tabPages)
      {
        tabControl1.Controls.Add(page);
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