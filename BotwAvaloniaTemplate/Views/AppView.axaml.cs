using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using BotwAvaloniaTemplate.ViewModels;
using System;

namespace BotwAvaloniaTemplate.Views
{
    public partial class AppView : Window
    {
        private DispatcherTimer loader = new();

        public AppView()
        {
            AvaloniaXamlLoader.Load(this);
            this.AttachDevTools();

            loader.Interval = new TimeSpan(0, 0, 0, 0, 400);
            loader.Tick += (_, _) => {
                if (((AppViewModel)DataContext!).IsLoading == true) {
                    ((AppViewModel)DataContext)!.Status += " .";
                    ((AppViewModel)DataContext)!.Status = ((AppViewModel)DataContext).Status.Replace(" . . . .", " .");
                }
            };

            loader.Start();
        }

        // Fix Win32 clipping issues
        protected override void HandleWindowStateChanged(WindowState state)
        {
            if (state == WindowState.Maximized) {
                Padding = new Thickness(7);
                ExtendClientAreaTitleBarHeightHint = 44;
                (DataContext as AppViewModel)!.IsMaximized = true;
            }
            else {
                Padding = new Thickness(0);
                ExtendClientAreaTitleBarHeightHint = 30;
                (DataContext as AppViewModel)!.IsMaximized = false;
            }

            base.HandleWindowStateChanged(state);
        }
    }
}
