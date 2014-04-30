using Microsoft.Practices.Unity;
using System;
using System.Windows;
using Unity.AOP.Demo.Services;
using Unity.AOP.Logging;

namespace Unity.AOP.Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [Dependency]
        public ISoapSender Sender { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Sender.Send(null);
        }
    }
}
