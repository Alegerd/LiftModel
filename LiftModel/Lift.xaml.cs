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
        double HouseHeight { get; set; }
        int currentFlorNum { get; set; }

        public Lift(double houseHeight)
        {
            InitializeComponent();
            Thickness thic = new Thickness();
            thic.Top = houseHeight - Height;
            Margin = thic;
            HouseHeight = houseHeight;
        }

        public void Move(int floor)
        {
            ThicknessAnimation liftanim = new ThicknessAnimation();
            liftanim.From = Margin;
            liftanim.Duration = TimeSpan.FromSeconds(Math.Abs(0.3*(floor-currentFlorNum)));
            liftanim.To = new Thickness(0,HouseHeight - (Height * (9-floor)),0,0);
            BeginAnimation(MarginProperty,liftanim);
            currentFlorNum = floor;
            
        }
    }
}
