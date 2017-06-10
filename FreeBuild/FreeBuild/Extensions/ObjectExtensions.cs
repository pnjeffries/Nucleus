using FreeBuild.Conversion;
using FreeBuild.IO;
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
        /// 'PropertyName.SubMethodName().SubSubPropertyName'.
        /// Methods and properties on the optional context object may also be invoked in the same way, via a
        /// '[CONTEXT]' redirection.  For example: '[CONTEXT].MethodName()'.  When switching to the context
        /// object the SetSourceObject method on it will be called and the current object or property value
        /// passed in.  This allows for complex operations to be performed in order to return a value
        /// provided that functionality is implemented in a suitable context object provided.</param>
        /// <returns></returns>
        public static object GetFromPath(this object obj, string path, IStringConversionContext context = null)
        {
            foreach (string substring in path.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string token = substring;
                if (obj == null) { return null; }

                Type type = obj.GetType();

                if (token.EqualsIgnoreCase(TextFormat.CONTEXT))
                {
                    context.SetSourceObject(obj);
                    obj = context;
                }
                else if (token.EndsWith("()")) // Method
                {
                    MethodInfo info = type.GetMethod(token.TrimEnd(')', '('), new Type[] { });
                    if (info == null) return null;
                    else
                        obj = info.Invoke(obj, null);
                }
                else // Property...
                {
                    PropertyInfo info;
                    string key = null;
                    if (token.EndsWith("]"))
                    {
                        token = token.TrimEnd(']');
                        int keyStart = token.LastIndexOf('[');
                        if (keyStart >= 0)
                        {
                            key = token.Substring(keyStart + 1);
                            token = token.Substring(0, keyStart);
                        }
                    }
                    if (key != null)
                    {
                        info = type.GetProperty(token, new Type[] { key.GetType() });
                    }
                    else info = type.GetProperty(token);
                    if (info == null)
                    {
                        //...or Field?
                        FieldInfo fInfo = type.GetField(token);
                        if (fInfo == null) return null;
                        else
                            obj = fInfo.GetValue(obj);
                    }
                    else
                    {
                        if (key != null)
                            obj = info.GetValue(obj, new object[] { key });
                        else obj = info.GetValue(obj, null);
                    }
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
        /// <param name="context">The (optional) string conversion context object.  If supplies this allows
        /// the '[CONTEXT]' keyword to be used within property paths in order to access properties and
        /// functions supplied on the context object.</param>
        /// <returns></returns>
        public static string ToString(this object obj, string format, char openTag = '{', char closeTag = '}',
            IStringConversionContext context = null)
        {
            StringBuilder resultBuilder = new StringBuilder();

            StringBuilder pathBuilder = new StringBuilder();

            context.SubComponentIndex = 0;

            if (context.HasSubComponentsToWrite(obj))
            {
                //Sub-items:
                while (context.HasSubComponentsToWrite(obj))
                {
                    CreateFormattedString(obj, format, openTag, closeTag, context, resultBuilder, pathBuilder);
                    resultBuilder.AppendLine();
                    context.SubComponentIndex++;
                }
            }
            else
            {
                CreateFormattedString(obj, format, openTag, closeTag, context, resultBuilder, pathBuilder);
            }

            return resultBuilder.ToString();
        }

        /// <summary>
        /// Generate a formatted string which represents the object or a subcomponent of it
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="format"></param>
        /// <param name="openTag"></param>
        /// <param name="closeTag"></param>
        /// <param name="context"></param>
        /// <param name="resultBuilder"></param>
        /// <param name="pathBuilder"></param>
        private static void CreateFormattedString(this object obj, string format, char openTag, char closeTag,
            IStringConversionContext context, StringBuilder resultBuilder, StringBuilder pathBuilder)
        {
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
                        resultBuilder.Append(obj.GetFromPath(pathBuilder.ToString(), context));
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

        }
    }
}
