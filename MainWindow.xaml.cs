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
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace KursachAng
{
 /// <summary>
 /// Interaction logic for MainWindow.xaml
 /// </summary>
public partial class MainWindow : Window
 {
 public string URL;
    public string Header = "Bearer ";
    public string Name;
    public void RemoveText(object sender, EventArgs e)
    {
        TextBox instance = (TextBox)sender;
        if (instance.Text == instance.Tag.ToString())
            instance.Text = "";
    }
    public void AddText(object sender, EventArgs e)
    {
        TextBox instance = (TextBox)sender;
        if (string.IsNullOrWhiteSpace(instance.Text))
            instance.Text = instance.Tag.ToString();
    }
    public MainWindow()
    {
        InitializeComponent();
    }
    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {

    }
    private void Button_Click(object sender, RoutedEventArgs e)
    {
        string IP = Text1.Text;
        string Port = Text2.Text;
        if (Port == "" || Port == "Порт (8080)")
        {
            Port = "8080";
        }
        URL = "http://" + IP + ":" + Port;
            try
            {
                var client = new RestClient(URL);
                var request = new RestRequest("", Method.GET);
                var getresult = client.Execute(request);
                if (getresult.StatusCode.ToString() == "OK")
                {
                    dynamic json = JObject.Parse(getresult.Content);
                    Name = json.message;
                    Chat chat = new Chat(Name, URL);
                    chat.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Не удалось подключиться к серверу", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            }
            catch
            {
                MessageBox.Show("Введены неверные данные", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}