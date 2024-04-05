using BlApi;
using BO;
using PL.Director;
using PL.Engineer;
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

namespace PL.Task
{
    /// <summary>
    /// Interaction logic for TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        static readonly IBl s_bl = BlApi.Factory.Get();
      //  int currentId = 0;



        public int currentId
        {
            get { return (int)GetValue(currentIdProperty); }
            set { SetValue(currentIdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for currentId.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty currentIdProperty =
            DependencyProperty.Register("currentId", typeof(int), typeof(TaskWindow), new PropertyMetadata(0));


        public TaskWindow(int id = 0)
        {
            InitializeComponent();

            if (id != 0)
                try
                {
                    CurrentTask = s_bl.Task.GetTaskDetails(id);
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }
            else
                CurrentTask = new BO.Task();
            IsShow = Visibility.Collapsed;
            OptionalDeps = s_bl.Task.GetAllTasks(t => CurrentTask == null || t.Id != CurrentTask.Id).Select(task => new TaskInList(task!));
             currentId = CurrentTask.Id;
        }



        public IEnumerable<TaskInList> OptionalDeps
        {
            get { return (IEnumerable<TaskInList>)GetValue(OptionalDepsProperty); }
            set { SetValue(OptionalDepsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OptionalDeps.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OptionalDepsProperty =
            DependencyProperty.Register("OptionalDeps", typeof(IEnumerable<TaskInList>), typeof(TaskWindow), new PropertyMetadata(null));



        public TaskInList CurrentDep
        {
            get { return (TaskInList)GetValue(CurrentDepProperty); }
            set { SetValue(CurrentDepProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentDep.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentDepProperty =
            DependencyProperty.Register("CurrentDep", typeof(TaskInList), typeof(TaskWindow), new PropertyMetadata(null));


        public BO.Task CurrentTask
        {
            get { return (BO.Task)GetValue(CurrentTaskProperty); }
            set { SetValue(CurrentTaskProperty, value); }
        }
        public static readonly DependencyProperty CurrentTaskProperty =
            DependencyProperty.Register("CurrentTask", typeof(BO.Task), typeof(TaskWindow), new PropertyMetadata(null));

        private void AddOrUpdate(object sender, RoutedEventArgs e)
        {
            Button? button = sender as Button;
            if (button != null)
            {
                if (button.Content is string buttonText && !string.IsNullOrEmpty(buttonText))
                {
                    try
                    {
                        if (buttonText == "Update")
                        {
                            s_bl.Task.Update(CurrentTask);
                        }
                        else if (buttonText == "Add")
                        {
                            s_bl.Task.Create(CurrentTask);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            new AllTaskInListWindow().ShowDialog();
        }


        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem != null)
            {
                CurrentDep = (TaskInList)comboBox.SelectedItem;
            }
        }

        private void addDep(object sender, RoutedEventArgs e)
        {
            IsShow = Visibility.Visible;
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CurrentTask.AllDependencies!.Add(CurrentDep);
            try
            {
                s_bl.Task.Update(CurrentTask);
                MessageBox.Show("התלות נוספה בהצלחה");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            new AllTaskInListWindow().ShowDialog();
        }


        public Visibility IsShow
        {
            get { return (Visibility)GetValue(IsShowProperty); }
            set { SetValue(IsShowProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsShow.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsShowProperty =
     DependencyProperty.Register("IsShow", typeof(Visibility), typeof(TaskWindow), new PropertyMetadata(Visibility.Collapsed));


    }
}