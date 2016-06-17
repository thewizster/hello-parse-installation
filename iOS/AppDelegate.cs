using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Foundation;
using UIKit;

namespace HelloParse.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init();

            // initialize ParseClient.
            // NOTE: Change sever URL. obviously you can't run parse-server on your device.
            Parse.ParseClient.Initialize(new Parse.ParseClient.Configuration
            {
                ApplicationId = "myAppId",
                Server = "http://localhost:1337/parse/"
            });

            UpdateInstallation();

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
		}

        public async void UpdateInstallation() {
            Parse.ParseInstallation installation = Parse.ParseInstallation.CurrentInstallation;
            installation["lastStartedAt"] = DateTime.UtcNow;
            await installation.SaveAsync();
        }
    }
}

