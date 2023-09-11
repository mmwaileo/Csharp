using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Monitoring_System
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        [STAThread]
        
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            SplashScreen splash = new SplashScreen();
            Thread splashThread = new Thread(new ThreadStart(() =>
            {
                Application.Run(splash);
            }));
            splashThread.Start();

            // Simulate some loading work
            Thread.Sleep(1000);
            /*
            Form1 mainForm = new Form1();
            mainForm.IsGPS1Up();
            mainForm.IsGPS2Up();
            */          
            splash.Invoke(new Action(() => splash.Close()));
            splashThread.Join();

            Application.Run(new Form1());

        }
    }
}
