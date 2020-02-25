using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    namespace Utils
    {
        public static class CsvMapper
        {
            private const char DefaultSeperator = ';';


            /// <summary>
            ///     Diese Erweiterungsmethode erstellt eine Instanz des generischen Types T und initialisiert deren
            ///     Eigenschaftswerte mit den csv-String. Zur Abfrage welche Eigenschaften initialisiert werden, dient
            ///     die csv-Kopfzeile.
            /// </summary>
            /// <typeparam name="T">Der generische Typ - muss eine Klasse sein und den Default- oder Standardkonstruktor bereitstellen.</typeparam>
            /// <param name="line">Der Erweiterungstyp String</param>
            /// <param name="csvPropertyNames"></param>
            /// <param name="seperator">Das csv-Trennzeichen.</param>
            /// <returns>Das erzeugte und belegte Target-Objekt.</returns>
            public static T CsvLineTo<T>(string line, string[] csvPropertyNames,
                char seperator = DefaultSeperator) where T : class, new()
            {
                if (line == null) throw new ArgumentNullException("The parameter 'line' can not be null!");
                if (csvPropertyNames == null)
                    throw new ArgumentNullException("The parameter 'csvPropertyNames' can not be null!");
                Type type = typeof(T);
                var dataObject = new T();
                string[] values = line.Split(seperator);

                for (int i = 0; i < csvPropertyNames.Length; i++)
                {
                    string value = values[i];
                    PropertyInfo propertyInfo = type.GetProperty(csvPropertyNames[i]);
                    if (propertyInfo != null && propertyInfo.CanWrite && i < values.Length)
                    {
                        if (propertyInfo.PropertyType.IsEnum)
                        {
                            if (value != null)
                                propertyInfo.SetValue(dataObject, Enum.Parse(propertyInfo.PropertyType, value));
                        }
                        else if (string.IsNullOrEmpty(value))
                        {
                            propertyInfo.SetValue(dataObject, null);
                        }
                        else if (propertyInfo.PropertyType.IsGenericType)
                        {
                            propertyInfo.SetValue(dataObject,
                                Convert.ChangeType(value, propertyInfo.PropertyType.GetGenericArguments()[0]));
                        }
                        else
                        {
                            object propertyValue = Convert.ChangeType(value, propertyInfo.PropertyType);
                            propertyInfo.SetValue(dataObject, propertyValue);
                        }
                    }
                }
                return dataObject;
            }

            /// <summary>
            ///     Liest die Daten aus einer CSV-Datei in einen IEnumerable des gewünschten Typs ein.
            ///     Grundsätzlich wird der Propertyname des einzulesenden Typs auf den Spaltennamen gemappt. Die
            ///     Typumwandlung erfolgt auf Basis des Propertytyps.
            ///     Für spezielle Mappings kann die Methode specialMapping verwendet werden. Dieser wird das zu befüllende
            ///     Objekt, die Headerzeile und die aktuelle Datenzeile mitgegeben. Die Methode kann dann ein oder mehrere
            ///     Properties sehr flexibel belegen
            /// </summary>
            /// <typeparam name="T">Typ des Zielobjekts</typeparam>
            /// <param name="fileName">Name der csv-Datei ohne Pfad. Es wird im Anwendungspfad gesucht</param>
            /// <param name="seperator">Trennzeichen in der csv-Datei (default: ;</param>
            /// <param name="specialMapping">Mappingmethode für spezielle Anforderungen</param>
            /// <returns></returns>
            public static IEnumerable<T> CsvFileToEnumerable<T>(string fileName, char seperator = DefaultSeperator,
                Action<T, string[], string[]> specialMapping = null) where T : class, new()
            {
                if (string.IsNullOrEmpty(fileName))
                    throw new ArgumentNullException("The parameter 'fileName' can not be null or empty!");
                string fullFileName = MyFile.GetFullNameInApplicationTree(fileName);
                if (string.IsNullOrEmpty(fullFileName))
                    throw new FileNotFoundException("The file could not be found i  application tree");
                string[] lines = File.ReadAllLines(fullFileName, Encoding.Default);
                if (lines == null || lines.Length == 0) throw new ApplicationException("csv-file is empty");
                string[] csvPropertyNames = lines[0].Split(seperator);
                if (csvPropertyNames == null || csvPropertyNames.Length == 0)
                    throw new ApplicationException("there are no propertynames in the headerline");
                var dataObjects = new T[lines.Length - 1];
                // Alle Properties des Zieltyps mit Daten aus gleichnamiger Spalte der CSV-Datei übernehmen
                for (int i = 1; i < lines.Length; i++)
                {
                    dataObjects[i - 1] = CsvLineTo<T>(lines[i], csvPropertyNames, seperator);
                }
                // spezielle Zuordnungen einer Zeile zu einem oder mehreren Properties werden im Anschluss extra behandelt
                if (specialMapping != null)
                {
                    for (int i = 1; i < lines.Length; i++)
                    {
                        specialMapping(dataObjects[i - 1], csvPropertyNames, lines[i].Split(seperator));
                    }
                }
                return dataObjects;
            }

            /// <summary>
            ///     Diese Methode ist eine allgemeine Methode zum Kopieren von
            ///     oeffentlichen Objekteigenschaften.
            /// </summary>
            /// <param name="target">Zielobjekt</param>
            /// <param name="source">Quelleobjekt</param>
            public static void CopyProperties(object target, object source)
            {
                if (target == null) throw new ArgumentNullException("target");
                if (source == null) throw new ArgumentNullException("source");
                Type sourceType = source.GetType();
                Type targetType = target.GetType();
                foreach (PropertyInfo piSource in sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(pi => pi.CanRead))
                {
                    if (!piSource.PropertyType.FullName.StartsWith("System.Collections.Generic.ICollection"))
                    // kein Navigationproperty
                    {
                        PropertyInfo piTarget = targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance).
                            SingleOrDefault(pi => pi.Name.Equals(piSource.Name));
                        if (piTarget == null) continue;
                        object value = piSource.GetValue(source, null);
                        piTarget.SetValue(target, value, null);
                    }
                }
            }
        }
    }
}
