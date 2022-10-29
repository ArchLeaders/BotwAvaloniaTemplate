using Avalonia;
using Avalonia.SettingsFactory;
using Avalonia.SettingsFactory.ViewModels;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Themes.Fluent;
using BotwAvaloniaTemplate.Dialogs;
using BotwAvaloniaTemplate.Extensions;
using BotwAvaloniaTemplate.Helpers;
using BotwAvaloniaTemplate.Models;
using BotwAvaloniaTemplate.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BotwAvaloniaTemplate.Views
{
    public partial class SettingsView : SettingsFactory, ISettingsValidator
    {
        public SettingsView() => AvaloniaXamlLoader.Load(this);
        public SettingsView(bool canClose = true)
        {
            AvaloniaXamlLoader.Load(this);

            // Old code :man_shrugging:
            ViewModel.IsSettingsOpen = true;

            // Very much unnecessary, but not having this bothers me.
            // Allows you to focus seemingly nothing.
            Grid focusDelegate = this.FindControl<Grid>("FocusDelegate")!;
            focusDelegate.PointerPressed += (_, _) => focusDelegate.Focus();
            Grid focusDelegate2 = this.FindControl<Grid>("FocusDelegate2")!;
            focusDelegate2.PointerPressed += (_, _) => focusDelegate.Focus();

            AfterSaveEvent += () => ViewModel.SettingsView = null;
            AfterCancelEvent += () => ViewModel.SettingsView = null;

            SettingsFactoryOptions options = new() {
                AlertAction = (msg) => MessageBox.Show(msg),
                BrowseAction = async (title) => await BrowserDialog.Folder.ShowDialog(title),
                FetchResource = (res) => Resource.Load(res).ParseJson<Dictionary<string, string>>(),
            };

            InitializeSettingsFactory(new SettingsFactoryViewModel(canClose), this, Config, options);
        }

        public bool? ValidateString(string key, string value)
        {
            return key switch {
                "BaseGame" => File.Exists($"{value}\\Pack\\Dungeon000.pack") && value.EndsWith("content"),
                "Update" => File.Exists($"{value}\\Actor\\Pack\\ActorObserverByActorTagTag.sbactorpack") && value.EndsWith("content"),
                "Dlc" => File.Exists($"{value}\\Pack\\AocMainField.pack") && value.EndsWith("content\\0010"),
                "BaseGameNx" => File.Exists($"{value}\\Actor\\Pack\\ActorObserverByActorTagTag.sbactorpack") && File.Exists($"{value}\\Pack\\Dungeon000.pack") && value.EndsWith("romfs"),
                "DlcNx" => File.Exists($"{value}\\Pack\\AocMainField.pack") && value.EndsWith("romfs"),
                "TestSetting" => File.Exists($"{this["Update"]}\\{value}\\ActorInfo.product.sbyml"),
                "Theme" => ValidateTheme(value),
                _ => null
            };
        }

        public bool? ValidateBool(string key, bool value)
        {
            return key switch {
                _ => null
            };
        }

        public static bool? ValidateTheme(string value)
        {
            App.Theme.Mode = value == "Dark" ? FluentThemeMode.Dark : FluentThemeMode.Light;
            Application.Current!.Styles[0] = App.Theme;
            return null;
        }

        public string? ValidateSave(Dictionary<string, bool?> validated)
        {
            // Add custom checks here...

            // By default return a standard error message,
            // or null where all validations passed
            return validated.Where(x => x.Value == false).Any() ? "One or more settings could not be verified. Please review your settings." : null;
        }

    }
}
