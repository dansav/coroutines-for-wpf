using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace CoroutinesForWpf
{
    public class BallAnimation : CoroutineBase
    {
        private readonly IEnumerator _mainCoroutine;
        private readonly Ellipse _ball;
        private readonly double _ballHeight;
        private readonly double _stepSize;
        private readonly double _halfStepSize;

        private Point _center;

        public BallAnimation(Ellipse ball, double stepSize)
        {
            _ball = ball;
            _stepSize = stepSize;
            _halfStepSize = _stepSize / 2;
            _ballHeight = ball.Height - 3; // line thickness ???

            _mainCoroutine = CreateMainCoroutine();
        }

        public override object Current => _mainCoroutine.Current;

        public bool Continue { private get; set; }

        public Point Center
        {
            set
            {
                _center = value;
                Canvas.SetLeft(_ball, _center.X + _halfStepSize);
            }
        }

        public override bool MoveNext()
        {
            return _mainCoroutine.MoveNext();
        }

        private IEnumerator CreateMainCoroutine()
        {
            yield return new WaitUntil(() => Continue);
            yield return RunDropBall();
            yield return RunBounceBall();
        }

        private IEnumerator RunDropBall()
        {
            var ballDropTarget = _center.Y + _halfStepSize;
            var dropStep = ballDropTarget / _stepSize;
            var ballY = 0d;
            Canvas.SetTop(_ball, ballY);

            while (ballY <= ballDropTarget)
            {
                ballY += dropStep;
                Canvas.SetTop(_ball, ballY);
                yield return null;
            }
        }

        private IEnumerator RunBounceBall()
        {
            double bounceHeight = 2 * _stepSize;

            int x = 0;
            while (true)
            {
                Canvas.SetTop(_ball, _center.Y + _ballHeight - bounceHeight * Math.Sin(Math.PI * x / _stepSize));
                x += 1;
                if (x >= _stepSize) x = 0;
                yield return null;
            }
            // ReSharper disable once IteratorNeverReturns
        }
    }
}