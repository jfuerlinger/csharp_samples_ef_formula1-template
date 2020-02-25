using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Formula1.Core.Test
{
    [TestClass()]
    public class ImportControllerTests
    {
        /// <summary>
        /// Als erste Übung die Rennen aus der XML-Datei parsen
        /// </summary>
        [TestMethod()]
        public void T01_LoadRacesFromRacesXmlTest()
        {
            var races = ImportController.LoadRacesFromRacesXml().ToList();
            Assert.AreEqual(21, races.Count());
            Assert.AreEqual("Melbourne", races.First().City);
            Assert.AreEqual(1, races.First().Number);
            Assert.AreEqual("Abu Dhabi", races.Last().City);
            Assert.AreEqual(21, races.Last().Number);
        }

        /// <summary>
        /// Alle Results in Collections laden.
        /// </summary>
        [TestMethod()]
        public void T02_LoadResultsFromResultsXmlTest()
        {
            var results = ImportController.LoadResultsFromXmlIntoCollections().ToList();
            Assert.AreEqual(11, results.GroupBy(res => res.Team).Count());
            Assert.AreEqual(24, results.GroupBy(res => res.Driver).Count());
            Assert.AreEqual(462, results.Count);
        }


    }

}