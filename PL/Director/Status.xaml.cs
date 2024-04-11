using BlApi;
using BO;
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

namespace PL.Director
{
    /// <summary>
    /// Interaction logic for Status.xaml
    /// </summary>
    public partial class Status : Window
    {
        static readonly IBl s_bl = BlApi.Factory.Get();

        public Status()
        {
            IsStartDate = Visibility.Visible;
            if (s_bl.StartDate != null)
                IsStartDate = Visibility.Collapsed;
            InitializeComponent();
        }
        public DateTime SelectedDate
        {
            get { return (DateTime)GetValue(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedDateProperty =
            DependencyProperty.Register("SelectedDate", typeof(DateTime), typeof(Status), new PropertyMetadata(null));



        public Visibility IsStartDate
        {
            get { return (Visibility)GetValue(IsStartDateProperty); }
            set { SetValue(IsStartDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsStartDatety.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsStartDateProperty =
            DependencyProperty.Register("IsStartDate", typeof(Visibility), typeof(Status), new PropertyMetadata(null));



        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("האם אתה בטוח?", "אישור", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                s_bl.ResetDB();
            }
        }

        public DateTime StartDateProj
        {
            get { return (DateTime)GetValue(StartDateProjProperty); }
            set { SetValue(StartDateProjProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartDateProj.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartDateProjProperty =
            DependencyProperty.Register("StartDateProj", typeof(DateTime), typeof(Status), new PropertyMetadata(null));


        private void Initialize_Click(object sender, RoutedEventArgs e)
        {
            s_bl.InitializeDB();
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox? comboBox = sender as ComboBox;
            DatePicker? d = sender as DatePicker;
            SelectedDate = (DateTime)d.SelectedDate;
        }

        private void make_luz_Click(object sender, RoutedEventArgs e)
        {
            s_bl.StartDate = SelectedDate;
            MessageBox.Show("Good, now you have to insert plann start date for all tasks"); 
        }
    }
}
