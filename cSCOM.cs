using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace UhaMaster
{

  /// <summary>
  /// Singleton class for access to serial VCP communication 
  /// </summary>
  public class cSCOM
  {

    public const short CMD_GET_DEV_ID = 0x710;
    public const short CMD_GET_STATUS = 0x712;
    public const short CMD_READ_VAR_REQUEST = 0x713;
    public const short CMD_TM_SET_POWER_OUTPUTS = 0x721;
    public const short CMD_TM_SET_ELV = 0x722;
    public const short CMD_TM_SET_AV = 0x723;
    public const short CMD_TM_SET_SERVOVALVES = 0x724;
    public const short CMD_TM_SET_PUMPS = 0x725;

    public const short CMD_MASTER_HB = 0x777;

    public const short CMD_TM_DEV_ID = 0x210;
    public const short CMD_TM_STATUS = 0x212;
    public const short CMD_TM_VAR_VALUE = 0x221;

    public const short CMD_IOD_BUTTON_STATE = 0x101;
    public const short CMD_IOU_BUTTON_STATE = 0x102;

    public const short CMD_SET_VAR_VALUE = 0x120;



    public const int HB_PERIOD = 1000;


    private string ComPort;

    // sigleton selfreference:
    private static cSCOM me;

    private int HbTimer;
    private SerialPort sp;
    private cVARS vars;
    byte[] rxData;

    /// <summary>
    /// Private constructor
    /// </summary>
    private cSCOM()
    {
      HbTimer = HB_PERIOD;
      sp = new SerialPort();
      sp.DataReceived += Sp_DataReceived;
      sp.BaudRate = 57600;
      sp.DataBits = 8;
      sp.StopBits = StopBits.One;
      sp.Parity = Parity.None;
      vars = cVARS.Instance;
      rxData = new byte[10];
      if (SerialPort.GetPortNames().Count() > 0)
      {
        sp.PortName = SerialPort.GetPortNames()[0];

      }
    }



    /// <summary>
    /// Gets a reference to single instance of cSCOM
    /// </summary>
    public static cSCOM Instance
    {
      get
      {
        if (me == null)
        {    // if object doesn't exist yet, create one using private c'tor
          me = new cSCOM();
        }
        return me;          // return reference
      }
    }


    public void SelectPort(string port)
    {
      sp.PortName = port;
    }

    public void Connect()
    {
      if (sp.IsOpen == false)
      {
        sp.Open();
      }
    }

    public void Disconnect()
    {
      if (sp.IsOpen == true)
      {
        sp.Close();
      }
    }

    public string[] GetPortList()
    {
      return SerialPort.GetPortNames();
    }


    // periodic update function
    public void Update_100ms()
    {
      // Send a heart beat to notify device that PC app is connected
      HbTimer -= 100;
      if(HbTimer <= 0 && sp.IsOpen)
      {
        HbTimer = HB_PERIOD;
        Send(CMD_MASTER_HB, 0, 0, 0, 0);
      }
      
    }

      public void Send(short cmd, short data1, short data2, short data3, short data4)
      {
          byte[] data;
          if (sp.IsOpen)
          {
              data = new byte[10];
              data[0] = (byte)(cmd >> 8);
              data[1] = (byte)(cmd & 0xFF);
              data[2] = (byte)(data1 >> 8);
              data[3] = (byte)(data1 & 0xFF);
              data[4] = (byte)(data2 >> 8);
              data[5] = (byte)(data2 & 0xFF);
              data[6] = (byte)(data3 >> 8);
              data[7] = (byte)(data3 & 0xFF);
              data[8] = (byte)(data4 >> 8);
              data[9] = (byte)(data4 & 0xFF);
              sp.Write(data, 0, 10);
          }
      }

      public void ScanVariable(short var, short period)
      {
        Send(CMD_READ_VAR_REQUEST, var, (short)(period/10),0,0);
      }

      private void Sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
      {
          if (sp.BytesToRead >= 10)
          {
              sp.Read(rxData, 0, 10);
              DecodeFrame(rxData);
          }
      }

      private void DecodeFrame(byte[] data)
      {
          short cmd = (short) (rxData[0] * 256 + rxData[1]);
          short data1 = (short)(rxData[2] * 256 + rxData[3]);
          short data2 = (short)(rxData[4] * 256 + rxData[5]);
          short data3 = (short)(rxData[6] * 256 + rxData[7]);
          short data4 = (short)(rxData[8] * 256 + rxData[9]);

      switch (cmd)
      {
        case CMD_TM_VAR_VALUE:
          short varId = (short)(data1 & 0x7FFF);
          bool valid = true;
          if ((data1 & 0x8000) != 0)
          {
            valid = false;
          }
          vars.SetVariable(varId, data2, valid);
          break;
      }

      }


    }
}
