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
using System.Management.Automation;
using System.Collections.ObjectModel;
using System.IO;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public decimal numericUpDown1_init, numericUpDown2_init, numericUpDown3_init, numericUpDown4_init, numericUpDown5_init, numericUpDown6_init;
        public Color OriginalBackground;

        public Form1()
        {
            InitializeComponent();
            label1.MouseEnter += label_MouseEnter;
            label1.MouseLeave += label_MouseLeave;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //decimal numericUpDown1_init, numericUpDown2_init, numericUpDown3_init, numericUpDown4_init, numericUpDown5_init, numericUpDown6_init;
            PowerShell PowerShellInstance = PowerShell.Create();
            PowerShellInstance.AddScript(@"Schtasks /Query /NH /TN alarm_1.1 | Out-String");
            PowerShellInstance.Invoke();
            Collection<PSObject> pSObjects = PowerShellInstance.Invoke();
            foreach (PSObject p in pSObjects)
            {
                StreamWriter sw = new StreamWriter(@"C:\test.txt");
                sw.WriteLine(p.ToString());
                sw.Close();
            }
            var lines = System.IO.File.ReadAllLines(@"C:\test.txt");
            string[] words = lines[2].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            numericUpDown1.Value = numericUpDown1_init = Convert.ToDecimal(words[2].Substring(0, 1));
            numericUpDown2.Value = numericUpDown2_init = Convert.ToDecimal(words[2].Substring(2, 2));
            numericUpDown3.Value = numericUpDown3_init = Convert.ToDecimal(words[2].Substring(5));

            PowerShellInstance.AddScript(@"Schtasks /Query /NH /TN alarm_1.2 | Out-String");
            PowerShellInstance.Invoke();
            pSObjects = PowerShellInstance.Invoke();
            foreach (PSObject p in pSObjects)
            {
                StreamWriter sw = new StreamWriter(@"C:\test.txt");
                sw.WriteLine(p.ToString());
                sw.Close();
            }
            lines = System.IO.File.ReadAllLines(@"C:\test.txt");
            words = lines[2].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            numericUpDown4.Value = numericUpDown4_init = Convert.ToDecimal(words[2].Substring(0, 1));
            numericUpDown5.Value = numericUpDown5_init = Convert.ToDecimal(words[2].Substring(2, 2));
            numericUpDown6.Value = numericUpDown6_init = Convert.ToDecimal(words[2].Substring(5));
            File.Delete(@"C:\test.txt");
        }

        private void label_MouseEnter(object sender, EventArgs e)
        {
            OriginalBackground = ((Label)sender).ForeColor;
            ((Label)sender).ForeColor = Color.Red;
        }

        private void label_MouseLeave(object sender, EventArgs e)
        {
            ((Label)sender).ForeColor = OriginalBackground;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ApplyChanges();
        }

        private void ApplyChanges()
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

            /*PowerShell PowerShellInstance = PowerShell.Create();
            PowerShellInstance.AddScript(@"Invoke-Command -ComputerName s4sh-desk -ScriptBlock {Schtasks /Change /TN alarm_1.1 /ST 00:00:00} -Credential s4sh-desk\s4sh | Out-String");
            PowerShellInstance.Invoke();*/
            Process p = new Process();
            p.StartInfo.FileName = "powershell.exe";
            p.StartInfo.Arguments = pwrshllin1;
            p.StartInfo.RedirectStandardOutput = false;
            p.StartInfo.Verb = "runas";
            p.Start();
            InputSimulator Simulator = new InputSimulator();
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

        private void label1_Click_1(object sender, EventArgs e)
        {
            if (Convert.ToInt32(numericUpDown1.Value) != numericUpDown1_init || Convert.ToInt32(numericUpDown2.Value) != numericUpDown2_init || Convert.ToInt32(numericUpDown3.Value) != numericUpDown3_init || Convert.ToInt32(numericUpDown4.Value) != numericUpDown4_init || Convert.ToInt32(numericUpDown5.Value) != numericUpDown5_init || Convert.ToInt32(numericUpDown6.Value) != numericUpDown6_init)
            {
                DialogResult result = MessageBox.Show("Были внесены изменения, произвести сохранение?", "Подтвердите действие", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    ApplyChanges();
                    this.Close();
                }
                else if (result == DialogResult.No)
                {
                    this.Close();
                }
                else if (result == DialogResult.Cancel)
                {

                }
            }
            else
                this.Close();
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

        const int WM_NCHITTEST = 0x84;
        const int HTCAPTION = 2;
        const int HTCLIENT = 1;

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_NCHITTEST && (int)m.Result == HTCLIENT)
                m.Result = (IntPtr)HTCAPTION;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
