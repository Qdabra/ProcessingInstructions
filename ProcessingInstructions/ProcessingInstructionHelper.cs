using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Qdabra.Utility
{
    public static class ProcessingInstructionHelper
    {
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

        public static IDictionary<string, string> GetPseudoAttributes(XProcessingInstruction pi)
        {
            return GetPseudoAttributes(pi.Data);
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

        public static string BuildProcessingInstruction(IDictionary<string, string> values)
        {
            if (values == null) { throw new ArgumentNullException("values"); }

            var el = new XElement("n", values.Select(kv => TryMakeAttribute(kv.Key, kv.Value)));
            var elXml = el.ToString();

            return Regex.Replace(elXml, @"^<n|\s*(></n|/)>$", "");
        }

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
