using System;
using System.Drawing;
using System.Windows.Forms;
namespace 假虛機
{
    public partial class 設定 : Form
    {
        public string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
        public 設定()
        {
            InitializeComponent();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            player.Stop();
            timer1.Enabled = false;
            Class1.x = this.Location.X;
            Class1.y = this.Location.Y;
            Form1 aa = new Form1();
            aa.StartPosition = FormStartPosition.Manual;
            aa.Location = new Point(Class1.x, Class1.y);
            aa.Show();
            this.Hide();
        }

        private void 設定_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;    
        }
         
        int x = 0;
        System.Media.SoundPlayer player = new System.Media.SoundPlayer();
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            int y = MousePosition.Y-this .Location .Y-35 ;
            if ((165 <= y) && (y <= 218))
            {
                x = 165;
                movebutton = true;
                player.SoundLocation = path +"\\music\\1.wav";
                Class1.musicnum = 1;
            }
            else if ((219 <= y) && (y <= 272))
            {
                x = 219;
                movebutton = true;
                player.SoundLocation = path + "\\music\\2.wav";
                Class1.musicnum = 2;
            }
            else if ((273 <= y) && (y <= 326))
            {
                x = 273;
                movebutton = true;
                player.SoundLocation = path + "\\music\\3.wav";
                Class1.musicnum = 3;
            }
            else if ((327 <= y) && (y <= 380))
            {
                x = 327;
                movebutton = true;
                player.SoundLocation = path + "\\music\\4.wav";
                Class1.musicnum = 4;
            }
            else if ((381 <= y) && (y <= 434))
            {
                x = 381;
                movebutton = true;
                player.SoundLocation = path + "\\music\\5.wav";
                Class1.musicnum =5 ;
            }
            else 
            {
                movebutton = false ;         
            }
            if((y<=434)&&(y>=165))
                player.Play();//撥一次
             
         }
        bool movebutton= false ;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (movebutton ==true )
            {
                if (x < pictureBox2.Location.Y)
                {
                    pictureBox2.Location = new Point(pictureBox2.Location.X, pictureBox2.Location.Y - 6);
                }
                else if (x> pictureBox2 .Location .Y )
                {
                    pictureBox2.Location = new Point(pictureBox2.Location.X, pictureBox2.Location.Y + 6);
                }
             if (pictureBox2.Location .Y == x)
             {
                 movebutton = false;
             }
            }
        }
    }
}
