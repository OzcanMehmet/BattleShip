using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
namespace BattleShipEngine
{   
    public class Gamer
    {
       
        internal     List<Ship>                Shiplist                                               =new            List<Ship>();
        internal     List<MapItemAtom>         TriedMapItem                                           =new            List<MapItemAtom>();
        internal     List<ShipAtom>            ShipShooted                                            =new            List<ShipAtom>();
        internal     ComparerAtom              Compare                                                =new            ComparerAtom();
        public       Room                      room                                                   =null;
        private      ShipAddStatus             shipAddStatus                                          =new            ShipAddStatus();      
        public       Gamer                     Enemy                                                  =null;
        public       bool                      Win                                                    { get;  set; }
        public       bool                      Loose                                                  { get; set; }
        public       bool                      OK                                                     { get; set; }
        public       string                    Name                                                   { get; set; } 
        public       bool                      Play                                                   { get; set; }                                                                          
        public       /*Constructor*/           Gamer()                                                
        {
            Name = "???";

        }
        public       void                      Join(Room RooM)                                        
        {
             room = RooM;
             room.GamerRight=this;
             Enemy = room.GamerLeft;
       
        }
        internal     bool                      IsShipAtom (Point location)                            
        {   
            foreach(Ship ship in Shiplist)           
            foreach(ShipAtom Atom in ship.ShipAtom)
            {         
            if (Compare.Equals(Atom, new ShipAtom(location, ship))) 
                return true;           
            }
            return false;                
        }
        internal     bool                      IsTried(Point location)                                
        {              
            foreach(MapItemAtom Atom in TriedMapItem)
            {
                if (Compare.Equals(new MapItemAtom(location), Atom))
                    return true;                                  
            }
            return false;                        
        }
        public       ShootEnemyStatus          Shoot(Point location)                                  
        {
            
            if (!IsTried(location))
            {
               
                room.ActiveGamer = Enemy;
                if (IsShipAtom(location))
                {
                    ShipShooted.Add(new ShipAtom(location,new Ship(this)));   
                    TriedMapItem.Add(new MapItemAtom(location));
                    if (IsWin())
                    {
                        Enemy.Win = true;
                        Loose=true;
                    }
                    return new ShootEnemyStatus()
                    {
                        //IsActive = room.IsActiveGamer(this),
                        IsShipAtom=true,                        
                        IsTried = false,
                        message = new Message() {Error=false,MessageData="Gemi Parçası vuruldu",} 
                    };
                }
                else
                {
                    TriedMapItem.Add(new MapItemAtom(location));
                    return  new ShootEnemyStatus()
                    {
                        //IsActive = room.IsActiveGamer(this),
                        IsShipAtom =false,
                        IsTried = false,
                        message = new Message() { Error = true, MessageData = "Karavana", }
                    };
                }
            }
            else
                return new ShootEnemyStatus()
                {
                    //IsActive = room.IsActiveGamer(this),
                    IsShipAtom = false,
                    IsTried = true,
                    message = new Message() { Error = true, MessageData = "Denendi", }
                }; 
            
        
        
        
        }
        private      bool                      IsShipAtomExist(Ship Ship)                             
        {
            foreach (ShipAtom Atom in Ship.ShipAtom)
            {

                if (Atom.Location.X < room.Mapsize && Atom.Location.Y < room.Mapsize)
                {
                    if (IsShipAtom(Atom.Location))
                    {
                        shipAddStatus.message.Error = true;
                        shipAddStatus.message.MessageData = "Ship atom exist at this point";
                        return true;
                    }                    
                }
                else
                {
                    shipAddStatus.message.Error = true;
                    shipAddStatus.message.MessageData = "Map Size";
                    return true;



                }
             
                  
                
            }
            return false;         
        }
        public       ShipAddStatus             AddShip(Point location,int Length,bool Vertical)       
        {
            shipAddStatus = new ShipAddStatus();
            
            if (this.Shiplist.Count < room.NumberofShip)
            {
                
                Ship BufferShip = new Ship();
                
                string s = BufferShip.ToString();
                Point BufferPoint;
                for (int i = 0; i < Length; i++)
                {
                    if (Vertical)
                    {
                        BufferPoint = new Point(location.X, location.Y + i);
                        BufferShip.AddShipAtom(BufferPoint);
                    }
                    else
                    {
                        BufferPoint = new Point(location.X + i, location.Y);
                        BufferShip.AddShipAtom(BufferPoint);
                    }

                }
                if (!IsShipAtomExist(BufferShip))
                {
                    Shiplist.Add(BufferShip);
                    if (Shiplist.Count == room.NumberofShip)
                    {
                        OK = true;
                    }
                    shipAddStatus.message.MessageData = "ship Added";
                    shipAddStatus.IsShipAdded = true;
                    return shipAddStatus;
                }
                else
                {
                    shipAddStatus.IsShipAdded=false;
                    
                    return shipAddStatus;                    
                }


               /* if (this.Shiplist.Count == room.NumberofShip)
                    OK = true;
                else
                    OK = false;*/
            }
            else
            {
               
               shipAddStatus.IsShipAdded = false;
               shipAddStatus.message.Error = true;
               shipAddStatus.message.MessageData = "ASırı Gemi";
                return shipAddStatus;
                
            }            
        }
        public       bool                      IsShipShooted(Point Location)                          
        {

            int counter=0;
            Ship BufShip=null;
              foreach (Ship ship in Shiplist)
                foreach (ShipAtom Atom in ship.ShipAtom)                
                    if (Compare.Equals(Atom, new ShipAtom(Location, ship)))                                    
                      foreach(ShipAtom shipAtom  in Atom.Parent.ShipAtom)
                        foreach (ShipAtom shipAtomShooted in ShipShooted)
                        {    BufShip=Atom.Parent;
                            if (Compare.Equals(shipAtom, shipAtomShooted))  
                            counter++;
                        }

              if (counter == BufShip.ShipAtom.Count)
                  return true;
              else
                  return false;
                       
        
               
                        
                
            
        
        
        
        }
        public       bool                      IsWin()                                                
        {
            bool Exist = true ;
            foreach (Ship ship in Shiplist)
                foreach (ShipAtom Atom in ship.ShipAtom)
                {
                   
                     foreach (ShipAtom ShootAtom in ShipShooted)
                         if ((Compare.Equals(Atom, ShootAtom)))
                         {
                             Exist = true;
                             break;
                         }
                         else
                         {
                             Exist = false;
                             
                            
                         }
                     if (!Exist)
                         return false;
                     
                        
                }
                                 
            return true; 
        }                              
    }
}
