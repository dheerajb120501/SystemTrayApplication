using System.Windows;
using NotifyIcon = System.Windows.Forms.NotifyIcon;

namespace WPFWithWinFormsPackaged
{
    public partial class App : System.Windows.Application
    {

        private NotifyIcon trayIcon;
        protected override void OnStartup(StartupEventArgs e)
        {
            trayIcon = new NotifyIcon()
            {
                Icon = ProjectResources.Red,
                ContextMenuStrip = new ContextMenuStrip()
                {
                    Items = { new ToolStripMenuItem("Get Channel Identifier", null, GetChannelIdentifier) },
                },
                Visible = true
            };
        
            base.OnStartup(e);        
        }

        async void GetChannelIdentifier(object? sender, EventArgs e)
        {
            var wns = new WNS();

            if (wns.IsSupported())
            {
                var channel = await wns.RequestChannelAsync();
                trayIcon.BalloonTipText = $"Channel url copied to clipboard. Expiry in {channel.ExpirationTime}";
                trayIcon.ShowBalloonTip(2000);
            }
            else
            {
                throw new Exception("WNS not supported");
            }
        }
    }

}
