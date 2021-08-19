using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UhaMaster
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                cManager mgr = new cManager();
            }
            catch (Exception e)   // handler for the case of some unhadnled exception (normally, it should never occur!)
            {
                MessageBox.Show("CHYBA:" + e.Message, "Kritická chyba, aplikace bude ukončena",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
