using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Game.Debug
{
    /// <summary>
    /// A class of debug commands which can be invoked to aid with testing and debugging
    /// </summary>
    public class DebugCommandLibrary
    {
        /// <summary>
        /// Reset the game engine
        /// </summary>
        public void reset()
        {
            GameEngine.Instance.Reset();
        }
    }

    /// <summary>
    /// Extension methods for DebugCommandLibrary
    /// </summary>
    public static class DebugCommandLibraryExtensions
    {
        /// <summary>
        /// Attempt to execute the command string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="debug"></param>
        /// <param name="command"></param>
        public static void RunCommand<T>(this T debug, string command)
            where T : DebugCommandLibrary
        {
            var type = debug.GetType();
            var tokens = command.Split(' ');
            var method = type.GetMethod(tokens[0]);
            if (method == null) throw new MethodAccessException("Method '" + tokens[0] + "' not found.");
            var parameters = method.GetParameters();
            var args = new object[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                args[i] = Convert.ChangeType(tokens[i + 1], parameters[i].ParameterType);
            }
            method.Invoke(debug, args);
        }
    }

}
