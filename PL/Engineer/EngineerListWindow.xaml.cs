using BlApi;
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
using BO;

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for EngineerListWindow.xaml
    /// </summary>
    public partial class EngineerListWindow : Window
    {
        static readonly IBl s_bl = BlApi.Factory.Get();
        public EngineerListWindow()
        {
            InitializeComponent();
            EngineerList = s_bl?.Engineer.GetAllEngineers()!;

        }
        public IEnumerable<BO.Engineer?> EngineerList
        {
            get { return (IEnumerable<BO.Engineer>)GetValue(EngineerListProperty); }
            set { SetValue(EngineerListProperty, value); }
        }

        public static readonly DependencyProperty EngineerListProperty =
            DependencyProperty.Register("EngineerList", typeof(IEnumerable<BO.Engineer>), typeof(EngineerListWindow), new PropertyMetadata(null));

        public EngineerExperience Experience { get; set; } = EngineerExperience.NONE;


        private void CbExperienceSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EngineerList = (Experience == EngineerExperience.NONE) ?
                s_bl?.Engineer.GetAllEngineers()! : s_bl?.Engineer.GetAllEngineers(item => item.Level == Experience)!;
        }

        private void new_eng(object sender, RoutedEventArgs e)
        {
            new EngineerWindow().ShowDialog();
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.Engineer? eng = (sender as ListView)?.SelectedItem as BO.Engineer;
            if (eng != null)
                new EngineerWindow(eng.Id).ShowDialog();
        }

        public void DeleteEngineerCommand(object parameter)
        {
            // המרת הפרמטר לאובייקט מהנדס
            var engineer = parameter as BO.Engineer;

            // בדיקה אם קיים אובייקט מהנדס
            if (engineer != null)
            {
                s_bl.Engineer.Delete(engineer.Id);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                var engineer = button.Tag as BO.Engineer;
                if (engineer != null)
                {
                    MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete {engineer.Name}?", "SURE", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            s_bl.Engineer.Delete(engineer.Id);
                            EngineerList = s_bl?.Engineer.GetAllEngineers()!;
                            MessageBox.Show($"engineer {engineer.Name} deleted succesfuly");
                        }
                        catch (Exception ex) { MessageBox.Show(ex.Message); }
                    }
                }
            }
        }


    }
}