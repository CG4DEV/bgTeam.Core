﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace bgTeam
{
    /// <summary>
    /// Utility class to provide documentation for various types where available with the assembly
    /// </summary>
    public static class DocumenationExtensions
    {
        /// <summary>
        /// A cache used to remember Xml documentation for assemblies
        /// </summary>
        private static readonly Dictionary<Assembly, XmlDocument> Cache = new Dictionary<Assembly, XmlDocument>();

        /// <summary>
        /// A cache used to store failure exceptions for assembly lookups
        /// </summary>
        private static readonly Dictionary<Assembly, Exception> FailCache = new Dictionary<Assembly, Exception>();

        /// <summary>
        /// Gets the summary portion of a type's documenation or returns an empty string if not available
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetSummary(this Type type)
        {
            var element = type.GetDocumentation();
            return element.GetSummary();
        }

        /// <summary>
        /// Returns the Xml documenation summary comment for this method
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        public static string GetSummary(this MethodInfo methodInfo)
        {
            var element = methodInfo.GetDocumentation();
            return element.GetSummary();
        }

        /// <summary>
        /// Returns the Xml documenation summary comment for this member
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        public static string GetSummary(this MemberInfo memberInfo)
        {
            var element = memberInfo.GetDocumentation();
            return element.GetSummary();
        }

        /// <summary>
        /// Find summary node and return inner text from this XmlElement
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string GetSummary(this XmlElement element)
        {
            var summary = element?.SelectSingleNode("summary");
            if (summary == null)
            {
                return string.Empty;
            }

            return summary.InnerText.Trim();
        }

        /// <summary>
        /// Provides the documentation comments for a specific method
        /// </summary>
        /// <param name="methodInfo">The MethodInfo (reflection data ) of the member to find documentation for</param>
        /// <returns>The XML fragment describing the method</returns>
        public static XmlElement GetDocumentation(this MethodInfo methodInfo)
        {
            var parametersString = new StringBuilder();
            foreach (var parameterInfo in methodInfo.GetParameters())
            {
                if (parametersString.Length > 0)
                {
                    parametersString.Append(",");
                }

                var paramType = parameterInfo.ParameterType;
                if (Nullable.GetUnderlyingType(paramType) != null)
                {
                    var nullableType = string.Join(",", paramType.GetGenericArguments().Select(x => x.FullName));
                    parametersString.Append($"System.Nullable{{{nullableType}}}");
                }
                else
                {
                    parametersString.Append(paramType.FullName);
                }
            }

            if (parametersString.Length > 0)
            {
                return XmlFromName(methodInfo.DeclaringType, 'M', $"{methodInfo.Name}({parametersString})");
            }
            else
            {
                return XmlFromName(methodInfo.DeclaringType, 'M', methodInfo.Name);
            }
        }

        /// <summary>
        /// Provides the documentation comments for a specific member
        /// </summary>
        /// <param name="memberInfo">The MemberInfo (reflection data) or the member to find documentation for</param>
        /// <returns>The XML fragment describing the member</returns>
        public static XmlElement GetDocumentation(this MemberInfo memberInfo)
        {
            return XmlFromName(memberInfo.DeclaringType, memberInfo.MemberType.ToString()[0], memberInfo.Name);
        }

        /// <summary>
        /// Provides the documentation comments for a specific type
        /// </summary>
        /// <param name="type">Type to find the documentation for</param>
        /// <returns>The XML fragment that describes the type</returns>
        public static XmlElement GetDocumentation(this Type type)
        {
            // Prefix in type names is T
            return XmlFromName(type, 'T', string.Empty);
        }

        /// <summary>
        /// Obtains the documentation file for the specified assembly
        /// </summary>
        /// <param name="assembly">The assembly to find the XML document for</param>
        /// <returns>The XML document</returns>
        /// <remarks>This version uses a cache to preserve the assemblies, so that the XML file is not loaded and parsed on every single lookup</remarks>
        public static XmlDocument XmlFromAssembly(this Assembly assembly)
        {
            if (FailCache.ContainsKey(assembly))
            {
                throw FailCache[assembly];
            }

            try
            {
                if (!Cache.ContainsKey(assembly))
                {
                    Cache[assembly] = XmlFromAssemblyNonCached(assembly);
                }

                return Cache[assembly];
            }
            catch (Exception ex)
            {
                FailCache[assembly] = ex;
                throw;
            }
        }

        /// <summary>
        /// Obtains the XML Element that describes a reflection element by searching the members for a member that has a name that describes the element.
        /// </summary>
        /// <param name="type">The type or parent type, used to fetch the assembly</param>
        /// <param name="prefix">The prefix as seen in the name attribute in the documentation XML</param>
        /// <param name="name">Where relevant, the full name qualifier for the element</param>
        /// <returns>The member that has a name that describes the specified reflection element</returns>
        private static XmlElement XmlFromName(this Type type, char prefix, string name)
        {
            string fullName = $"{prefix}:{type.FullName}";
            if (!string.IsNullOrEmpty(name))
            {
                fullName = $"{fullName}.{name}";
            }

            var xmlDocument = XmlFromAssembly(type.Assembly);
            var matchedElement = xmlDocument["doc"]["members"].SelectSingleNode($"member[@name='{fullName}']") as XmlElement;
            return matchedElement;
        }

        /// <summary>
        /// Loads and parses the documentation file for the specified assembly
        /// </summary>
        /// <param name="assembly">The assembly to find the XML document for</param>
        /// <returns>The XML document</returns>
        private static XmlDocument XmlFromAssemblyNonCached(Assembly assembly)
        {
            var assemblyFilename = assembly.Location;
            using (var streamReader = new StreamReader(Path.ChangeExtension(assemblyFilename, ".xml")))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.Load(streamReader);
                return xmlDocument;
            }
        }
    }
}
