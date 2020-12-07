using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace COMP2084Assignment.Models
{
    public class Game
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public int GenreId { get; set; }

        public string Description { get; set; }

        [Required]
        public string Developer { get; set; }

        [Required]
        public string Publisher { get; set; }

        public string PlatformId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        public string Photo { get; set; }

        public List<Genre> Genres { get; set; }
        public List<GameGenre> GameGenres { get; set; }
        public List<GamePlatform> GamePlatforms { get; set; }

    }
}
