namespace Starfield_DataManagement.Properties
{
    internal partial class Settings
    {
        public Settings()
        {
            PropertyChanged += Settings_PropertyChanged;
        }

        private void Settings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Default.Save();
        }

    }
}
