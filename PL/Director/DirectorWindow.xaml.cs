using PL.Engineer;
using PL.Task;
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
    /// Interaction logic for DirectorWindow.xaml
    /// </summary>
    public partial class DirectorWindow : Window
    {
        public DirectorWindow()
        {
            InitializeComponent();
            Choose= new Frame();
        }



        public Frame Choose
        {
            get { return (Frame)GetValue(ChooseProperty); }
            set { SetValue(ChooseProperty, value); }
        }

        public static readonly DependencyProperty ChooseProperty =
            DependencyProperty.Register("Choose", typeof(Frame), typeof(DirectorWindow), new PropertyMetadata(null));



        private void Choose_frame(object sender, RoutedEventArgs e)
        {

            switch ((sender as Button).Content.ToString())
            {
                case "מהנדסים":
                    Choose.Content = new EngineerListWindow();
                    break;
                case "משימות":
                    Choose.Content = new EngineerWindow();
                    break;
                case "תרשים גאנט":
                    Choose.Content = new MainWindow();
                    break;
                default:
                    break;
            }
        }

    }
}
