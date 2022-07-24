using System;
using System.Configuration;
using System.Drawing.Text;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using Configuration = System.Configuration.Configuration;

namespace ProductRoadSign
{
    public class Config
    {
        public static string[] FindFont()
        {
            var result = SystemFonts.Collection.Families.Select(_ => _.Name).ToList();
            //FontCollection collection = new();
            //FontFamily family = collection.Add("./Fonts/msyh.ttf");
            //Font font = family.CreateFont(18, FontStyle.Italic);
            //result.Add(font.Name);
            return result.ToArray();
        }
        private static readonly Configuration configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        private static AppSettingsSection setting = configFile.AppSettings;
        private static readonly Config config = new Config();
        public static Config Default => config;
        private static FontModel[] GetFonts()
        {
            List<FontModel> result = new();
            foreach (var item in SystemFonts.Collection.Families)
            {
                result.Add(new FontModel { Name = item.Name, Value = item.Name });
            }
            return result.ToArray();
        }
        public static IList<ColorModel> GetColors()
        {
            List<ColorModel> result = new();
            foreach (var item in typeof(Color).GetFields())
            {
                Color value = (Color)item.GetValue(typeof(Color));

                result.Add(new ColorModel(item.Name, value));
            }
            return result;
        }
        public static ICollection<ColorModel> Colors => GetColors();
        public static IReadOnlyCollection<FontModel> Fonts = GetFonts();
        public static IEnumerable<string> Sources {
            get => LoadArray("Sources");
            set => Save("Sources", value);
        }
        public static IEnumerable<string> ProductTypes
        {
            get => LoadArray("ProductTypes");
            set => Save("ProductTypes", value);
        }

        public static string Get(string name)
        {
            return config[name];
        }
        public static T Get<T>(string name)
        {
            var type = typeof(Config).GetProperty(name);
            var value = (T)type.GetValue(typeof(T));
            return value;
        }
        public static void Set<T>(string name,T value)
        {
            var type = typeof(Config).GetProperty(name);
            type.SetValue(typeof(T), value);
        }
        public static void Set(string name, string value)
        {
            Save(name, value);
        }
        public static void Set(Type type)
        {
            var name = type.Name;
            var value = type.ToString();
            Save(name, value);
        }
        public static string TempPath => System.IO.Path.GetTempPath();

        public static int MonthPixel {
            get { return int.TryParse(Load("MonthPixel"), out int result) ? result : 40; }
            set { Save("MonthPixel", value.ToString()); }
        }
        public static int Margins {
            get { return int.TryParse(Load("Margins"), out int result) ? result : 10; }
            set { Save("Margins", value.ToString()); }
        }
        public static int SplitLine
        {
            get { return int.TryParse(Load("SplitLine"), out int result) ? result : 1; }
            set { Save("SplitLine", value.ToString()); }
        }
        public static string TypeSource
        {
            get => Load("TypeSource");
            set => Save("TypeSource", value);
        }
        public static string StartSource
        {
            get => Load("StartSource");
            set => Save("StartSource", value);
        }
        public static string EndSource
        {
            get => Load("EndSource");
            set => Save("EndSource", value);
        }

        public static int RulerSize {
            get { return int.TryParse(Load("RulerSize"), out int result) ? result : 18; }
            set { Save("RulerSize", value.ToString()); }
        }
        public static string RulerFont {
            get { return string.IsNullOrWhiteSpace(Load("RulerFont")) ? "Heiti SC": Load("RulerFont"); }
            set { Save("RulerFont", value.ToString()); }
        }
        public static string RulerColor
        {
            get { return string.IsNullOrWhiteSpace(Load("RulerColor")) ? "#000000" : Load("RulerColor"); }
            set { Save("RulerColor", value.ToString()); }
        }


        public static int TopSize {
            get { return int.TryParse(Load("TopSize"), out int result) ? result : 18; }
            set { Save("TopSize", value.ToString()); }
        }
        public static string TopFont {
            get { return string.IsNullOrWhiteSpace(Load("TopFont")) ? "Heiti SC" : Load("TopFont"); }
            set { Save("TopFont", value.ToString()); }
        }
        public static string TopColor
        {
            get { return string.IsNullOrWhiteSpace(Load("TopColor")) ? "#FFFFFF" : Load("TopColor"); }
            set { Save("TopColor", value.ToString()); }
        }
        public static string TopBackGroundColorl
        {
            get { return string.IsNullOrWhiteSpace(Load("TopBackGroundColorl")) ? "#1E90FF" : Load("TopBackGroundColorl"); }
            set { Save("TopBackGroundColorl", value.ToString()); }
        }
        public static IEnumerable<string> TopSource {
            get => LoadArray("TopSource");
            set => Save("TopSource", value);
        }

        public static int CenterSize {
            get { return int.TryParse(Load("CenterSize"), out int result) ? result : 14; }
            set { Save("CenterSize", value.ToString()); }
        }
        public static string CenterFont {
            get { return string.IsNullOrWhiteSpace(Load("CenterFont")) ? "Heiti SC" : Load("CenterFont"); }
            set { Save("CenterFont", value.ToString()); }
        }
        public static string CenterColor {
            get { return string.IsNullOrWhiteSpace(Load("TopColor")) ? "#000000" : Load("CenterColor"); }
            set { Save("CenterColor", value.ToString()); }
        }
        public static string CenterBackGroundColorl {
            get { return string.IsNullOrWhiteSpace(Load("CenterBackGroundColorl")) ? "#F0F8FF" : Load("CenterBackGroundColorl"); }
            set { Save("CenterBackGroundColorl", value.ToString()); }
        }
        public static IEnumerable<string> CenterSource
        {
            get => LoadArray("CenterSource");
            set => Save("CenterSource", value);
        }

        public static int BottomSize {
            get { return int.TryParse(Load("BottomSize"), out int result) ? result : 18; }
            set { Save("BottomSize", value.ToString()); }
        }
        public static string BottomFont {
            get { return string.IsNullOrWhiteSpace(Load("BottomFont")) ? "Heiti SC" : Load("BottomFont"); }
            set { Save("BottomFont", value.ToString()); }
        }
        public static string BottomColor {
            get { return string.IsNullOrWhiteSpace(Load("BottomColor")) ? "#FFFFFF" : Load("BottomColor"); }
            set { Save("BottomColor", value.ToString()); }
        }
        public static string BottomBackGroundColorl {
            get { return string.IsNullOrWhiteSpace(Load("BottomBackGroundColorl")) ? "#1E90FF" : Load("BottomBackGroundColorl"); }
            set { Save("BottomBackGroundColorl", value.ToString()); }
        }
        public static IEnumerable<string> BottomSource
        {
            get => LoadArray("BottomSource");
            set => Save("BottomSource", value);
        }

        private static void Save(string name,string value)
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var setting = configFile.AppSettings.Settings;
            if (setting[name] == null)
            {
                setting.Add(name, value);
            }
            else
            {
                setting[name].Value = value;
            }
            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
        }
        private static string Load(string name)
        {
            var result = ConfigurationManager.AppSettings[name];
            result = string.IsNullOrEmpty(result) ? "" : result;
            return result;
        }
        private static void Save<T>(string name,T value)
        {
            Save(name, System.Text.Json.JsonSerializer.Serialize(value));
        }
        private static string[] LoadArray(string name)
        {
            string[] result;
            var value = Load(name);
            try
            {
                result = System.Text.Json.JsonSerializer.Deserialize<string[]>(value);
            }
            catch
            {
                result = new string[] { };
            }
            return result;
        }
        public string this[string name] {
            get { return Load(name); }
            set { Save(name, value); }
        }
    }
}

