using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using BattleShipServer;
using System.Net;
using System.Net.Sockets;

namespace SerVer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private Server server = null;
        private Thread thread = null;
        private Button Resume = null;
        private void Form1_Load(object sender, EventArgs e)
        {
                   
            IPHostEntry Host = Dns.GetHostEntry(Dns.GetHostName());
            lblIp.Text = Host.AddressList.FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork).ToString();
            Resume=new Button()
            {
                Parent=this,
                Visible=false,
                Location=BtnStart.Location,
                Enabled=false,
                Text="Resume",
            };
            Resume.Click += Resume_Click;
 
        }

        void Resume_Click(object sender, EventArgs e)
        {
            Resume.Enabled = false;
            button2.Enabled = true;
            thread.Resume();
        }
        

        private void BtnStart_Click(object sender, EventArgs e)
        {
            server = new Server();
            thread = new Thread(new ThreadStart(server.ServerStart));
            thread.Start();
            thread.IsBackground = true;
            BtnStart.Visible = false;
            Resume.Visible = true;
        }

       

        private void button2_Click(object sender, EventArgs e)
        {
            thread.Suspend();
            Resume.Enabled = true;
            button2.Enabled = false;
            
        }
    }
}
