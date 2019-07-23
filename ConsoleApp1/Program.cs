using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;

namespace ConsoleApp1
{
    public partial class Program
    {
        static void Main()
        {
            PowerShell PowerShellInstance = PowerShell.Create();
            PowerShellInstance.AddScript(@"param($times = 1); $myshell = New-Object -com ""Wscript.Shell""; for ($i = 0; $i -lt $times; $i++) {; Start-Sleep -Seconds 10; $myshell.sendkeys("".""); }");
            PowerShellInstance.Invoke();
        }
    }
}
