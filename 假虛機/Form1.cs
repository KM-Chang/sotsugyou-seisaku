using System;
using System.Drawing;
using System.Windows.Forms;

namespace 假虛機
{
    public partial class Form1 : Form
    {
        int a=0;
        public Form1()
        {
            if (Class1.x != 0 && Class1 .y != 0)
            {
                this.Location = new Point(Class1.x, Class1.y);
            }
            InitializeComponent();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            a++;
            if (a == 3)
                a = 0;
            switch (a)
            {
                case 0:
                    pictureBox4.Visible = true;
                    pictureBox5.Visible = false;
                    pictureBox6.Visible = false;

                    break;
                case 1:
                    pictureBox4.Visible = false;
                    pictureBox5.Visible = true;
                    pictureBox6.Visible = false;

                    break;
                case 2:
                    pictureBox4.Visible = false;
                    pictureBox5.Visible = false;
                    pictureBox6.Visible = true;

                    break;
                case 3:
                    pictureBox4.Visible = false;
                    pictureBox5.Visible = false;
                    pictureBox6.Visible = false;

                    break;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            a--;
            if (a == -1)
                a = 2;
            switch (a)
            {
                case 0:
                    pictureBox4.Visible = true;
                    pictureBox5.Visible = false;
                    pictureBox6.Visible = false;

                    break;
                case 1:
                    pictureBox4.Visible = false;
                    pictureBox5.Visible = true;
                    pictureBox6.Visible = false;

                    break;
                case 2:
                    pictureBox4.Visible = false;
                    pictureBox5.Visible = false;
                    pictureBox6.Visible = true;

                    break;
                case 3:
                    pictureBox4.Visible = false;
                    pictureBox5.Visible = false;
                    pictureBox6.Visible = false;

                    break;
            }

        }

      

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            Class1.x = this.Location.X;
            Class1.y = this.Location.Y ;
           
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            if ((Class1.x == 0) && (Class1.y == 0)) 
            {
                Class1.x = this.Location.X;
                Class1.y = this.Location.Y;
            }
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Class1.x = this.Location.X;
            Class1.y = this.Location.Y;
          
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Class1.x = this.Location.X;
            Class1.y = this.Location.Y;
           
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(System.Environment.ExitCode);
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            if (pictureBox4.Visible == true)
            {
                開始 aa = new 開始();
                aa.StartPosition = FormStartPosition.Manual;
                aa.Location = new Point(Class1.x, Class1.y);
                aa.Show();
                this.Hide();
            }
           if(pictureBox5.Visible == true)
           {
               設定 aa = new 設定();
               aa.StartPosition = FormStartPosition.Manual;
               aa.Location = new Point(Class1.x, Class1.y);
               aa.Show();
               this.Hide();
           }

           if (pictureBox6.Visible == true)
           {
               說明 aa = new 說明();
               aa.StartPosition = FormStartPosition.Manual;
               aa.Location = new Point(Class1.x, Class1.y);
               aa.Show();
               this.Hide();
           }
        }
    }
}
