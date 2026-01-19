using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RemoteManager.Services
{
    public static class AppPath
    {
        public static readonly string JsonPath =
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "RemoteManager",
                "servers.json");
    }
}
