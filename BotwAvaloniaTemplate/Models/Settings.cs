global using static BotwAvaloniaTemplate.Models.Settings;
using Avalonia.Themes.Fluent;
using BotwAvaloniaTemplate.Attributes;
using BotwAvaloniaTemplate.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Environment;

namespace BotwAvaloniaTemplate.Models
{
    public class Settings
    {
        //
        // Static
        public static Settings Config { get; set; } = new();

        // 
        // App settings

        public bool RequiresInput { get; set;} = true;

        [JsonIgnore]
        public string DataFolder
            => RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? $"{GetFolderPath(SpecialFolder.LocalApplicationData)}/{nameof(BotwAvaloniaTemplate)}" : $"{GetFolderPath(SpecialFolder.ApplicationData)}/{nameof(BotwAvaloniaTemplate)}";

        //
        // Settings

        [Setting("Base Game Directory", "The folder containing the base game files for BOTW, without the update dir DLC files. The last folder should be \"content\", e.g. \"C:\\Games\\Botw\\BaseGame\\content\"")]
        public string BaseGame { get; set; } = "";

        [Setting("Update Directory", "The folder containing the update files for BOTW, version 1.5.0. The last folder should be \"content\", e.g. \"C:\\Games\\Botw\\Update\\content\"")]
        public string Update { get; set; } = "";

        [Setting("DLC Directory", "The folder containing the DLC files for BOTW, version 3.0. The last folder should be \"0010\", e.g. \"C:\\Games\\Botw\\DLC\\content\\0010\"")]
        public string Dlc { get; set; } = "";

        [Setting("Switch Base Game Directory", "Path should end in '01007EF00011E000\\romfs'")]
        public string BaseGameNx { get; set; } = "";

        [Setting("Switch DLC Directory", "Path should end in '01007EF00011F001\\romfs'")]
        public string DlcNx { get; set; } = "";

        [Setting(UiType.Dropdown, "Dark", "Light")]
        public string Theme { get; set; } = "Dark";

        // 
        // Functions
        public void LoadConfig()
        {
            if (File.Exists($"{Config.DataFolder}\\{nameof(Config)}.json")) {
                Config = JsonSerializer.Deserialize<Settings>(File.ReadAllText($"{Config.DataFolder}\\{nameof(Config)}.json")) ?? new();
                return;
            }
            else if (File.Exists($"{Config.DataFolder}\\..\\bcml\\settings.json")) {

                Dictionary<string, object> settings =
                    JsonSerializer.Deserialize<Dictionary<string, object>>(File.ReadAllText($"{Config.DataFolder}\\..\\bcml\\settings.json")) ?? new();

                BaseGame = settings["game_dir"].ToString() ?? "";
                Update = settings["update_dir"].ToString() ?? "";
                Dlc = settings["dlc_dir"].ToString() ?? "";
                BaseGameNx = settings["game_dir_nx"].ToString() ?? "";
                DlcNx = settings["dlc_dir_nx"].ToString() ?? "";

                // Optionally allow input to be no longer required
                RequiresInput = false;
            }

            Save();
        }

        public void Save()
        {
            Directory.CreateDirectory(Config.DataFolder);
            File.WriteAllText($"{Config.DataFolder}\\{nameof(Config)}.json", JsonSerializer.Serialize(this, new JsonSerializerOptions() { WriteIndented = true }));
        }

        //
        // Parameter Validators

        public bool? Validate(string? path, string name)
        {
            if (string.IsNullOrEmpty(path))
                return null;

            if (File.Exists(path))
                return false;

            return name switch {
                "BaseGame" => File.Exists($"{path}\\Pack\\Dungeon000.pack") && path.EndsWith("content"),
                "Update" => File.Exists($"{path}\\Actor\\Pack\\ActorObserverByActorTagTag.sbactorpack") && path.EndsWith("content"),
                "Dlc" => File.Exists($"{path}\\Pack\\AocMainField.pack") && path.EndsWith("content\\0010"),
                "BaseGameNx" => File.Exists($"{path}\\Actor\\Pack\\ActorObserverByActorTagTag.sbactorpack") && File.Exists($"{path}\\Pack\\Dungeon000.pack") && path.EndsWith("romfs"),
                "DlcNx" => File.Exists($"{path}\\Pack\\AocMainField.pack") && path.EndsWith("romfs"),
                "Theme" => ValidateTheme(path, name),
                _ => null,
            };
        }

        public bool? ValidateTheme(string value, string _2)
        {
            App.Theme.Mode = value == "Dark" ? FluentThemeMode.Dark : FluentThemeMode.Light;
            App.Current!.Styles[0] = App.Theme;
            return null;
        }

        //
        // Parameter Alt Setters

        public async Task<string?> Setter(string title, string name)
        {
            return name switch {
                _ => await BrowserDialog.Folder.BrowseDialog(title),
            };
        }

        //
        // Custom save validation logc

        public KeyValuePair<bool, string> ValidateSave(Dictionary<string, bool?> values)
        {
            if (values["BaseGame"] == false) {
                return new(false, "The WiiU game path is invalid.\nPlease correct or delete it before saving.");
            }

            if (values["Update"] == false) {
                return new(false, "The WiiU update path is invalid.\nPlease correct or delete it before saving.");
            }

            if (values["Dlc"] == false) {
                return new(false, "The WiiU DLC path is invalid.\nPlease correct or delete it before saving.");
            }

            if (values["BaseGameNx"] == false && values["BaseGame"] == false) {
                return new(false, "The Switch game/update path is invalid.\nPlease correct or delete it before saving.");
            }

            if (values["DlcNx"] == false) {
                return new(false, "The Switch DLC path is invalid.\nPlease correct or delete it before saving.");
            }

            if (values["BaseGame"] == null && values["Update"] == null && values["BaseGameNx"] == null) {
                return new(false, "No game path has been set for Switch or WiiU.\nPlease set one of them before saving.");
            }

            if (values["BaseGame"] == true && values["Update"] == null) {
                return new(false, "The WiiU update path has not been set.\nPlease set it before saving.");
            }

            return new(true, null!);
        }
    }
}
