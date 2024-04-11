
using BlApi;
using DO;
using Microsoft.VisualBasic;
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ShowDirectorPage(object sender, RoutedEventArgs e)
        {
            new DirectorWindow().Show();
        }

        private void ShowIdBox_click(object sender, RoutedEventArgs e)
        {
            string inputValue = Interaction.InputBox("Insert ID:", "wellcome engineer", "");

            if (!string.IsNullOrEmpty(inputValue))
            {
                if (int.TryParse(inputValue, out int id))
                    try
                    {
                        s_bl.Engineer.GetEngineerDetails(id);
                        new ConnectEngineer(id).Show();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Sorry, " + ex.Message);
                    }
                else
                    MessageBox.Show("invalid ID. try again");
            }
        }

        public static readonly DependencyProperty CurrentTimeProperty =
       DependencyProperty.Register("CurrentTime", typeof(DateTime), typeof(MainWindow), new PropertyMetadata(DateTime.Now));

        public DateTime CurrentTime
        {
            get { return (DateTime)GetValue(CurrentTimeProperty); }
            set { SetValue(CurrentTimeProperty, value); }
        }

        private void Add_one_day(object sender, RoutedEventArgs e)
        {
            s_bl.AdvanceTimeByDay();
            CurrentTime = CurrentTime.AddDays(1);
        }

        private void Add_one_hour(object sender, RoutedEventArgs e)
        {
            s_bl.AdvanceTimeByHour();
            CurrentTime = CurrentTime.AddHours(1);
        }
    }

}