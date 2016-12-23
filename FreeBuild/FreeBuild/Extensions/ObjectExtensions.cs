using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FreeBuild.Extensions
{
    /// <summary>
    /// Static extension methods on any object type
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Get the value of a property on this object at the specified path
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="path">The path, consisting of property names and sub-property names
        /// separated by '.' characters.  For example: 'PropertyName.SubPropertyName.SubSubPropertyName' etc.
        /// Parameterless methods may also be invoked by adding '()', i.e.:
        /// 'PropertyName.SubMethodName().SubSubPropertyName'</param>
        /// <returns></returns>
        public static object GetValue(this object obj, string path)
        {
            foreach (string token in path.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (obj == null) { return null; }

                Type type = obj.GetType();

                if (token.EndsWith("()")) // Method
                {
                    MethodInfo info = type.GetMethod(token.TrimEnd(')', '('));
                    if (info == null) return null;
                    else
                        obj = info.Invoke(obj, null);
                }
                else // Property...
                {
                    PropertyInfo info = type.GetProperty(token);
                    if (info == null)
                    {
                        //...or Field?
                        FieldInfo fInfo = type.GetField(token);
                        if (fInfo == null) return null;
                        else
                            obj = fInfo.GetValue(obj);
                    }
                    else
                        obj = info.GetValue(obj, null);
                }
            }
            return obj;
        }

        /// <summary>
        /// Convert this object to a string in the specified format.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="format">The format to be used to construct the text string.
        /// This should take the form of a string interspersed with property paths surrounded by
        /// open and close tag characters, the values of which will be inserted into the text.
        /// For example "The value of the subproperty is: {PropertyName.SubPropertyName}"</param>
        /// <param name="openTag">The opening tag for specifying property paths.  Default is '{'.</param>
        /// <param name="closeTag">The closing tag for specifying property paths.  Default is '}'.</param>
        /// <returns></returns>
        public static string ToString(this object obj, string format, char openTag = '{', char closeTag = '}')
        {
            StringBuilder resultBuilder = new StringBuilder();

            StringBuilder pathBuilder = new StringBuilder();

            int tagLevel = 0;

            for (int i = 0; i < format.Length; i++)
            {
                char c = format[i];
                if (c == openTag) tagLevel++; // Open tag
                else if (c == closeTag && tagLevel > 0) // Close tag
                {
                    tagLevel--;
                    if (tagLevel == 0)
                    {
                        resultBuilder.Append(obj.GetValue(pathBuilder.ToString()));
                        pathBuilder.Clear();
                    }
                }
                else if (tagLevel == 0) //Use text verbatim
                {
                    resultBuilder.Append(c);
                }
                else //Tags open
                {
                    pathBuilder.Append(c);
                }
            }

            return resultBuilder.ToString();
        }
    }
}
