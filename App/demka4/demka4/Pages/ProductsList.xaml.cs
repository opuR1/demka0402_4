using demka4.Model;
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

namespace demka4.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProductsList.xaml
    /// </summary>
    public partial class ProductsList : Page
    {
        private List<Producers> _allProducers = new List<Producers>();
        private List<Products> _allProducts = new List<Products>();
        private Users _currentUser;

        public ProductsList(Users user)
        {
            InitializeComponent();
            _currentUser = user;
            if (_currentUser != null)
            {
                if (Application.Current.MainWindow is MainWindow mainWindow)
                {
                    mainWindow.SetName(_currentUser.FullName);
                }
            }
            else
            {
                if (Application.Current.MainWindow is MainWindow mainWindow)
                {
                    mainWindow.SetName(string.Empty);
                }
            }
            LoadData();
            LoadFilters();
            GetRole();
        }
        private void LoadData()
        {
            using (var db = kr_de4Entities.GetContext())
            {
                _allProducers = db.Producers.ToList();
                _allProducts = db.Products.Include(p => p.Units).Include(p => p.Suppliers).Include(p => p.Producers).Include(p => p.Categories).ToList();
                lbProducts.ItemsSource = _allProducts;
                ApplyFilters();
            }
        }

        private void LoadFilters()
        {
            var AllProducers = new Producers { ProducerId = 0, ProducerName = "Все производители" };
            _allProducers.Insert(0, AllProducers);
            cmbFilter.ItemsSource = _allProducers;
            cmbFilter.DisplayMemberPath = "ProducerName";
            cmbFilter.SelectedValuePath = "ProducerId";
            cmbFilter.SelectedIndex = 0;
        }
        private void ApplyFilters()
        {
            if (_allProducts == null || _allProducts.Count == 0) return;
            var filtered = _allProducts.AsEnumerable();
            string searchText = tbFind.Text.Trim().ToLower();

            if (!string.IsNullOrEmpty(searchText))
            {
                filtered = filtered.Where(p => p.ProductName.ToLower().Contains(searchText) || p.Categories.CategoryName.ToLower().Contains(searchText) || p.Description.ToLower().Contains(searchText));

            }
            if (cmbFilter.SelectedValue is int selectedProducerId && selectedProducerId > 0)
            {
                filtered = filtered.Where(p => p.ProducerId == selectedProducerId);
            }
            if (cmbSort.SelectedItem is ComboBoxItem selectedSort)
            {
                switch (selectedSort.Content.ToString())
                {
                    case "Количество по возр.":
                        filtered = filtered.OrderBy(p => p.StorageCount);
                        break;
                    case "Количество по уб.":
                        filtered = filtered.OrderByDescending(p => p.StorageCount);
                        break;
                }
            }
            lbProducts.ItemsSource = filtered.ToList();
        }

        private void GetRole()
        {
            if (_currentUser != null)
            {
                switch (_currentUser.RoleId)
                {
                    case 1:
                        HeadPanel.Visibility = Visibility.Visible;
                        btnAdd.Visibility = Visibility.Visible;
                        btnOrders.Visibility = Visibility.Visible;
                        break;
                    case 2:
                        HeadPanel.Visibility = Visibility.Visible;
                        btnAdd.Visibility = Visibility.Collapsed;
                        btnOrders.Visibility = Visibility.Visible;
                        break;
                    case 3:
                        HeadPanel.Visibility = Visibility.Collapsed;
                        btnAdd.Visibility = Visibility.Collapsed;
                        btnOrders.Visibility = Visibility.Collapsed;
                        break;
                    default:
                        HeadPanel.Visibility = Visibility.Collapsed;
                        btnAdd.Visibility = Visibility.Collapsed;
                        btnOrders.Visibility = Visibility.Collapsed;
                        break;
                }
            }
        }
        private void tbFind_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void cmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void cmbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void btnOrders_Click(object sender, RoutedEventArgs e)
        {
            if (_currentUser != null)
            {
                NavigationService.Navigate(new OrderList(_currentUser));
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProductEdit(null, _currentUser));
        }

        private void lbProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_currentUser != null)
            {
                if (_currentUser.RoleId != 1)
                {
                    return;
                }
                if (lbProducts.SelectedItem is Products selectedProduct)
                {
                    NavigationService.Navigate(new ProductEdit(selectedProduct, _currentUser));
                }
            }
        }
    }
}
