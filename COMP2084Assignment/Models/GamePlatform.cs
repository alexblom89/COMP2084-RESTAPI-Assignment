using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COMP2084Assignment.Models
{
    public class GamePlatform
    {
        public int GameId { get; set; }
        public Game Game { get; set; }
        public int PlatformId { get; set; }
        public Platform Platform { get; set; }
    }
}
