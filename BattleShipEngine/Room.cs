using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BattleShipEngine
{
    public class Room
    {
       public /*Constructor*/       Room(Gamer gamer)               
        {

            GamerLeft = gamer;
            gamer.room = this;
            ActiveGamer = gamer;
            Mapsize = 10;
            NumberofShip = 5;
        }    
       public Gamer                 GamerLeft                       =null;
       public Gamer                 GamerRight                      =null;                                                                    
       public Gamer                 ActiveGamer                     =null;
       public int                   Mapsize                         {get;set;}
       public int                   NumberofShip                    {get;set;}
       public bool                  Play                            =false;                  
       public bool                  gamerok(Gamer gamer,bool ok)    
       {
           gamer.Play=true;
           
           
           if (GamerLeft.Play && GamerRight!=null && GamerRight.Play)
           {
              return Play = true;
           }
           else
             return  Play = false;
           
           


       
       }
       public bool                  GamerIsOK(Gamer gamer)          
       {
           if (gamer.OK)
               return true;
           else
               return false;
       
       
       }
       public bool                  IsActiveGamer(Gamer gamer)      
       {

           if (gamer.Name==ActiveGamer.Name)
           {
               return true;
           }
           else
               return false;
       }
       
    }
}
