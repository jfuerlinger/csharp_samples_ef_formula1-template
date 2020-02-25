using Formula1.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Utils;

namespace Formula1.Core
{
    /// <summary>
    /// Daten sind in XML-Dateien gespeichert und werden per Linq2XML
    /// in die Collections geladen.
    /// </summary>
    public static class ImportController
    {
        /// <summary>
        /// Daten der Rennen werden per Linq2XML aus der
        /// XML-Datei ausgelesen und in die Races-Collection gespeichert.
        /// Races werden nicht aus den Results geladen, weil sonst die
        /// Rennen in der Zukunft fehlen
        /// </summary>
        public static IEnumerable<Race> LoadRacesFromRacesXml()
        {
            List<Race> races = new List<Race>();
            string racesPath = MyFile.GetFullNameInApplicationTree("Races.xml");
            var xElement = XDocument.Load(racesPath).Root;
            if (xElement != null)
            {
                races = (from race in xElement.Elements("Race")
                         select new Race
                         {
                             Number = (int)race.Attribute("round"),
                             Date = (DateTime)race.Element("Date"),
                             Country = race.Element("Circuit")?.Element("Location")
                                     ?.Element("Country")?.Value,
                             City = race.Element("Circuit")?.Element("Location")
                                     ?.Element("Locality")?.Value,
                         }).ToList();
            }
            return races;
        }

        /// <summary>
        /// Aus den Results werden alle Collections, außer Races gefüllt.
        /// Races wird extra behandelt, um auch Rennen ohne Results zu verwalten
        /// </summary>
        public static IEnumerable<Result> LoadResultsFromXmlIntoCollections()
        {
            List<Race> races = LoadRacesFromRacesXml().ToList();
            List<Driver> drivers = new List<Driver>();
            List<Team> teams = new List<Team>();
            List<Result> results = null;
            string racesPath = MyFile.GetFullNameInApplicationTree("Results.xml");
            var xElement = XDocument.Load(racesPath).Root;
            if (xElement != null)
            {
                results = xElement.Elements("Race").Elements("ResultsList").Elements("Result")
                    .Select(result => new Result
                    {
                        Race = GetRace(result, races),
                        Driver = GetDriver(result, drivers),
                        Team = GetTeam(result, teams),
                        Position = (int)result.Attribute("position"),
                        Points = (int)result.Attribute("points"),
                    }).ToList();
            }
            return results;
        }


        /// <summary>
        /// Team wird aus XML vom einzelnen Resultgeparst. 
        /// Wenn es noch nicht in der Teamliste ist, wird es eingefügt und zurückgegeben.
        /// Sonst wird das Element aus der Teamliste zurückgegeben
        /// </summary>
        /// <param name="result"></param>
        /// <param name="teams"></param>
        /// <returns></returns>
        private static Team GetTeam(XElement result, List<Team> teams)
        {
            Team newTeam = new Team
            {
                Name = (string)result.Element("Constructor")?.Element("Name"),
                Nationality = (string)result.Element("Constructor")?.Element("Nationality")
            };
            Team teamInList = teams.FirstOrDefault(team => team.Name == newTeam.Name);
            if (teamInList != null)  // Team kommt bereits in results vor
            {
                return teamInList;
            }
            teams.Add(newTeam);
            return newTeam;
        }


        /// <summary>
        /// Gibt das Race für das Result zurück, indem das Race
        /// aus den Races auf Basis der Rennnummer gesucht wird.
        /// </summary>
        /// <param name="xElement"></param>
        /// <param name="races"></param>
        /// <returns></returns>
        private static Race GetRace(XElement xElement, List<Race> races)
        {
            int raceNumber = (int)xElement.Parent?.Parent?.Attribute("round");
            return races.Single(race => race.Number == raceNumber);
        }

        /// <summary>
        /// Fahrer wird aus XML für ein Result geparst.
        /// War der Fahrer schon in der Liste, wir dieser zurückgegeben.
        /// Sonst wird der Fahrer in die Liste gestellt und zurückgegeben.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="drivers"></param>
        /// <returns></returns>
        private static Driver GetDriver(XElement result, List<Driver> drivers)
        {
            Driver newDriver = new Driver
            {
                FirstName = (string)result.Element("Driver")?.Element("GivenName"),
                LastName = (string)result.Element("Driver")?.Element("FamilyName"),
                Nationality = (string)result.Element("Driver")?.Element("Nationality"),
            };
            Driver driverInList = drivers.SingleOrDefault(driver =>
                                        driver.LastName == newDriver.LastName &&
                                        driver.FirstName == newDriver.FirstName);
            if (driverInList != null)  // Fahrer war schon in der Liste
            {
                return driverInList;
            }
            drivers.Add(newDriver);
            return newDriver;
        }
    }
}