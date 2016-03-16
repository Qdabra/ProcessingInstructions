using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;

namespace Qdabra.Utility.Tests
{
    [TestClass]
    public class ProcessingInstructionHelper
    {
        private XDocument SampleDoc =
            XDocument.Parse("<?book author=\"J.K. Rowling\" title=\"Harry Potter &amp; the Order of the Phoenix\" genre=\"fiction\"?><book />");

        [TestMethod]
        public void GetAttributesFromString()
        {
            var attrs =
                PIHelper.GetPseudoAttributes(
                    "book=\"Harry Potter &amp; the Philosopher&apos;s Stone\" characters='Harry &amp; Ron &amp; Hermione'");

            Assert.AreEqual(attrs.Count, 2);
            Assert.AreEqual(attrs["book"], "Harry Potter & the Philosopher's Stone");
            Assert.AreEqual(attrs["characters"], "Harry & Ron & Hermione");
        }

        [TestMethod]
        public void BuildProcessingInstruction()
        {
            const string houses = "Gryffindor, Hufflepuff, Slytherin, Ravenclaw";
            const string pets = "Hedwig, Harry's owl, Scabbers, Ron's rat, Crookshanks, Hermione's cat";

            var pi = PIHelper.BuildProcessingInstructionValue(new Dictionary<string, string>
            {
                {"houses", houses},
                {"pets", pets}
            });

            Assert.IsFalse(Regex.IsMatch(pi, @"^\s.*|\.*\s$"), "Value has leading or trailing whitespace");

            var attrs = PIHelper.GetPseudoAttributes(pi);

            Assert.AreEqual(attrs.Count, 2);
            Assert.AreEqual(attrs["houses"], houses);
            Assert.AreEqual(attrs["pets"], pets);
        }

        [TestMethod]
        public void GetProcessingInstruction()
        {
            var pi = PIHelper.GetProcessingInstruction(SampleDoc, "book");
            var attrs = PIHelper.GetPseudoAttributes(pi);

            Assert.AreEqual(attrs.Count, 3);
            Assert.AreEqual(attrs["title"], "Harry Potter & the Order of the Phoenix");

            var pi2 = PIHelper.GetProcessingInstruction(SampleDoc, "title");
            
            Assert.IsNull(pi2);
        }
    }
}
