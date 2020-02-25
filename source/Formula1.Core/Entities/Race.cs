using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Formula1.Core.Entities
{
    public class Race
    {
        public int Id { get; set; }
        public int? Number { get; set; }
        [Required, MaxLength(200)]
        public string Country { get; set; }
        [Required, MaxLength(200)]
        public string City { get; set; }
        [Required, Column(TypeName = "date")]
        public DateTime Date { get; set; }

        public IEnumerable<Result> Results { get; set; }

        public Race()
        {
            Results = new List<Result>();
        }

        public override string ToString()
        {
            return $"{Number} {Date.ToShortDateString()} {Country}";
        }
    }
}
