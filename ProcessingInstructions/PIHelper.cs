using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Qdabra.Utility
{
    /// <summary>
    /// Utilities for working with XML processing instructions
    /// </summary>
    public static class PIHelper
    {
        /// <summary>
        /// Extracts the pseudo-attributes in the specified value
        /// </summary>
        /// <param name="processingInstructionValue">The value of a processing instruction formatted to contain pseudo-attributes</param>
        /// <returns>A dictionary from the attributes' names to their respective values</returns>
        public static IDictionary<string, string> GetPseudoAttributes(string processingInstructionValue)
        {
            var docXml = "<n " + processingInstructionValue + "/>";
            try
            {
                var doc = XElement.Parse(docXml);

                return doc.Attributes().ToDictionary(a => a.Name.LocalName, a => a.Value);
            }
            catch
            {
                throw new FormatException("Not a valid pseudo-attribute format: " + processingInstructionValue);
            }
        }

        /// <summary>
        /// Extracts the pseudo-attributes in the specified processing instruction
        /// </summary>
        /// <param name="processingInstruction">A processing instruction formatted to contain pseudo-attributes</param>
        /// <returns>A dictionary from the attributes' names to their respective values</returns>
        public static IDictionary<string, string> GetPseudoAttributes(XProcessingInstruction processingInstruction)
        {
            if (processingInstruction == null) { throw new ArgumentNullException("processingInstruction"); }

            return GetPseudoAttributes(processingInstruction.Data);
        }

        private static XAttribute TryMakeAttribute(string name, string value)
        {
            try
            {
                return new XAttribute(name, value ?? "");
            }
            catch
            {
                throw new FormatException("Invalid pseudo-atttribute name: " + name);
            }
        }

        /// <summary>
        /// Builds a processing instruction value with pseudo-attributes for the specified values
        /// </summary>
        /// <param name="values">A dictionary from attribute names to their values. Names must be valid XML attribute names and not contain a namespace prefix.</param>
        /// <returns>The formatted processing instruction value</returns>
        public static string BuildProcessingInstructionValue(IDictionary<string, string> values)
        {
            if (values == null) { throw new ArgumentNullException("values"); }

            var el = new XElement("n", values.Select(kv => TryMakeAttribute(kv.Key, kv.Value)));
            var elXml = el.ToString();

            return Regex.Replace(elXml, @"^<n\s*|\s*(></n|/)>$", "");
        }

        /// <summary>
        /// Locates the first processing instruction with the specified name in the specified parent node.
        /// </summary>
        /// <param name="parent">A container node (document node, element, etc.)</param>
        /// <param name="name">The name of the processing instruction to locate.</param>
        /// <returns>The located processing instruction, or null if none is found</returns>
        public static XProcessingInstruction GetProcessingInstruction(XContainer parent, string name)
        {
            if (parent == null) { throw new ArgumentNullException("parent"); }
            if (string.IsNullOrWhiteSpace(name)) { throw new ArgumentOutOfRangeException("name"); }

            return parent.Nodes()
                .OfType<XProcessingInstruction>()
                .FirstOrDefault(pi => string.Equals(pi.Target, name, StringComparison.InvariantCulture));
        }
    }
}
