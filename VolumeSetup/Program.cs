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
            const char doubleQuote = '"';
            string script = @"
C:\Users\S4sh\source\repos\WindowsFormsApp1\nircmd.exe setdefaultsounddevice 'Speakers' 1

Add-Type -TypeDefinition @'
using System.Runtime.InteropServices;
[Guid(" + doubleQuote + @"5CDF2C82-841E-4546-9722-0CF74078229A" + doubleQuote + @"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
interface IAudioEndpointVolume
{
    // f(), g(), ... are unused COM method slots. Define these if you care
    int f(); int g(); int h(); int i();
    int SetMasterVolumeLevelScalar(float fLevel, System.Guid pguidEventContext);
    int j();
    int GetMasterVolumeLevelScalar(out float pfLevel);
    int k(); int l(); int m(); int n();
    int SetMute([MarshalAs(UnmanagedType.Bool)] bool bMute, System.Guid pguidEventContext);
    int GetMute(out bool pbMute);
}
[Guid(" + doubleQuote + @"D666063F-1587-4E43-81F1-B948E807363F" + doubleQuote + @"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
interface IMMDevice
{
    int Activate(ref System.Guid id, int clsCtx, int activationParams, out IAudioEndpointVolume aev);
}
[Guid(" + doubleQuote + @"A95664D2-9614-4F35-A746-DE8DB63617E6" + doubleQuote + @"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
interface IMMDeviceEnumerator
{
    int f(); // Unused
    int GetDefaultAudioEndpoint(int dataFlow, int role, out IMMDevice endpoint);
}
[ComImport, Guid(" + doubleQuote + @"BCDE0395-E52F-467C-8E3D-C4579291692E" + doubleQuote + @")] class MMDeviceEnumeratorComObject { }
public class Audio
{
    static IAudioEndpointVolume Vol()
    {
        var enumerator = new MMDeviceEnumeratorComObject() as IMMDeviceEnumerator;
        IMMDevice dev = null;
        Marshal.ThrowExceptionForHR(enumerator.GetDefaultAudioEndpoint(/*eRender*/ 0, /*eMultimedia*/ 1, out dev));
        IAudioEndpointVolume epv = null;
        var epvid = typeof(IAudioEndpointVolume).GUID;
        Marshal.ThrowExceptionForHR(dev.Activate(ref epvid, /*CLSCTX_ALL*/ 23, 0, out epv));
        return epv;
    }
    public static float Volume
    {
        get { float v = -1; Marshal.ThrowExceptionForHR(Vol().GetMasterVolumeLevelScalar(out v)); return v; }
        set { Marshal.ThrowExceptionForHR(Vol().SetMasterVolumeLevelScalar(value, System.Guid.Empty)); }
    }
    public static bool Mute
    {
        get { bool mute; Marshal.ThrowExceptionForHR(Vol().GetMute(out mute)); return mute; }
        set { Marshal.ThrowExceptionForHR(Vol().SetMute(value, System.Guid.Empty)); }
    }
}
'@


[single]$n = 0.03;
[audio]::Volume = $n;

while ( $n - 0.01 -le 0.15 )
{
  [audio]::Volume = $n;
  $n = $n + 0.01;
  while ( $n -le 0.1 )
  {
    if ( -not(get-process | where {$_.ProcessName -eq 'browser'})) 
    { 
        C:\Users\S4sh\source\repos\WindowsFormsApp1\nircmd.exe setdefaultsounddevice 'Headphones' 1
        exit
    }
    Start-Sleep -s 15;
    [audio]::Volume = $n;
    $n = $n + 0.01;
  }
  if ( -not(get-process | where {$_.ProcessName -eq 'browser'})) 
    { 
        C:\Users\S4sh\source\repos\WindowsFormsApp1\nircmd.exe setdefaultsounddevice 'Headphones' 1
        exit
    }
  Start-Sleep -s 30;
}

[audio]::Volume = 0.1;
$n = 0;

while ($n -le 720)
{
    if ( -not(get-process | where {$_.ProcessName -eq 'browser'})) 
    { 
        C:\Users\S4sh\source\repos\WindowsFormsApp1\nircmd.exe setdefaultsounddevice 'Headphones' 1
        exit 
    }
    $n = $n + 1;
    Start-Sleep -s 10;
}


Stop-Process -Name browser
C:\Users\S4sh\source\repos\WindowsFormsApp1\nircmd.exe setdefaultsounddevice 'Headphones' 
";
            PowerShell powerShell = PowerShell.Create();
            powerShell.AddScript(script);
            powerShell.Invoke();
        }
    }
}
