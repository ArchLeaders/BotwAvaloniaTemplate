global using static BotwAvaloniaTemplate.App;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Themes.Fluent;
using BotwAvaloniaTemplate.Extensions;
using BotwAvaloniaTemplate.ViewModels;
using BotwAvaloniaTemplate.Views;
using Material.Icons;
using System;
using System.Threading.Tasks;

namespace BotwAvaloniaTemplate
{
    public partial class App : Application
    {
        public static AppView View { get; private set; } = null!;
        public static AppViewModel ViewModel { get; private set; } = null!;
        public static FluentTheme Theme { get; set; } = new(new Uri($"avares://{nameof(BotwAvaloniaTemplate)}/Styles"));

        public override void Initialize() => AvaloniaXamlLoader.Load(this);
        public override async void OnFrameworkInitializationCompleted()
        {
            Config.LoadConfig();

            Theme.Mode = Config.Theme == "Dark" ? FluentThemeMode.Dark : FluentThemeMode.Light;
            Current!.Styles[0] = Theme;

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) {

                // Create desktop instance
                View = new();
                desktop.MainWindow = View;

                // Create data context
                ViewModel = new();
                View.DataContext = ViewModel;

                // Make sure settings are always set
                if (Config.RequiresInput) {
                    ViewModel.SettingsView = new(canClose: false);
                    ViewModel.SetStatus("Waiting for settings input", MaterialIconKind.BoxVariant);

                    await Task.Run(() => {
                        while (Config.RequiresInput)
                            Task.Delay(100);
                    });

                    ViewModel.SetStatus();
                }
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
