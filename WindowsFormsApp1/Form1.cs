using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using WindowsInput.Native;
using WindowsInput;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string pwrshllin1, pwrshllin2;
            int[] updownarr = { Convert.ToInt32(numericUpDown1.Value), Convert.ToInt32(numericUpDown2.Value), Convert.ToInt32(numericUpDown3.Value), Convert.ToInt32(numericUpDown4.Value), Convert.ToInt32(numericUpDown5.Value), Convert.ToInt32(numericUpDown6.Value) };
            string[] timearr = new string[6];

            for (int i = 0; i < 6; i++)
            {
                if ((updownarr[i] >= 0) && (updownarr[i] < 10))
                    timearr[i] = "0";
                else
                    timearr[i] = "";
            }

            pwrshllin1 = "Schtasks /Change /TN alarm_1.1 /ST " + timearr[0] + numericUpDown1.Value + ":" + timearr[1] + numericUpDown2.Value + ":" + timearr[2] + numericUpDown3.Value;
            pwrshllin2 = "Schtasks /Change /TN alarm_1.2 /ST " + timearr[3] + numericUpDown4.Value + ":" + timearr[4] + numericUpDown5.Value + ":" + timearr[5] + numericUpDown6.Value;

            Process p = new Process();
            InputSimulator Simulator = new InputSimulator();
            p.StartInfo.FileName = "powershell.exe";
            p.StartInfo.Arguments = pwrshllin1;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = false;
            p.StartInfo.Verb = "runas";
            p.Start();
            Simulator.Mouse.MoveMouseTo(30000, 35000);
            Simulator.Mouse.Sleep(30);
            Simulator.Mouse.LeftButtonClick();
            Simulator.Mouse.Sleep(30);
            Simulator.Keyboard.TextEntry("193203");
            Simulator.Mouse.Sleep(30);
            Simulator.Keyboard.KeyPress(VirtualKeyCode.RETURN);
            Simulator.Mouse.Sleep(30);

            p.StartInfo.FileName = "powershell.exe";
            p.StartInfo.Arguments = pwrshllin2;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = false;
            p.StartInfo.Verb = "runas";
            p.Start();
            Simulator.Mouse.Sleep(30);
            Simulator.Mouse.LeftButtonClick();
            Simulator.Mouse.Sleep(30);
            Simulator.Keyboard.TextEntry("193203");
            Simulator.Mouse.Sleep(30);
            Simulator.Keyboard.KeyPress(VirtualKeyCode.RETURN);
            Simulator.Mouse.Sleep(30);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown2_ValueChanged_1(object sender, EventArgs e)
        {

        }

        private void numericUpDown3_ValueChanged_1(object sender, EventArgs e)
        {

        }
    }
}
