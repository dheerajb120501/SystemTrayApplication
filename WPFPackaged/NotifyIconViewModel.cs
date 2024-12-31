using System.Windows;
using System.Windows.Input;
using Microsoft.Windows.AppNotifications.Builder;
using Microsoft.Windows.AppNotifications;

namespace WPFPackaged
{
    public class NotifyIconViewModel
    {

        public ICommand GetChannelUriCommand
        {
            get { return new DelegateCommand { CommandAction = () =>  GetChannelIdentifier() }; }
        }

        async void GetChannelIdentifier()
        {
            var wns = new WNS();

            if (wns.IsSupported())
            {
                var channel = await wns.RequestChannelAsync();
                var appNotification = new AppNotificationBuilder()
                            .AddText($"Channel Uri copied to the clipboard. Valid till {channel.ExpirationTime}")
                            .BuildNotification();
                AppNotificationManager.Default.Show(appNotification);
            }
            else
            {
                throw new Exception("WNS not supported");
            }
        }
    }
}