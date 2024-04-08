using BlApi;
using BO;
using PL.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for ConnectEngineer.xaml
    /// </summary>
    public partial class ConnectEngineer : Window
    {
        readonly IBl s_bl = BlApi.Factory.Get();
        private MediaPlayer media1, media2;



        public ConnectEngineer(int id)
        {
            InitializeComponent();
            IsShow = Visibility.Collapsed;
            ShowTask = Visibility.Visible;

            try
            {

                media1 = new MediaPlayer();
                media2 = new MediaPlayer();
                media2.Open(new Uri("C:\\Users\\elchanan\\source\\repos\\dotNet5784_-8120_8674\\PL\\Tunes\\good.wmv", UriKind.RelativeOrAbsolute));
                media1.Open(new Uri("C:\\Users\\elchanan\\source\\repos\\dotNet5784_-8120_8674\\PL\\Tunes\\champion.wmv", UriKind.RelativeOrAbsolute));

                CurrentEngineer = s_bl.Engineer.GetEngineerDetails(id);
                BringTasks();

                if (CurrentEngineer == null || CurrentEngineer.Task == null)
                {
                    IsShow = Visibility.Visible;
                    ShowTask = Visibility.Collapsed;
                }
                else if (CurrentEngineer.Task != null)
                {
                    CurrentTask = s_bl.Task.GetTaskDetails(CurrentEngineer.Task.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public BO.Task CurrentTask
        {
            get { return (BO.Task)GetValue(CurrentTaskProperty); }
            set { SetValue(CurrentTaskProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentTask.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentTaskProperty =
            DependencyProperty.Register("CurrentTask", typeof(BO.Task), typeof(ConnectEngineer), new PropertyMetadata(null));

        public BO.Engineer CurrentEngineer
        {
            get { return (BO.Engineer)GetValue(CurrentEngineerProperty); }
            set { SetValue(CurrentEngineerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentEngineer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentEngineerProperty =
            DependencyProperty.Register("CurrentEngineer", typeof(BO.Engineer), typeof(ConnectEngineer), new PropertyMetadata(null));

        public IEnumerable<TaskInList>? OptionalTasks
        {
            get { return (IEnumerable<TaskInList>)GetValue(OptionalTasksProperty); }
            set { SetValue(OptionalTasksProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OptionalTasks.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OptionalTasksProperty =
            DependencyProperty.Register("OptionalTasks", typeof(IEnumerable<TaskInList>), typeof(ConnectEngineer), new PropertyMetadata(null));

        public Visibility IsShow
        {
            get { return (Visibility)GetValue(IsShowProperty); }
            set { SetValue(IsShowProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsShow.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsShowProperty =
            DependencyProperty.Register("IsShow", typeof(Visibility), typeof(ConnectEngineer), new PropertyMetadata(Visibility.Collapsed));
        private void complete_task(object sender, RoutedEventArgs e)
        {
            try
            {
                BO.Task doneTask = CurrentTask;
                doneTask.CompletedDate = s_bl.Clock;
                s_bl.Task.Update(doneTask);
                media1.Play();
                MessageBox.Show("CHAMPION!");
                CurrentTask = null;
                IsShow = Visibility.Visible;
                ShowTask = Visibility.Collapsed;

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


            }
        }

        public TaskInList SelectedTask
        {
            get { return (TaskInList)GetValue(SelectedTaskProperty); }
            set { SetValue(SelectedTaskProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedTask.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedTaskProperty =
            DependencyProperty.Register("SelectedTask", typeof(TaskInList), typeof(ConnectEngineer), new PropertyMetadata(null));

        private void BringTasks()
        {
            IEnumerable<BO.Task> tempTask = s_bl.Task.GetAllTasks(item => item != null && item.Engineer == null &&
            item.ComplexityLevel <= CurrentEngineer.Level && item.StartDate == null)!;
            if (tempTask != null)
                OptionalTasks = s_bl.TaskInList.GetAllTasksInList(tempTask);
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are u sure about this task?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    BO.Task origin_t = s_bl.Task.GetTaskDetails(SelectedTask.Id);
                    origin_t.Engineer = new BO.EngineerInTask() { Id = CurrentEngineer.Id, Name = CurrentEngineer.Name };
                    origin_t.StartDate = origin_t.PlannedStartDate >= s_bl.Clock ? s_bl.Clock : origin_t.PlannedStartDate;
                    s_bl.Task.Update(origin_t);
                    media2.Play();
                    IsShow = Visibility.Collapsed;
                    ShowTask = Visibility.Visible;
                    CurrentTask = s_bl.Task.GetTaskDetails(SelectedTask.Id);
                    BringTasks();

                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
