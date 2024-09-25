using SlimDX;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AirTableHockeyGame
{
    internal class Paddle : Ball
    {
        public Paddle(float mass, float radius) : base(mass, radius)
        {
            System.Drawing.Color color = System.Drawing.Color.Green;
            DrawingShape = new Ellipse()
            {
                Width = Radius * 2,
                Height = Radius * 2,
                Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B)),
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };
            Faceoff();
        }

        public override void Faceoff()
        {
            Position = new Vector3(142, 345, 0); // Starting position of the paddle
            Canvas.SetTop(DrawingShape, Position.Y);
            Canvas.SetLeft(DrawingShape, Position.X);
        }

        public void RestrictMovement(float canvasHeight, float canvasWidth)
        {
            // Restrict movement vertically
            if (Position.Y - Radius <= 280)
            {
                Position = new Vector3(Position.X, 280 + Radius, 0); // Restrict to player's half
            }
            else if (Position.Y + Radius >= canvasHeight)
            {
                Position = new Vector3(Position.X, canvasHeight - Radius, 0);
            }

            // Restrict movement horizontally (left and right bounds)
            if (Position.X <= 0)
            {
                Position = new Vector3(Radius, Position.Y, 0);
            }
            else if (Position.X + Radius * 2 >= canvasWidth)
            {
                Position = new Vector3(canvasWidth - Radius * 2, Position.Y, 0);
            }
        }

        public override void UpdatePosition(float deltaTime, float canvasHeight, float canvasWidth, bool IsMoving)
        {
            // Paddle movement is controlled by the player, so this method might just
            // restrict the paddle movement to within bounds.
            RestrictMovement(canvasHeight, canvasWidth);

            // Update the drawing shape position
            if (this.DrawingShape != null)
            {
                Canvas.SetTop(DrawingShape, Position.Y);
                Canvas.SetLeft(DrawingShape, Position.X);
            }
        }
    }
}