using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IDEXlan.Model
{
    //Clase base para la creacion de commandos para aplicar en XAML con MVVM
    public class CommandBase : ICommand
    {        
        private Action<object> execAction;
        private Func<object, bool> canExecFunc;

        public CommandBase(Action<object> execAction)
        {
            this.execAction = execAction;
            this.canExecFunc = null;
        }

        public CommandBase(Action<object> execAction, Func<object, bool> canExecFunc)
        {
            this.execAction = execAction;
            this.canExecFunc = canExecFunc;
        }

        public bool CanExecute(object parameter)
        {
            if (canExecFunc != null)
                return canExecFunc.Invoke(parameter);
            else
                return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (execAction != null)
                execAction.Invoke(parameter);
        }
    }
}
