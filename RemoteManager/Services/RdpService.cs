using RemoteManager.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace RemoteManager.Services
{
    public static class RdpService
    {
        public static void Connect(RemoteServer server)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "cmdkey",
                Arguments = $"/generic:TERMSRV/{server.Address} /user:{server.UserName} /pass:{server.Password}",
                CreateNoWindow = true,
                UseShellExecute = false
            })?.WaitForExit();


            Process.Start(new ProcessStartInfo
            {
                FileName = "mstsc",
                Arguments = $"/v:{server.Address} /admin",
                UseShellExecute = true
            });
        }
    }
}
