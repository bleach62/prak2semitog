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
    /// Логика взаимодействия для Agent.xaml
    /// </summary>
    public partial class Agent : Window
    {
        private Connection _connection = new Connection();
        int currentId = 0;
        public Agent()
        {
            InitializeComponent();
            var agents = _connection.Practica.Agents.ToList();
            AgentsDataGrid.ItemsSource = agents;
        }
        private void AgentsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AgentsDataGrid.SelectedItem != null)
            {
                Agents selectedRow = (Agents)AgentsDataGrid.SelectedItem;
                FirstNameTextBox.Text = selectedRow.FirstName;
                LastNameTextBox.Text = selectedRow.LastName;
                PatronymicTextBox.Text = selectedRow.MiddleName;
                DealPercentTextBox.Text = selectedRow.DealShare.ToString();
                currentId = Convert.ToInt32(selectedRow.Id_Agent.ToString());
            }


        }
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (currentId != 0)
            {
                Agents agent = new Agents();
                agent.FirstName = FirstNameTextBox.Text;
                agent.LastName = LastNameTextBox.Text;
                agent.MiddleName = PatronymicTextBox.Text;
                agent.DealShare = Convert.ToInt32(DealPercentTextBox.Text);
                var UpdateAgent = _connection.Practica.Agents.Where(x => (x.Id_Agent == currentId)).First();
                if (UpdateAgent != null)
                {
                    if (agent.FirstName != null)
                    {
                        _connection.Practica.Agents.Where(x => (x.Id_Agent == currentId)).First().FirstName = agent.FirstName;
                        _connection.Practica.SaveChanges();
                    }
                    if (agent.LastName != null)
                    {
                        _connection.Practica.Agents.Where(x => (x.Id_Agent == currentId)).First().LastName = agent.LastName;
                        _connection.Practica.SaveChanges();
                    }
                    if (agent.MiddleName != null)
                    {
                        _connection.Practica.Agents.Where(x => (x.Id_Agent == currentId)).First().MiddleName = agent.MiddleName;
                        _connection.Practica.SaveChanges();
                    }
                    if (agent.DealShare != null)
                    {
                        _connection.Practica.Agents.Where(x => (x.Id_Agent == currentId)).First().DealShare = agent.DealShare;
                        _connection.Practica.SaveChanges();
                    }
                    _connection.Practica.SaveChanges();

                }
                _connection = new Connection();
                var agents = _connection.Practica.Agents.ToList();
                AgentsDataGrid.ItemsSource = agents;
                FirstNameTextBox.Text = null;
                LastNameTextBox.Text = null;
                PatronymicTextBox.Text = null;
                DealPercentTextBox.Text = null;
                currentId = 0;
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (currentId != 0)
            {
                var filmDelete = _connection.Practica.Agents.Where(x => (x.Id_Agent == currentId)).First();
                var demantsDelete = _connection.Practica.Demands.Where(x => x.Id_Agent == currentId);
                var supplyDelete = _connection.Practica.Supply.Where(x => x.Id_Agent == currentId);
                _connection.Practica.Agents.Remove(filmDelete);

                foreach (var demant in demantsDelete)
                {
                    _connection.Practica.Demands.Remove(demant);
                    var dealsDelete = _connection.Practica.Deals.Where(x => x.Id_Demand == demant.Id_Demand);
                    foreach (var deal in dealsDelete)
                    {
                        _connection.Practica.Deals.Remove(deal);
                    }
                }
                foreach (var sup in supplyDelete)
                {
                    _connection.Practica.Supply.Remove(sup);
                }
                _connection.Practica.SaveChanges();
                _connection = new Connection();
                var agents = _connection.Practica.Agents.ToList();
                AgentsDataGrid.ItemsSource = agents;
                FirstNameTextBox.Text = null;
                LastNameTextBox.Text = null;
                PatronymicTextBox.Text = null;
                DealPercentTextBox.Text = null;
                currentId = 0;
            }
        }

        private void AddWindow_Click(object sender, RoutedEventArgs e)
        {
            {
                Agents agent = new Agents();
                agent.FirstName = FirstNameTextBox.Text;
                agent.LastName = LastNameTextBox.Text;
                agent.MiddleName = PatronymicTextBox.Text;
                agent.DealShare = Convert.ToInt32(DealPercentTextBox.Text);
                if (agent.FirstName != null && agent.LastName != null && agent.MiddleName != null && agent.DealShare != null)
                {
                    _connection.Practica.Agents.Add(agent);
                    _connection.Practica.SaveChanges();
                }
                else
                {
                    MessageBox.Show("Пустые столбцы");
                }
                _connection = new Connection();
                var agents = _connection.Practica.Agents.ToList();
                AgentsDataGrid.ItemsSource = agents;
                FirstNameTextBox.Text = null;
                LastNameTextBox.Text = null;
                PatronymicTextBox.Text = null;
                DealPercentTextBox.Text = null;
                currentId = 0;
            }
        }

        private void SearchTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                _connection = new Connection();
                var agents = _connection.Practica.Agents.ToList();
                AgentsDataGrid.ItemsSource = agents;
                return;
            }
                

            var values = SearchTextBox.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var filtered = _connection.Practica.Agents.Where(agent => values.Any(value => agent.FirstName.Contains(value)));

            AgentsDataGrid.ItemsSource = filtered.ToList();
        }



    }
}
