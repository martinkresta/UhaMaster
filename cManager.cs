using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Timers;

namespace UhaMaster
{
  public class cManager
  {

    private frmMain frm;
    private cSCOM scom;

    private System.Timers.Timer tim;

    public cManager()
    {

      try
      {
        scom = cSCOM.Instance;
        tim = new System.Timers.Timer();
        tim.Interval = 100;
        tim.Elapsed += Tim_Elapsed;
        tim.Start();
        // instantiate and show main form
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        frm = new frmMain(this);
        Application.Run(frm);
      }
      catch (Exception ex)
      {
        Err.Handle(this, ex);
      }
    }

    private void Tim_Elapsed(object sender, ElapsedEventArgs e)
    {
      scom.Update_100ms();
    }
  }

}
