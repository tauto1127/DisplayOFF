using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

namespace DisplayOFF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {

        [DllImport("user32.dll")]
        static extern bool GetAsyncKeyState(int nVirtKey);

        private NotifyIcon _notifyIcon;
       

        private Task task = Task.Run((() =>
        {
            while (true)
            {
                if (GetAsyncKeyState(0x5B) && GetAsyncKeyState(0x4F))
                {
                    PowerOff();
                }
            }
        }));
        

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OffButton_OnClick(object sender, RoutedEventArgs e)
        {
            PowerOff();
        }
        const int SC_MONITORPOWER = 0xf170;
        const int WM_SYSCOMMAND = 0x112;
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr PostMessage(int hWnd, uint Msg, int wParam, int lParam);
        public static void PowerOff()
        {
//モニター停止
            try
            {
                PostMessage(-1, WM_SYSCOMMAND, SC_MONITORPOWER, 2);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void KeyloggerButton_OnClick(object sender, RoutedEventArgs e)
        {
            bool result = GetAsyncKeyState(0x0D);
            if (result)
            {
                Label.Content = "押されています";
            }
        }

        private void MinButton_OnClick(object sender, RoutedEventArgs e)
        {
            
            ShowInTaskbar = false;

            _notifyIcon = new NotifyIcon();
            _notifyIcon.Text = "DisplayOFF";
            _notifyIcon.Icon = new Icon("taskTrayIcon.ico");

            _notifyIcon.Visible = true;

            ContextMenuStrip menuStrip = new ContextMenuStrip();

            ToolStripMenuItem menuItem = new ToolStripMenuItem();
            menuItem.Text = "終了";
            menuStrip.Items.Add(menuItem);
            menuItem.Click += new EventHandler(exit_Click);

            _notifyIcon.ContextMenuStrip = menuStrip;

            _notifyIcon.MouseClick += new MouseEventHandler(_notifyIcon_MouseClick);
            
            this.Visibility = Visibility.Collapsed;
        }

        private void _notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                this.Visibility = System.Windows.Visibility.Visible;
                this.WindowState = WindowState.Normal;
            }
        }

        private void exit_Click(object sender, EventArgs e)
        {
            _notifyIcon.Dispose();
            System.Windows.Application.Current.Shutdown();
        }
    }
}