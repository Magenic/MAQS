using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public static class XBullet
    {
        /// <summary>
        /// Wraps a try catch allowing you to specify what to return
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="MessagePrefix">The message prefix logged to Trace.Writeline with exception</param>
        /// <param name="Code">A call to the code section, to return type T</param>
        /// <param name="Error">The error to be handled and logged and optionally called back</param>
        /// <returns></returns>
        public static T Proof<T>(string MessagePrefix , Func<string,T> Code, Action<Exception> Error = null) where T:new()
        {
            T result = new T();
            try {

                result = Code("Code");
                return result;

            } catch(Exception iox)
            {
                Trace.Write(MessagePrefix);
                Trace.WriteLine(iox.Message);
                Trace.WriteLine(iox.StackTrace);
                if(Error!=null)  Error(iox);
                return result;
            }

        }
    }
}
