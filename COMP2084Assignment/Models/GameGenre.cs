﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COMP2084Assignment.Models
{
    public class GameGenre
    {
        public int GameId { get; set; }
        public Game Game { get; set; }
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}
