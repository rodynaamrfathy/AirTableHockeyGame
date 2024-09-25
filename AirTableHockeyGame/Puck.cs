 using SlimDX;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AirTableHockeyGame
{
    public class Puck : Ball
    {
        public Puck(float mass, float radius) : base(mass, radius)
        {
            DrawingShape = new Ellipse()
            {
                Width = Radius * 2,
                Height = Radius * 2,
                Fill = new SolidColorBrush(Colors.Black),
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };
            Faceoff();
        }

        public override void Faceoff()
        {
            Position = new Vector3(130, 280, 0);
            Canvas.SetTop(DrawingShape, Position.Y);
            Canvas.SetLeft(DrawingShape, Position.X);
        }

        public override void UpdatePosition(float deltaTime, float canvasHeight, float canvasWidth)
        {
            // Call base method to update position and apply velocity
            base.UpdatePosition(deltaTime, canvasHeight, canvasWidth);
        }

        public override void ApplyFriction(float deltaTime)
        {
            float frictionCoefficient = 0.995f;
            Velocity *= frictionCoefficient; // Reduce velocity over time
        }
    }
}