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
        public bool LiftIsCalled { get; set; }
        public List<Human> People = new List<Human>();
        public List<Human> WaitingPeople = new List<Human>();
        public List<Human> CamePeople = new List<Human>();

        public delegate void LiftCallingDel(Floor floor);
        public event LiftCallingDel CallLift; 

        public Floor(int number)
        {
            InitializeComponent();
            Number = number;

            int numberOfPeople = Tools.rnd.Next(1,6);
            for (int i = 0; i < numberOfPeople; i++)
            {
                Human newHuman = new Human(Number, i+1);
                newHuman.moveToWaiting += AddToWaiting;
                newHuman.humanCame += CallingLift;
                People.Add(newHuman);
                //Canvas.SetRight(newHuman, (i*40)+200);
                //Canvas.SetBottom(newHuman,21);
                newHuman.Margin = new Thickness(400-i*40, 15, 0, 0);
                floorCanvas.Children.Add(newHuman);

            }
        }

        public void AddToWaiting(Human human)
        {
            WaitingPeople.Add(human);
            human.waitingNumber = WaitingPeople.Count();
            People.Remove(human);
            //Thickness newThickness = new Thickness();
            //newThickness.Right = 500;
            human.MoveHuman(-1);
        }
        public void MoveHumanToFloor(Human human)
        {
            CamePeople.Add(human);
            human.CameToNeededFloor(CamePeople.Count);
        }
        private void CallingLift()
        {
            if (!LiftIsCalled)
            {
                CallLift(this);
                LiftIsCalled = true;
            }
        }
        public void MoveHumansInLift(Human human)
        {
            floorCanvas.Children.Remove(human);
        }

    }
}
