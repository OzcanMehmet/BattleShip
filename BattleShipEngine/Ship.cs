using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BattleShipEngine
{
    public class Ship
    {
        
        internal  List<IShipAtom> ShipAtom                    =new List<IShipAtom>();
        internal  Gamer           Parent                      =null;        
        public    void            AddShipAtom(Point Location) 
        {
            //if (!IsShipAtom(Location))
                ShipAtom.Add(new ShipAtom(Location, this));
          //  else
          //      return this;
          //  return this;
        }
        public    /*Constructor*/ Ship(Gamer parent)          
        {
            Parent = parent;
        }
        public    /*Constructor*/ Ship()                      
        {
           
        }
     
        

    }
}
