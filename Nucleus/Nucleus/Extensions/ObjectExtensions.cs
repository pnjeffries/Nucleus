﻿using Nucleus.Conversion;
using Nucleus.IO;
using Nucleus.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Extensions
{
    /// <summary>
    /// Static extension methods on any object type
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Set the value of a property on this object at the specified path
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="path">The path, consisting of property names and sub-property names
        /// separated by '.' characters.  For example: 'PropertyName.SubPropertyName.SubSubPropertyName' etc.
        /// Parameterless methods may also be invoked by adding '()', i.e.:
        /// 'PropertyName.SubMethodName().SubSubPropertyName'.
        /// Methods and properties on the optional context object may also be invoked in the same way, via a
        /// '*' redirection.  For example: '*.MethodName()'.  When switching to the context
        /// object the SetSourceObject method on it will be called and the current object or property value
        /// passed in.  This allows for complex operations to be performed in order to return a value
        /// provided that functionality is implemented in a suitable context object provided.</param>
        /// <param name="value">The value to be assigned to the property</param>
        /// <param name="context">The (optional) string conversion context object.  If supplies this allows
        /// the '*' symbol to be used within property paths in order to access properties and
        /// functions supplied on the context object.</param>
        public static void SetByPath(this object obj, string path, object value, IStringConversionContext context = null)
        {
            object setOn = obj;
            int iLast = path.LastIndexOf('.');
            if (iLast > 0)
            {
                string rootPath = path.Substring(0, iLast);
                setOn = obj.GetFromPath(rootPath, context);
                path = path.Substring(iLast + 1);
            }
            if (setOn != null)
            {
                PropertyInfo pInfo = setOn.GetType().GetProperty(path);
                if (value != null && value is IConvertible && !pInfo.PropertyType.IsAssignableFrom(value.GetType()))
                {
                    value = Convert.ChangeType(value, pInfo.PropertyType);
                }
                pInfo.SetValue(setOn, value);
            }
        }

        /// <summary>
        /// Generate a binding chain for the specified path on this object.
        /// A binding chain is a list of all objects in the chain which implement the
        /// INotifyPropertyChanged interface accompanied by the name of the property
        /// on that object which, when changed, will require the bindings of objects 
        /// further down the chain to be refreshed.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IList<BindingChainLink> GenerateBindingChain(this object obj, string path)
        {
            var result = new List<BindingChainLink>();
            string[] subStrings = path.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            object nextObj = obj;
            for (int i = 0; i < subStrings.Length; i++)
            {
                if (nextObj is INotifyPropertyChanged)
                {
                    result.Add(new UI.BindingChainLink((INotifyPropertyChanged)nextObj, subStrings[i]));
                }
                nextObj = nextObj.GetFromPath(subStrings[i]);
            }
            return result;
        }

        /// <summary>
        /// Get the value of a property on this object at the specified path
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="path">The path, consisting of property names and sub-property names
        /// separated by '.' characters.  For example: 'PropertyName.SubPropertyName.SubSubPropertyName' etc.
        /// Parameterless methods may also be invoked by adding '()', i.e.:
        /// 'PropertyName.SubMethodName().SubSubPropertyName'.
        /// Methods and properties on the optional context object may also be invoked in the same way, via a
        /// '*' redirection.  For example: '*.MethodName()'.  When switching to the context
        /// object the SetSourceObject method on it will be called and the current object or property value
        /// passed in.  This allows for complex operations to be performed in order to return a value
        /// provided that functionality is implemented in a suitable context object provided.</param>
        /// <param name="context">The (optional) string conversion context object.  If supplies this allows
        /// the '*' symbol to be used within property paths in order to access properties and
        /// functions supplied on the context object.</param>
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
                else if (token.EndsWith("()")) // Parameterless Method
                {
                    MethodInfo info = type.GetMethod(token.TrimEnd(')', '('), new Type[] { });
                    if (info == null) return null;
                    else
                        obj = info.Invoke(obj, null);
                }
                else if (token.EndsWith(")")) // Method with parameters
                {
                    string param = null;
                    token = token.TrimEnd(')');
                    int paramStart = token.LastIndexOf('(');
                    if (paramStart >= 0)
                    {
                        param = token.Substring(paramStart + 1);
                        token = token.Substring(0, paramStart);
                    }
                    MethodInfo info = type.GetMethod(token, new Type[] { param.GetType() });
                    if (info == null)
                    {
                        info = type.GetMethod(token);
                    }
                    if (info == null) return null;
                    else
                        obj = info.Invoke(obj, new object[] { param });
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
                            if (keyStart == 0) token = "Item";
                        }
                    }
                    if (key != null)
                    {
                        info = type.GetProperty(token, new Type[] { key.GetType() });
                        if (info == null)
                        {
                            //Property accessor isn't indexed, but maybe the object itself is...
                            info = type.GetProperty(token);
                            if (info != null)
                            {
                                obj = info.GetValue(obj, null);
                                if (obj != null)
                                {
                                    info = obj.GetType().GetProperty("Item", new Type[] { key.GetType() });
                                }
                            }
                        }
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
        /// the '*' symbol to be used within property paths in order to access properties and
        /// functions supplied on the context object.</param>
        /// <returns></returns>
        public static string ToString(this object obj, string format, char openTag = '{', char closeTag = '}',
            string ifTag = TextFormat.IF, string thenTag = TextFormat.THEN, IStringConversionContext context = null)
        {
            StringBuilder resultBuilder = new StringBuilder();

            StringBuilder pathBuilder = new StringBuilder();

            context.SubComponentIndex = 0;

            if (context.HasSubComponentsToWrite(obj))
            {
                int oldIndex = context.SubComponentIndex;
                //Sub-items:
                while (context.HasSubComponentsToWrite(obj))
                {
                    CreateFormattedString(obj, format, openTag, closeTag, 
                        ifTag, thenTag, context, resultBuilder, pathBuilder);
                    resultBuilder.AppendLine();
                    context.SubComponentIndex++;
                }
                context.SubComponentIndex = oldIndex;
            }
            else
            {
                CreateFormattedString(obj, format, openTag, closeTag, 
                    ifTag, thenTag, context, resultBuilder, pathBuilder);
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
            string ifTag, string thenTag, IStringConversionContext context, StringBuilder resultBuilder, StringBuilder pathBuilder)
        {
            int tagLevel = 0;

            //IF stages:
            const int NO_IFS = 0; //Not current in a conditional
            const int IF_CONDITION = 1; //Parsing the condition
            const int IF_TRUE = 2; //If statement is true
            const int IF_FALSE = 3; //If statement is false

            int ifLevel = NO_IFS;
            
            for (int i = 0; i < format.Length; i++)
            {
                char c = format[i];

                if (ifTag != null && format.AppearsAt(i, ifTag))
                {
                    i += ifTag.Length - 1; //Jump forward
                    if (ifLevel != NO_IFS)
                    {
                        ifLevel = NO_IFS; //Close if
                    }
                    else
                    {
                        ifLevel = IF_CONDITION; //Open if
                        //tagLevel++; //Look to recieve a value
                    }
                }
                else if (ifLevel == NO_IFS || ifLevel == IF_TRUE)
                {
                    
                    if (c == openTag) tagLevel++; // Open tag
                                                  //TODO: End of line
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
                else if (ifLevel == IF_CONDITION)
                {
                    if (format.AppearsAt(i,thenTag))
                    {
                        string expression = pathBuilder.ToString().Trim();
                        pathBuilder.Clear();
                        //Invert via the '!' operator
                        bool invert = false;
                        if (expression.StartsWith("!"))
                        {
                            invert = true;
                            expression = expression.TrimStart('!');
                        }
                        object condition = obj.GetFromPath(expression, context);
                        
                        if ((condition == null || (condition is bool && !(bool)condition)) != invert)
                        {
                            // Condition is null or false - do not write
                            ifLevel = IF_FALSE;
                        }
                        else
                        {
                            // Condition is true or non-null
                            ifLevel = IF_TRUE;
                        }
                    }
                    else
                    {
                        pathBuilder.Append(c);
                    }
                }

                //End conditionals after a newline
                if (ifLevel != NO_IFS && c == '\n')
                {
                    ifLevel = NO_IFS;
                }
            }
        }
    }
}
