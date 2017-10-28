using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace pw.lena.slave.winpc
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (CheckExistRunProgram())
            {
                MessageBox.Show("The requested App has already been started!");
            }
            else
            {
                if (args.Length == 0) //with arg - start ivisible mode
                    Application.Run(new MainForm(false));
                else
                    Application.Run(new MainForm(true));
            }
        }

        private static bool CheckExistRunProgram()
        {
            //true - run copy of program exist;
            bool first = true;
            try
            {
                Process[] ps = Process.GetProcesses();
                foreach (Process p in ps)
                {
                    if (p.ProcessName.ToLower().Equals(getAppName()))
                    {
                        if (first)
                            first = false;
                        else
                            return true;
                        //p.Kill(); /// kill process
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR " + ex.Message);
                return false;
            }
            return false;
        }

        private static string getAppName()
        {
            System.Diagnostics.Process p = System.Diagnostics.Process.GetCurrentProcess();
            int id = p.Id;
            return p.ProcessName;
        }
    }
}
