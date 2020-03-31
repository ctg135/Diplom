using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using Newtonsoft.Json;
using Web_Service.Models;

namespace Testter
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            switch(ModelSelector.Text)
            {
                case "Autho":
                    Model.Text = JsonConvert.SerializeObject(new Autho() { Login = "testLogin", Password = "testPassword" });
                    break;
                case "Request":
                    Model.Text = JsonConvert.SerializeObject(new Request() { Session = null, Query = null } );
                    break;
            }
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Testter");

            HttpRequestMessage request = new HttpRequestMessage();

            request.RequestUri = new Uri(Address.Text);
            request.Headers.Add("Accept", "application/json");
            

            switch (Method.Text)
            {
                case "GET":
                    request.Method = HttpMethod.Get;
                    break;
                case "PUT":
                    request.Method = HttpMethod.Put;
                    request.Content = new StringContent(RequestContent.Text);
                    break;
                case "POST":
                    request.Method = HttpMethod.Post;
                    request.Content = new StringContent(RequestContent.Text);
                    break;
                case "DELETE":
                    request.Method = HttpMethod.Delete;
                    request.Content = new StringContent(RequestContent.Text);
                    break;
            }


            string temp1 = "H E A D E R S\n";
            foreach (var header in request.Headers)
            {
                temp1 += $"# {header.Key}:\n";
                List<string> vals = new List<string>(header.Value);
                foreach (var val in vals)
                {
                    temp1 += $" # {val}\n";
                }
            }
            temp1 += "C O N T E N T  H E A D E R S\n";
            if(request.Content != null)
                foreach (var header in request.Content.Headers)
                {
                    temp1 += $"# {header.Key}:\n";
                    List<string> vals = new List<string>(header.Value);
                    foreach (var val in vals)
                    {
                        temp1 += $" * {val}\n";
                    }
                }
            RequestHeaders.Text = temp1;

            HttpResponseMessage responseMessage = await client.SendAsync(request);

            ResponseContent.Text = responseMessage.Content.ReadAsStringAsync().Result;

            string temp = "H E A D E R S\n";
            foreach(var header in responseMessage.Headers)
            {
                temp += $"# {header.Key}:\n";
                List<string> vals = new List<string>(header.Value);
                foreach (var val in vals)
                {
                    temp += $" * {val}\n";
                }
            }
            temp += responseMessage.StatusCode + " " + responseMessage.ReasonPhrase;
            temp += "C O N T E N T  H E A D E R S\n";
            if (responseMessage.Content != null)
                foreach(var header in responseMessage.Content.Headers)
                {
                    temp += $"# {header.Key}:\n";
                    List<string> vals = new List<string>(header.Value);
                    foreach (var val in vals)
                    {
                        temp += $" * {val}\n";
                    }
                }
                ResponseHeaders.Text = temp;
        }
    }
}
