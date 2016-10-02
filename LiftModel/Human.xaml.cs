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

namespace LiftModel
{
    /// <summary>
    /// Логика взаимодействия для Human.xaml
    /// </summary>
    public partial class Human : UserControl
    {
        int Floor { get; set; }
        DispatcherTimer timer = new DispatcherTimer();

        public delegate void MethodContainer(Human human);
        public event MethodContainer moveToWaiting;

        public Human(int floor)
        {
            InitializeComponent();
            Floor = floor;
            timer.Tick += timer_Tick;
            timer.Interval = TimeSpan.FromSeconds(Tools.rnd.Next(2,30));
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            human_png.Source = new BitmapImage(new Uri("E:/Programming/C#/LiftModel/LiftModel/redHuman.png"));
            moveToWaiting(this);
        }
    }
}
