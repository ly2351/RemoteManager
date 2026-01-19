using RemoteManager.Models;
using System.Windows;

namespace RemoteManager.Views
{
    public partial class ServerEditDialog : Window
    {
        private readonly RemoteServer _server;

        public ServerEditDialog(RemoteServer server)
        {
            InitializeComponent();

            _server = server;
            DataContext = _server;

            // PasswordBox 不能直接绑定，这里手动赋值
            PasswordBoxPassword.Password = _server.Password ?? string.Empty;
        }

        /// <summary>
        /// 对外统一调用入口（新增 / 编辑共用）
        /// </summary>
        public static bool Show(RemoteServer server)
        {
            var dialog = new ServerEditDialog(server)
            {
                Owner = Application.Current.MainWindow
            };

            return dialog.ShowDialog() == true;
        }

        /// <summary>
        /// 确定
        /// </summary>
        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            // 手动把 PasswordBox 的值写回模型
            _server.Password = PasswordBoxPassword.Password;

            // 简单校验（可按需加强）
            if (string.IsNullOrWhiteSpace(_server.Name))
            {
                MessageBox.Show("服务器名称不能为空");
                return;
            }

            if (string.IsNullOrWhiteSpace(_server.Address))
            {
                MessageBox.Show("服务器地址不能为空");
                return;
            }

            DialogResult = true;
        }

        /// <summary>
        /// 取消
        /// </summary>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
