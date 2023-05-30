using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
namespace 假虛機
{
    public partial class 說明 : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
        private enum MouseMessages
        {
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x0202,
            WM_MOUSEMOVE = 0x0200,
            WM_MOUSEWHEEL = 0x020A,
            WM_RBUTTONDOWN = 0x0204,
            WM_RBUTTONUP = 0x0205
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        private const int WH_MOUSE_LL = 14;
        private static LowLevelMouseProc _proc = new LowLevelMouseProc(HookCallback);
        private static IntPtr _hookID = IntPtr.Zero;
        private static int xPos, yPos;
        
        //---------------------------------------------------------------------------
        //MOUSE HOOK 相關宣告結束
        //---------------------------------------------------------------------------


        private static int PrevX, PrevY;
        private static bool press_flag;

        private static bool turn_up;
        private static bool turn_down;

        private static bool move_up;
        private static bool move_down;



        private static IntPtr SetHook(LowLevelMouseProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_MOUSE_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        //---------------------------------------------------------------------------
        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);
        private static IntPtr HookCallback(
            int nCode, IntPtr wParam, IntPtr lParam)
        {

            MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
            xPos = hookStruct.pt.x;
            yPos = hookStruct.pt.y;


            if (nCode >= 0)
            {
                switch ((MouseMessages)wParam.ToInt32())
                {
                    case MouseMessages.WM_LBUTTONDOWN:

                        press_flag = true;
                        PrevX = xPos;
                        PrevY = yPos;
                        break;

                    case MouseMessages.WM_LBUTTONUP:

                        press_flag = false;
                        turn_up = false;
                        turn_down = false;
                        break;

                    case MouseMessages.WM_MOUSEMOVE:

                        if (press_flag)
                        {
                            turn_up = (yPos < PrevY);
                            turn_down = (yPos > PrevY);
                        }
                        PrevX = xPos;
                        PrevY = yPos;
                        break;
                }
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        public 說明()
        {

            InitializeComponent();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Class1.x = this.Location.X;
            Class1.y = this.Location.Y;
            Form1  aa = new Form1();
            aa.StartPosition = FormStartPosition.Manual;
            aa.Location = new Point(Class1.x, Class1.y);
            aa.Show();
            this.Hide();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (turn_up)
            {
                move_up = true;
                move_down = false;  //預防兩造同時 true
            }

            if (turn_down)
            {
                if (pictureBox1.Location.Y != 0)
                {
                    move_down = true;
                }
                move_up = false; //預防兩造同時 true
            }
            turn_up = false;
            turn_down = false;        
        }
        
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (move_up)
            {
                if (pictureBox3.Location.Y != 20) 
                {
                    pictureBox1.Location = new Point(pictureBox1.Location.X, pictureBox1.Location.Y - 20);
                    pictureBox3.Location = new Point(pictureBox3.Location.X, pictureBox3.Location.Y - 20);
                    if (pictureBox3.Location.Y == 20)
                    {
                        move_up = false;
                    }
                }
            }

            if (move_down)
            {
                if (pictureBox1.Location.Y !=45)
                {
                    pictureBox1.Location = new Point(pictureBox1.Location.X, pictureBox1.Location.Y + 20);
                    pictureBox3.Location = new Point(pictureBox3.Location.X, pictureBox3.Location.Y + 20);
                    if (pictureBox1.Location.Y == 45)
                    {
                        move_down = false;
                    }
                }
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            UnhookWindowsHookEx(_hookID);
            timer2.Enabled = false;
            timer1.Enabled = false;
            Class1.x = this.Location.X;
            Class1.y = this.Location.Y;
            Form1 aa = new Form1();
            aa.StartPosition = FormStartPosition.Manual;
            aa.Location = new Point(Class1.x, Class1.y);
            aa.Show();
            this.Hide();
        }

        private void 說明_Load(object sender, EventArgs e)
        {
            _hookID = SetHook(_proc);
            timer1.Enabled = true;
            timer2.Enabled = true;
        }
    }
}
