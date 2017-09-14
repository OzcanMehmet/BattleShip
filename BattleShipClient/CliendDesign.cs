using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using BattleShipServer;
using System.Net;


namespace BattleShipClient
{
    public partial class CliendDesign : Form
    {
        public               Thread                     thread                                                               = null;
        private              Button                     BtnNewGame                                                           = null;
        private              Button                     BtnJoin                                                              = null;        
        private              Button                     BtnBuildRoom                                                         = null;
        private              Button                     BtnJoinRoom                                                          = null;
        internal             Button                     BtnStart                                                             = null;
        internal             Button                     BtnAddShip                                                           = null;
        private              Button                     BtnReflesh                                                           = null;
        private              Button                     BtnCreateMapButton                                                   = null;
        private              Button                     BtnShoot                                                             = null;
        private              TextBox                    txtRoomName                                                          = null;
        internal             ListBox                    LstRomList                                                           = null;
        internal             ListBox                    LstShipLength                                                        = null;
        private              Panel                      PanelAdd                                                             = null;        
        public               Panel                      PanelBegin                                                           = null;
        internal             Panel                      PanelNewRoom                                                         = null;
        private              Panel                      PanelJoin                                                            = null;
        internal             Panel                      PanelAddMap                                                          = null;
        internal             Panel                      PanelEnemyMap                                                        = null;
        internal             Label                      Statusmassage                                                        = null;
        private              ComboBox                   ComboBxY                                                             = null;
        private              ComboBox                   ComboBxX                                                             = null;                     
        public               CheckBox                   ChckboxVertical                                                      = null;
        private              object                     ShootSender                                                          = null;
        public               Label                      label                                                                = null;
        private              int                        offset                                                               { get; set; }
        private              bool                       ShipAdd                                                              { get; set; }
        private              Client                     client                                                               = null;
        public               Label                      LabelActive                                                          = null;
        public               /*Constructor*/            CliendDesign()                                                       
        {         
            offset = 1;
            InitializeComponent();                      
            this.Size = new Size(650, 550);              
        }       
        private              void                       Form1_Load(object sender, EventArgs e)                               
        {
            LabelActive=new  Label()
            {
                Parent=this,
                Location=new Point(150,this.Height-55),
                Anchor=((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left))),
                Text="ACTİVE",
                ForeColor=Color.Green,
                Visible=false,

            };            
            
            Statusmassage = new Label()
            {
                Parent=this,
                Location=new Point(0,this.Height-55),
                Anchor=((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left))),
            };            
            PanelBegin = new Panel()
            {
                Parent = this,
                Visible=true,
                Enabled=false,
                Size=new Size(500,500),
            };                      
            BtnNewGame=new Button()
            {
                Parent=PanelBegin,
                Location=new Point(0,0),
                Size=new Size(100,50),
                Text="Create Room",                
            };
            BtnNewGame.Click += BtnNewGame_Click;
            BtnJoin = new Button()
            {
                Parent = PanelBegin,
                Location = new Point(0, 50),
                Size = new Size(100, 50),
                Text = "Join",
            };
            BtnJoin.Click += BtnJoin_Click;
            /////////////////////////////////
            PanelNewRoom = new Panel()
            {
                Parent = this,
                Visible=false,
                Size = new Size(500, 500),
            };
            BtnBuildRoom = new Button()
            {
                Parent = PanelNewRoom,            
                Location = new Point(0, 0),
                Size = new Size(100, 50),
                Text = "New Game",
            };
            BtnBuildRoom.Click += BtnBuildRoom_Click;
            
            txtRoomName = new TextBox()
            {
                Parent = PanelNewRoom,            
                Location = new Point(0, 50),
                Size = new Size(100, 50),
            };
            /////////////////////////
            PanelJoin = new Panel()
            {
                Parent = this,
                Visible = false,
                Size = new Size(500, 500),
            };
            BtnJoinRoom = new Button()
            {
                Parent = PanelJoin,
                Enabled =false,
                Location = new Point(0, 0),
                Size = new Size(100, 50),
                Text = "Connect",

            };
           
            BtnJoinRoom.Click += BtnJoinRoom_Click;
            LstRomList = new ListBox()
            {
                Parent=PanelJoin,
               
                Location = new Point(0, 50),
                Size = new Size(100, 50),
                


            };
            LstRomList.SelectedIndexChanged += LstRomList_SelectedIndexChanged;
            BtnReflesh = new Button()
            {
                Parent = PanelJoin,
               
                Location = new Point(100, 50),
                Size = new Size(100, 40),
                Text = "Reflesh",

            };
            BtnReflesh.Click += Reflesh_Click;
            ////////////////////////////////////
            PanelEnemyMap=new Panel()
            {
                Parent=this,
                Visible = false,
                Size = new Size(510, 600),
                Location = new Point(0, 0),
            };
            /////////////////////////////
            PanelAdd = new Panel()
            {
                Parent = this,
                Visible=false,
                Size=new Size(200,200),               
            };
            PanelAddMap = new Panel()
            {
                Parent = this,
                Visible = false,
                Size = new Size(600, 600),
                Location=new Point(0,0)
            };
            BtnStart = new Button()
            {
                Parent = PanelAddMap,
                Enabled=false,
                Location = new Point(515, 0),
                Size = new Size(50, 50),
                Text="Start",
            }; BtnStart.Click += Start_Click;  
            ChckboxVertical = new CheckBox()
            {
                Parent = PanelAddMap,
                Location = new Point(515, 55),
                Text="Vertical",
                Checked=false,
            };
            LstShipLength = new ListBox()
            {
                Parent = PanelAddMap,
                Location = new Point(515, 95),                
            };
            LstShipLength.SelectedIndexChanged += lstbxShipLength_SelectedIndexChanged;
            LstShipLength.Items.AddRange(new object[] {"2","3","3","4","5"});
            label=new Label()
            {
                Parent = PanelAddMap,
                Location = new Point(510, 80),
                Text="Ship Length",

            };
                        
            
                      
            BtnAddShip = new Button() 
            {
                Parent=PanelAdd,
                Location = new Point(100,100),
                Size = new Size(50, 50),
                Text="Add",
            };
            BtnAddShip.Click += AddShip_Click;            
            ComboBxX=new ComboBox()
            {
                Parent = PanelAdd,
                Location=new Point(0,0),
                Size=new Size(50,50),                
            };            
            ComboBxY = new ComboBox()
            {
                Parent = PanelAdd,
                Location = new Point(0, 20),
                Size = new Size(50, 50),                
            };
            for (int i = 0; i < 10; i++)
            {
                ComboBxX.Items.Add(i);
                ComboBxY.Items.Add(i);
            }
            CreateMap(BtnCreateMapButton,PanelAddMap,offset,true);
            CreateMap(BtnShoot,PanelEnemyMap, offset, false);
            
            

            
//////////////////////////////////////////////////////////////////////////////
            
        }
        private              void                       lstbxShipLength_SelectedIndexChanged(object sender, EventArgs e)     
        {
            ShipAdd = true;
        }
        private              void                       LstRomList_SelectedIndexChanged(object sender, EventArgs e)          
        {
            if(LstRomList.SelectedIndex>=0)
            BtnJoinRoom.Enabled = true;
        }
        private              void                       Start_Click(object sender, EventArgs e)                              
        {
            client.start();
            
        }
        private              void                       AddShip_Click(object sender, EventArgs e)                            
        {

            //client.AddShip(comboboxX.SelectedIndex,comboboxY.SelectedIndex);
        }
        private              void                       Reflesh_Click(object sender, EventArgs e)                            
        {
            LstRomList.Items.Clear();
            client.roomListbool = true;
            
            client.JoinRoom();
            BtnJoinRoom.Enabled = false;
        }
        public               void                       CreateMap(Button buton,Panel panel,int offset,bool Add)              
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j <10; j++)
                {
                 buton = new Button()
                 {
                     Parent =panel,
                     Visible=true,                     
                     Size = new Size(50, 50),
                     BackColor=Color.Blue,
                     Location = new Point(i *(50 + offset), j * (50 + offset))
                 };
                 if (Add)
                     buton.Click += Add_Click;
                 else
                     buton.Click += Shoot_Click;
                }
           }
      
        
        }
        private              void                       Shoot_Click(object sender, EventArgs e)                              
        {
            PanelEnemyMap.Enabled = false;
            ShootSender = (Control)sender;
            client.ShootCordinateSend(GetCordinate(sender).X, GetCordinate(sender).Y);
            
        }               
        public               void                       ShootMap(bool IsShip)                                                
        {
            if(IsShip)
            ((Button)ShootSender).BackColor=Color.Red;
            else
            ((Button)ShootSender).BackColor = Color.DarkBlue;
        }
        public               void                       ShootMymap(int X,int Y,bool Isship)                                  
        {
            ShootSender=GetShipbutton(X,Y);
            ShootMap(Isship);
            

        }
        public               void                       EndMessage(string mesaj)                                             
        {
            MessageBox.Show(mesaj);
        }
        public               Control                    GetShipbutton(int x,int y)                                           
        {
            foreach (Control bufbutton in PanelAddMap.Controls)
                if (bufbutton is Button)
                    if (GetCordinate(bufbutton).X == x && GetCordinate(bufbutton).Y == y)
                    {
                        return bufbutton;
                    }                    
            return new Control();
        }
        public               void                       CreaMapDrawShip(Point Cordinate,bool vertical,int Length)            
        {
            
            if (vertical)
            {
                for (int j = Cordinate.Y; j <(Cordinate.Y + Length); j++)
                {
                    foreach (Control bufbutton in PanelAddMap.Controls)
                        if (bufbutton is Button)
                        {
                            if ((GetCordinate(bufbutton).Y == j) && (GetCordinate(bufbutton).X == Cordinate.X))
                            {
                                bufbutton.BackColor = Color.Green;
                            }
                        }
                }
            }
            else
            {
                for (int i = Cordinate.X; i < (Cordinate.X + Length); i++)
                    foreach (Control bufbutton in PanelAddMap.Controls)
                        if (bufbutton is Button)
                            if ((GetCordinate(bufbutton).X == i) && (GetCordinate(bufbutton).Y == Cordinate.Y))
                                bufbutton.BackColor = Color.Green; ;
            }

        
        
        }
        public               Point                      GetCordinate(object sender)                                          
        {
            Button bufbuton = (Button)sender;

            return new Point((bufbuton.Location.X) / (bufbuton.Size.Height + offset), (bufbuton.Location.Y) / (bufbuton.Size.Height + offset));
        }
        public               Point                      GetCordinate(Control sender)                                         
        {
            Button bufbuton = (Button)sender;

            return new Point((bufbuton.Location.X) / (bufbuton.Size.Height + offset), (bufbuton.Location.Y) / (bufbuton.Size.Height + offset));
        }      
        public               void                       Add_Click(object sender, EventArgs e)                                
        {
            if (ShipAdd && LstShipLength.SelectedItem!=null)
                client.AddShip(GetCordinate(sender), ChckboxVertical.Checked, int.Parse(LstShipLength.SelectedItem.ToString()));
            LstShipLength.SelectedItem = null;
            ShipAdd = false;
        }
        private              void                       BtnJoin_Click(object sender, EventArgs e)                            
        {
            
            client.roomListbool = true;
            PanelBegin.Visible = false;
            PanelJoin.Visible = true;
            client.JoinRoom();                        
        }
        private              void                       BtnNewGame_Click(object sender, EventArgs e)                         
        {
            
            client.roomListbool = false;
            PanelBegin.Visible = false;
            PanelNewRoom.Visible = true;
            
            
            
           


        }
        private              void                       BtnJoinRoom_Click(object sender, EventArgs e)                        
        {
            client.JoinRoom(LstRomList.GetItemText(LstRomList.SelectedItem));           
            client.roomListbool = false;
            PanelJoin.Visible = false;
            
            

        }
        private              void                       BtnBuildRoom_Click(object sender, EventArgs e)                       
        {
            PanelNewRoom.Visible = false;
            Statusmassage.Text = null;
            client.newRoom(txtRoomName.Text);
           
            
        }        
        private              void                       comboBox1_SelectedIndexChanged(object sender, EventArgs e)           
        {

        }
        private              void                       button1_Click(object sender, EventArgs e)                            
        {
            if(thread!=null)
            thread.Abort();            
            this.Close();
        }
        private              void                       BtnConnect_Click(object sender, EventArgs e)                         
        {
            if (txtbxIp.Text != null)
            {
                
                client = new Client(this,txtbxIp.Text);
                thread = new Thread(new ThreadStart(client.Connection));
                thread.Start();
                thread.IsBackground = true;
                BtnConnect.Visible = false;
                txtbxIp.Visible = false;
            }
        }

       
    
        
    }
}
