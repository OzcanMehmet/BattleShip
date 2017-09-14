using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace BattleShipEngine
{
    public class ShipAtom : IShipAtom,IMapItemAtom
    {
       public   Point              Location                                { get;set; }
       public   Ship               Parent                                  { get;set; }       
       public   /*Constructor*/    ShipAtom(Point location, Ship parent)   
        {
            Location = location;
            Parent = parent;
        }
    }           
}
