using Avalonia.Controls;
using System.Threading.Tasks;
using BotwAvaloniaTemplate.Dialogs;

namespace BotwAvaloniaTemplate.Extensions
{
    public enum MessageBoxButtons { Ok, OkCancel, YesNo, YesNoCancel }
    public enum MessageBoxResult { Cancel, No, Ok, Yes }
    public enum Formatting { None, Markdown }
    public static class ViewExt
    {
        public static Task<MessageBoxResult> ShowMessageBox(this Window parent, string text, string title = "Warning", MessageBoxButtons buttons = MessageBoxButtons.Ok,
            Formatting formatting = Formatting.None) => MessageBox.Show(text, title, buttons, parent, formatting);
    }
}
