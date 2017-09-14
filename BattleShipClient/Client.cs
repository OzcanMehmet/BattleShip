using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Web.Script.Serialization;
using System.Drawing;
using System.Threading;

namespace BattleShipClient
{
    class Client
    {/// Delegatelerde boşa kullanılmış var 
       private    String               IP                                                             = null;
       private    TcpClient            client                                                         = null;
       private    NetworkStream        Stream                                                         = null;
       private    BinaryWriter         writer                                                         = null;
       private    BinaryReader         Reader                                                         = null;
       private    CliendDesign         Form                                                           = null;
       private    JavaScriptSerializer serializer                                                     = new JavaScriptSerializer();
       private    RoomList             roomlist                                                       = null;
       public     bool                 roomListbool                                                   = false;
       private    bool                 GamerOk                                                        { get; set; }
       private    bool                 Play                                                           { get; set; }
       private    BeginDataBack        databack                                                       = new BeginDataBack();
       private    ShipAddStatus        shipaddstatus                                                  = null;
       delegate   void                 AddRoomlist                                                    (List<string> roomlist);
       delegate   void                 message                                                        (string msj,bool Error);
       delegate   void                 StartConfigStart                                               (bool ok);
       delegate   void                 ConfigBool                                                     (bool ok);
       delegate   void                 connectedConfig                                                ();
       delegate   void                 lstbxShipLengthitemRemove                                      ();
       delegate   void                 shipaddpanel                                                   (bool show,bool Isplay);
       delegate   void                 ShipAddclick                                                   ();
       delegate   void                 Config                                                         ();      
       public     /*Constructor*/      Client(CliendDesign form,string Ip)                            
       {
           IP = Ip;
           Form = form;
           client = new TcpClient();
       
       }
       public     void                 ShipAddPanelLocationChange()                                   
       {
           if (Form.PanelAddMap.InvokeRequired)
               Form.PanelAddMap.Invoke(new Config(ShipAddPanelLocationChange), new object[] { });
           else
               Form.PanelAddMap.Location = new Point(525, 0);
       }
       public     void                 FormNewSize()                                                  
       {
           if (Form.InvokeRequired)
               Form.Invoke(new Config(FormNewSize), new object[] { });
           else
               Form.Size=new Size((Form.PanelAddMap.Width*2)-150,Form.Size.Height+20);
       }
       public     void                 ActiveLabelConfigcolor(bool IsActive)                          
       {
           if (Form.LabelActive.InvokeRequired)
               Form.LabelActive.Invoke(new ConfigBool(ActiveLabelConfigcolor), new object[] {IsActive });
           else
               if (IsActive)
                   Form.LabelActive.ForeColor = Color.Green;
               else
                   Form.LabelActive.ForeColor=Color.Red;

       }
       public     void                 ActiveLabelConfig(bool IsActive)                               
       {
           if (Form.LabelActive.InvokeRequired)
               Form.LabelActive.Invoke(new ConfigBool(ActiveLabelConfig), new object[] {IsActive });
           else
               if(IsActive)
               Form.LabelActive.Visible = true;
               else
                   Form.LabelActive.Visible=false;
       }
       public     void                 ShipAddButtonClickTake()                                       
       {
           //foreach(Control in Form.PanelAddMap.Controls)
           if (Form.BtnAddShip.InvokeRequired)
               Form.BtnAddShip.Invoke(new ShipAddclick(ShipAddButtonClickTake), new object[] { });
           else
           Form.BtnAddShip.Click -= Form.Add_Click;
              
           
       
       }
       public     void                 JoinRoom(string RoomName)                                      
       {
           try
           {
               roomListbool = true;
               writer.Write(serializer.Serialize(new BeginData() { Name = RoomName, NewRoom = false, }));
           }
           catch (Exception)
           { statusmessage("Connect Error",true); }
       }
       public     void                 JoinRoom()                                                     
       {
           try
           {
               roomListbool = true;
               writer.Write(serializer.Serialize(new BeginData() { Name = "???", NewRoom = false, }));
           }
           catch (Exception)
           { statusmessage("Connect Error",true); }
       }
       public     void                 AddShip(Point cordinate, bool Vertical, int length)            
       {
           try
           {
               Stream.Flush();
               writer.Flush();
               writer.Write(serializer.Serialize(new GamerStatus() { IsGamerActive = GamerOk }));
               writer.Write(serializer.Serialize(new ShipData() { X = cordinate.X, Y = cordinate.Y, Isvertical = Vertical, Shiplength = length }));
               Cordinate = cordinate;
               Isvertical = Vertical;
               Shiplength = length;
           }
           catch (Exception)
           { statusmessage("Connect Error",true); }
       }
       public     void                 newRoom(string RoomName)                                       
       {
           if (RoomName.Trim() != "" && RoomName != "???")
           {
               try
               {
                   writer.Write(serializer.Serialize(new BeginData() { Name = RoomName, NewRoom = true, }));
               }
               catch (Exception)
               { statusmessage("Connect Error",true); }
           }
           else
           {
               statusmessage("Karakter uygun değil",true);
               NewRoomPanelActive();
           }
       }
       public     void                 ShootCordinateSend(int x, int y)                               
       {
           try
           {
               writer.Write(serializer.Serialize(new ShootCordinate() { X = x, Y = y }));
           }
           catch (Exception)
           { statusmessage("Connec Error",true); }
       }
       public     void                 start()                                                        
       {
           try
           {
               writer.Write(serializer.Serialize(new GamerStatus() { IsGamerActive = true }));
           }
           catch (Exception)
           { statusmessage("Connect Error",true); }
       }              
                                     //Main Function Connection 
       public     void                 Connection()                                                   
       {
           try
           {
               client.Connect(IP, 59876);
               Stream = client.GetStream();
               writer = new BinaryWriter(Stream);
               Reader = new BinaryReader(Stream);
               PanelBeginActive();
               statusmessage("Connected",false);




               while (client.Connected)
               {
                   if (!Play)
                   {
                       do
                       {
                           databack = (BeginDataBack)serializer.Deserialize(Reader.ReadString(), typeof(BeginDataBack));
                           if (roomListbool)
                           {
                               roomlist = (RoomList)serializer.Deserialize(Reader.ReadString(), typeof(RoomList));
                               Roomlist(roomlist.RoomlistString);
                           }
                       } while (!databack.connect);
                       /////
                       do
                       {
                           databack = (BeginDataBack)serializer.Deserialize(Reader.ReadString(), typeof(BeginDataBack));
                           if (!databack.connect)
                           {
                               statusmessage("Player is waiting",false);
                               ShipaddPanelConfig(databack.connect, false);
                           }
                           else
                           {
                               statusmessage("Connected",false);
                               ShipaddPanelConfig(databack.connect, false);
                           }
                       } while (!databack.connect);
                       ///shipadd 
                       while (databack.connect)
                       {

                           GamerStatus gamerstatus;

                           do
                           {

                               gamerstatus = (GamerStatus)serializer.Deserialize(Reader.ReadString(), typeof(GamerStatus));
                               shipaddstatus = (ShipAddStatus)serializer.Deserialize(Reader.ReadString(), typeof(ShipAddStatus));
                               
                               if (shipaddstatus.IsShipAdded && shipaddstatus.message != null)
                               {
                                   Form.CreaMapDrawShip(Cordinate, Isvertical, Shiplength);
                                   lstbxShipLengthItemRemove();

                               }
                               if (shipaddstatus.message != null)
                                   if (shipaddstatus.IsShipAdded)
                                       statusmessage(shipaddstatus.message.MessageData,false);
                                   else
                                       statusmessage(shipaddstatus.message.MessageData,true);
                           }
                           while (!gamerstatus.IsGamerActive);


                           StartConfigDesign(gamerstatus.IsGamerActive);
                           Boolean IsWait = false;
                           while (!((GameStatus)serializer.Deserialize(Reader.ReadString(), typeof(GameStatus))).IsPlay)
                           {
                               statusmessage("Player is waiting",false);
                               IsWait = true;

                           }
                           if (IsWait)
                               writer.Write(serializer.Serialize(new GamerStatus() { IsGamerActive = false, }));

                           statusmessage("Started",false);
                           ShipaddPanelConfig(gamerstatus.IsGamerActive, true);
                           ShipAddButtonClickTake();
                           PanelEnemyShow();
                           ShipAddPanelLocationChange();
                           FormNewSize();

                           if (!((GamerStatus)serializer.Deserialize(Reader.ReadString(), typeof(GamerStatus))).IsGamerActive)
                           { PanelEnemyShow(false); ActiveLabelConfigcolor(false); }
                           ActiveLabelConfig(true);
                               
                              
                          

                           ShootEnemyStatus shootenemystatus = null;
                           ShootStatus shootstatus = null;
                           PlayStatus playstatus = null;

                           while (true)
                           {
                               ///  map
                               do
                               {
                                   playstatus = (PlayStatus)serializer.Deserialize(Reader.ReadString(), typeof(PlayStatus));


                                   if (((GamerStatus)serializer.Deserialize(Reader.ReadString(), typeof(GamerStatus))).IsGamerActive)
                                   {
                                       //enemymap
                                       shootenemystatus = (ShootEnemyStatus)serializer.Deserialize(Reader.ReadString(), typeof(ShootEnemyStatus));
                                       if (!shootenemystatus.IsTried)
                                       {
                                           PanelEnemyShow(false);
                                           ActiveLabelConfigcolor(false);
                                           statusmessage(shootenemystatus.message.MessageData,shootenemystatus.message.Error);
                                           Form.ShootMap(shootenemystatus.IsShipAtom);

                                       }
                                       else
                                       {
                                           ActiveLabelConfigcolor(true);
                                           PanelEnemyShow(true);
                                           statusmessage(shootenemystatus.message.MessageData,true);
                                       }
                                   }
                                   else
                                   {
                                       ///mymap

                                       shootstatus = (ShootStatus)serializer.Deserialize(Reader.ReadString(), typeof(ShootStatus));
                                       PanelEnemyShow(true);
                                       ActiveLabelConfigcolor(true);
                                       Form.ShootMymap(shootstatus.X, shootstatus.Y, shootstatus.IsShip);
                                   }






                               } while (!(playstatus.Win || playstatus.Loose));
                               if (playstatus.Win)
                                   Form.EndMessage("You Win");
                               if (playstatus.Loose)
                                   Form.EndMessage("Game Over");
                               statusmessage("End",true);









                           }








                       }


                   }




               }
           }
           catch (Exception)
           {
               if (!databack.connect)
               {
                   statusmessage("Server is not exist",true);
                   IpConnectItemVisible();
               }         
               Form.EndMessage("Connect Error");
               client.Close();
               Close();              
           }
          
        }
       public     void                 Close()                                                        
       {
           if (Form.InvokeRequired)
               Form.Invoke(new Config(Close), new object[] { });
           else
           {
               
               Form.thread.Abort();
           }

       }
       public     void                 IpConnectItemVisible()                                         
       {
           if(Form.BtnConnect.InvokeRequired)           
              Form.BtnConnect.Invoke(new Config(IpConnectItemVisible),new object[] {});
           else
               Form.BtnConnect.Visible=true;
           if(Form.txtbxIp.InvokeRequired)           
              Form.txtbxIp.Invoke(new Config(IpConnectItemVisible),new object[] {});
           else
               Form.txtbxIp.Visible=true;

          


       }
       public     void                 PanelEnemyShow()                                               
       {
           if(Form.PanelEnemyMap.InvokeRequired)
               Form.PanelEnemyMap.Invoke(new Config(PanelEnemyShow),new object[] {});
           else
           Form.PanelEnemyMap.Visible=true;

       }
       public     void                 PanelEnemyShow(bool Enabled)                                   
       {
           if (Form.PanelEnemyMap.InvokeRequired)
               Form.PanelEnemyMap.Invoke(new ConfigBool(PanelEnemyShow), new object[] {Enabled });
           else
               if (Enabled)
                   Form.PanelEnemyMap.Enabled = true;
               else
                   Form.PanelEnemyMap.Enabled = false;
           
       }
       public     void                 lstbxShipLengthItemRemove()                                    
      {
          if (Form.LstShipLength.InvokeRequired)
              Form.LstShipLength.Invoke(new lstbxShipLengthitemRemove(lstbxShipLengthItemRemove), new object[] { });
          else
              Form.LstShipLength.Items.Remove(Shiplength.ToString());

       
       }       
       public     void                 Roomlist(List<string> roomlist)                                
        {
            if (Form.LstRomList.InvokeRequired)
            {
                Form.LstRomList.Invoke(new AddRoomlist(Roomlist), new object[] { roomlist });              
            }
            else
            {                
                if(roomlist!=null)
                    foreach (string room in roomlist)                                          
                        Form.LstRomList.Items.Add(room.Trim());
                  
            }
        }       
       public     void                 ShipaddPanelConfig(bool show,bool Isplay)                      
        {
            if (Form.PanelAddMap.InvokeRequired)
                Form.PanelAddMap.Invoke(new shipaddpanel(ShipaddPanelConfig), new object[] { show,Isplay });
            if (Form.BtnStart.InvokeRequired && Form.LstShipLength.InvokeRequired && Form.ChckboxVertical.InvokeRequired && Form.label.InvokeRequired)
            {
                
                Form.BtnStart.Invoke(new shipaddpanel(ShipaddPanelConfig), new object[] { show, Isplay });
                Form.LstShipLength.Invoke(new shipaddpanel(ShipaddPanelConfig), new object[] { show, Isplay });
                Form.ChckboxVertical.Invoke(new shipaddpanel(ShipaddPanelConfig), new object[] { show, Isplay });
                Form.label.Invoke(new shipaddpanel(ShipaddPanelConfig), new object[] { show, Isplay });
            }
            else
                if (show)
                {
                    if (Isplay)
                    {
                        Form.PanelAddMap.Enabled = false;
                        Form.PanelAddMap.Location = new Point(510, Form.PanelAddMap.Location.Y);
                        Form.BtnStart.Visible = false;
                        Form.LstShipLength.Visible = false;
                        Form.ChckboxVertical.Visible = false;
                        Form.label.Visible=false;
                    }
                    Form.PanelAddMap.Visible = true;
                }
                else
                    Form.PanelAddMap.Visible = false;    
                              
        }       
       public     void                 NewRoomPanelActive()                                           
        {
            if (Form.PanelNewRoom.InvokeRequired)
            {
                Form.PanelNewRoom.Invoke(new connectedConfig(NewRoomPanelActive), new object[] { });                                       
            }
            else
            {
                Form.PanelNewRoom.Visible = true;                            
            }                        
        }
       public     void                 PanelBeginActive()                                             
       {
           if (Form.PanelBegin.InvokeRequired)
               Form.PanelBegin.Invoke(new Config(PanelBeginActive), new object[] { });
           else
               Form.PanelBegin.Enabled=true;
       }
       private    Point                Cordinate                                                      { get; set; }
       private    bool                 Isvertical                                                     { get; set; }
       private    int                  Shiplength                                                     { get; set; }                   
       public     void                 StartConfigDesign(bool ok)                                     
        {

            if (Form.BtnStart.InvokeRequired && Form.BtnAddShip.InvokeRequired)
            {
                Form.BtnStart.Invoke(new StartConfigStart(StartConfigDesign), new object[] { ok});
                Form.BtnStart.Invoke(new ConfigBool(StartConfigDesign), new object[] { ok });
            }
            else
            {
                if (ok)
                {
                    Form.BtnStart.Enabled = true;
                    Form.BtnAddShip.Enabled = false;
                }
                else
                {
                    Form.BtnStart.Enabled = false;
                    Form.BtnAddShip.Enabled = true; 
                }
            }
        }
       public     void                 statusmessage(string msj,bool Error)                           
        {
            if (Form.Statusmassage.InvokeRequired)
                Form.Statusmassage.Invoke(new message(statusmessage), new object[] { msj,Error });
            else               
                Form.Statusmassage.Text = msj;
            
           if (Form.Statusmassage.InvokeRequired)
               Form.Statusmassage.Invoke(new message(statusmessage), new object[] { msj, Error });
           if (Error)
                Form.Statusmassage.ForeColor = Color.Red;
            else
                Form.Statusmassage.ForeColor = Color.Green;
           
        }
      
       
       
        
    }
}
