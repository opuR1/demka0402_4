using demka4.Model;
using System.Data.Entity;
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
    /// Логика взаимодействия для OrderList.xaml
    /// </summary>

    public partial class OrderList : Page
    {
        private Users _currentUser;
        private List<Orders> _allOrders = new List<Orders>();
        public OrderList(Users user)
        {
            InitializeComponent();
            _currentUser = user;
            LoadButtons();
            LoadData();
        }
        private void LoadData()
        {
            using (var db = kr_de4Entities.GetContext())
            {
                _allOrders = db.Orders.Include(o => o.Users).Include(o => o.OrderStatuses).Include(o => o.PickupPoints.Cities).ToList();
                lbOrders.ItemsSource = _allOrders;
            }
        }
        private void LoadButtons()
        {
            switch (_currentUser.RoleId)
            {
                case 1:
                    btnAdd.Visibility = Visibility.Visible;
                    btnEdit.Visibility = Visibility.Visible;
                    break;
                case 2:
                    btnAdd.Visibility = Visibility.Collapsed;
                    btnEdit.Visibility = Visibility.Collapsed;
                    break;
                default:
                    btnAdd.Visibility = Visibility.Collapsed;
                    btnEdit.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OrderEdit(lbOrders.SelectedItem as Orders, _currentUser));
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OrderEdit(null, _currentUser));
        }
    }
}
