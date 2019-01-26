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
        private Point mouseOffset;
        private bool isMouseDown = false;

        public Form1()
        {
            InitializeComponent();
            close_window.MouseEnter += label_MouseEnter;
            close_window.MouseLeave += label_MouseLeave;
            background.MouseDown += Form1_MouseDown;
            background.MouseMove += Form1_MouseMove;
            background.MouseUp += Form1_MouseUp;
            //trayicon.Visible = false;
            this.trayicon.MouseClick += new MouseEventHandler(trayicon_MouseClick);
            this.Resize += new System.EventHandler(this.Form1_Resize);
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            int xOffset;
            int yOffset;

            if (e.Button == MouseButtons.Left)
            {
                xOffset = e.X;
                yOffset = e.Y;
                mouseOffset = new Point(xOffset, yOffset);
                isMouseDown = true;
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                Point currentScreenPos = PointToScreen(e.Location);
                Location = new Point(currentScreenPos.X - mouseOffset.X, currentScreenPos.Y - mouseOffset.Y);
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            // Changes the isMouseDown field so that the form does
            // not move unless the user is pressing the left mouse button.
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
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

        private void apply_button_Click(object sender, EventArgs e)
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

            numericUpDown1_init = numericUpDown1.Value;
            numericUpDown2_init = numericUpDown2.Value;
            numericUpDown3_init = numericUpDown3.Value;
            numericUpDown4_init = numericUpDown4.Value;
            numericUpDown5_init = numericUpDown5.Value;
            numericUpDown6_init = numericUpDown6.Value;
        }

        private void close_window_Click(object sender, EventArgs e)
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

        private void minimized_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;
                trayicon.Visible = true;
            }
        }

        private void trayicon_MouseClick(object sender, MouseEventArgs e)
        {
            trayicon.Visible = false;
            this.ShowInTaskbar = true;
            WindowState = FormWindowState.Normal;
        }
    }
}
