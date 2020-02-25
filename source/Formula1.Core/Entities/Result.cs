using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Formula1.Core.Entities
{
    public class Result
    {
        public int Id { get; set; }

        [Required, ForeignKey("RaceId")]
        public Race Race { get; set; }

        [Required, ForeignKey("TeamId")]
        public Team Team { get; set; }

        [Required, ForeignKey("DriverId")]
        public Driver Driver { get; set; }

        public int Position { get; set; }

        public int Points { get; set; }
        // FKs
        public int DriverId { get; set; }
        public int TeamId { get; set; }
        public int RaceId { get; set; }

        public override string ToString()
        {
            return $"{Race} {Driver} Platz: {Position} Punkte: {Points}";
        }


        /// <summary>
        ///     WM-Punkte aus Position ermitteln
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public static int GetPoints(int position)
        {
            switch (position)
            {
                case 1:
                    return 25;
                case 2:
                    return 18;
                case 3:
                    return 15;
                case 4:
                    return 12;
                case 5:
                    return 10;
                case 6:
                    return 8;
                case 7:
                    return 6;
                case 8:
                    return 4;
                case 9:
                    return 2;
                case 10:
                    return 1;
                default:
                    return 0;
            }
        }
    }
}
