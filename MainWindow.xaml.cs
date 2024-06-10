using prak2semitog.Models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace prak2semitog
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void ToAgentWindow(object sender, RoutedEventArgs e)
        {
            Agent window = new Agent();
            window.Show();
        }
        private void ToDealsWindow(object sender, RoutedEventArgs e)
        {
            DealsWindow window = new DealsWindow();
            window.Show();
        }
        private void ToSuppliesWindow(object sender, RoutedEventArgs e)
        {
            Supplies window = new Supplies();
            window.Show();
        }
    }
}
