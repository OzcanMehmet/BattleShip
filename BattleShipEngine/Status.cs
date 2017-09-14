using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BattleShipEngine
{
   
    public class BeginData                      
    {
        public /*Constructor*/ BeginData()                       
       {
           Name = "???";       
       }
        public bool            NewRoom                           { get; set; }
        public string          Name                              { get; set; }      
       

   }
    public class RoomList                       
   {
       public List<string> RoomlistString { get; set; }
       public int NumberofRoomListMember { get; set; }
   }
    public class BeginDataBack                  
   {

       public bool connect { get; set; }

   }    
    public class ShipData:Cordinate             
    {                  
        public bool Isvertical { get; set; }
        public int  Shiplength { get; set; }        
    }
    public class GamerStatus                    
    {
        public bool IsGamerActive { get; set; }
    }
    public class WhichGamer                     
    {
        public bool IsEnemy { get; set;}    
    }
    public class GameStatus                     
    {

        public bool IsPlay { get; set; }
    
    }
    public class ShipAddStatus                  
    {
       public ShipAddStatus()
        {
            message = new Message();
        
        }
       public Message message { get; set; }
       public bool IsShipAdded { get; set;}
    }
    public class ShootCordinate:Cordinate       
    {
        
    }
    public class ShootEnemyStatus               
    {
        public bool IsTried { get; set; }
        public bool IsShipAtom { get; set; }
        public Message message { get; set; }

    }
    public class ShootStatus:Cordinate          
    {
        public bool IsShip { get; set; }
         
    }
    public class PlayStatus                     
    {
        public bool Win { get; set; }
        public bool Loose { get; set; }

    }
    public class Message                        
    {
        public Message()
        {
            MessageData ="";
        
        }
        public bool   Error { get; set; }
        public string MessageData { get; set; }
    }                                      
    public class Cordinate                      
    {
        public int X { get;set;}
        public int Y { get;set;}
    }
}
