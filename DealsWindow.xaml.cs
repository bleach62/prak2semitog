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
using System.Windows.Shapes;

namespace prak2semitog
{
    /// <summary>
    /// Логика взаимодействия для DealsWindow.xaml
    /// </summary>
    public partial class DealsWindow : Window
    {
        public Connection connection = new Connection();
        public Supply supply = new Supply();
        public Demands demand = new Demands();
        public DealsWindow()
        {
            InitializeComponent();
            SupplyDataGrid.ItemsSource = connection.Practica.Supply.ToList();
            DemandDataGrid.ItemsSource = connection.Practica.Demands.ToList();
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(DemandPercentTextBox.Text) || string.IsNullOrEmpty(SupplyPercentTextBox.Text))
            {
                return;
            }
            Models.Deals deals = new Models.Deals();
            deals.Id_Demand = Convert.ToInt32(DemandPercentTextBox.Text);
            deals.Id_Supply = Convert.ToInt32(SupplyPercentTextBox.Text);
            connection.Practica.Deals.Add(deals);
            connection.Practica.SaveChanges();
            MessageBox.Show("Сделка зафиксирована");
        }

        private void DemandDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            demand = DemandDataGrid.SelectedItem as Demands;
            DemandPercentTextBox.Text = demand.Id_Demand.ToString();
        }

        private void SupplyDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            supply = SupplyDataGrid.SelectedItem as Supply;
            SupplyPercentTextBox.Text = supply.Id_Supply.ToString();
        }
    }
}
