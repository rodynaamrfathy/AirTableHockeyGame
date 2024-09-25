using SlimDX;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AirTableHockeyGame
{
    public class Paddle : Ball
    {
        public Paddle(float mass, float radius) : base(mass, radius)
        {
            DrawingShape = new Ellipse()
            {
                Width = Radius * 2,
                Height = Radius * 2,
                Fill = new SolidColorBrush(Colors.Green),
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };
            Faceoff();
        }

        public override void Faceoff()
        {
            Position = new Vector3(142, 345, 0);
            Canvas.SetTop(DrawingShape, Position.Y);
            Canvas.SetLeft(DrawingShape, Position.X);
        }

        public void RestrictMovement(float canvasHeight, float canvasWidth)
        {
            // Restrict paddle to the playing field
            if (Position.X < 0) Position = new Vector3(0, Position.Y, 0);
            if (Position.X + Radius * 2 > canvasWidth) Position = new Vector3(canvasWidth - Radius * 2, Position.Y, 0);
            if (Position.Y < 0) Position = new Vector3(Position.X, 0, 0);
            if (Position.Y + Radius * 2 > canvasHeight) Position = new Vector3(Position.X, canvasHeight - Radius * 2, 0);
            Canvas.SetLeft(DrawingShape, Position.X);
            Canvas.SetTop(DrawingShape, Position.Y);
        }
    }
}