using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RPrint
{
    static class Program
    {
        public static event EventHandler<MyEventArgs> WriteLog;

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Main from = new Main();
            WriteLog += from.WriteToLabel;
            Application.Run(from);
        }

        /// <summary>
        /// 应用程序域异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (WriteLog != null)
            {
                WriteLog(sender, new MyEventArgs(e.ExceptionObject.ToString()));
            }
        }

        /// <summary>
        /// 线程异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            if (WriteLog != null)
            {
                WriteLog(sender, new MyEventArgs(e.Exception.Message));
            }
        }
    }
}
