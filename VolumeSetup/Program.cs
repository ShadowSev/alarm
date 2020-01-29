using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;

namespace VolumeSetup
{
    class Program
    {
        static void Main()
        {
            PowerShell PowerShellInstance = PowerShell.Create();
            PowerShellInstance.AddScript(@"");
            PowerShellInstance.Invoke();
        }
    }
}
