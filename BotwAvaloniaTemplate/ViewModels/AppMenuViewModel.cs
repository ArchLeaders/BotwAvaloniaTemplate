using Avalonia;
using Avalonia.Platform;
using BotwAvaloniaTemplate.Dialogs;
using BotwAvaloniaTemplate.Extensions;
using BotwAvaloniaTemplate.Helpers;
using ReactiveUI;
using System;
using System.IO;

namespace BotwAvaloniaTemplate.ViewModels
{
    public partial class AppViewModel : ReactiveObject
    {
        // 
        // File

        public void Quit()
        {
            Environment.Exit(1);
        }

        //
        // Tools

        public void Settings() => SettingsView ??= new(canClose: true);

        //
        // About

        public void Help() => MessageBox.Show(Resource.Load("Assets\\Help.md").ToString(), "Help", formatting: Formatting.Markdown);
        public void Credits() => MessageBox.Show(Resource.Load("Assets\\Credits.md").ToString(), "Credits", formatting: Formatting.Markdown);
    }
}
