using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Key2Mouse
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(Keys vKey);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        
        public Form1()
        {
            InitializeComponent();
        }

        public bool Click = false;
        private const int MOUSELEFT_DOWN = 0x0004;
        private const int MOUSELEFT_UP = 0x0002;
        private const int MOUSE_MOVE = 0x0001;

        private int dx = 0;
        private int dy = 0;
        public double sleep = 0;
        public double sleepDia = 0;

        int mouseDirCount = 0;

        int[] keys = new int[] {0, 0, 0, 0, 0};
        int keyPress = 0;

        //0 = nan, 1 = up, 2 down, ect
        int latched = 0;

        Button[] buttons = new Button[5];

        bool clicked = false;

        private void autoClick()
        {

            buttons[0] = button1;
            buttons[1] = button2;
            buttons[2] = button3;
            buttons[3] = button4;
            buttons[4] = button6;


            while (true)
            {

                if (Click == true)
                {

                    if (GetAsyncKeyState((Keys)keys[0]) != 0)
                    {
                        dy -= 1;
                    }

                    if (GetAsyncKeyState((Keys)keys[1]) != 0)
                    {
                       dy += 1;
                    }

                    if (GetAsyncKeyState((Keys)keys[2]) != 0)
                    {
                       dx -= 1;
                    }

                    if (GetAsyncKeyState((Keys)keys[3]) != 0)
                    {
                       dx += 1;        
                    }

                    if (GetAsyncKeyState((Keys)keys[4]) != 0 && !clicked)
                    {
                        mouse_event(dwFlags: MOUSELEFT_DOWN, dx: 0, dy: 0, cButtons: 0, dwExtraInfo: 0);
                        clicked = true;
                    }

                    if (GetAsyncKeyState((Keys)keys[4]) == 0 && clicked)
                    {
                        mouse_event(dwFlags: MOUSELEFT_UP, dx: 0, dy: 0, cButtons: 0, dwExtraInfo: 0);
                        clicked = false;
                    }

                    if (mouseDirCount > 1)
                    {
                        mouse_event(dwFlags: MOUSE_MOVE, dx: dx * (int)Math.Floor((decimal)sleepDia), dy: dy * (int)Math.Floor((decimal)sleepDia), cButtons: 0, dwExtraInfo: 0);
                    }
                    else
                    {
                        mouse_event(dwFlags: MOUSE_MOVE, dx: dx * (int)Math.Floor((decimal)sleep), dy: dy * (int)Math.Floor((decimal)sleep), cButtons: 0, dwExtraInfo: 0);
                    }

                    //mouse_event(dwFlags: MOUSE_MOVE, dx: dx * (int)Math.Floor((decimal)adderX), dy: dy * (int)Math.Floor((decimal)adderY), cButtons: 0, dwExtraInfo: 0);

                    dx = 0;
                    dy = 0;
                }
                else
                {

                    if (latched != 0)
                    {

                        for (int x = 0; x < 255; x++)
                        {

                            int keyPressValue = GetAsyncKeyState((Keys)x);

                            if (keyPressValue != 0 && x != 1)
                            {
                                Console.WriteLine(x);
                                keyPress = x;

                                keys[latched - 1] = keyPress;
                                buttons[latched - 1].Text = keyPress.ToString();
                                latched = 0;

                                break;

                            }
                        }
                    }
                }

                Thread.Sleep(1);
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Click = !Click;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            Thread AC = new Thread(autoClick);
            AC.Start();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            sleep = Convert.ToInt32(Math.Round(((NumericUpDown) sender).Value, 0)) / 4;
            sleepDia = sleep * 0.7;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            latched = 1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            latched = 2;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            latched = 3;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            latched = 4;
        }


        private void button6_Click(object sender, EventArgs e)
        {
            latched = 5;
        }


        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
