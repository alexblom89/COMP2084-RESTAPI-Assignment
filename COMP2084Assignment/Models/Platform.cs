using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace COMP2084Assignment.Models
{
    public class Platform
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Developer { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        public List<GamePlatform> GamePlatforms { get; set; }


    }
}
