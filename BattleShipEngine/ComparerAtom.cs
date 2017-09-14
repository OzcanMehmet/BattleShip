using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShipEngine
{
    class ComparerAtom : IEqualityComparer<ShipAtom>
    {
        public  bool    Equals(ShipAtom First, ShipAtom Second)      
        {
            if (First.Location.X == Second.Location.X && First.Location.Y == Second.Location.Y)
                return true;
            else
                return false;
        }
        public  bool    Equals(MapItemAtom First, MapItemAtom Second)
        {
            if (First.Location.X == Second.Location.X && First.Location.Y == Second.Location.Y)
                return true;
            else 
                return false;
        }                
        public  int     GetHashCode(ShipAtom obj)                    
        {
            return 1;
        }
    }                   
}
