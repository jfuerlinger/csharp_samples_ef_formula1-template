using Formula1.Core.Contracts;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Formula1.Core.Entities
{
    public class Team : ICompetitor
    {
        public int Id { get; set; } // Primärschlüssel für DB

        [Required, MaxLength(50)]
        public string Name { get; set; }

        [Required, MaxLength(50)]
        public string Nationality { get; set; }
        public ICollection<Driver> Drivers { get; set; }

        public Team()
        {
            Drivers = new List<Driver>();
        }


        public override string ToString()
        {
            return $"{Name} Nationalität: {Nationality}";
        }

    }
}
