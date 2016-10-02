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
        public int Number {get; set;}
        private bool LiftIsCalled { get; set; }
        private Lift Lift { get; set;}
        public List<Human> People = new List<Human>();
        public List<Human> WaitingPeople = new List<Human>();

        public delegate void LiftCallingDel(Floor floor);
        public event LiftCallingDel CallLift; 

        public Floor(int number, Lift lift)
        {
            InitializeComponent();
            Number = number;
            Lift = lift;

            int numberOfPeople = Tools.rnd.Next(1,6);
            for (int i = 0; i < numberOfPeople; i++)
            {
                Human newHuman = new Human(Number, i+1);
                newHuman.moveToWaiting += AddToWaiting;
                People.Add(newHuman);
                Canvas.SetRight(newHuman, i*60);
                Canvas.SetBottom(newHuman,21);
                floorCanvas.Children.Add(newHuman);
            }
        }

        public void AddToWaiting(Human human)
        {
            if (!LiftIsCalled) CallLift(this);
            LiftIsCalled = true;
            WaitingPeople.Add(human);
            human.waitingNumber = WaitingPeople.Count();
            //double dx = (400-(human.waitingNumber * 80) + (5 - human.Number)*80)-80;
            People.Remove(human);
            Thickness newThickness = new Thickness();
            newThickness.Right = 500;
            human.MoveHuman(newThickness);
        }
        public void MoveHumansInLift(Human human)
        {
            floorCanvas.Children.Remove(human);
        }

    }
}
