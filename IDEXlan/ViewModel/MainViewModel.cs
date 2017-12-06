using ICSharpCode.AvalonEdit.Document;
using IDEXlan.Model;
using IDEXlan.Analizer;
using System.IO;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;

namespace IDEXlan.ViewModel
{
    public class MainViewModel: NotifyBase
    {
        string rutaArchivoActual;
        string[] tokens = new string[0];
        private TextDocument code;

        public TextDocument Code
        {
            get { return code; }
            set
            {
                code = value;
                LinesCount = code.LineCount + "";
                OnPropertyChanged("Code");
            }
        }

        private List<TablaSimbolosModel> lexicoAnalizer;

        public List<TablaSimbolosModel> LexicoAnalizer
        {
            get { return lexicoAnalizer; }
            set
            {
                lexicoAnalizer = value;
                OnPropertyChanged("LexicoAnalizer");
            }
        }

        private List<ErrorTableModel> syntaticAnalizerL;

        public List<ErrorTableModel> SyntaticAnalizerL
        {
            get { return syntaticAnalizerL; }
            set
            {
                syntaticAnalizerL = value;
                OnPropertyChanged("SyntaticAnalizerL");
            }
        }

        private string textOutput;

        public string TextOutput
        {
            get { return textOutput; }
            set
            {
                textOutput = value;
                OnPropertyChanged("TextOutput");
            }
        }

        private string linesCount;

        public string LinesCount
        {
            get { return $"Numero de lineas: {linesCount}";  }
            set
            {
                linesCount = value;
                OnPropertyChanged("LinesCount");
            }
        }

        private Visibility Visibility;

        public Visibility Visibilidad
        {
            get { return Visibility; }
            set { Visibility = value; OnPropertyChanged("Visibilidad"); }
        }

        public ICommand TokensBtnCommand { get; set; }
        public ICommand SaveBtnCommand { get; set; }
        public ICommand FastSaveBtnCommand { get; set; }
        public ICommand OpenBtnCommand { get; set; }
        public ICommand LexicoAnalizerBtnCommand { get; set; }
        public ICommand CloseBtnCommand { get; }
        public ICommand AboutBtnCommand { get; set; }
        public ICommand CloseErrorWinCommand { get; set; }

        public MainViewModel()
        {
            rutaArchivoActual = string.Empty;
            Visibilidad = Visibility.Collapsed;
            Code = new TextDocument();
            LexicoAnalizer = new List<TablaSimbolosModel>();
            SyntaticAnalizerL = new List<ErrorTableModel>();
            TokensBtnCommand = new CommandBase((p) => SyntaticAnalizer(p));
            SaveBtnCommand = new CommandBase(p => GuardarArchivo(Code.Text));
            FastSaveBtnCommand = new CommandBase(p => GuardadoRapido());
            OpenBtnCommand = new CommandBase(p => AbrirArchivoUI());
            LexicoAnalizerBtnCommand = new CommandBase(p => LexicoAnalizerUI());
            AboutBtnCommand = new CommandBase(p => new AcercaDe().Show());
            CloseBtnCommand = new CommandBase(p => { Window w = p as Window; w.Close(); });
            CloseErrorWinCommand = new CommandBase(p => Visibilidad = Visibility.Collapsed);
        }

        public void GuardarArchivo(string codigo)
        {
            if (rutaArchivoActual != string.Empty)
            {
                File.WriteAllText(rutaArchivoActual, codigo);
                MessageBox.Show($"El documento ha sido guardado exitosamente en:\n{rutaArchivoActual}", "¡Guardado con exito!");
            }
            else if ((rutaArchivoActual == string.Empty) && (codigo != string.Empty))
            {
                SaveFileDialog save = new SaveFileDialog
                {
                    Filter = "Archivo de codigo xlang|*.xlang"
                };
                var res = save.ShowDialog();
                if (res == true)
                {
                    rutaArchivoActual = save.FileName;
                    File.WriteAllText(rutaArchivoActual, codigo);
                    MessageBox.Show($"El documento ha sido guardado exitosamente en:\n{rutaArchivoActual}", "¡Guardado con exito!");
                }
            }
            else
            {
                MessageBox.Show("No se ha abierto un archivo de xlang aun", "Error");
            }
        }

        public void AbrirArchivoUI()
        {
            OpenFileDialog abrir = new OpenFileDialog
            {
                Title = "Abrir archivo xlang",
                Filter = "Archivo de codigo xlang|*.xlang"
            };
            var res = abrir.ShowDialog();
            if (res == true)
            {
                rutaArchivoActual = abrir.FileName;
                Code.Text = File.ReadAllText(rutaArchivoActual);
            }
        }

        public void GuardadoRapido()
        {
            if (rutaArchivoActual != string.Empty || Code.Text != string.Empty)
            {
                MessageBoxResult respuesta = MessageBox.Show("¿Deseas guardar el archivo existente?", "Antes de crear nuevo", MessageBoxButton.YesNoCancel);
                switch (respuesta)
                {
                    case MessageBoxResult.Yes:
                        GuardarArchivo(Code.Text);
                        break;
                    case MessageBoxResult.No:
                        rutaArchivoActual = string.Empty;
                        Code.Text = string.Empty;
                        break;
                    case MessageBoxResult.Cancel:
                        break;
                }
            }
        }

        public void LexicoAnalizerUI()
        {
            TextOutput = "";
            List<string> tokensLista = new List<string>();
            if (Code.Text != string.Empty)
            {
                tokens = Code.Text.Split(' ');
                foreach (var item in tokens)
                {
                    if (item.Contains("\n"))
                    {
                        string[] temp = item.Split('\n');
                        foreach (var tempItem in temp)
                        {
                            if (tempItem.Contains("\r"))
                                tokensLista.Add(tempItem.Substring(0, tempItem.Length - 1));
                            else
                                tokensLista.Add(tempItem);
                        }
                    }
                    else
                    {
                        tokensLista.Add(item);
                    }
                }
                tokens = tokensLista.ToArray();
                foreach (var item in tokens)
                    TextOutput += item + "\n";

            }
            else
            {
                MessageBox.Show("No hay datos en el archivo, abre uno o crealo", "Importante");
            }

            ExpresionesReg reg = new ExpresionesReg();
            List<TablaSimbolosModel> simbolos = new List<TablaSimbolosModel>();
            simbolos.Clear();
            if (tokens.Length > 0 || tokens.Equals(null))
            {
                foreach (var item in tokens)
                {
                    simbolos.Add(new TablaSimbolosModel
                    {
                        Simbolo = item,
                        Definicion = reg.ConvertirToken(item),
                        Comentario = ""
                    });
                }
                LexicoAnalizer = null;
                LexicoAnalizer = simbolos;
            }
            else
            {
                MessageBox.Show("Antes de generar la tabla de tokens genera los tokens", "Error, no se han generado tokens", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public void SyntaticAnalizer(object parameter)
        {
            //Grid g = parameter as Grid;
            Visibilidad = Visibility.Visible;
            SyntacticAnalizer analizer = new SyntacticAnalizer(Code.Text);
            SyntaticAnalizerL = analizer.Analize();
        }

    }
}
