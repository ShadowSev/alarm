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
            //string strCmdText2;
            InputSimulator Simulator = new InputSimulator();
            strCmdText1 = "Schtasks /Change /TN alarm_1.1 /ST 06:11:00";
            //strCmdText2 = "Schtasks /Change /TN alarm_1.2 /ST 06:10:00";
            Process.Start("powershell.exe", strCmdText1);
            //Process.Start("powershell.exe", strCmdText2);
        }
    }
}
