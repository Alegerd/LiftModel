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
using System.Windows.Threading;
using System.Windows.Media.Animation;

namespace LiftModel
{
    /// <summary>
    /// Логика взаимодействия для Lift.xaml
    /// </summary>
    public partial class Lift : UserControl
    {
        private double HouseHeight { get; set; }
        private int currentFloorNum = 8;
        private List<Floor> Floors = new List<Floor>(8);//список этажей
        private List<Human> PeopleInLift = new List<Human>(5);
        private List<Floor> CallingFloors = new List<Floor>();
        private int directFloor { get; set; }
        private double Distiantion { get; set; }
        private int LiftKurs { get; set; }
        private Floor currentFloor { get; set; }
        private DispatcherTimer LiftTimer = new DispatcherTimer();
        private Floor FloorLiftOn { get; set; }
        LiftState liftState;
        Direction direction;

        enum LiftState
        {
            isMoving,
            isStanding
        };

        enum Direction
        {
            Up,
            Down
        };

        public Lift(double houseHeight, List<Floor> Floors)
        {
            InitializeComponent();
            Thickness thic = new Thickness();
            thic.Top = houseHeight - Height;
            Margin = thic;
            HouseHeight = houseHeight;
            this.Floors = Floors;
            FloorLiftOn = Floors[7];
            LiftTimer.Tick += LiftTimer_Tick;
            LiftTimer.Interval = TimeSpan.FromMilliseconds(1);
            liftState = LiftState.isStanding;
        }

        private void Move(Floor floor)
        {
            liftState = LiftState.isMoving; //лифт движется
            LiftKurs = (floor.Number > FloorLiftOn.Number) ? 1 : -1;
            Distiantion = Math.Abs(floor.Number * 100 - FloorLiftOn.Number * 100);
            LiftTimer.Start();
            //ThicknessAnimation liftanim = new ThicknessAnimation();
            //liftanim.From = Margin;
            //liftanim.Duration = TimeSpan.FromMilliseconds(Math.Abs(0.3 * (floor.Number - currentFloorNum)) *10000 );
            //liftanim.To = new Thickness(0, HouseHeight - (Height * (9 - floor.Number)), 0, 0);
            //liftanim.Completed += LiftStopped;
            //BeginAnimation(MarginProperty, liftanim);

            //if (FloorLiftOn.Number == currentFloor.Number)
            //{
            //    if (PeopleInLift.Count != 0) GetPeopleOut();
            //    TakePeopleFromCurrentFloor();
            //    currentFloor.LiftIsCalled = false;
            //    ChooseFloorToMove();
            //}
        }

        private void LiftTimer_Tick(object sender, EventArgs e)
        {
            if (Distiantion == 0)
            {
                LiftTimer.Stop();
                LiftStopped(null, null);
            }
            else
            {
                Distiantion -= 2;
                Margin = new Thickness(0, Margin.Top + (LiftKurs * 2), 0, 0);
            }

        }

        private void LiftStopped(object sender, EventArgs e)
        {
            liftState = LiftState.isStanding;
            FloorLiftOn = currentFloor;
            if (PeopleInLift.Count != 0) GetPeopleOut();
            currentFloor.LiftIsCalled = false;
            TakePeopleFromCurrentFloor();
            ChooseFloorToMove();
        }


        private void TakePeopleFromCurrentFloor()
        {

            foreach (Human waitingHuman in currentFloor.WaitingPeople)
            {
                if (PeopleInLift.Count < 5)
                {
                    PeopleInLift.Add(waitingHuman);
                    currentFloor.MoveHumansInLift(waitingHuman);
                    waitingHuman.Margin = new Thickness(0, 0, 0, 0);

                    //Canvas.SetLeft(waitingHuman, 40 * (PeopleInLift.Count - 1));//помещаем человека в лифт
                    //Canvas.SetBottom(waitingHuman, 21);
                    waitingHuman.Margin = new Thickness(40 * (PeopleInLift.Count - 1), 15, 0, 0);
                    liftCanvas.Children.Add(waitingHuman);
                }
            }
            foreach (Human human in PeopleInLift)//очистка поля ожидания от тех кто уже в лифте
            {
                currentFloor.WaitingPeople.Remove(human);
            }

            if (currentFloor.WaitingPeople.Count == 0)
                CallingFloors.Remove(currentFloor);
            else
                currentFloor.LiftIsCalled = true;

        }


        private void GetPeopleOut()
        {
            foreach (Human human in PeopleInLift)
            {
                if (currentFloor.Number == human.FloorHumanWants)
                {
                    liftCanvas.Children.Remove(human);
                    currentFloor.floorCanvas.Children.Add(human);
                    currentFloor.MoveHumanToFloor(human);

                }
            }
            foreach (Human human in currentFloor.CamePeople) //очистка списка лифта от тех, кто вышел
            {
                PeopleInLift.Remove(human);
                foreach (Human hum in PeopleInLift)
                {
                    hum.Margin = new Thickness(40 * (PeopleInLift.Count - 1), 15, 0, 0);
                }
            }
        }                             //дописать

        public void AcceptCall(Floor floor)
        {
            CallingFloors.Add(floor);
            ChooseFloorToMove();
        }

        private void ChooseFloorToMove()
        {
            if (liftState == LiftState.isStanding)
            {
                if (PeopleInLift.Count != 0) MoveToChoosenFloors();
                else if (CallingFloors.Count != 0)
                    {
                        CallingFloors.Sort(delegate (Floor floor1, Floor floor2)
                        { return floor1.Number.CompareTo(floor2.Number); });

                        currentFloor = CallingFloors[0];
                        Move(currentFloor);
                    }
            }

        }

        private void MoveToChoosenFloors()
        {
            currentFloor = Floors[PeopleInLift[0].FloorHumanWants - 1];
            Move(currentFloor);
        }
    }
}
