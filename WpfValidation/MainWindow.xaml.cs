using System.Windows;

namespace WpfValidation
{
    public partial class MainWindow : Window
    {
        private readonly User _user = new User();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = _user;
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            _user.ValidateAll();

            if (_user.HasErrors)
            {
                MessageBox.Show(
                    "Пожалуйста, исправьте ошибки в форме перед отправкой.",
                    "Ошибка валидации",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
            else
            {
                SuccessBanner.Visibility = Visibility.Visible;
            }
        }
    }
}