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

namespace LiftModel
{
    /// <summary>
    /// Логика взаимодействия для House.xaml
    /// </summary>
    public partial class House : UserControl
    {
        List<Floor> Floors = new List<Floor>();
        Lift lift;

        public House()
        {
            InitializeComponent();
        }
        public House(Thickness thickness)
        {  
            InitializeComponent();
            Margin = thickness;
            CreateLift();
            CreateFloors();
            foreach (var floor in Floors)
            {
                floor.CallLift += lift.AcceptCall;
            }
        }

        private void CreateLift()
        {
            lift = new Lift(Height);
            Canvas.SetBottom(lift,200);
            liftPlace.Children.Add(lift);
        }
        private void CreateFloors()
        {
            for (int i = 0; i < 8; i++)
            {
                Floor floor = new Floor(i+1,lift);
                sp.Children.Add(floor);
                Floors.Add(floor);
            }
        }

    }
}
