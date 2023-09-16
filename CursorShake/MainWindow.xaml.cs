using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CursorShake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Vector2 prevPosition = new Vector2();
        Vector2 lastPosition = new Vector2();
        double size = 0;
        double sizePrev = 0;

        public MainWindow()
        {
            InitializeComponent();

            this.ToTransparentWindow();

            GlobalMouseHandler.MouseAction += MoveWindow;
            GlobalMouseHandler.Start();

            var timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 5);
            timer.Tick += (sender, e) => TickEvent();
            timer.Start();
        }

        private void MoveWindow(Point point)
        {
            Dispatcher.Invoke(() =>
            {
                var vecPrev = new Vector2(prevPosition.X, prevPosition.Y);
                var vecLast = new Vector2(lastPosition.X, lastPosition.Y);
                var vecNow = new Vector2((float)point.X, (float)point.Y);

                var dot = Vector2.Dot(Vector2.Normalize(vecNow - vecLast), Vector2.Normalize(vecLast - vecPrev));
                if (dot < -Math.Cos(60 / (180 * Math.PI)))
                {
                    size += .8;
                }
                if (size > 0.7)
                {
                    var length = Vector2.Distance(vecNow, vecLast);
                    if (length > 12)
                    {
                        size += 0.1;
                    }
                }
                TickEvent();

                prevPosition = lastPosition;
                lastPosition = vecNow;

                mainWindow.Left = point.X;
                mainWindow.Top = point.Y;
            });
        }

        private void TickEvent()
        {
            size -= 0.01;

            size = Math.Min(Math.Max(size, -0.05), 1.4);
            Debug.WriteLine("Size: {0}", size);

            var length = Math.Min(Math.Max(size, 0), 1);
            //sizePrev = sizePrev * 0.995 + length * 0.005;
            border.Height = Math.Min(182, Math.Pow(length, 0.9) * 182) + 18;

            mainWindow.Visibility = length > 0 ? Visibility.Visible : Visibility.Hidden;
        }

        private void mainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            GlobalMouseHandler.Stop();
        }
    }
}
