using FreeBuild.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Conversion
{
    /// <summary>
    /// A record that describes a mapping from one property to another, possibly via a converter function
    /// </summary>
    [Serializable]
    public class PropertyMapping : NotifyPropertyChangedBase
    {
        #region Properties

        /// <summary>
        /// The path of the property on type A
        /// </summary>
        public string PathA { get; set; }

        /// <summary>
        /// The path of the property of type B
        /// </summary>
        public string PathB { get; set; }

        /// <summary>
        /// The direction of this mapping.
        /// Default is 'both'.
        /// </summary>
        public ConversionDirections Direction { get; set; } = ConversionDirections.Both;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public PropertyMapping()
        {

        }

        /// <summary>
        /// Path constructor
        /// </summary>
        /// <param name="pathA"></param>
        /// <param name="pathB"></param>
        public PropertyMapping(string pathA, string pathB)
        {
            PathA = pathA;
            PathB = pathB;
        }

        /// <summary>
        /// Path and direction constructor
        /// </summary>
        /// <param name="pathA"></param>
        /// <param name="pathB"></param>
        /// <param name="direction"></param>
        public PropertyMapping(string pathA, string pathB, ConversionDirections direction) : this(pathA, pathB)
        {
            Direction = direction;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Apply this property mapping to transfer data
        /// between object a and object b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public void ApplyAtoB(object a, object b)
        {
            //Tokenise property path
            string[] pathATokens = PathA.Split('.');
            string[] pathBTokens = PathB.Split('.');

            //Find resolved source and target objects
            object source = GetPathEnd(a, pathATokens);
            object target = GetPathEnd(b, pathBTokens);

            if (source != null && target != null)
            {
                string sourceProp = pathATokens.Last();
                string targetProp = pathBTokens.Last();
                CopyPropertyData(source, target, sourceProp, targetProp);
            }
        }

        /// <summary>
        /// Apply this property mapping to transfer data
        /// between object b and object a
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public void ApplyBtoA(object a, object b)
        {
            //Tokenise property path
            string[] pathATokens = PathA.Split('.');
            string[] pathBTokens = PathB.Split('.');

            //Find resolved source and target objects
            object source = GetPathEnd(b, pathBTokens);
            object target = GetPathEnd(a, pathATokens);

            if (source != null && target != null)
            {
                string sourceProp = pathBTokens.Last();
                string targetProp = pathATokens.Last();
                CopyPropertyData(source, target, sourceProp, targetProp);
            }
        }


        protected void CopyPropertyData(object source, object target, string sourceProp, string targetProp)
        {
            Type sourceType = source.GetType();
            Type targetType = target.GetType();
            PropertyInfo sInfo = sourceType.GetProperty(sourceProp);
            PropertyInfo tInfo = targetType.GetProperty(targetProp);
            object value = sInfo.GetValue(source);
            //TODO: Run through converter?
            tInfo.SetValue(target, value);
        }


        /// <summary>
        /// Get the penultimate value in a property path chain -
        /// i.e. the one that the property the path leads to must be gotten or set on
        /// </summary>
        /// <param name="startObject"></param>
        /// <param name="pathTokens"></param>
        /// <returns></returns>
        protected object GetPathEnd(object startObject, string[] pathTokens)
        {
            object result = startObject;
            int i = 0;
            while (i < pathTokens.Length - 2 && result != null)
            {
                Type type = result.GetType();
                PropertyInfo pInfo = type.GetProperty(pathTokens[i]);
                if (pInfo != null) result = pInfo.GetValue(result);
                else result = null; //Throw error?
            }
            return result;
        }

        /// <summary>
        /// Obtain an inverted copy of this property mapping
        /// </summary>
        /// <returns></returns>
        public PropertyMapping Invert()
        {
            return new PropertyMapping(PathB, PathB, Direction.Invert());
        }

        #endregion
    }
}
