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
        public decimal numericUpDown1_init, numericUpDown2_init, numericUpDown3_init;
        public Color OriginalBackground;
        public int UpDownSymb2, UpDownSymb3, UpDownSymb5;
        public string timeString;
        public string tempPath = System.IO.Path.GetTempPath();
        public bool afterTray = false;
        private Point mouseOffset;
        private bool isMouseDown = false;

        //Create form
        public Form1()
        {
            InitializeComponent();
            close_window.MouseEnter += label_MouseEnter1;
            close_window.MouseLeave += label_MouseLeave;
            minimized.MouseEnter += label_MouseEnter2;
            minimized.MouseLeave += label_MouseLeave;
            background.MouseDown += Form1_MouseDown;
            background.MouseMove += Form1_MouseMove;
            background.MouseUp += Form1_MouseUp;
            this.trayicon.MouseClick += new MouseEventHandler(trayicon_MouseClick);
            this.Resize += new System.EventHandler(this.Form1_Resize);
        }

        //Load form
        private void Form1_Load(object sender, EventArgs e)
        {
            schtasks_check();
        }

        //Checking tasks for existence
        private void schtasks_check()
        {
            Directory.CreateDirectory(tempPath + @"alarm");
            PowerShell PowerShellInstance = PowerShell.Create();
            PowerShellInstance.AddScript(@"Schtasks /Query /NH /TN alarm_1 > $env:temp\alarm\schtasksList.txt");
            PowerShellInstance.Invoke();
            if (new FileInfo(tempPath + @"alarm\schtasksList.txt").Length == 0)
            {
                File.Delete(tempPath + @"alarm\schtasksList.txt");
                Directory.Delete(tempPath + @"alarm");
                schtasks_create();
            }
            else
            {
                File.Delete(tempPath + @"alarm\schtasksList.txt");
                Directory.Delete(tempPath + @"alarm");
                schtasks_read();
            }
        }

        //Creating tasks
        private void schtasks_create()
        {
            PowerShell PowerShellInstance = PowerShell.Create();
            PowerShellInstance.AddScript(@"Schtasks /Create /SC daily /TN alarm_1 /ST 08:00:00 /TR C:\yandexradio.url");
            PowerShellInstance.AddScript(@"Schtasks /Create /SC daily /TN alarm_2 /ST 08:00:00 /TR ""C:\Users\a.podchasov\source\repos\Alarm\ConsoleApp1\bin\Debug\MouseMove.exe""");
            PowerShellInstance.AddScript(@"Schtasks /Create /SC daily /TN alarm_3 /ST 08:00:00 /TR ""C:\Users\a.podchasov\source\repos\Alarm\ConsoleApp1\bin\Debug\VolumeSetup.exe""");
            PowerShellInstance.Invoke();
        }

        //Call task --> writing task parameters to file --> redefinition task parameters --> writing parameters to variables --> deleting file
        private void schtasks_read()
        {
            PowerShell PowerShellInstance = PowerShell.Create();
            PowerShellInstance.AddScript(@"Schtasks /Query /NH /TN alarm_1 | Out-String");
            PowerShellInstance.Invoke();
            Collection<PSObject> pSObjects = PowerShellInstance.Invoke();
            foreach (PSObject p in pSObjects)
            {
                StreamWriter sw = new StreamWriter(tempPath + @"schtasksParam.txt");
                sw.WriteLine(p.ToString());
                sw.Close();
            }
            schtasks_fileread();
            numericUpDown1.Value = numericUpDown1_init = Convert.ToDecimal(timeString.Substring(0, UpDownSymb2));
            numericUpDown2.Value = numericUpDown2_init = Convert.ToDecimal(timeString.Substring(UpDownSymb3, 2));
            numericUpDown3.Value = numericUpDown3_init = Convert.ToDecimal(timeString.Substring(UpDownSymb5));
            File.Delete(tempPath + @"schtasksParam.txt");
        }

        //Reading schtasks parameters from file
        private void schtasks_fileread()
        {
            var lines = System.IO.File.ReadAllLines(tempPath + @"schtasksParam.txt");
            string[] words = lines[2].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            timeString = words[2];
            if ((timeString.Substring(1, 1)) != ":")
            {
                UpDownSymb2 = 2;
                UpDownSymb3 = 3;
                UpDownSymb5 = 6;
            }
            else
            {
                UpDownSymb2 = 1;
                UpDownSymb3 = 2;
                UpDownSymb5 = 5;
            }
        }
        
        //Coloring buttons
        private void label_MouseEnter1(object sender, EventArgs e)
        {
            OriginalBackground = ((Label)sender).ForeColor;
            ((Label)sender).ForeColor = Color.Red;
        }

        //Coloring buttons
        private void label_MouseEnter2(object sender, EventArgs e)
        {
            OriginalBackground = ((Label)sender).ForeColor;
            ((Label)sender).ForeColor = Color.FromArgb(255, 117, 132, 148);
        }

        //Coloring buttons
        private void label_MouseLeave(object sender, EventArgs e)
        {
                ((Label)sender).ForeColor = OriginalBackground;
        }

        //"Apply" button
        private void apply_button_Click(object sender, EventArgs e)
        {
            ApplyChanges("alarm_1");
            ApplyChanges("alarm_2");
            //ApplyChanges("alarm_3");
        }

        //Traking if mouse is down on form
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

        //Form moving with down mouse
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                Point currentScreenPos = PointToScreen(e.Location);
                Location = new Point(currentScreenPos.X - mouseOffset.X, currentScreenPos.Y - mouseOffset.Y);
            }
        }

        //Traking if mouse is up on form
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = false;
            }
        }

        private void background_Paint(object sender, PaintEventArgs e)
        {

        }

        //Saving edited schtasks parameters
        private void ApplyChanges(object sender)
        {
            int[] updownarr = { Convert.ToInt32(numericUpDown1.Value), Convert.ToInt32(numericUpDown2.Value), Convert.ToInt32(numericUpDown3.Value) };
            string[] timearr = new string[3];

            for (int i = 0; i < 3; i++)
            {
                if ((updownarr[i] >= 0) && (updownarr[i] < 10))
                    timearr[i] = "0";
                else
                    timearr[i] = "";
            }
            PowerShell PowerShellInstance = PowerShell.Create();
            PowerShellInstance.AddScript(@"Schtasks /Delete /TN " + sender + " /F");
            PowerShellInstance.AddScript(@"Schtasks /Create /SC daily /TN " + sender + " /ST " + timearr[0] + numericUpDown1.Value + ":" + timearr[1] + numericUpDown2.Value + ":" + timearr[2] + numericUpDown3.Value + " /TR C:\\yandexradio.url");
            PowerShellInstance.Invoke();
            //PowerShellInstance.AddScript(@"Schtasks /Change /TN " + sender + " /ST " + timearr[0] + numericUpDown1.Value + ":" + timearr[1] + numericUpDown2.Value + ":" + timearr[2] + numericUpDown3.Value + " /s mtdw10nb04 /ru " + "itfb\a.podchasov" + " /rp " + "2G_uXkME-t");

            /*Process p = new Process();
            p.StartInfo.FileName = "powershell.exe";
            p.StartInfo.Arguments = "Schtasks /Change /TN " + sender + " /ST " + timearr[0] + numericUpDown1.Value + ":" + timearr[1] + numericUpDown2.Value + ":" + timearr[2] + numericUpDown3.Value;
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
            Simulator.Mouse.Sleep(30);*/

            /*PowerShell PowerShellInstance = PowerShell.Create();
            PowerShellInstance.AddScript(@"Invoke-Command -ComputerName s4sh-desk -ScriptBlock {Schtasks /Change /TN alarm_1.1 /ST 00:00:00} -Credential s4sh-desk\s4sh | Out-String");
            PowerShellInstance.Invoke();*/
            numericUpDown1_init = numericUpDown1.Value;
            numericUpDown2_init = numericUpDown2.Value;
            numericUpDown3_init = numericUpDown3.Value;
        }

        //Exit form
        private void close_window_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(numericUpDown1.Value) != numericUpDown1_init || Convert.ToInt32(numericUpDown2.Value) != numericUpDown2_init || Convert.ToInt32(numericUpDown3.Value) != numericUpDown3_init)
            {
                DialogResult result = MessageBox.Show("Были внесены изменения, произвести сохранение?", "Подтвердите действие", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    ApplyChanges("alarm_1");
                    ApplyChanges("alarm_2");
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

        //Minimizing form
        private void minimized_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            minimized.ForeColor = OriginalBackground;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;
                trayicon.Visible = true;
            }
        }

        //Clicking on tray icon
        private void trayicon_MouseClick(object sender, MouseEventArgs e)
        {
            trayicon.Visible = false;
            this.ShowInTaskbar = true;
            WindowState = FormWindowState.Normal;
        }
    }
}