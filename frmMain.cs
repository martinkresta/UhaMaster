using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace UhaMaster
{
    public partial class frmMain : Form
    {
        private cManager mgr;
        private cSCOM scom;
        private cVARS vars;
      



        private bool mUserInput;

        public frmMain(cManager Mgr)
        {
            mgr = Mgr;
            scom = cSCOM.Instance;
            vars = cVARS.Instance;
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            try
            {
                // Initialize the GUI
                DateTime buildDate = new FileInfo(Assembly.GetExecutingAssembly().Location).LastWriteTime;

               

                // Udpdate version info:
                Version version = Assembly.GetExecutingAssembly().GetName().Version;
                // form title
                this.Text = "UHA Master"  + " " + version.Major + "." + version.Minor;
                // about label 
                // lblVersionInfo.Text = version.Major + "." + version.Minor + "." + version.Build;
                // lblBuildDate.Text = buildDate.ToShortDateString();


                cbPort.Items.Clear();
                foreach (string s in scom.GetPortList())
                {
                    cbPort.Items.Add(s);
                }
                if (cbPort.Items.Count > 0)
                {
                    cbPort.SelectedIndex = 0;
                }

            }
            catch (Exception ex)
            {
                Err.Handle(this, ex);
            }

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            scom.Connect();
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            scom.Disconnect();
        }

    private void timRefresh_Tick(object sender, EventArgs e)
    {

      cVariable tmp;
      foreach (Control tb in gbTemperatures.Controls)
      {
        if (tb is TextBox)
        {
          tmp = vars.GetVariable(Convert.ToInt16(tb.Tag));
          double val = tmp.Value / 10.0;
          if (tmp.State == eVarState.evs_Valid)
          {
            tb.BackColor = SystemColors.Control;
          }
          else
          {
            tb.BackColor = Color.Red;
          }
          tb.Text = val.ToString("0.0");
        }
      }

      foreach (Control tb in gbTempsIOBD.Controls)
      {
        if (tb is TextBox)
        {
          tmp = vars.GetVariable(Convert.ToInt16(tb.Tag));
          double val = tmp.Value / 10.0;
          if (tmp.State == eVarState.evs_Valid)
          {
            tb.BackColor = SystemColors.Control;
          }
          else
          {
            tb.BackColor = Color.Red;
          }
          tb.Text = val.ToString("0.0");
        }
      }

      foreach (Control tb in gbElHeater.Controls)
      {
        double val;
        if (tb is TextBox)
        {
          tmp = vars.GetVariable(Convert.ToInt16(tb.Tag));
          if (Convert.ToInt16(tb.Tag)!= cVARS.VAR_BAT_SOC)
          {
            val = tmp.Value / 10.0;
          }
          else
          {
            val = tmp.Value;
          }
          if (tmp.State == eVarState.evs_Valid)
          {
            tb.BackColor = SystemColors.Control;
          }
          else
          {
            tb.BackColor = Color.Red;
          }
          tb.Text = val.ToString("0.0");
        }
      }


      /* El heaters update */
      tmp = vars.GetVariable(cVARS.VAR_EL_HEATER_STATUS);
      if (tmp.State == eVarState.evs_Valid)
      {
        cbElStatus.BackColor = SystemColors.Control;
      }
      else
      {
        cbElStatus.BackColor = Color.Red;
      }
      cbElStatus.SelectedIndex = tmp.Value;

      tmp = vars.GetVariable(cVARS.VAR_EL_HEATER_CURRENT);
      if (tmp.State == eVarState.evs_Valid)
      {
        tbHeaterPower.BackColor = SystemColors.Control;
      }
      else
      {
        tbHeaterPower.BackColor = Color.Red;
      }
      tbHeaterPower.Text = (tmp.Value/100.0).ToString(); 

      /*update network status*/
      tmp = vars.GetVariable(cVARS.VAR_NETWORK_STATUS);
      short ns = tmp.Value;
      chbIOD.Checked =   ((ns & 0x02) == 0x02) ? true: false;
      chbIOU.Checked =   ((ns & 0x04) == 0x04) ? true: false;
      chbTECHM.Checked = ((ns & 0x08) == 0x08) ? true : false;
      chbELECON.Checked = ((ns & 0x10) == 0x10) ? true : false;
      chbREKU.Checked = ((ns & 0x20) == 0x20) ? true : false;
      chbRPI.Checked = ((ns & 0x40) == 0x40) ? true : false;


      // update watermeter values

      tmp = vars.GetVariable(cVARS.VAR_CONS_COLD);
      if (tmp.State == eVarState.evs_Valid)
      {
        tbConsCold.BackColor = SystemColors.Control;
      }
      else
      {
        tbConsCold.BackColor = Color.Red;
      }
      tbConsCold.Text = (tmp.Value / 10.0).ToString();

      tmp = vars.GetVariable(cVARS.VAR_CONS_HOT);
      if (tmp.State == eVarState.evs_Valid)
      {
        tbConsHot.BackColor = SystemColors.Control;
      }
      else
      {
        tbConsHot.BackColor = Color.Red;
      }
      tbConsHot.Text = (tmp.Value / 10.0).ToString();




      tmp = vars.GetVariable(cVARS.VAR_FLOW_COLD);
      if (tmp.State == eVarState.evs_Valid)
      {
        tbFlowCold.BackColor = SystemColors.Control;
      }
      else
      {
        tbFlowCold.BackColor = Color.Red;
      }
      tbFlowCold.Text = (tmp.Value / 10.0).ToString();

      tmp = vars.GetVariable(cVARS.VAR_FLOW_HOT);
      if (tmp.State == eVarState.evs_Valid)
      {
        tbFlowHot.BackColor = SystemColors.Control;
      }
      else
      {
        tbFlowHot.BackColor = Color.Red;
      }
      tbFlowHot.Text = (tmp.Value / 10.0).ToString();


    }

    private void btnScan_Click(object sender, EventArgs e)
    {
      foreach (Control tb in gbTemperatures.Controls)
      {
        if (tb is TextBox)
        {
          if (Convert.ToInt16(tb.Tag) != 0)
          {
            scom.ScanVariable(Convert.ToInt16(tb.Tag), 1000);
          }
        }
      }
    }

    private void btnStopScan_Click(object sender, EventArgs e)
    {
      foreach (Control tb in gbTemperatures.Controls)
      {
          if (tb is TextBox)
          {
            if (Convert.ToInt16(tb.Tag) != 0)
            {
              scom.ScanVariable(Convert.ToInt16(tb.Tag), 0);
            }
          }
      }
    }

    private void btnScanIOBD_Click(object sender, EventArgs e)
    {
      foreach (Control tb in gbTempsIOBD.Controls)
      {
        if (tb is TextBox)
        {
          if (Convert.ToInt16(tb.Tag) != 0)
          {
            scom.ScanVariable(Convert.ToInt16(tb.Tag), 1000);
          }
        }
      }
    }

    private void cbPort_SelectedIndexChanged(object sender, EventArgs e)
    {
      scom.SelectPort(cbPort.SelectedItem.ToString());
    }

    private void chbPumpBoiler_CheckedChanged(object sender, EventArgs e)
    {
      SendPumpCntrol();
    }

    private void chbPumpRadiators_CheckedChanged(object sender, EventArgs e)
    {
      SendPumpCntrol();
    }

    private void chbPumpWall_CheckedChanged(object sender, EventArgs e)
    {
      SendPumpCntrol();
    }

    private void SendPumpCntrol()
    {
      short pumps = 0;
      if (chbPumpBoiler.Checked)
      {
        pumps |= 0x01;
      }
      if (chbPumpWall.Checked)
      {
        pumps |= 0x02;
      }
      if (chbPumpRadiators.Checked)
      {
        pumps |= 0x04;
      }
      scom.Send(cSCOM.CMD_TM_SET_PUMPS, pumps, 0, 0, 0);
    }

    private void chbWaterValve_CheckedChanged(object sender, EventArgs e)
    {
      short valve = 0;
      if (chbWaterValve.Checked)
      {
        valve = 1;
      }
      scom.Send(cSCOM.CMD_TM_SET_ELV, valve, 0, 0, 0);
    }




    private void btnSetSOC_Click(object sender, EventArgs e)
    {
      scom.Send(cSCOM.CMD_SET_VAR_VALUE, cVARS.VAR_BAT_SOC, Convert.ToInt16(nudSoc.Value), 0, 0);
    }

    private void btnSetCharging_Click(object sender, EventArgs e)
    {
      scom.Send(cSCOM.CMD_SET_VAR_VALUE, cVARS.VAR_CHARGING_A10, Convert.ToInt16(nudCharging.Value * 10), 0, 0);
    }

    private void btnSetLoad_Click(object sender, EventArgs e)
    {
      scom.Send(cSCOM.CMD_SET_VAR_VALUE, cVARS.VAR_LOAD_A10, Convert.ToInt16(nudLoad.Value * 10), 0, 0);
    }

    private void btnSetVbat_Click(object sender, EventArgs e)
    {
      scom.Send(cSCOM.CMD_SET_VAR_VALUE, cVARS.VAR_BAT_VOLTAGE_V10, Convert.ToInt16(nudVbat.Value * 10), 0, 0);
    }


    // stting the servovalves
    private void tbRad_Scroll(object sender, EventArgs e)
    {
      scom.Send(cSCOM.CMD_TM_SET_SERVOVALVES, Convert.ToInt16(tbRad.Value), Convert.ToInt16(tbWall.Value), 0, 0);
    }

    private void trackBar1_Scroll(object sender, EventArgs e)
    {
      scom.Send(cSCOM.CMD_TM_SET_SERVOVALVES, Convert.ToInt16(tbRad.Value), Convert.ToInt16(tbWall.Value), 0, 0);
    }

    private void label59_Click(object sender, EventArgs e)
    {

    }
  }
}
