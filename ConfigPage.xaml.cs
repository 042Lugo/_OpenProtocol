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
using System.IO;

namespace IHM_1v0
{
    /// <summary>
    /// Lógica interna para ConfigPage.xaml
    /// </summary>
    public partial class ConfigPage : Window
    {
        public ConfigPage()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Conf add = new Conf();
            if (!File.Exists("setup.msh"))
                add.Show();
            else
                MessageBox.Show("Já existe uma operação denifida... Remova a atual para criar uma nova");
        }

        private void btnComecar_Click(object sender, RoutedEventArgs e)
        {

            IHM ihm = new IHM();
            if (!File.Exists("setup.msh"))
                MessageBox.Show("Nenhuma operação definida...");
            else
            {
                this.Close();
                ihm.Show();
            }
        }

        private void btnApagar_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists("setup.msh"))
                File.Delete("setup.msh");
            else
                MessageBox.Show("Nenhuma operação definida...");
        }
    }
}
