using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace IHM_1v0
{
    /// <summary>
    /// Lógica interna para Conf.xaml
    /// </summary>
    public partial class Conf : Window
    {
        public Conf()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;

        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {

            saveFile saveFile = new();

            saveFile.toFile(lbIP.Text,
                lbPorta.Text,
                lbNome.Text,
                cbBanco.IsChecked.ToString(),
                lbBanco.Text,
                lbUsuario.Text,
                lbSenha.Text );

            this.Close();
            

        }
    }
}
