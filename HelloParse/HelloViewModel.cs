using System;
using System.ComponentModel;
using Parse;
using System.Threading.Tasks;

namespace HelloParse
{
	public class HelloViewModel : INotifyPropertyChanged
	{
		public HelloViewModel()
		{
			// initialize the properties
			this.Message = "Contacting extraterrestrials. Please wait...";
			this.IsWorking = true;

            // Save data to parse-server then update the view model properties
            var result = SaveToParseServerAsync("Hello World!");

            result.ContinueWith((antecedant) =>
            {
                this.Message = antecedant.Result;
                this.IsWorking = false;
            });
        }

        #region Parse code
        public async Task<string> SaveToParseServerAsync(string msgToSave)
		{
            string result = msgToSave;
            ParseInstallation installation = ParseInstallation.CurrentInstallation;
            this.InstallationId = installation.InstallationId.ToString();

            // create a ParseObject and set the message data
            var world = new ParseObject("World");
            world["message"] = "Hello world!";
            world["installation"] = installation; // if removed saves every time without exception

            // Attempt to save to the parse-server
            var contTask = world.SaveAsync();
            await contTask.ContinueWith((antecedant) =>
            {
                if (antecedant.Status == TaskStatus.Faulted)
                {
                    result = antecedant.Exception.InnerException.ToString();
                }
            });

            return result;
        }
        #endregion

        #region INotifyPropertyChanged Implimentation
        public event PropertyChangedEventHandler PropertyChanged;

		// very basic implimentation
		public void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				var e = new PropertyChangedEventArgs(propertyName);
				this.PropertyChanged(this, e);
			}
		}
#endregion

        #region Bindable properties
		private string _message = "";
		public string Message
		{
			get { return _message; }
			set
			{
				_message = value;
				this.OnPropertyChanged("Message");
			}
		}

        private string _installationId = "";
        public string InstallationId
        {
            get { return _installationId; }
            set
            {
                _installationId = value;
                this.OnPropertyChanged("InstallationId");
            }
        }

        private bool _isWorking = false;
		public bool IsWorking
		{
			get { return _isWorking; }
			set
			{
				_isWorking = value;
				this.OnPropertyChanged("IsWorking");
			}
		}
        #endregion
	}
}

