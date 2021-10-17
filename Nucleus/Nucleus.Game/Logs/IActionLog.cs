using Nucleus.Base;
using Nucleus.Game;
using Nucleus.Logs;
using Nucleus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Logs
{
    /// <summary>
    /// An interface for log objects which report on actions
    /// </summary>
    public interface IActionLog : IScriptedLog
    {

    }

    /// <summary>
    /// Extension methods for the IActionLog interface
    /// </summary>
    public static class IActionLogExtensions
    {
        /// <summary>
        /// Write a pre-scripted entry to the log based on a key and a set of subject objects,
        /// if they are currently known to the player
        /// </summary>
        /// <param name="key"></param>
        /// <param name="subjects"></param>
        /// <remarks>To be written, at least one element in the subjects must be known to the player.
        /// Any unknown subjects will be replaced with a dummy 'something' element.</remarks>
        public static void WriteScripted(this IActionLog log, EffectContext context, string key, params object[] subjects)
        {
            bool known = false;

            for (int i = 0; i < subjects.Length; i++)
            {
                if (subjects[i] is Element el)
                {
                    if (context.IsPlayerAwareOf(el)) known = true;
                    else
                    {
                        // Disabled temporarily - was not working well for items and calling things 'a something'...
                        //subjects[i] = new GameElement("something");
                    }
                }
            }
            if (known) log.WriteScripted(key, subjects);
        }
    }
}
