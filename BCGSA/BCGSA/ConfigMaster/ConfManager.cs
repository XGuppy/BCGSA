using System;
using System.IO;
namespace BCGSA.ConfigMaster
{
    internal sealed class ConfManager
    {
        private static readonly string ConfigFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "conf.json");
        private static readonly ConfManager Instance = new ConfManager();

        private Settings _settings;

        private ConfManager()
        {
            if (File.Exists(ConfigFileName))
            {
                using (var sr = new StreamReader(ConfigFileName))
                {
                    _settings = Settings.FromJson(sr.ReadToEnd());
                }
            }
            else
            {
                using (var sw = new StreamWriter(ConfigFileName))
                {
                    _settings = Settings.FromJson(Settings.GetDefault());
                    sw.Write(Settings.GetDefault());
                }
            }
        }

        public static ConfManager GetManager() => Instance;

        public void ResetToDefault()
        {
            _settings = Settings.FromJson(Settings.GetDefault());
        }

        public void SaveConfiguration()
        {
            using (var sw = new StreamWriter(ConfigFileName))
            {
                sw.Write(_settings.ToString());
            }
        }

        public string ConnectMod { get => _settings.ConnectMode; set => _settings.ConnectMode = value; }

        public bool InvesX { get => _settings.InversX; set => _settings.InversX = value; }

        public bool InvesY { get => _settings.InversY; set => _settings.InversY = value; }
    }
}
