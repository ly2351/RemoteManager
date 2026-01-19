using RemoteManager.Models;
using RemoteManager.Services;
using RemoteManager.Views;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace RemoteManager.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        // ----------------------
        // 数据
        // ----------------------
        public ObservableCollection<RemoteServer> Servers { get; }

        private RemoteServer _selectedServer;
        public RemoteServer SelectedServer
        {
            get => _selectedServer;
            set
            {
                if (_selectedServer != value)
                {
                    _selectedServer = value;
                    OnPropertyChanged(nameof(SelectedServer));

                    // 通知命令刷新可用状态
                    EditCommand.RaiseCanExecuteChanged();
                    DeleteCommand.RaiseCanExecuteChanged();
                }
            }
        }

        // ----------------------
        // 命令
        // ----------------------
        public RelayCommand AddCommand { get; }
        public RelayCommand EditCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand ReloadCommand { get; }
        public RelayCommand<RemoteServer> ConnectCommand { get; }

        // ----------------------
        // 构造
        // ----------------------
        public MainViewModel()
        {
            Servers = new ObservableCollection<RemoteServer>(ServerStorageService.Load());

            AddCommand = new RelayCommand(Add);
            EditCommand = new RelayCommand(Edit, () => SelectedServer != null);
            DeleteCommand = new RelayCommand(Delete, () => SelectedServer != null);
            ReloadCommand = new RelayCommand(Reload);
            ConnectCommand = new RelayCommand<RemoteServer>(Connect);
        }

        // ----------------------
        // CRUD 方法
        // ----------------------
        private void Add()
        {
            var server = new RemoteServer { Id = Guid.NewGuid().ToString() };

            // 弹窗编辑
            if (ServerEditDialog.Show(server))
            {
                Servers.Add(server);
                Save();
            }
            Reload();
        }

        private void Edit()
        {
            if (SelectedServer == null) return;

            var copy = new RemoteServer
            {
                Id = SelectedServer.Id,
                Name = SelectedServer.Name,
                Address = SelectedServer.Address,
                UserName = SelectedServer.UserName,
                Password = SelectedServer.Password
            };

            if (ServerEditDialog.Show(copy))
            {
                var target = Servers.First(s => s.Id == copy.Id);
                target.Name = copy.Name;
                target.Address = copy.Address;
                target.UserName = copy.UserName;
                target.Password = copy.Password;
                Save();
            }
            Reload();
        }

        private void Delete()
        {
            if (SelectedServer == null) return;

            if (MessageBox.Show("确认删除该服务器？", "警告", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                return;

            Servers.Remove(SelectedServer);
            SelectedServer = null; // 删除后取消选中
            Save();
            Reload();
        }

        public void Reload()
        {
            Servers.Clear();
            foreach (var s in ServerStorageService.Load())
                Servers.Add(s);
        }

        private void Connect(RemoteServer server)
        {
            if (server == null) return;

            if (MessageBox.Show($"确认连接 {server.Name}？", "确认", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                RdpService.Connect(server);
            }
        }

        // ----------------------
        // 保存 JSON
        // ----------------------
        private void Save()
        {
            ServerStorageService.Save(Servers);
        }

        // ----------------------
        // INotifyPropertyChanged
        // ----------------------
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
