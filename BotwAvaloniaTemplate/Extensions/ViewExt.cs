using Avalonia.Controls;
using System.Threading.Tasks;
using BotwAvaloniaTemplate.Dialogs;
using Avalonia.Dialogs;

namespace BotwAvaloniaTemplate.Extensions
{
    public enum MessageBoxButtons { Ok, OkCancel, YesNo, YesNoCancel }
    public enum MessageBoxResult { Cancel, No, Ok, Yes }
    public enum Formatting { None, Markdown }
    public enum BrowserDialog { File, Folder, FileFolder }

    public static class ViewExt
    {
        public static Task<MessageBoxResult> ShowMessageBox(this Window _, string text, string title = "Warning", MessageBoxButtons buttons = MessageBoxButtons.Ok,
            Formatting formatting = Formatting.None) => MessageBox.Show(text, title, buttons, formatting);

        public static async Task<string?> BrowseDialog(this BrowserDialog browser, string title = "")
        {
            if (browser == BrowserDialog.Folder) {
                OpenFolderDialog dialog = new() { Title = title };
                var result = await dialog.ShowAsync(View);
                return result;
            }
            else if (browser == BrowserDialog.File) {
                OpenFileDialog dialog = new() { Title = title };
                var result = await dialog.ShowAsync(View);
                return result?[0];
            }
            else {
                OpenFileDialog ofd = new() { Title = title };
                var result = await ofd.ShowManagedAsync(View, new ManagedFileDialogOptions { AllowDirectorySelection = true });
                return result[0];
            }
        }
    }
}
