namespace WinFormUnpackaged
{
    public class TrayContext : ApplicationContext
    {
        private NotifyIcon trayIcon;

        public TrayContext()
        {
            trayIcon = new NotifyIcon()
            {
                Icon = SystemIcons.Information,
                ContextMenuStrip = new ContextMenuStrip()
                {
                    Items = { new ToolStripMenuItem("Get Channel Identifier", null, GetChannelIdentifier) },
                },
                Visible = true
            };
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
