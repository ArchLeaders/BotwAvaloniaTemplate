using ReactiveUI;
using System;

namespace BotwAvaloniaTemplate.ViewModels
{
    public partial class AppViewModel : ReactiveObject
    {
        public void Quit()
        {
            Environment.Exit(1);
        }
    }
}
