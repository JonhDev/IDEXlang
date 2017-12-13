using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDEXlan.Model
{
    //Clase base encargada de implementar INotifyPropertyChanged para aplicar patron MVVM de manera sencilla
    public class NotifyBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        //Metodo que lanza el evento PropertyChangedEventHandler para aplicar MVVM 
        public virtual void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
