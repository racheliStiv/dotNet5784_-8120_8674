using BlApi;
using DO;
using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for Engineer.xaml
    /// </summary>
    public partial class EngineerWindow : Window
    {
        static readonly IBl s_bl = BlApi.Factory.Get();
        public EngineerWindow(int id = 0)
        {
            InitializeComponent();
            if (id != 0)
                try { CurrentEngineer = s_bl.Engineer.GetEngineerDetails(id); }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }

            else
                CurrentEngineer = new BO.Engineer();
        }
        public BO.Engineer CurrentEngineer
        {
            get { return (BO.Engineer)GetValue(CurrentEngineerProperty); }
            set { SetValue(CurrentEngineerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentEngineer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentEngineerProperty =
            DependencyProperty.Register("CurrentEngineer", typeof(BO.Engineer), typeof(EngineerWindow), new PropertyMetadata(null));


        private void AddOrUpdate(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            if (button != null)
            {
                try
                {
                    string buttonText = button.Content.ToString()!;
                    if (buttonText == "Update")
                        s_bl.Engineer.Update(CurrentEngineer);
                    else if (buttonText == "Add")
                        s_bl.Engineer.Create(CurrentEngineer);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            //לא אמור להיות ככה אבל זה בינתיים לראות שזה התעדכן
            new EngineerListWindow().ShowDialog();
        }
    }
}




