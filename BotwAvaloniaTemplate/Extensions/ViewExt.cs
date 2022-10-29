using Avalonia.Controls;
using System.Threading.Tasks;
using BotwAvaloniaTemplate.Dialogs;
using Avalonia.Platform.Storage;
using System;
using Avalonia.VisualTree;
using System.Linq;

namespace BotwAvaloniaTemplate.Extensions
{
    public enum MessageBoxButtons { Ok, OkCancel, YesNo, YesNoCancel }
    public enum MessageBoxResult { Cancel, No, Ok, Yes }
    public enum Formatting { None, Markdown }
    public enum BrowserDialog { File, Folder, FileFolder }

    public static class ViewExt
    {
        private static IStorageFolder? LastSelectedDirectory;
        private static IStorageFolder? LastSaveDirectory;

        public static Task<MessageBoxResult> ShowMessageBox(this Window _, string text, string title = "Warning", MessageBoxButtons buttons = MessageBoxButtons.Ok,
            Formatting formatting = Formatting.None) => MessageBox.Show(text, title, buttons, formatting);

        public static async Task<string?> ShowDialog(this BrowserDialog browser, string title = "")
        {
            string? path = null;

            if (browser == BrowserDialog.Folder) {
                var result = await (View.GetVisualRoot() as TopLevel)!.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions() {
                    Title = title,
                    SuggestedStartLocation = LastSelectedDirectory
                });

                IStorageItem? item = result.FirstOrDefault() is IStorageItem _item ? _item : null;
                if (item != null) {
                    path = item.TryGetUri(out Uri? uri) ? uri.ToString() : item.Name;
                    LastSelectedDirectory = item as IStorageFolder;
                }
            }
            else if (browser == BrowserDialog.File) {
                var result = await (View.GetVisualRoot() as TopLevel)!.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions() {
                    Title = title,
                    SuggestedStartLocation = LastSelectedDirectory
                });

                IStorageItem? item = result.FirstOrDefault() is IStorageItem _item ? _item : null;
                if (item != null) {
                    path = item.TryGetUri(out Uri? uri) ? uri.ToString() : item.Name;
                    LastSelectedDirectory = await item.GetParentAsync();
                }
            }
            else {
                var result = await (View.GetVisualRoot() as TopLevel)!.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions() {
                    Title = title,
                    SuggestedStartLocation = LastSaveDirectory
                });
                path = result != null ? result.TryGetUri(out Uri? uri) ? uri.ToString() : result.Name : null;
                LastSaveDirectory = result != null ? await result.GetParentAsync() : null;
            }

            return path?.Remove(0, 8);
        }
    }
}
