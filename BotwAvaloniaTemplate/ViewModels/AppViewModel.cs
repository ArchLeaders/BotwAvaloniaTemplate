using Avalonia.Controls;
using BotwAvaloniaTemplate.Views;
using Material.Icons;
using ReactiveUI;
using System.Runtime.InteropServices;

namespace BotwAvaloniaTemplate.ViewModels
{
    public partial class AppViewModel : ReactiveObject
    {
        //
        // Child Panels

        private SettingsView? settingsView;
        public SettingsView? SettingsView {
            get => settingsView;
            set => this.RaiseAndSetIfChanged(ref settingsView, value);
        }

        //
        // Status

        public void SetStatus(string msg = "Ready", MaterialIconKind icon = MaterialIconKind.CardsOutline, bool? isLoading = null)
        {
            IsLoading = isLoading == null ? !IsLoading : (bool)isLoading;
            Status = msg;
            StatusIcon = icon;
        }

        private MaterialIconKind statusIcon = MaterialIconKind.CardsOutline;
        public MaterialIconKind StatusIcon {
            get => statusIcon;
            set => this.RaiseAndSetIfChanged(ref statusIcon, value);
        }

        private string status = "Ready";
        public string Status {
            get => status;
            set => this.RaiseAndSetIfChanged(ref status, value);
        }


        //
        // META

        private bool isMaximized = false;
        public bool IsMaximized {
            get => isMaximized;
            set => this.RaiseAndSetIfChanged(ref isMaximized, value);
        }

        private bool isEdited = false;
        public bool IsEdited {
            get => isEdited;
            set => this.RaiseAndSetIfChanged(ref isEdited, value);
        }

        private bool isSettingsOpen = false;
        public bool IsSettingsOpen {
            get => isSettingsOpen;
            set => this.RaiseAndSetIfChanged(ref isSettingsOpen, value);
        }

        private bool isLoading = false;
        public bool IsLoading {
            get => isLoading;
            set => this.RaiseAndSetIfChanged(ref isLoading, value);
        }

        public bool IsWindows { get; set; } = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        public void ChangeState(string _) => View.WindowState = View.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        public void Minimize(string _) => View.WindowState = WindowState.Minimized;
    }
}
