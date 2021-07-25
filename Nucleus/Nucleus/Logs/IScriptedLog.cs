using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Logs
{
    public interface IScriptedLog : ILog
    {
        /// <summary>
        /// Does this log have a pre-defined script for the specified key?
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool HasScriptFor(string key);

        /// <summary>
        /// Write a pre-scripted entry to the log based on a key and a set of subject objects
        /// </summary>
        /// <param name="key"></param>
        /// <param name="subjects"></param>
        void WriteScripted(string key, params object[] subjects);
    }
}
