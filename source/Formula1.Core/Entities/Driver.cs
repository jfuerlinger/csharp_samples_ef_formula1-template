using Formula1.Core.Contracts;
using System.ComponentModel.DataAnnotations;

namespace Formula1.Core.Entities
{
    public class Driver : ICompetitor
    {
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string FirstName { get; set; }

        [Required, MaxLength(50)]
        public string LastName { get; set; }
        public string Nationality { get; set; }


        public override string ToString()
        {
            return $"{LastName} {FirstName}";
        }

        public string Name => ToString();
    }
}
