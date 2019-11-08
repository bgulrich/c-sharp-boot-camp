using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UiGoodAndBad
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void BadClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            button.IsEnabled = false;
            BadProgressRing.IsActive = true;

            for (var i = 0; i <= 10; ++i)
            {
                // simulate long-running synchronous work
                Task.Delay(1000).Wait();

                BadProgressBar.Value = 10 * i;
            }

            button.IsEnabled = true;
            BadProgressRing.IsActive = false;
        }

        private async void GoodClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            button.IsEnabled = false;
            GoodProgressRing.IsActive = true;

            for (var i = 0; i <= 10; ++i)
            {
                // simulate long-running synchronous work
                await Task.Delay(1000);

                GoodProgressBar.Value = 10 * i;
            }

            button.IsEnabled = true;
            GoodProgressRing.IsActive = false;
        }

    }
}
