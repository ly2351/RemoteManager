using RemoteManager.Services;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace RemoteManager.Views
{
    /// <summary>
    /// JsonEditorWindow.xaml 的交互逻辑
    /// </summary>
    public partial class JsonEditorWindow : Window
    {
        public JsonEditorWindow()
        {
            InitializeComponent();
            LoadJson();
        }

        private void LoadJson()
        {
            JsonTextBox.Text = File.ReadAllText(AppPath.JsonPath);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 校验 JSON 格式
                JsonDocument.Parse(JsonTextBox.Text);

                File.WriteAllText(AppPath.JsonPath, JsonTextBox.Text);
                DialogResult = true;
                Close();
            }
            catch
            {
                MessageBox.Show("JSON 格式不正确", "错误",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
