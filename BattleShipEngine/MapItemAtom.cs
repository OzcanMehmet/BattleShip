using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BattleShipEngine
{
    class MapItemAtom:IMapItemAtom
    {
        public Point Location                 { get; set; }
        public MapItemAtom(Point location)    
        {
            Location = location;
        }

    }
}
