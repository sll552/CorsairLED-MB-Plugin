using System;
using System.Windows.Forms;

namespace MusicBeePlugin
{
  public partial class ClSettings : Form
  {
    private readonly ClDeviceController _deviceController;

    public ClSettings(ClDeviceController dc)
    {
      _deviceController = dc ?? throw new ArgumentNullException(nameof(dc));
      InitializeComponent();
      if (dc.IsInitialized())
      {
        UpdateValues();
      }      
    }

    private void UpdateValues()
    {
      if (detectedKeyboardLabel != null)
      {
        detectedKeyboardLabel.Text = _deviceController.GetKeyboardModel();
      }
    }

    private void saveCloseButton_Click(object sender, EventArgs e)
    {
      Hide();
    }
  }
}