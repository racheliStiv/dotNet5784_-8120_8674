using BlApi;
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
    /// Interaction logic for AllTaskInListWindow.xaml
    /// </summary>
    public partial class AllTaskInListWindow : Window
    {
        static readonly IBl s_bl = BlApi.Factory.Get();

        public AllTaskInListWindow()
        {
            InitializeComponent();
            TaskList = s_bl?.TaskInList.GetAllTasksInList()!;
        }

        public IEnumerable<BO.TaskInList> TaskList
        {
            get { return (IEnumerable<BO.TaskInList>)GetValue(taskListProperty); }
            set { SetValue(taskListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TaskList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty taskListProperty =
            DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.TaskInList>), typeof(AllTaskInListWindow), new PropertyMetadata(null));

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.TaskInList? t = (sender as ListView)?.SelectedItem as BO.TaskInList;
            new TaskWindow(t!.Id).ShowDialog();
        }

        private void new_task(object sender, RoutedEventArgs e)
        {
            new TaskWindow().ShowDialog();
        }
    }
}
