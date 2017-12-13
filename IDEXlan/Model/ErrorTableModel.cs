using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDEXlan.Model
{
    //Clase modelo que ayuda al listado de los errores en los analizadores
    public class ErrorTableModel
    {
        public int Line { get; set; }
        public string Error { get; set; }
    }
}
