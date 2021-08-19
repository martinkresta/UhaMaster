using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UhaMaster
{
    public enum eVarState
    {
        evs_Unknown,
        evs_Valid,
        evs_Invalid
    }

  class cVariable
  {
    public DateTime UpdateTime { get; }
    public eVarState State { get; set; }
    public short Value { get; set; }

    public cVariable()
    {
      this.State = eVarState.evs_Unknown;
      this.Value = 0;
    }

    public void SetVariable(short value, bool valid)
    {
      this.Value = value;
      if (valid)
      {
        this.State = eVarState.evs_Valid;
      }
      else
      {
        this.State = eVarState.evs_Invalid;
      }
    }





    }
}
