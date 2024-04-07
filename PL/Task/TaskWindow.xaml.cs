using BlApi;
using BO;
using PL.Director;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.Task
{
    /// <summary>
    /// Interaction logic for TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        static readonly IBl s_bl = BlApi.Factory.Get();
        //  int curentId = 0;

        public int curentId
        {
            get { return (int)GetValue(curentIdProperty); }
            set { SetValue(curentIdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for curentId.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty curentIdProperty =
            DependencyProperty.Register("curentId", typeof(int), typeof(TaskWindow), new PropertyMetadata(0));



        public Visibility IsAdd
        {
            get { return (Visibility)GetValue(IsAddProperty); }
            set { SetValue(IsAddProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsAdd.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsAddProperty =
            DependencyProperty.Register("IsAdd", typeof(Visibility), typeof(TaskWindow), new PropertyMetadata(null));


        public TaskWindow(int id = 0)
        {

            if (id != 0)
                try
                {
                    IsAdd = Visibility.Visible;
                    CurrentTask = s_bl.Task.GetTaskDetails(id);
                    OptionalEngs = s_bl.Engineer.GetAllEngineers(item => item.Level >= CurrentTask.ComplexityLevel && (item.Task == null));
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }
            else
            {
                CurrentTask = new BO.Task();
                IsAdd = Visibility.Collapsed;
            }
            IsShow = Visibility.Collapsed;
            OptionalDeps = s_bl.Task.GetAllTasks(t => CurrentTask == null || t.Id != CurrentTask.Id).Select(task => new TaskInList(task!));
            curentId = CurrentTask.Id;
            InitializeComponent();
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
                            if (SelectedEngineer != 0)
                            {
                                BO.Engineer myEng = s_bl.Engineer.GetEngineerDetails(SelectedEngineer);
                                CurrentTask.Engineer = new EngineerInTask() { Id = myEng.Id, Name = myEng.Name };
                                CurrentTask.StartDate = CurrentTask.PlannedStartDate >= s_bl.Clock ? CurrentTask.PlannedStartDate : s_bl.Clock;
                            }
                            s_bl.Task.Update(CurrentTask);
                            MessageBox.Show("Updated task succesfuly");
                            this.Close();
                        }
                        else if (buttonText == "Add")
                        {
                            s_bl.Task.Create(CurrentTask);
                            MessageBox.Show("Added task succesfuly");
                            this.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
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



        public IEnumerable<BO.Engineer> OptionalEngs
        {
            get { return (IEnumerable<BO.Engineer>)GetValue(OptionalEngsProperty); }
            set { SetValue(OptionalEngsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OptionalEngs.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OptionalEngsProperty =
            DependencyProperty.Register("OptionalEngs", typeof(IEnumerable<BO.Engineer>), typeof(TaskWindow), new PropertyMetadata(null));



        public int SelectedEngineer
        {
            get { return (int)GetValue(SelectedEngineerProperty); }
            set { SetValue(SelectedEngineerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedEngineer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedEngineerProperty =
            DependencyProperty.Register("SelectedEngineer", typeof(int), typeof(TaskWindow), new PropertyMetadata(0));

        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            ComboBox? comboBox = sender as ComboBox;
            if (comboBox.SelectedValue != null)
            {
                BO.Engineer selectedeng = (BO.Engineer)comboBox.SelectedItem;
                SelectedEngineer = selectedeng.Id;
            }
        }
    }
}