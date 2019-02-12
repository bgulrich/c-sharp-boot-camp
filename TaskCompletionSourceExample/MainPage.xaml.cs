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

namespace TaskCompletionSourceExample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private TaskCompletionSource<object> tcs;

        public MainPage()
        {
            this.InitializeComponent();
            Workflow();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            tcs.TrySetResult(null);
        }

        private async Task Workflow()
        {
            tcs = new TaskCompletionSource<object>();

            // wait for first click
            await tcs.Task;

            int iteration = 1;

            while(true)
            {
                Button.IsEnabled = false;

                await CountDown($"Long running task {iteration++}");

                tcs = new TaskCompletionSource<object>();

                Button.IsEnabled = true;

                await tcs.Task;
            }
        }

        private async Task CountDown(string prefix)
        {
            for (int i = 3; i > 0; --i)
            {
                InfoTextBlock.Text = $"{prefix}\r\nCompleting in...{i}";
                await Task.Delay(1000);
            }

            InfoTextBlock.Text = $"{prefix}\r\nComplete";
        }
    }
}
