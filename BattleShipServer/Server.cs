using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleShipEngine;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Drawing;
using System.IO;
using System.Web.Script.Serialization;

namespace BattleShipServer
{
    public class Server
    {
        private        TcpListener               ClientListen                                    { get; set; }
        private        List<ConnectionClient>    ConnectionList                                  = new List<ConnectionClient>();        
        public         void                      ServerStart()                                   
        {
            ClientListen = new TcpListener(IPAddress.Any, 59876);
            try
            {
            ClientListen.Start();            
                while (true)
                {

                    while (!ClientListen.Pending())
                    {
                        Thread.Sleep(200);
                    }
                    ConnectionClient NewConnect = new ConnectionClient(ClientListen);

                    ConnectionList.Add(NewConnect);
                    NewConnect.ConnectionList = ConnectionList;
                    Thread ClientThread = new Thread(new ThreadStart(NewConnect.HandleConnection));
                    ClientThread.Start();
                    NewConnect.server = this;
                    NewConnect.thread = ClientThread;

                }
            }
            catch(Exception)
            {
                ClientListen.Stop();
                
            }
        }
        public         void                      AbortClient(Thread Abortthread)                 
        {
            Abortthread.Abort();
        }
        internal       class                     ConnectionClient                                
        {
            public ConnectionClient(TcpListener listen)
            {
                Listen = listen;
                Client = Listen.AcceptTcpClient();
                Stream = Client.GetStream();
                writer = new BinaryWriter(Stream);
                Reader = new BinaryReader(Stream);
            
            }
            public Server server = null;  
            public    Thread thread=null;
            private   JavaScriptSerializer       Serilizer                     = new JavaScriptSerializer();
            internal  List<ConnectionClient>     ConnectionList                = new List<ConnectionClient>();
            internal  TcpListener                Listen                        = null;
            internal  TcpClient                  Client                        = null;
           
            private   NetworkStream              Stream                        = null;
            private   Room                       room                          = null;
            private   Gamer                      gamer                         = null;           
            private   BeginData                  begindata                     = null;
            private   ShipData                   shipdata                      = null;
            private   GamerStatus                gamerstatus                   = null;
            private   ShipAddStatus              shipstatus                    = null;
            private   RoomList                   roomlist                      = null;
            internal   BinaryWriter              writer                        = null;            
            internal BinaryWriter enemywriter = null;
            private   BinaryReader               Reader                        = null;                       
            public    void                       HandleConnection()            
            {

                try
                {
                    while (Client.Connected)
                    {








                        if (room == null)
                        {

                            begindata = (BeginData)Serilizer.Deserialize(Reader.ReadString(), typeof(BeginData));
                            Stream.Flush();

                            if (begindata.NewRoom)
                            {
                                room = new Room(gamer = new Gamer());
                                room.GamerLeft.Name = begindata.Name;
                                writer.Write(Serilizer.Serialize(new BeginDataBack() { connect = true, }));
                                writer.Write(Serilizer.Serialize(new BeginDataBack() { connect = false, }));

                            }
                            else
                            {

                                if (begindata.Name != "???")
                                {
                                    room = ConnectClient(begindata.Name);

                                    if (room != null)
                                    {
                                        writer.Write(Serilizer.Serialize(new BeginDataBack() { connect = true, }));
                                        writer.Write(Serilizer.Serialize(new BeginDataBack() { connect = true, }));
                                        enemywriter.Write(Serilizer.Serialize(new BeginDataBack() { connect = true, }));
                                    }
                                    else
                                    {
                                        writer.Write(Serilizer.Serialize(new BeginDataBack() { connect = false, }));
                                    }


                                }
                                else
                                {
                                    roomlist = new RoomList();
                                    roomlist.RoomlistString = WaitingRoom();

                                    writer.Write(Serilizer.Serialize(new BeginDataBack() { connect = false, }));


                                    writer.Write(Serilizer.Serialize(roomlist));

                                }
                            }
                        }
                        else
                        {

                            if (!room.Play)//point
                            {
                                gamerstatus = (GamerStatus)Serilizer.Deserialize(Reader.ReadString(), typeof(GamerStatus));
                                if (!room.Play)
                                    if (!gamerstatus.IsGamerActive)
                                    {
                                        shipdata = (ShipData)Serilizer.Deserialize(Reader.ReadString(), typeof(ShipData));
                                        shipstatus = gamer.AddShip(new Point(shipdata.X, shipdata.Y), shipdata.Shiplength, shipdata.Isvertical);
                                        writer.Write(Serilizer.Serialize(new GamerStatus() { IsGamerActive = gamer.OK }));
                                        writer.Write(Serilizer.Serialize(shipstatus));
                                        Stream.Flush();
                                        writer.Flush();

                                    }
                                    else
                                    {

                                        if (!room.gamerok(gamer, gamerstatus.IsGamerActive))
                                        {

                                            writer.Write(Serilizer.Serialize(new GameStatus() { IsPlay = false, }));
                                            gamer.OK = true;


                                        }
                                        else
                                        {
                                            writer.Write(Serilizer.Serialize(new GameStatus() { IsPlay = true, }));
                                            enemywriter.Write(Serilizer.Serialize(new GameStatus() { IsPlay = true, }));

                                            writer.Write(Serilizer.Serialize(new GamerStatus() { IsGamerActive = true }));
                                            enemywriter.Write(Serilizer.Serialize(new GamerStatus() { IsGamerActive = false, }));
                                        }

                                    }

                            }
                            else //Play 
                            {

                                ShootCordinate shootcordinate = (ShootCordinate)Serilizer.Deserialize(Reader.ReadString(), typeof(ShootCordinate));
                                ShootEnemyStatus shoostatus = gamer.Enemy.Shoot(new Point(shootcordinate.X, shootcordinate.Y));
                                writer.Write(Serilizer.Serialize(new PlayStatus() { Win = gamer.Win, Loose = gamer.Loose }));
                                writer.Write(Serilizer.Serialize(new GamerStatus() { IsGamerActive = room.IsActiveGamer(gamer) }));
                                writer.Write(Serilizer.Serialize(shoostatus));
                                if (!shoostatus.IsTried)
                                {
                                    enemywriter.Write(Serilizer.Serialize(new PlayStatus() { Win = gamer.Enemy.Win, Loose = gamer.Enemy.Loose }));
                                    enemywriter.Write(Serilizer.Serialize(new GamerStatus() { IsGamerActive = room.IsActiveGamer(gamer.Enemy) }));
                                    enemywriter.Write(Serilizer.Serialize(new ShootStatus() { X = shootcordinate.X, Y = shootcordinate.Y, IsShip = shoostatus.IsShipAtom }));
                                }




                            }

                        }






                    }
                }
                catch (Exception)
                {
                    writer.Dispose();
                    Reader.Dispose();
                    Stream.Dispose();
                 
                    foreach (ConnectionClient client in ConnectionList)
                        if (client.Client == this.Client)
                        {
                            ConnectionList.Remove(client);
                            break;
                        }


                    Client.Close();
                    server.AbortClient(thread);
                    
                }
                
                
            }                                                                          
            private   Point                      newPoint(string X, string Y)  
            {
                return new Point(Convert.ToInt32(X), Convert.ToInt32(Y));
            }
            private   Room                       ConnectClient(string name)    
            {
                Room room = null;

                foreach (ConnectionClient Waitclient in ConnectionList)
                    if (Waitclient.room != null && Waitclient.room.GamerLeft.Name == name)
                    {
                        gamer = new Gamer();
                        
                        gamer.Join(Waitclient.room);
                        room = Waitclient.room.GamerLeft.room;
                        gamer.Enemy = room.GamerLeft;
                        room.GamerLeft.Enemy = gamer;
                        enemywriter = Waitclient.writer;
                        Waitclient.enemywriter = writer;
                        


                    }
                return room;



            }
            private   List<string>               WaitingRoom()                 
            {

                List<string> gamerlist = new List<string>();
                foreach (ConnectionClient Waitclient in ConnectionList)
                {
                    if (Waitclient.room != null && Waitclient.room.GamerRight == null)
                    {

                        gamerlist.Add(Waitclient.room.GamerLeft.Name);
                    }

                }
                return gamerlist;

            }                                
           
          

        }
    }
}
    

