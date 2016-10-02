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
    /// Логика взаимодействия для Lift.xaml
    /// </summary>
    public partial class Lift : UserControl
    {
        private double HouseHeight { get; set; }
        private int currentFloorNum { get; set; }
        private List<Human> PeopleInLift = new List<Human>(5);
        private List<Floor> CallingFloors = new List<Floor>();
        private List<int> FloorsPeopleWant = new List<int>();

        public Lift(double houseHeight)
        {
            InitializeComponent();
            Thickness thic = new Thickness();
            thic.Top = houseHeight - Height;
            Margin = thic;
            HouseHeight = houseHeight;

        }

        private void Move(Floor floor)
        {
            ThicknessAnimation liftanim = new ThicknessAnimation();
            liftanim.From = Margin;
            liftanim.Duration = TimeSpan.FromSeconds(Math.Abs(0.3 * (floor.Number - currentFloorNum)));
            liftanim.To = new Thickness(0, HouseHeight - (Height * (9 - floor.Number)), 0, 0);
            liftanim.Completed += TakePeople;
            BeginAnimation(MarginProperty, liftanim);
            currentFloorNum = floor.Number;
        }
        private void TakePeople(object sender, EventArgs e)
        {
            foreach (Human waitingHuman in CallingFloors[0].WaitingPeople)
            {
                if(PeopleInLift.Count != PeopleInLift.Capacity)
                {
                    PeopleInLift.Add(waitingHuman);
                    //CallingFloors[0].WaitingPeople.Remove(waitingHuman);
                    CallingFloors[0].MoveHumansInLift(waitingHuman);

                    int floorHumanWant = waitingHuman.ChooseFloor();
                    if (!FloorsPeopleWant.Contains(floorHumanWant))
                    {
                        FloorsPeopleWant.Add(floorHumanWant);
                        Grid.SetColumn(waitingHuman, waitingHuman.waitingNumber);
                        liftGrid.Children.Add(waitingHuman);
                    }
                }
            }
        }

        public void AcceptCall(Floor floor)
        {
            CallingFloors.Add(floor);
            ChooseFloorToMove(floor);
        }
        private void ChooseFloorToMove(Floor floor)
        {
            if (floor == CallingFloors[0]) Move(floor);
        }
    }
}
