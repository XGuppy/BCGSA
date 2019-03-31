using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Android.Hardware;

namespace BCGSA.ConfigMaster
{
    public sealed class ConfManager
    {
        private static readonly string ConfigFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "conf.json");
        private static List<string> _listModes = Enum.GetNames(typeof(SensorDelay)).ToList();

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

        public static ConfManager GetManager => Instance;

        public void ResetToDefault()
        {
            _settings = Settings.FromJson(Settings.GetDefault());
        }

        public void SaveConfiguration()
        {
            using (var sw = new StreamWriter(ConfigFileName))
            {
                sw.Write(_settings.ToJson());
            }
        }

        public static List<string> GetModes
        {
            get =>
             _listModes;
            set
            {
                if (_listModes != value)
                {
                    _listModes = value;
                }
            }
        }


        public string ConnectMod
        {
            get => _settings.ConnectMode;
            set
            {
                _settings.ConnectMode = value;
                SaveConfiguration();
            }
        }

        public bool InversX
        {
            get => _settings.InversX;
            set
            {
                _settings.InversX = value;
                SaveConfiguration();
            }
        }

        public bool InversY
        {
            get => _settings.InversY;
            set
            {
                _settings.InversY = value;
                SaveConfiguration();
            }
        }
        
        public bool InversZ
        {
            get => _settings.InversZ;
            set
            {
                _settings.InversZ = value;
                SaveConfiguration();
            }
        }
    }
}
