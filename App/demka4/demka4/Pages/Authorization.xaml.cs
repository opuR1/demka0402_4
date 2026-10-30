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
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Page
    {
        public Authorization()
        {
            InitializeComponent();
        }
        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            string password = pbPassword.Password;
            string Login = tbLogin.Text;
            using (var db = kr_de4Entities.GetContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Login == Login && u.Password == password);
                if (user != null)
                {
                    NavigationService.Navigate(new ProductsList(user));
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnGuest_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProductsList(null));
        }
    }
}
