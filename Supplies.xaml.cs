using prak2semitog.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для Supplies.xaml
    /// </summary>
    public partial class Supplies : Window
    {
        ObservableCollection<SupplyView> itemsSFV = new ObservableCollection<SupplyView>();
        private Connection _connection = new Connection();
        DataTable _supplyTable = new DataTable();

        public Supplies()
        {
            InitializeComponent();
            LoadGrid();
            LoadCbAgent();
            LoadCbClient();
            LoadCbRealEstate();
        }
        private void PriceTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Allow only numerical input
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void PriceTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Allow certain keys like Backspace, Delete, Tab, Enter, and Arrow keys
            if (e.Key == Key.Back || e.Key == Key.Delete || e.Key == Key.Tab ||
                e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.Enter || e.Key == Key.Return)
            {
                e.Handled = false;
            }
            else if (!IsKeyAllowed(e.Key))
            {
                e.Handled = true;
            }
        }

        private static bool IsTextAllowed(string text)
        {
            // Regular expression to match numeric input
            Regex regex = new Regex("[^0-9]");
            return !regex.IsMatch(text);
        }

        private static bool IsKeyAllowed(Key key)
        {
            // Check if the key is a numeric key (0-9)
            return (key >= Key.D0 && key <= Key.D9) || (key >= Key.NumPad0 && key <= Key.NumPad9);
        }
        public void LoadCbAgent()
        {
            AgentCb.ItemsSource = _connection.Practica.Agents.ToList();
            AgentCb.DisplayMemberPath = "FirstName";
            AgentCb.SelectedValuePath = "Id_Agent";
        }
        public void LoadCbClient()
        {
            ClientCb.ItemsSource = _connection.Practica.Clients.ToList();
            ClientCb.DisplayMemberPath = "FirstName";
            ClientCb.SelectedValuePath = "Id_Client";
        }
        public void LoadCbRealEstate()
        {
            RealEstateCb.ItemsSource = _connection.Practica.RealEstates.ToList();
            RealEstateCb.DisplayMemberPath = "Adress_Street";
            RealEstateCb.SelectedValuePath = "Id_RealEstate";
        }
        public void LoadGrid()
        {
            Connection connection = new Connection();
            var supply = connection.Practica.Supply.ToList();
            var agent = connection.Practica.Agents.ToList();
            var client = connection.Practica.Clients.ToList();
            var realState = connection.Practica.RealEstates.ToList();
            foreach (var s in supply)
            {
                var findagent = agent.FirstOrDefault(x => x.Id_Agent == s.Id_Agent);
                var findRealState = realState.FirstOrDefault(x => x.Id_RealEstate == s.Id_RealEstate);
                var findClient = client.FirstOrDefault(x => x.Id_Client == s.Id_Client);
                SupplyView supplyForView = new SupplyView();

                supplyForView.Agent = findagent.LastName + " " + findagent.FirstName + " " + findagent.MiddleName;
                supplyForView.IdAgent = findagent.Id_Agent;

                supplyForView.Client = findClient.LastName + " " + findClient.FirstName + " " + findClient.MiddleName;
                supplyForView.IdClient = findClient.Id_Client;

                supplyForView.RealEstate = findRealState.Adress_City + " " + findRealState.Adress_Street + " " + findRealState.Adress_House;
                supplyForView.IdRealEstate = findRealState.Id_RealEstate;

                supplyForView.Price = s.Price;

                supplyForView.IdSupply = s.Id_Supply;

                itemsSFV.Add(supplyForView);
                SupplyDataGrid.ItemsSource = itemsSFV;
            }
        }

        private Supply _selectedSupply;
        public int selectedRealEstateId;
        int idSupply;
        public SupplyView sgv = new SupplyView();
        private void SupplyDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SupplyDataGrid.SelectedItems != null)
            {
                SupplyView row = (SupplyView)SupplyDataGrid.SelectedItem;

                if (row != null)
                {
                    idSupply = row.IdSupply;
                    foreach (Agents item in AgentCb.Items)
                    {
                        if (item.Id_Agent == row.IdAgent)
                        {
                            AgentCb.SelectedItem = item;
                            break;
                        }
                    }
                    foreach (RealEstates item in RealEstateCb.Items)
                    {
                        if (item.Id_RealEstate == row.IdRealEstate)
                        {
                            RealEstateCb.SelectedItem = item;
                            break;
                        }
                    }
                    foreach (Clients item in ClientCb.Items)
                    {
                        if (item.Id_Client == row.IdClient)
                        {
                            ClientCb.SelectedItem = item;
                            break;
                        }
                    }
                    PriceTextBox.Text = row.Price.ToString();
                }
            }
        }

        
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (SupplyDataGrid.SelectedItems != null)
            {
                Agents agent = AgentCb.SelectedItem as Agents;
                Clients client = ClientCb.SelectedItem as Clients;
                RealEstates realEstate = RealEstateCb.SelectedItem as RealEstates;

                Supply supply = new Supply();

                supply.Price = Convert.ToInt32(PriceTextBox.Text);
                supply.Id_Supply = idSupply;
                supply.Id_Agent = agent.Id_Agent;
                supply.Id_Client = client.Id_Client;
                supply.Id_RealEstate = realEstate.Id_RealEstate;
                var UpdateSupply = _connection.Practica.Supply.Where(x => (x.Id_Supply == idSupply)).First();
                if (UpdateSupply != null)
                {
                    if (supply.Price != 0)
                    {
                        _connection.Practica.Supply.Where(x => (x.Id_Supply == idSupply)).First().Price = supply.Price;
                        _connection.Practica.SaveChanges();
                    }
                    if (supply.Id_Agent != 0)
                    {
                        _connection.Practica.Supply.Where(x => (x.Id_Supply == idSupply)).First().Id_Agent = supply.Id_Agent;
                        _connection.Practica.SaveChanges();
                    }
                    if (supply.Id_Client != 0)
                    {
                        _connection.Practica.Supply.Where(x => (x.Id_Supply == idSupply)).First().Id_Client = supply.Id_Client;
                        _connection.Practica.SaveChanges();
                    }
                    if (supply.Id_RealEstate != 0)
                    {
                        _connection.Practica.Supply.Where(x => (x.Id_Supply == idSupply)).First().Id_RealEstate = supply.Id_RealEstate;
                        _connection.Practica.SaveChanges();
                    }
                    _connection.Practica.SaveChanges();

                }

            }
            itemsSFV.Clear();
            LoadGrid();
            PriceTextBox.Text = null;
            AgentCb.SelectedItem = null;
            ClientCb.SelectedItem = null;
            RealEstateCb.SelectedItem = null;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (SupplyDataGrid.SelectedItems != null)
            {
                var sypplyDelete = _connection.Practica.Supply.Where(x => (x.Id_Supply == idSupply)).First();
                _connection.Practica.Supply.Remove(sypplyDelete);
                _connection.Practica.SaveChanges();
            }
            itemsSFV.Clear();
            LoadGrid();
            AgentCb.SelectedItem = null;
            ClientCb.SelectedItem = null;
            PriceTextBox.Text = null;
            RealEstateCb.SelectedItem = null;
        }

        private void AddWindow_Click(object sender, RoutedEventArgs e)
        {
            Agents agent = AgentCb.SelectedItem as Agents;
            Clients client = ClientCb.SelectedItem as Clients;
            RealEstates realEstate = RealEstateCb.SelectedItem as RealEstates;

            Supply supply = new Supply();

            supply.Price = Convert.ToInt32(PriceTextBox.Text);
            supply.Id_Agent = agent.Id_Agent;
            supply.Id_Client = client.Id_Client;
            supply.Id_RealEstate = realEstate.Id_RealEstate;
            if (supply.Price != 0 && supply.Id_Agent != 0 && supply.Id_Client != 0 && supply.Id_RealEstate != 0)
            {
                _connection.Practica.Supply.Add(supply);
                _connection.Practica.SaveChanges();
            }
            else
            {
                MessageBox.Show("Пустые столбцы");
            }
            itemsSFV.Clear();
            LoadGrid();
            PriceTextBox.Text = null;
            AgentCb.SelectedItem = null;
            ClientCb.SelectedItem = null;
            RealEstateCb.SelectedItem = null;
        }
     
        
       
    }
}
