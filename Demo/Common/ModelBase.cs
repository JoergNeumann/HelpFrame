using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Demo.Common
{
    public class ModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void SetProperty<T>(ref T storage, T value, [CallerMemberName]String propertyName = null)
        {
            if (value.Equals(storage))
                return;
            storage = value;
            this.OnPropertyChanged(propertyName);
        }
    }
}
