using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;

namespace Unity.AOP.Demo.Samples
{
    /// <summary>
    /// Interaction logic for AsyncOperationsWindow.xaml
    /// </summary>
    public partial class AsyncOperationsWindow : Window
    {
        public AsyncOperationsWindow()
        {
            InitializeComponent();
        }

        private void OnLoadClick(object sender, RoutedEventArgs e)
        {
            LoadInCurrentThread();
        }

        private void LoadInCurrentThread()
        {
            foreach (var data in GetData())
            {
                dataTabs.Items.Add(new TabItem() { Header = "Tab", Content = new TextBlock() { Text = data } });
            }
        }

        private IEnumerable<string> GetData()
        {
            yield return LoadDataItemFromWebAddress("http://www.baidu.com/");
            yield return LoadDataItemFromWebAddress("http://www.google.com/");
            yield return LoadDataItemFromWebAddress("http://www.bing.com/");
            yield return LoadDataItemFromWebAddress("http://www.sohu.com/");
            yield return LoadDataItemFromWebAddress("http://www.sina.com/");
            yield return LoadDataItemFromWebAddress("http://www.qq.com/");
        }

        private string LoadDataItemFromWebAddress(string address)
        {
            return new StreamReader(WebRequest.CreateHttp(address).GetResponse().GetResponseStream()).ReadToEnd();
        }

        private void OnClearClick(object sender, RoutedEventArgs e)
        {
            dataTabs.Items.Clear();
        }
    }
}
