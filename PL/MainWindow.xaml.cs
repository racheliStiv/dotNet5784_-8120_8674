using BlApi;
using DO;
using PL.Director;
using PL.Engineer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using System.Xml.Serialization;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static readonly IBl s_bl = BlApi.Factory.Get();
        public MainWindow()
        {
            InitializeComponent();
            CurrentTime = s_bl.Clock;
            
        }

        private string engId = "Enter ID";
        public string EngId
        {
            get { return engId; }
            set
            {
                engId = value;
                OnPropertyChanged(nameof(EngId)); 
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("האם אתה בטוח?", "אישור", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                s_bl.ResetDB();
            }
        }

        private void Initialize_Click(object sender, RoutedEventArgs e)
        {
            s_bl.InitializeDB();
        }

        private void ShowDirectorPage(object sender, RoutedEventArgs e)
        {
            new DirectorWindow().Show();
        }

        private void ShowIdBox_click(object sender, RoutedEventArgs e)
        {
            IdBox.Visibility = Visibility.Visible;
        }

        private void ShowEngineerPage_click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = int.Parse(EngId);
                new EngineerWindow(id).Show();
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter a valid integer ID.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }
    }
}
