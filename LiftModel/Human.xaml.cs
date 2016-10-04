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
    /// Логика взаимодействия для Human.xaml
    /// </summary>
    public partial class Human : UserControl
    {
        int Floor { get; set; }
        public int Number { get; set; }
        //public Thickness InstantMargin { get; set; }
        public int waitingNumber{ get; set; } 
        public int FloorHumanWants { get; set; }
        DispatcherTimer timer = new DispatcherTimer();

        public delegate void MethodContainer(Human human);
        public delegate void HumanCameDel();

        public event HumanCameDel humanCame; 
        public event MethodContainer moveToWaiting;

        public Human(int floor, int number)
        {
            InitializeComponent();
            Number = number;
            //InstantMargin = Margin;
            label.Content = floor;
            Floor = floor;
            timer.Tick += timer_Tick;
            timer.Interval = TimeSpan.FromSeconds(Tools.rnd.Next(2, 60));
            timer.Start();
        }

        public int ChooseFloor()
        {
            FloorHumanWants = Tools.rnd.Next(1, 9);
            #region
            if (FloorHumanWants == Floor)
                if (Floor != 8)
                {
                    FloorHumanWants++;
                }
                else FloorHumanWants--;
            #endregion//фикс от выпадания того-же этажа

            label.Content = FloorHumanWants;
            return FloorHumanWants;
        }
        public void CameToNeededFloor()
        {
            human_png.Source = new BitmapImage(new Uri("E:/Programming/C#/LiftModel/LiftModel/human.png"));
            waitingNumber = 0;
            //Margin = InstantMargin;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            ChooseFloor();
            human_png.Source = new BitmapImage(new Uri("E:/Programming/C#/LiftModel/LiftModel/redHuman.png"));
            moveToWaiting(this);
        }

        public void MoveHuman(Thickness newThickness)
        {
            ThicknessAnimation peopleAnim = new ThicknessAnimation();
            peopleAnim.From = Margin;
            peopleAnim.Duration = TimeSpan.FromSeconds(2);
            peopleAnim.To = newThickness;
            peopleAnim.Completed += HumanCame;
            BeginAnimation(MarginProperty, peopleAnim);
            
        }

        private void HumanCame(object sender, EventArgs e)
        {
            humanCame();
        }
    }
}
