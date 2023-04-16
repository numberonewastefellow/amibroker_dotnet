// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoginBox.xaml.cs" company="KriaSoft LLC">
//   Copyright © 2013 Konstantin Tarkus, KriaSoft LLC. See LICENSE.txt
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Threading.Tasks;
using System.Windows.Controls;

namespace MilkyAmiBroker.Plugins.Controls
{
    /// <summary>
    /// Interaction logic for LoginBox.xaml
    /// </summary>
    public partial class LoginBox : UserControl
    {
        private readonly DataSource dataSource;

        public LoginBox(DataSource dataSource)
        {
            this.dataSource = dataSource;
            InitializeComponent();
            LogMe.Log("loginbox_init");
        }

        private bool issocketCOnnected = false;
        private void btnLogin_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                LogMe.Log($"Hello {txtUserName.Text}, broker:{dataSource.Broker}, dbpaht: {dataSource.DatabasePath}, maindow:{dataSource.MainWnd}");
                System.Windows.MessageBox.Show($"Hello {txtUserName.Text}, broker:{dataSource.Broker}, dbpaht: {dataSource.DatabasePath}, maindow:{dataSource.MainWnd}");
                if (!issocketCOnnected)
                {
                    issocketCOnnected = true;
                    LogMe.Log("socket connections is intiated from loginwindow");
                 MilkyAmiPlugin.StartSocketWithoutTask(new[] { "from loginwindow" });
                 LogMe.Log("finished socket connections is intiated from loginwindow");

                    //     Task.Run(() =>
                    //{
                    //    LogMe.Log("is side task");
                    //    return MilkyAmiPlugin.StartSocket(new[] { "" });
                    //});
                }
            }
            catch (System.Exception exception)
            {

              LogMe.Log(exception.Message);
            }
        }
    }
}
