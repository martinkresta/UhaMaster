using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UhaMaster
{

    // singleton class holding values of all process variables / measured data
    class cVARS
    {

        public const byte VAR_NETWORK_STATUS = 1;

        public const byte VAR_TEMP_BOILER = 100;
        public const byte VAR_TEMP_BOILER_IN = 101;
        public const byte VAR_TEMP_BOILER_OUT = 102;
        public const byte VAR_TEMP_TANK_IN_H = 103;
        public const byte VAR_TEMP_TANK_OUT_H = 104;
        public const byte VAR_TEMP_TANK_1 = 105;
        public const byte VAR_TEMP_TANK_2 = 106;
        public const byte VAR_TEMP_TANK_3 = 107;
        public const byte VAR_TEMP_TANK_4 = 108;
        public const byte VAR_TEMP_TANK_5 = 109;
        public const byte VAR_TEMP_TANK_6 = 110;
        public const byte VAR_TEMP_WALL_IN = 111;
        public const byte VAR_TEMP_WALL_OUT = 112;
        public const byte VAR_TEMP_BOILER_EXHAUST = 113;
        public const byte VAR_TEMP_RAD_H = 114;
        public const byte VAR_TEMP_RAD_C = 115;
        public const byte VAR_TEMP_TANK_IN_C = 116;
        public const byte VAR_TEMP_TANK_OUT_C = 117;


        public const byte VAR_TEMP_TECHM_BOARD = 120;
        public const byte VAR_TEMP_IOBOARD_D = 121;
        public const byte VAR_TEMP_IOBOARD_U = 122;
        public const byte VAR_TEMP_ELECON_BOARD = 123;
        public const byte VAR_TEMP_DOWNSTAIRS = 124;

        public const byte VAR_EL_HEATER_STATUS = 80;
        public const byte VAR_EL_HEATER_POWER = 81;
    public const byte VAR_EL_HEATER_CURRENT = 82;


    public const byte VAR_BAT_SOC = 10;
        public const byte VAR_BAT_VOLTAGE_V10 = 11;
        public const byte VAR_LOAD_A10 = 12;
        public const byte VAR_CHARGING_A10 = 13;

    public const byte VAR_FLOW_COLD = 90;
    public const byte VAR_FLOW_HOT = 91;
    public const byte VAR_CONS_COLD = 92;
    public const byte VAR_CONS_HOT = 93;



    public const short NUM_OF_VARIABLES = 255;

    // selfreference

    private static cVARS me;

        private cVariable[] mVariables;

        /// <summary>
        /// Private construtor
        /// </summary>
        private cVARS()
        {
          mVariables = new cVariable[NUM_OF_VARIABLES];
          for (int i = 0; i < NUM_OF_VARIABLES; i++)
          {
            mVariables[i] = new cVariable();
          }
        }

        static public cVARS Instance
        {
            get
            {
                if (me == null)
                {
                    me = new cVARS();
                    return me;
                }
                else
                {
                    return me;
                }
            }
            
        }

      public cVariable GetVariable(int varId)
      {
        if (varId < NUM_OF_VARIABLES)
        {
          return mVariables[varId];
        }
      return null;
      }

      public void SetVariable(int varId, short value, bool valid)
      {
        if (varId < NUM_OF_VARIABLES)
        {
          mVariables[varId].SetVariable(value, valid);
        }
      }
  }
}
