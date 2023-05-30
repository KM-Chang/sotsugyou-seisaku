using System;
using System.Drawing;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;
using System.Threading;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.InteropServices;

namespace 假虛機
{
    public partial class 開始 : Form
    {
        private const int APPCOMMAND_VOLUME_MUTE = 0x80000;
        private const int APPCOMMAND_VOLUME_UP = 0x0a0000;
        private const int APPCOMMAND_VOLUME_DOWN = 0x090000;
        private const int WM_APPCOMMAND = 0x319;

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageW(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
        public string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
        int iCount = 0;
        public 開始()
        {
            InitializeComponent();
            //危險圖
            //危險圖
          
            pictureBox5.Parent = webCamPictureBox1  ;
            pictureBox5.Location = new Point(33, 180);
            //yes
            pictureBox7.Parent = pictureBox6;
            pictureBox7.Location = new Point(20, 80);
            //no
            pictureBox9.Parent = pictureBox6;
            pictureBox9.Location = new Point(90, 80);

            //是否為樣本
            pictureBox6.Parent =  pictureBox16 ;
            pictureBox6.Location = new Point(42, 50);
            //設定完畢
            pictureBox10.Parent =  pictureBox16 ;
            pictureBox10.Location = new Point(42, 50);
            //樣本拍攝無效
            pictureBox12.Parent =  pictureBox16 ;
            pictureBox12.Location = new Point(42, 50);

            //ok_2
            pictureBox13.Parent = pictureBox12;
            pictureBox13.Location = new Point(50, 80);
            //ok_1
            pictureBox11.Parent = pictureBox10;
            pictureBox11.Location = new Point(50, 80);

            //錯誤訊息
            pictureBox14.Parent =  pictureBox16 ;
            pictureBox14.Location = new Point(42, 50);

            //ok_3
            pictureBox15.Parent = pictureBox14;
            pictureBox15.Location = new Point(50, 80);

            //小閃燈
            pictureBox17.Parent = webCamPictureBox1;
            pictureBox17.Location = new Point(10, 10);
        }

        private void 開始_Load(object sender, EventArgs e)
        {
            if (File.Exists("takming.txt") == true)
            {
                Process p = new Process();
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.FileName = "opencv.exe";
                p.Start();
                //File.Delete("takming.txt");
                webCamPictureBox1.Image = null;
                for (int i = 0; i <= 20; i++)
                    SendMessageW(this.Handle, WM_APPCOMMAND, this.Handle, (IntPtr)APPCOMMAND_VOLUME_DOWN);

                for (int i = 0; i < 9; i++)
                    SendMessageW(this.Handle, WM_APPCOMMAND, this.Handle, (IntPtr)APPCOMMAND_VOLUME_UP);

                webCamPictureBox1.CaptureInterval = 200;
                webCamPictureBox1.Start();
            }
            else
            {
                pictureBox2.Visible = false;
                
                pictureBox16.Visible = true;
                webCamPictureBox1.Visible = false;
                pictureBox14.Visible = true;
                pictureBox15.Visible = true;
            }
        }
        WebClient client = new WebClient();
        int ftpimg = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (iCount > 7)
           {
                this.webCamPictureBox1.Image.Save("picture\\" + (iCount - 8) + ".jpg");
                Bitmap imgoutput = new Bitmap(webCamPictureBox1.Image, 90,120 ); //輸出一個新圖片
                imgoutput.Save("pictures\\" + (iCount - 8) + ".jpg");
                ////刪除
                System.IO.File.Delete("pictures\\" + (iCount - 8) + ".jpg");

                double  statuss ;
                int stat;
                try
                {
                    
                    SqlConnection conn = new SqlConnection("Server=127.0.0.1;initial catalog=openeyes;User ID=sa;Password=open16CV;");

                  SqlCommand cmd = new SqlCommand();
                    conn.Open();
                    
                    cmd.CommandText = "select  危險指數 from 行車紀錄 where 員工編號= '" + Class1.userid + "' and 日期 =(select max(日期)  from 行車紀錄 where 員工編號= '" + Class1.userid + "' )";
                    cmd.Connection = conn;
                    SqlDataReader dr = cmd.ExecuteReader();
                    dr.Read();
                    statuss = double.Parse (dr.GetValue(0).ToString());
                    stat = Convert.ToInt16 (Math.Floor(Math.Abs(statuss)));
                    if (stat >= 1)
                    {
                        pictureBox5.Visible = true;
                        playerss = true;
                        if (stat != samples) 
                        {
                            SendMessageW(this.Handle, WM_APPCOMMAND, this.Handle, (IntPtr)APPCOMMAND_VOLUME_UP);
                            SendMessageW(this.Handle, WM_APPCOMMAND, this.Handle, (IntPtr)APPCOMMAND_VOLUME_UP);
                            SendMessageW(this.Handle, WM_APPCOMMAND, this.Handle, (IntPtr)APPCOMMAND_VOLUME_UP);
                            samples = stat;
                            //123456789
                            col = true;
                        }
                   }
                    else
                    {
                     
                        if (stat != samples)
                        {
                            for (int i = 0; i <= 20; i++)
                                SendMessageW(this.Handle, WM_APPCOMMAND, this.Handle, (IntPtr)APPCOMMAND_VOLUME_DOWN);

                            for (int i = 0; i < 9; i++)
                                SendMessageW(this.Handle, WM_APPCOMMAND, this.Handle, (IntPtr)APPCOMMAND_VOLUME_UP);
                            samples = stat;
                            //123456789
                            col = false;
                        }
                        pictureBox5.Visible = false;
                        playerss = false;
                    }
 
                    dr.Dispose();
                    cmd.Dispose();
                    //conn.Dispose();              
                    ftpimg++;
                }
                catch
                {

                }
            }
            if (iCount ==18) 
                iCount = 8; 
            else
                iCount++;
            if (ftpimg == 59) { ftpimg = 0; }
        }
        int samples=0;
        Boolean col=false ;
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.webCamPictureBox1.Image.Save("picture\\sample.jpg");
            Process p = new Process();
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.FileName = "helloval.exe";
            p.Start();
            Thread.Sleep(1200);
            try
            {
                this.pictureBox16.Load("picture\\sample.jpg");
                pictureBox16.Visible  = true;
                webCamPictureBox1.Visible = false;
                pictureBox6.Visible = true;
                pictureBox7.Visible = true;
                pictureBox9.Visible = true;

        }
            catch
            {
                this.pictureBox16.Load("picture\\nogood.jpg");
                webCamPictureBox1.Visible = false; 
                pictureBox16.Visible = true;    
                pictureBox12.Visible = true;
                pictureBox13.Visible = true;
            }
            finally { p.Dispose(); }
        
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {   
            Process p = new Process();
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.FileName = "cl.bat";
            p.Start();

            webCamPictureBox1.Stop();

            p.Dispose();
            deletes();
            System.Environment.Exit(System.Environment.ExitCode);
        }
        public void deletes() 
        {
            for (int j = 0; j <= 49; j ++  )
            {
                try
                {
                    File.Delete("picture\\" + j + ".jpg");
                }
                catch { }
            }

            try
            {
                File.Delete("picture\\aa.jpg");
            }
            catch { }

            try
            {
                File.Delete("picture\\bb.jpg");
            }
            catch { }

            try
            {
                File.Delete("picture\\sample.jpg");
            }
            catch { }


            
            try
            {
                File.Delete("picture\\sample1.jpg");
            }
            catch { }

        }
  

        Boolean playerss = false;
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (playerss)
            {
                System.Media.SoundPlayer player = new System.Media.SoundPlayer();
                player.SoundLocation = path + "\\music\\" + Class1. musicnum + ".wav";
                player.Play();//撥一次
               
                //   player.Dispose();
            }
        }
       
        //yes
        
        private void pictureBox7_Click(object sender, EventArgs e)
        {
            
            pictureBox13.Visible = true;
            pictureBox6.Visible = false;
            pictureBox7.Visible = false;
            pictureBox9.Visible = false;
            pictureBox10.Visible = true;
           
            pictureBox11.Visible = true;

        }
        //no
        
        private void pictureBox9_Click(object sender, EventArgs e)
        {
            
            pictureBox16.Visible = false;
            webCamPictureBox1.Visible = true;
      
            pictureBox6.Visible = false;
            pictureBox7.Visible = false;
            pictureBox9.Visible = false;
        }

       //ok
        
        private void pictureBox11_Click(object sender, EventArgs e)
        {
           
            pictureBox10.Visible = false;
            pictureBox11.Visible = false;
            pictureBox16.Visible = false;
            webCamPictureBox1.Visible = true;
            pictureBox2.Visible = false;
        
            pictureBox17.Visible = true;
            Process p = new Process();
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.FileName = "openyoureye.exe";
            p.StartInfo.Arguments = Class1.userid;
            p.Start();

            timer1.Enabled = true;
            pictureBox2.Visible = false;
           
        }

        
        private void pictureBox13_Click(object sender, EventArgs e)
        {
            pictureBox16.Visible = false;
            webCamPictureBox1.Visible = true;
            pictureBox12.Visible = false;
            pictureBox13.Visible = false;
            
        }

        private void pictureBox15_Click(object sender, EventArgs e)
        {
            pictureBox14.Visible = false;
            pictureBox15.Visible = false;
        }
        Boolean firsts=false ;
        private void timer3_Tick(object sender, EventArgs e)
        {
            if (!col)
            {
                if (firsts)
                {
                    pictureBox17.Load("pictures//g.png");
                    firsts = false;
                }
                else 
                {
                    pictureBox17.Load("pictures//g1.png");
                    firsts = true;
                }
            }
            else 
            {
                if (firsts)
                {
                    pictureBox17.Load("pictures//r.png");
                    firsts = false;
                }
                else
                {
                    pictureBox17.Load("pictures//r1.png");
                    firsts = true;
                }
            }
           

        }
    }
}