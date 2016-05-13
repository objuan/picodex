using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine
{
    public class Debug
    {
        public static void Log(String message)
        {
            System.Console.WriteLine(message);
            //System.Diagnostics.Debug.(message);
        }
         
        public static void LogError(String message)
        {
            System.Console.WriteLine(message);
            //System.Diagnostics.Debug.(message);
        }
         public static void LogWarning(String message)
        {
            System.Console.WriteLine(message);
            //System.Diagnostics.Debug.(message);
        }
         public static void LogException(Exception ex)
        {
            System.Console.WriteLine(ex.ToString());
            //System.Diagnostics.Debug.(message);
        }
   
        public static void Assert(bool cond, String message, String detail)
        {
            System.Diagnostics.Debug.Assert(cond,message,detail);
        }
        public static void Assert(bool cond, String message)
        {
            System.Diagnostics.Debug.Assert(cond, message);
        }
        public static void Assert(bool cond)
        {
            System.Diagnostics.Debug.Assert(cond);
        }
    }
}
