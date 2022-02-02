using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace Backend
{
    public class LogHelper
    {
        /* This function will help to log various information with absolute path of filename.
         * So it will be easy to track logs.
         */
        public static ILog GetLogger([CallerFilePath] string filename = "")
        {
            return LogManager.GetLogger(filename);
        }
    }
}