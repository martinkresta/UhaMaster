/*  Static Class: Err                                                            */
/*  Project: Universal                                                           */
/*  Author: Martin Kresta                                                        */

using System;
using System.Windows.Forms;

namespace UhaMaster
{
    /// <summary>
    /// Static class for handling application exceptions
    /// </summary>
    static class Err
    {

        /// <summary>
        /// Universal error handler, displaying messagebox with stack trace
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        /// <param name="description"></param>
        /// <param name="id"></param>
        public static void Handle(object source, Exception e, string description = "", int id = -1)
        {
            string text = "Popis chyby: " + Environment.NewLine;
            if (description != "")
            {
                text += description + Environment.NewLine;
            }
            text += e.Message + Environment.NewLine + Environment.NewLine;
            text += "Stack trace: " + Environment.NewLine;
            text += e.StackTrace + Environment.NewLine; ;

            MessageBox.Show("Popis chyby:" + Environment.NewLine + e.Message,
                            "Chyba!",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
        }
    }
}
