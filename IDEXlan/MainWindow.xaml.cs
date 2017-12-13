using IDEXlan.Analizer;
using IDEXlan.Model;
using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Windows;


namespace IDEXlan
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //Se hace uso de MVVM por lo cual toda la logica de programacion se encuentra en el View Model de esta clase
            //View Model de esta clase: MainViewModel.cs
        }
    }
}
