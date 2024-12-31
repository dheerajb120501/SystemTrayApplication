using Microsoft.Windows.PushNotifications;
using Windows.ApplicationModel.DataTransfer;

namespace WinFormUnpackaged
{
    public class WNS
    {
        public WNS() { }

        public Boolean IsSupported()
        {
            return PushNotificationManager.IsSupported();
        }

        public async Task<PushNotificationChannel> RequestChannelAsync()
        {
            Guid remoteId = Guid.Parse("1df54533-3101-4bd7-88f6-8de401939158");
            var channelOperation = PushNotificationManager.Default.CreateChannelAsync(remoteId);

            channelOperation.Progress = (sender, args) =>
            {
                if (args.status == PushNotificationChannelStatus.InProgress)
                {
                    Console.WriteLine("INFO: Channel request is in progress.");
                }
                else if (args.status == PushNotificationChannelStatus.InProgressRetry)
                {
                    Console.WriteLine($"ERROR: {args.extendedError} \nThe channel request is in back-off retry mode because of a retryable error! Expect delays in acquiring it. RetryCount = {args.retryCount}");
                }
            };

            var result = await channelOperation;

            if (result.Status == PushNotificationChannelStatus.CompletedSuccess)
            {

                var channelUri = result.Channel.Uri;

                SetTextToClipboard(channelUri.ToString());
                Console.WriteLine($"INFO: Channel URI: {channelUri}");

                return result.Channel;
            }
            else if (result.Status == PushNotificationChannelStatus.CompletedFailure)
            {
                Console.WriteLine($"ERROR: {result.ExtendedError}\nWe hit a critical non-retryable error with channel request!");
                throw new Exception("We hit a critical non-retryable error with channel request!");
            }
            else
            {
                Console.WriteLine($"ERROR: {result.ExtendedError}\nSome other failure occurred!");
                throw new Exception("Some other failure occurred!");
            }
        }

        public void SetTextToClipboard(string text) { 
            var dataPackage = new DataPackage(); 
            dataPackage.SetText(text); 
            Windows.ApplicationModel.DataTransfer.Clipboard.SetContent(dataPackage); 
        }
    }
}
