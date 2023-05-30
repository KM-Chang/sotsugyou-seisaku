using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
namespace 假虛機
{
    public partial class 登入畫面 : Form
    {
        public 登入畫面()
        {
            InitializeComponent();
        }

       

        private void pictureBox5_Click(object sender, EventArgs e)
        {
           Class1.userid = textBox1.Text;

            Class1.x = this.Location.X;
            Class1.y = this.Location.Y;
            Form1 aa = new Form1();
            aa.StartPosition = FormStartPosition.Manual;
            aa.Location = new Point(Class1.x, Class1.y);
            aa.Show();
            this.Hide();
        }

        private void 登入畫面_Load(object sender, EventArgs e)
        {
          
        }
    }
}
