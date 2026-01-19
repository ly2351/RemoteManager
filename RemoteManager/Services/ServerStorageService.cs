using RemoteManager.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Windows;

namespace RemoteManager.Services
{
    public static class ServerStorageService
    {
        public static List<RemoteServer> Load()
        {
            Ensure();

            var json = File.ReadAllText(AppPath.JsonPath);
            return JsonSerializer.Deserialize<List<RemoteServer>>(json)
                   ?? new List<RemoteServer>();
        }

        public static void Save(IEnumerable<RemoteServer> list)
        {
            Ensure();

            var json = JsonSerializer.Serialize(list,
                new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                });

            File.WriteAllText(AppPath.JsonPath, json);
        }

        private static void Ensure()
        {
            var dir = Path.GetDirectoryName(AppPath.JsonPath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            if (!File.Exists(AppPath.JsonPath))
                File.WriteAllText(AppPath.JsonPath, "[]");
        }
    }
}
