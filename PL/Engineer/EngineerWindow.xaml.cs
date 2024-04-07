using BlApi;
using BO;
using DO;
using PL.Task;
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
            if (id != 0)
                try
                {
                    CurrentEngineer = s_bl.Engineer.GetEngineerDetails(id);
                    if (CurrentEngineer.Task != null)
                        SelectedTask = CurrentEngineer.Task.Id;
                    OptionalTasks = s_bl.Task.GetAllTasks(item => item.ComplexityLevel <= CurrentEngineer.Level && (item.Engineer == null));
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }

            else
                CurrentEngineer = new BO.Engineer();
            currentId = CurrentEngineer.Id;
            InitializeComponent();

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
                    {
                        if (SelectedTask != 0)
                        {
                            BO.Task myTask = s_bl.Task.GetTaskDetails(SelectedTask);
                            myTask.Engineer = new EngineerInTask() { Id = CurrentEngineer.Id, Name = CurrentEngineer.Name };
                            myTask.StartDate = myTask.PlannedStartDate >= s_bl.Clock ? myTask.PlannedStartDate : s_bl.Clock;
                            s_bl.Task.Update(myTask);
                        }
                        s_bl.Engineer.Update(CurrentEngineer);
                        MessageBox.Show("Updated engineer succesfuly");
                        this.Close();
                    }
                    else if (buttonText == "Add")
                        s_bl.Engineer.Create(CurrentEngineer);
                    MessageBox.Show("Added engineer succesfuly");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            //לא אמור להיות ככה אבל זה בינתיים לראות שזה התעדכן
            new EngineerListWindow().ShowDialog();
        }


        public IEnumerable<BO.Task> OptionalTasks
        {
            get { return (IEnumerable<BO.Task>)GetValue(OptionalTasksProperty); }
            set { SetValue(OptionalTasksProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OptionalTasks.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OptionalTasksProperty =
            DependencyProperty.Register("OptionalTasks", typeof(IEnumerable<BO.Task>), typeof(EngineerWindow), new PropertyMetadata(null));

        public int currentId
        {
            get { return (int)GetValue(currentIdProperty); }
            set { SetValue(currentIdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for currentId.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty currentIdProperty =
            DependencyProperty.Register("currentId", typeof(int), typeof(TaskWindow), new PropertyMetadata(0));



        public int SelectedTask
        {
            get { return (int)GetValue(SelectedTaskProperty); }
            set { SetValue(SelectedTaskProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedTask.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedTaskProperty =
            DependencyProperty.Register("SelectedTask", typeof(int), typeof(EngineerWindow), new PropertyMetadata(0));


        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            ComboBox? comboBox = sender as ComboBox;
            if (comboBox.SelectedValue != null)
            {
                BO.Task selectedTask = (BO.Task)comboBox.SelectedItem;
                SelectedTask = selectedTask.Id;
            }
        }

        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            ComboBox? comboBox = sender as ComboBox;
            if (comboBox.SelectedValue != null)
            {
                //CurrentEngineer.Level = comboBox.SelectedItem;
            }
        }
    }
}




