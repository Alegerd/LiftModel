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
        private DispatcherTimer timer = new DispatcherTimer();
        private DispatcherTimer animationTimer = new DispatcherTimer();
        private double Distanation { get; set; }
        private int AnimKurs { get; set; }

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
            animationTimer.Tick += AnimationTimer_Tick;
            animationTimer.Interval = TimeSpan.FromMilliseconds(1);
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
        public void CameToNeededFloor(List<Human> PeopleOnFloor)
        {
            Margin = new Thickness(0, 0, 0, 0);
            human_png.Source = new BitmapImage(new Uri("E:/Programming/C#/LiftModel/LiftModel/human.png"));
            waitingNumber = 1;
            MoveHumanToFloor(PeopleOnFloor);
            //Margin = InstantMargin;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            ChooseFloor();
            human_png.Source = new BitmapImage(new Uri("E:/Programming/C#/LiftModel/LiftModel/redHuman.png"));
            moveToWaiting(this);
        }

        public void MoveHuman(int AnimKurs)
        {
            //ThicknessAnimation peopleAnim = new ThicknessAnimation();
            //peopleAnim.From = Margin;
            //peopleAnim.Duration = TimeSpan.FromSeconds(2);
            //peopleAnim.To = newThickness;
            //peopleAnim.Completed += HumanCame;
            //BeginAnimation(MarginProperty, peopleAnim);
            this.AnimKurs = -1;
            Distanation = Math.Abs(400 - ((waitingNumber - 1) * 40) - (Number * 40))*2;
            animationTimer.Start();
        }

        private void MoveHumanToFloor(List<Human> PeopleOnFloor)
        {
            //ThicknessAnimation peopleAnim = new ThicknessAnimation();
            //peopleAnim.From = Margin;
            //peopleAnim.Duration = TimeSpan.FromSeconds(2);
            //peopleAnim.To = newThickness;
            //peopleAnim.Completed += HumanCame;
            //BeginAnimation(MarginProperty, peopleAnim);
            this.AnimKurs = 1;
            Distanation = Math.Abs(600 - 40);
            animationTimer.Start();
        }

        private void HumanCame(object sender, EventArgs e)
        {
            humanCame();
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            if (Distanation == 0)
            {
                animationTimer.Stop();
                HumanCame(null, null);
            }
            else
            {
                Distanation -= 5;
                Margin = new Thickness(Margin.Left + (AnimKurs * 5), 0, 0, 0);
            }

        }
    }
}
