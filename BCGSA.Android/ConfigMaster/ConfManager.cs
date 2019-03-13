using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;
using System.IO;
namespace BCGSA.ConfigMaster
{
    public sealed class ConfManager: INotifyPropertyChanged
    {
        private static readonly string ConfigFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "conf.json");
        private List<string> _listModes = Enum.GetNames(typeof(SensorSpeed)).ToList();

        private static readonly ConfManager Instance = new ConfManager();
        private Settings _settings;

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

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

        public List<string> GetModes
        {
            get =>
             _listModes;
            set
            {
                if (_listModes != value)
                {
                    _listModes = value;
                    OnPropertyChanged(nameof(GetModes));
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
                OnPropertyChanged(nameof(_settings.ConnectMode));
            }
        }

        public bool InversX
        {
            get => _settings.InversX;
            set
            {
                _settings.InversX = value;
                SaveConfiguration();
                OnPropertyChanged(nameof(_settings.InversX));
            }
        }

        public bool InversY
        {
            get => _settings.InversY;
            set
            {
                _settings.InversY = value;
                SaveConfiguration();
                OnPropertyChanged(nameof(_settings.InversY));
            }
        }
    }
}
