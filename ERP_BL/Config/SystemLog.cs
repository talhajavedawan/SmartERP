using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
namespace ERP_BL.Databases
{
    public static class SystemLog
    {

        public static int CurrentUserId;
        public static string CurrentUseruserName;
        //Logger logger = LogManager.GetLogger("foo");
        public static void LogInfo(Type declaringType, string text)
        {
            LogManager.GetLogger(declaringType.FullName).Info(text + "User Id =" + CurrentUserId + " User Name =" + CurrentUseruserName);
        }

        public static void LogWarn(Type declaringType, string text)
        {
            LogManager.GetLogger(declaringType.FullName).Warn(text + "User Id =" + CurrentUserId + " User Name =" + CurrentUseruserName);
        }

        public static void LogDebug(Type declaringType, string text)
        {
            LogManager.GetLogger(declaringType.FullName).Debug(text + "User Id =" + CurrentUserId + " User Name =" + CurrentUseruserName);
        }


        public static void LogTrace(Type declaringType, string text)
        {
            LogManager.GetLogger(declaringType.FullName).Trace(text + "User Id =" + CurrentUserId + " User Name =" + CurrentUseruserName);
        }

        public static void LogError(Type declaringType, string text)
        {
            LogManager.GetLogger(declaringType.FullName).Error(text + "User Id =" + CurrentUserId + " User Name =" + CurrentUseruserName);
        }
        public static void LogFatal(Type declaringType, string text)
        {
            LogManager.GetLogger(declaringType.FullName).Fatal(text + "User Id =" + CurrentUserId + " User Name =" + CurrentUseruserName);
        }

    }
}



