using System;
using System.Collections;
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

namespace CoroutinesForWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const double StepSize = 40;

        private Point _center;
        private Ellipse _ball;
        private Step[] _steps;
        private Point _start;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            SizeChanged += OnSizeChanged;

            _ball = new Ellipse { Width = 20, Height = 20, Stroke = Brushes.Black, StrokeThickness = 2 };
            _steps = Enumerable.Range(0, 4).Select(_ => new Step(StepSize)).ToArray();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Initialize();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Scene.Children.Add(_ball);
            foreach (var step in _steps)
            {
                Scene.Children.Add(step.Horizontal);
                Scene.Children.Add(step.Vertical);
            }

            Initialize();

            Executor.StartCoroutine(RunAnimation());
        }

        private void Initialize()
        {
            _center = new Point(Scene.ActualWidth / 2, Scene.ActualHeight / 2);
            _start = _center + new Vector(-2 * StepSize, -2 * StepSize);

            foreach (UIElement sceneChild in Scene.Children)
            {
                Canvas.SetLeft(sceneChild, 0);
                Canvas.SetTop(sceneChild, 0);
            }
        }

        private IEnumerator RunAnimation()
        {
            yield return BuildStairsAndDropBall();
            yield return RunStairsAndBounceBall();
        }

        private IEnumerator BuildStairsAndDropBall()
        {
            Canvas.SetLeft(_ball, _center.X + 10);

            var ballDropDistance = _center.Y + 20;
            var dropStep = ballDropDistance / (4 * StepSize);
            var ballY = 0d;
            Canvas.SetTop(_ball, ballY);

            while (_steps[0].Position.X < _start.X + 4 * StepSize)
            {
                for (int i = 0; i < _steps.Length; i++)
                {
                    if (i == 0 || _steps[i - 1].Position.X > _start.X + StepSize)
                    {
                        _steps[i].Update(_start, true);
                    }
                }

                ballY += dropStep;
                Canvas.SetTop(_ball, ballY);

                yield return null;
            }
        }

        private IEnumerator RunStairsAndBounceBall()
        {
            double x = 0;
            while (true)
            {
                for (int i = 0; i < _steps.Length; i++)
                {
                    _steps[i].Update(_start, true);
                }

                Canvas.SetTop(_ball, _center.Y + 17 - 2 * StepSize * Math.Abs(Math.Sin(Math.PI * x / StepSize)));

                x += 1;
                yield return null;
            }
        }
    }

    public class Step
    {
        private readonly double _size;

        private double i;

        public Step(double size)
        {
            _size = size;
            Horizontal = new Line { X1 = -_size, X2 = 0, Stroke = Brushes.Black, StrokeThickness = 1, Visibility = Visibility.Collapsed };
            Vertical = new Line { Y1 = 0, Y2 = _size, Stroke = Brushes.Black, StrokeThickness = 1, Visibility = Visibility.Collapsed };
        }

        public Line Horizontal { get; }
        public Line Vertical { get; }

        public Point Position => new Point(Canvas.GetLeft(Vertical), Canvas.GetTop(Vertical));

        public void Update(Point p, bool visible)
        {
            if (i > _size * 4) i = 0;

            var pp = p + new Vector(i, i);


            Horizontal.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
            Vertical.Visibility = Horizontal.Visibility;
            Canvas.SetLeft(Horizontal, pp.X);
            Canvas.SetTop(Horizontal, pp.Y);
            Canvas.SetLeft(Vertical, pp.X);
            Canvas.SetTop(Vertical, pp.Y);

            i += 1;
        }
    }
}
