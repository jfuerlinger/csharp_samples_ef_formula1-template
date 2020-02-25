namespace Formula1.Core.Contracts
{
    /// <summary>
    /// Ein Wettkämpfer muss die Fähigkeit besitzen seinen Namen auszugeben
    /// </summary>
    public interface ICompetitor
    {
        /// <summary>
        /// Liefert den Namen des Wettkämpfers
        /// Teamname oder Fahrername
        /// </summary>
        string Name { get; }
    }
}

