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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace LiftModel
{
    /// <summary>
    /// Логика взаимодействия для Floor.xaml
    /// </summary>
    public partial class Floor : UserControl
    {
        private int Number {get; set;}
        private Lift Lift { get; set;}
        public List<Human> People = new List<Human>();
        public Queue<Human> WaitingPeople = new Queue<Human>();

        public delegate void MethodContainer(Human human);
        public event MethodContainer moveToWaiting;

        public Floor(int number, Lift lift)
        {
            InitializeComponent();
            Number = number;
            Lift = lift;

            int numberOfPeople = Tools.rnd.Next(1,7);
            for (int i = 0; i < numberOfPeople; i++)
            {
                Human newHuman = new Human(Number);
                newHuman.moveToWaiting += AddToWaiting;
                People.Add(newHuman);
                Grid.SetRow(newHuman, 1);
                Grid.SetColumn(newHuman, (10-i));
                floorGrid.Children.Add(newHuman);
            }
        }
        public void AddToWaiting(Human human)
        {
            People.Remove(human);
            WaitingPeople.Enqueue(human);
            MoveHumanToWaitingArea(human);
        }

        private void MoveHumanToWaitingArea(Human human)
        {
            ThicknessAnimation peopleAnim = new ThicknessAnimation();
            peopleAnim.From = human.Margin;
            peopleAnim.Duration = TimeSpan.FromSeconds(2);
            peopleAnim.To = new Thickness(-500, 0, 0, 0);
            BeginAnimation(MarginProperty, peopleAnim);

            //Grid.SetColumn(human, WaitingPeople.ToList().IndexOf(human));
        }

        private void floor_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Lift.Move(Number);
        }
    }
}
