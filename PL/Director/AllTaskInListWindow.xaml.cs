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
using BO;
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
            TaskList = s_bl?.TaskInList.GetAllTasksInList(s_bl.Task.GetAllTasks())!;
            EngineerNames = s_bl!.Engineer.GetAllEngineers()
                .Where(engineer => !string.IsNullOrEmpty(engineer!.Name))
                .Select(engineer => engineer!.Name)
                .ToList()!;
        }


        public IEnumerable<string> EngineerNames
        {
            get { return (IEnumerable<string>)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register("EngineerNames", typeof(IEnumerable<string>), typeof(AllTaskInListWindow), new PropertyMetadata(null));


        public IEnumerable<TaskInList?> TaskList
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
        public string CurrentName { get; set; } = "";

        private void engineerCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TaskList = (CurrentName == "") ?

               s_bl.TaskInList.GetAllTasksInList(s_bl.Task.GetAllTasks()!) :
                s_bl.TaskInList.GetAllTasksInList(s_bl.Task.GetAllTasks(t => t != null && t.Engineer != null && t.Engineer.Name == CurrentName));
        }

        public EngineerExperience Experience { get; set; } = EngineerExperience.NONE;
        public BO.TaskStatus Status { get; set; } = BO.TaskStatus.NONE;

        private void levelTask_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TaskList = (Experience == EngineerExperience.NONE) ?
              s_bl?.TaskInList.GetAllTasksInList(s_bl.Task.GetAllTasks())! : s_bl?.TaskInList.GetAllTasksInList(s_bl.Task.GetAllTasks(t => t != null && t.ComplexityLevel == Experience))!;
        }
    }
}
