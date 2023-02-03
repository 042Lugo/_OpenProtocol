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
    /// Lógica interna para IHM.xaml
    /// </summary>
    public partial class IHM : Window
    {
        Controller controller = new Controller();
        public IHM()
        {
            
            InitializeComponent();
            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;
           
          

            if (File.Exists("setup.msh"))
            {
                string[] line = File.ReadAllLines("setup.msh");
                lbNome.Content = line[2].Replace("NAME=>", "");



                try
                {
                    controller.StartServer(line[0].Replace("IP=>", ""), int.Parse(line[1].Replace("PORT=>", "")));
                   
                }
                catch
                {
                    MessageBox.Show("Configuração inválida");
                    this.Close();
                }

            }

            System.Windows.Threading.DispatcherTimer dispatcherTimer1 = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer1.Tick += new EventHandler(dispatcherTimer1_Tick);
            dispatcherTimer1.Interval = new TimeSpan(0, 0, 5);
            dispatcherTimer1.Start();

            System.Windows.Threading.DispatcherTimer dispatcherTimer2 = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer2.Tick += new EventHandler(dispatcherTimer2_Tick);
            dispatcherTimer2.Interval = new TimeSpan(100);
            dispatcherTimer2.Start();
        }

        private void dispatcherTimer1_Tick(object sender, EventArgs e)
        {
            controller.message = "00209999000000000000\0";
            controller.sendMID();
        }
        private void dispatcherTimer2_Tick(object sender, EventArgs e)
        {
            if (!controller._connected)
                this.Visibility = Visibility.Hidden;

            lbTorque.Content = controller.torque;
            lbAngulo.Content = controller.angle;
            lbStatus.Content = controller.status;
          
            Uri resourceUri = new Uri(@"\images\img"+controller.batchCount+".jpg", UriKind.Absolute);
            imgProduto.Source = new BitmapImage(resourceUri);
            if (controller.status == "OK")
                lbStatus.Foreground = Brushes.GreenYellow;
            else
                lbStatus.Foreground = Brushes.Red;
        }

    }
}
