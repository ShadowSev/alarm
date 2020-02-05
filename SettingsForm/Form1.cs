using System;
using System.Drawing;
using System.Windows.Forms;
using System.Management.Automation;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;

namespace SettingsForm
{
    public partial class Form1 : Form
    {
        public decimal numericUpDown1_init, numericUpDown2_init, numericUpDown3_init;
        public Color OriginalBackground;
        public int UpDownSymb2, UpDownSymb3, UpDownSymb5;
        public string timeString;
        public string tempPath = System.IO.Path.GetTempPath();
        public string programfilesPath = Environment.ExpandEnvironmentVariables("%ProgramFiles(x86)%");
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

        //Checking tasks for existence, disable and fixing collisions
        private void schtasks_check()
        {
            Directory.CreateDirectory(tempPath + @"Alarm");
            PowerShell PowerShellInstance = PowerShell.Create();
            int[] updownarr = { Convert.ToInt32(numericUpDown1.Value), Convert.ToInt32(numericUpDown2.Value), Convert.ToInt32(numericUpDown3.Value) };
            string[] timearr = new string[3];
            for (int i = 0; i < 3; i++)
            {
                if ((updownarr[i] >= 0) && (updownarr[i] < 10))
                    timearr[i] = "0";
                else
                    timearr[i] = "";
            }
            PowerShellInstance.AddScript(@"Schtasks /Query /NH /TN Alarm > $env:temp\Alarm\schtasksList.txt");
            PowerShellInstance.Invoke();
            if (new FileInfo(tempPath + @"Alarm\schtasksList.txt").Length == 0)
            {
                File.Delete(tempPath + @"Alarm\schtasksList.txt");
                schtasks_create();
            }
            else
            {
                var lines = System.IO.File.ReadAllLines(tempPath + @"Alarm\schtasksList.txt");
                if (Regex.IsMatch(lines[2], "Отключено") || Regex.IsMatch(lines[2], "Disabled"))
                {
                    File.Delete(tempPath + @"Alarm\schtasksList.txt");
                    PowerShellInstance.AddScript(@"Enable-ScheduledTask Alarm");
                    PowerShellInstance.Invoke();
                }
            }
            schtasks_read();
            File.WriteAllText(tempPath + @"Alarm\alarm_template.xml", alarm_template(timearr[0] + numericUpDown1.Value, timearr[1] + numericUpDown2.Value, timearr[2] + numericUpDown3.Value));
            PowerShellInstance.AddScript(@"Register-ScheduledTask -xml (Get-Content " + tempPath + @"Alarm\alarm_template.xml | Out-String) -TaskName Alarm -Force");
            PowerShellInstance.Invoke();
            File.Delete(tempPath + @"Alarm\alarm_template.xml");
        }

        //Creating tasks
        private void schtasks_create()
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
            File.WriteAllText(tempPath + @"Alarm\alarm_template.xml", alarm_template("08", "00", "00"));
            PowerShellInstance.AddScript(@"Register-ScheduledTask -xml (Get-Content " + tempPath + @"Alarm\alarm_template.xml | Out-String) -TaskName Alarm -Force");
            PowerShellInstance.Invoke();
            File.Delete(tempPath + @"Alarm\alarm_template.xml");
        }

        //Xml template for schtasks import
        private string alarm_template(string hr, string min, string sec)
        {
            const char doubleQuote = '"';
            var text_template = @"<?xml version=" + doubleQuote + @"1.0" + doubleQuote + @" encoding=" + doubleQuote + @"UTF-16" + doubleQuote + @"?>
<Task version=" + doubleQuote + @"1.2" + doubleQuote + @" xmlns=" + doubleQuote + @"http://schemas.microsoft.com/windows/2004/02/mit/task" + doubleQuote + @">
  <RegistrationInfo>
    <Author>S4SH-DESK\S4sh</Author>
    <URI>\Alarm</URI>
  </RegistrationInfo>
  <Triggers>
    <CalendarTrigger>
      <StartBoundary>2020-02-05T" + hr + @":" + min + @":" + sec + @"</StartBoundary>
      <Enabled>true</Enabled>
      <ScheduleByDay>
        <DaysInterval>1</DaysInterval>
      </ScheduleByDay>
    </CalendarTrigger>
  </Triggers>
  <Principals>
    <Principal id=" + doubleQuote + @"Author" + doubleQuote + @">
      <UserId>S-1-5-21-1768319085-1959801792-1367335388-1001</UserId>
      <LogonType>InteractiveToken</LogonType>
      <RunLevel>LeastPrivilege</RunLevel>
    </Principal>
  </Principals>
  <Settings>
    <MultipleInstancesPolicy>IgnoreNew</MultipleInstancesPolicy>
    <DisallowStartIfOnBatteries>true</DisallowStartIfOnBatteries>
    <StopIfGoingOnBatteries>true</StopIfGoingOnBatteries>
    <AllowHardTerminate>true</AllowHardTerminate>
    <StartWhenAvailable>false</StartWhenAvailable>
    <RunOnlyIfNetworkAvailable>false</RunOnlyIfNetworkAvailable>
    <IdleSettings>
      <StopOnIdleEnd>true</StopOnIdleEnd>
      <RestartOnIdle>false</RestartOnIdle>
    </IdleSettings>
    <AllowStartOnDemand>true</AllowStartOnDemand>
    <Enabled>true</Enabled>
    <Hidden>true</Hidden>
    <RunOnlyIfIdle>false</RunOnlyIfIdle>
    <WakeToRun>true</WakeToRun>
    <ExecutionTimeLimit>PT72H</ExecutionTimeLimit>
    <Priority>7</Priority>
  </Settings>
  <Actions Context=" + doubleQuote + @"Author" + doubleQuote + @">
    <Exec>
      <Command>" + doubleQuote + @"C:\Program Files (x86)\S4sh\Alarm\Seturl.exe" + doubleQuote + @"</Command>
    </Exec>
    <Exec>
      <Command>" + doubleQuote + @"C:\Program Files (x86)\S4sh\Alarm\MouseMove.exe" + doubleQuote + @"</Command>
    </Exec>
    <Exec>
      <Command>" + doubleQuote + @"C:\Program Files (x86)\S4sh\Alarm\VolumeSetup.exe" + doubleQuote + @"</Command>
    </Exec>
  </Actions>
</Task>";
            return (string)text_template;
        }

            //Call task --> writing task parameters to file --> redefinition task parameters --> writing parameters to variables --> deleting file
            private void schtasks_read()
        {
            PowerShell PowerShellInstance = PowerShell.Create();
            PowerShellInstance.AddScript(@"Schtasks /Query /NH /TN Alarm | Out-String");
            PowerShellInstance.Invoke();
            Collection<PSObject> pSObjects = PowerShellInstance.Invoke();
            foreach (PSObject p in pSObjects)
            {
                StreamWriter sw = new StreamWriter(tempPath + @"Alarm\schtasksList.txt");
                sw.WriteLine(p.ToString());
                sw.Close();
            }
            schtasks_fileread();
            numericUpDown1.Value = numericUpDown1_init = Convert.ToDecimal(timeString.Substring(0, UpDownSymb2));
            numericUpDown2.Value = numericUpDown2_init = Convert.ToDecimal(timeString.Substring(UpDownSymb3, 2));
            numericUpDown3.Value = numericUpDown3_init = Convert.ToDecimal(timeString.Substring(UpDownSymb5));
            File.Delete(tempPath + @"Alarm\schtasksList.txt");
        }

        //Reading schtasks parameters from file
        private void schtasks_fileread()
        {
            var lines = System.IO.File.ReadAllLines(tempPath + @"Alarm\schtasksList.txt");
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
            ApplyChanges();
        }

        //Traking if mouse button pressed up on form
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

        //Form moving with pressed mouse button
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                Point currentScreenPos = PointToScreen(e.Location);
                Location = new Point(currentScreenPos.X - mouseOffset.X, currentScreenPos.Y - mouseOffset.Y);
            }
        }

        //Traking if mouse button not pressed up on form
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
        private void ApplyChanges()
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
            PowerShellInstance.AddScript(@"Schtasks /Delete /TN Alarm /F");
            File.WriteAllText(tempPath + @"Alarm\alarm_template.xml", alarm_template(timearr[0] + numericUpDown1.Value, timearr[1] + numericUpDown2.Value, timearr[2] + numericUpDown3.Value));
            PowerShellInstance.AddScript(@"Register-ScheduledTask -xml (Get-Content " + tempPath + @"Alarm\alarm_template.xml | Out-String) -TaskName Alarm -Force");
            PowerShellInstance.Invoke();
            File.Delete(tempPath + @"Alarm\alarm_template.xml");
            numericUpDown1_init = numericUpDown1.Value;
            numericUpDown2_init = numericUpDown2.Value;
            numericUpDown3_init = numericUpDown3.Value;
        }

        //Exit scenario
        private void close_window_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(numericUpDown1.Value) != numericUpDown1_init || Convert.ToInt32(numericUpDown2.Value) != numericUpDown2_init || Convert.ToInt32(numericUpDown3.Value) != numericUpDown3_init)
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
            {
                this.Close();
            }
            schtasks_setwake();
            Directory.Delete(tempPath + @"Alarm", true);
        }

        //Adding waking up settings for all schtasks
        private void schtasks_setwake()
        {
            PowerShell PowerShellInstance = PowerShell.Create();
            PowerShellInstance.AddScript(@"Export-ScheduledTask -TaskName Alarm | out-file " + tempPath + @"Alarm\alarm.xml");
            PowerShellInstance.Invoke();
            foreach (var line in File.ReadAllLines(tempPath + @"Alarm\alarm.xml"))
            {
                File.AppendAllText(tempPath + @"Alarm\alarm_wake.xml", line + "\r\n");
                if (line.Contains("   </IdleSettings>"))
                {
                    File.AppendAllText(tempPath + @"Alarm\alarm_wake.xml", "    <WakeToRun>true</WakeToRun> \n");
                    PowerShellInstance.Invoke();
                }
            }
            PowerShellInstance.AddScript(@"Register-ScheduledTask -xml (Get-Content " + tempPath + @"Alarm\alarm_wake.xml | Out-String) -TaskName Alarm -Force");
            PowerShellInstance.Invoke();
            File.Delete(tempPath + @"Alarm\alarm_wake.xml");
        }

        //Minimizing form to tray
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