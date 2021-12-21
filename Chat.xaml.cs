using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
namespace System.Windows.Controls
{
    public static class MyExt
    {
        public static void PerformClick(this Button btn)
        {
            btn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }
    }
    public static class Ext2
    {
        public static void AppendText(this RichTextBox box, string text, SolidColorBrush brush)
        {
            TextRange tr = new TextRange(box.Document.ContentEnd, box.Document.ContentEnd);
            tr.Text = text;
            tr.ApplyPropertyValue(TextElement.ForegroundProperty, brush);
        }
    }
}
namespace KursachAng
{
    /// <summary>
    /// Логика взаимодействия для Chat.xaml
    /// </summary>
    ///
    public partial class Chat : Window
    {
        string URL;
        string Name;
        int err;
        int chatWindowItemIndex = 0;

        System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
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
        public Chat(string name, string url)
        {
            InitializeComponent();
            this.Name = name;
            this.URL = url;
            this.Title = name;


            var client = new RestClient(URL);
            RestRequest request = new RestRequest("/api/hello", Method.GET);
            insertItemInChatWindow(JObject.Parse(client.Execute(request).Content)["message"]);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var client = new RestClient(URL);

            RestRequest request = null;

            if (SendMsgBox.Text.ToString() != "" && SendMsgBox.Text.ToCharArray()[0] == '/')
            {
                request = new RestRequest("/api" + SendMsgBox.Text.ToString(), Method.GET);
            }
            else if (SendMsgBox.Text.ToString() != "")
            {
                request = new RestRequest("/api/", Method.POST);
                request.AddJsonBody(new { msg = SendMsgBox.Text});
            }
            else request = new RestRequest("/api/", Method.GET);

        

            var getResult = client.Execute(request);

            dynamic json = JObject.Parse(getResult.Content);
            insertItemInChatWindow(json["message"]);
        }

        private void insertItemInChatWindow(dynamic message)
        {
            ChatWindow.Items.Insert(chatWindowItemIndex, message);
            chatWindowItemIndex++;

            SendMsgBox.Text = "";
            SendMsgBox.Tag = "Введите сюда сообщение...";
        }

        private void ChatWindow_DoubleClick(object sender, MouseEventArgs e)
        {
            int index = ChatWindow.SelectedIndex;
            if (index >= 0)
            {
                SendMsgBox.Text = "цитата: ^" + ChatWindow.Items[index].ToString() + "^";
            }
        }
        private void SendMsgBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.KeyDown += HandleKeyPress;
        }
        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Send.PerformClick();
            }
        }
    }
}