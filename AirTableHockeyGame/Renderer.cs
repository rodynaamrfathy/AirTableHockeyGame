using System.Windows.Controls;

namespace AirTableHockeyGame
{
    public class Renderer
    {
        private Canvas canvas;

        public Renderer(Canvas canvas)
        {
            this.canvas = canvas;
        }

        public void AddShapeToCanvas(Ball shape)
        {
            if (!canvas.Children.Contains(shape.DrawingShape))
            {
                canvas.Children.Add(shape.DrawingShape);
            }
        }

        public void UpdateCanvas()
        {
            // This can be extended later for more advanced rendering
        }

        public void UpdateCanvas(Ball shape)
        {
            Canvas.SetLeft(shape.DrawingShape, shape.Position.X);
            Canvas.SetTop(shape.DrawingShape, shape.Position.Y);
        }
    }
}