using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CoroutinesForWpf.Example
{
    public class Step
    {
        private readonly double _maxOffset;

        private double _offset;

        public Step(double size, double maxOffset)
        {
            _maxOffset = maxOffset;
            Horizontal = new Line { X1 = -size, X2 = 0, Stroke = Brushes.Black, StrokeThickness = 1, Visibility = Visibility.Collapsed };
            Vertical = new Line { Y1 = 0, Y2 = size, Stroke = Brushes.Black, StrokeThickness = 1, Visibility = Visibility.Collapsed };
        }

        public Line Horizontal { get; }
        public Line Vertical { get; }

        public Point Position => new Point(Canvas.GetLeft(Vertical), Canvas.GetTop(Vertical));

        public void Update(Point origin, bool visible)
        {
            Horizontal.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
            Vertical.Visibility = Horizontal.Visibility;

            if (_offset >= _maxOffset)
            {
                _offset = 0;
            }

            var position = origin + new Vector(_offset, _offset);
            Horizontal.SetPositionOnCanvas(position);
            Vertical.SetPositionOnCanvas(position);

            _offset += 1;
        }
    }
}