using System.Windows;
using System.Windows.Controls;

namespace CoroutinesForWpf
{
    public static class UIElementExtensions
    {
        public static void SetPositionOnCanvas(this UIElement element, Point position)
        {
            Canvas.SetLeft(element, position.X);
            Canvas.SetTop(element, position.Y);
        }
    }
}