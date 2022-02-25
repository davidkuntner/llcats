using System.Windows;
using System.Windows.Input;

namespace llcats.TrayIcon
{
    public class NotifyIconViewModel
    {
        /// <summary>
        /// Shows a window, if none is already open.
        /// </summary>
        
        public ICommand ShowWindowCommand
        {
            get
            {
                return new DelegateCommand
                {
                    CanExecuteFunc = () => Application.Current.MainWindow != null ||
                        !Application.Current.MainWindow.IsVisible,

                    CommandAction = () =>
                    {
                        Application.Current.MainWindow = new MainWindow();
                        Application.Current.MainWindow.Show();
                    }
                };
            }
        }

        /// <summary>
        /// Shuts down the application
        /// </summary>

        public ICommand ExitApplicationCommand
        {
            get
            {
                return new DelegateCommand { CommandAction = () => Application.Current.Shutdown() };
            }
        }
    }
}