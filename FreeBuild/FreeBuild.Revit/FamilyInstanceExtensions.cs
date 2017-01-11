using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Revit
{
    /// <summary>
    /// Extension methods for Revit FamilyInstance objects
    /// </summary>
    public static class FamilyInstanceExtensions
    {
        /// <summary>
        /// Set the value of a parameter on this instance
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="parameter"></param>
        /// <param name="value"></param>
        public static bool SetParameter(this FamilyInstance obj, BuiltInParameter parameter, int value)
        {
            Parameter param = obj.get_Parameter(parameter);
            if (param != null) return param.Set(value);
            else return false;
        }

        /// <summary>
        /// Set the value of a parameter on this instance
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="parameter"></param>
        /// <param name="value"></param>
        public static bool SetParameter(this FamilyInstance obj, BuiltInParameter parameter, double value)
        {
            Parameter param = obj.get_Parameter(parameter);
            if (param != null) return param.Set(value);
            else return false;
        }

        /// <summary>
        /// Set the value of a parameter on this instance
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="parameter"></param>
        /// <param name="value"></param>
        public static bool SetParameter(this FamilyInstance obj, BuiltInParameter parameter, string value)
        {
            Parameter param = obj.get_Parameter(parameter);
            if (param != null) return param.Set(value);
            else return false;
        }

        /// <summary>
        /// Set the value of a parameter on this instance
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="parameter"></param>
        /// <param name="value"></param>
        public static bool SetParameter(this FamilyInstance obj, BuiltInParameter parameter, ElementId value)
        {
            Parameter param = obj.get_Parameter(parameter);
            if (param != null) return param.Set(value);
            else return false;
        }

        /// <summary>
        /// Set the value of a parameter on this instance by name
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetParameter(this FamilyInstance obj, string parameterName, int value)
        {
            Parameter param = obj.LookupParameter(parameterName);
            if (param != null) return param.Set(value);
            else return false;
        }

        /// <summary>
        /// Set the value of a parameter on this instance by name
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetParameter(this FamilyInstance obj, string parameterName, double value)
        {
            Parameter param = obj.LookupParameter(parameterName);
            if (param != null) return param.Set(value);
            else return false;
        }

        /// <summary>
        /// Set the value of a parameter on this instance by name
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetParameter(this FamilyInstance obj, string parameterName, string value)
        {
            Parameter param = obj.LookupParameter(parameterName);
            if (param != null) return param.Set(value);
            else return false;
        }

        /// <summary>
        /// Set the value of a parameter on this instance by name
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetParameter(this FamilyInstance obj, string parameterName, ElementId value)
        {
            Parameter param = obj.LookupParameter(parameterName);
            if (param != null) return param.Set(value);
            else return false;
        }
    }
}
