using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicBeePlugin
{
  public partial class CL_Settings : Form
  {
    private CL_DeviceController deviceController = null;

    public CL_Settings() : this(null)
    {
    }

    public CL_Settings(CL_DeviceController dc)
    {
      if (dc != null)
      {
        this.deviceController = dc;
      }
      InitializeComponent();
    }

  }
}
