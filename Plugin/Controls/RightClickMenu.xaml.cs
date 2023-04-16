// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RightClickMenu.xaml.cs" company="KriaSoft LLC">
//   Copyright © 2013 Konstantin Tarkus, KriaSoft LLC. See LICENSE.txt
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;

namespace MilkyAmiBroker.Plugins.Controls
{
    /// <summary>
    /// Interaction logic for RightClickMenu user control.
    /// </summary>
    public partial class RightClickMenu : UserControl
    {
        private readonly DataSource dataSource;

        public RightClickMenu(DataSource dataSource)
        {
            this.dataSource = dataSource;
            this.InitializeComponent();
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            var loginBox = new LoginBox(this.dataSource);

            var window = new Window
            {
                Title = "Connect to the Data Source",
                Content = loginBox,
                ResizeMode = ResizeMode.NoResize,
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            window.ShowDialog();
        }

        private void Disconnect_Click(object sender, RoutedEventArgs e)
        {
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "This is a demo plug-in built with AmiBroker .NET SDK. For more info visit: ",
                "AmiBroker® Demo Milky Plug-in",
                MessageBoxButton.OK);
        }
    }
}
