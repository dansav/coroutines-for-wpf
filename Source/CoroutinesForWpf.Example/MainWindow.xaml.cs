using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CoroutinesForWpf.Example
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const double StepSize = 40;
        private const int StepCount = 6; // must be even
        private const int MiddleStep = StepCount / 2;

        private readonly Ellipse _ball;
        private readonly Step[] _steps;
        private readonly BallAnimation _ballAnimation;

        private Point _center;
        private Point _start;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            SizeChanged += OnSizeChanged;

            _ball = new Ellipse { Width = 20, Height = 20, Stroke = Brushes.Black, StrokeThickness = 2 };
            _steps = Enumerable.Range(0, StepCount).Select(_ => new Step(StepSize, StepSize * StepCount)).ToArray();

            _ballAnimation = new BallAnimation(_ball, StepSize);
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

            Start.Coroutine(StairsAnimation());
            Start.Coroutine(_ballAnimation);
        }

        private void Initialize()
        {
            _center = new Point(Math.Round(Scene.ActualWidth / 2), Math.Round(Scene.ActualHeight / 2));
            _start = _center + new Vector(-MiddleStep * StepSize, -MiddleStep * StepSize);

            foreach (UIElement sceneChild in Scene.Children)
            {
                Canvas.SetLeft(sceneChild, 0);
                Canvas.SetTop(sceneChild, 0);
            }

            _ballAnimation.Center = _center;
        }

        private IEnumerator StairsAnimation()
        {
            yield return BuildStairs();
            yield return RunStairs();
        }

        private IEnumerator BuildStairs()
        {
            while (_steps[0].Position.X < _start.X + StepCount * StepSize - 1)
            {
                for (int i = 0; i < _steps.Length; i++)
                {
                    if (i == 0 || _steps[i - 1].Position.X >= _start.X + StepSize)
                    {
                        _steps[i].Update(_start, true);
                    }
                }

                _ballAnimation.Continue = _steps[MiddleStep].Position.X >= _start.X + StepSize;
                yield return null;
            }
        }

        private IEnumerator RunStairs()
        {
            while (true)
            {
                foreach (var step in _steps)
                {
                    step.Update(_start, true);
                }
                yield return null;
            }
        }
    }
}
