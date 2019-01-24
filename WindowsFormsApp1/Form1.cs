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
            string strCmdText1;
            string strCmdText2;
            InputSimulator Simulator = new InputSimulator();
            strCmdText1 = "Schtasks /Change /TN alarm_1.1 /ST 06:11:00";
            strCmdText2 = "Schtasks /Change /TN alarm_1.2 /ST 06:11:00";
            Process p = new Process();
            p.StartInfo.FileName = "powershell.exe";
            p.StartInfo.Arguments = strCmdText1;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = false;
            p.StartInfo.Verb = "runas";
            p.Start();
            Simulator.Mouse.MoveMouseTo(30000, 35000);
            Simulator.Mouse.Sleep(20);
            Simulator.Mouse.LeftButtonClick();
            Simulator.Mouse.Sleep(20);
            Simulator.Keyboard.TextEntry("193203");
            Simulator.Mouse.Sleep(20);
            Simulator.Keyboard.KeyPress(VirtualKeyCode.RETURN);
            Simulator.Mouse.Sleep(20);
            p.StartInfo.FileName = "powershell.exe";
            p.StartInfo.Arguments = strCmdText2;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = false;
            p.StartInfo.Verb = "runas";
            p.Start();
            Simulator.Mouse.Sleep(20);
            Simulator.Mouse.LeftButtonClick();
            Simulator.Mouse.Sleep(20);
            Simulator.Keyboard.TextEntry("193203");
            Simulator.Mouse.Sleep(20);
            Simulator.Keyboard.KeyPress(VirtualKeyCode.RETURN);
            Simulator.Mouse.Sleep(20);
        }
    }
}
