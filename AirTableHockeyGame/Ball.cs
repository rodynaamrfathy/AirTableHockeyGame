using SlimDX;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace AirTableHockeyGame
{
    public class Ball
    {
        public Vector3 Position { get; set; }
        public Vector3 Velocity { get; set; }
        public float Radius { get; set; }
        public float Mass { get; set; }
        public Ellipse DrawingShape { get; set; }
        public bool IsMoving { get; set; }

        public Ball(float mass, float radius)
        {
            Mass = mass;
            Radius = radius;
            Velocity = new Vector3(0, 0, 0);
            IsMoving = false;
        }

        public virtual void Faceoff()
        {
            // To be overridden by specific shapes
        }

        public virtual void UpdatePosition(float deltaTime, float canvasHeight, float canvasWidth)
        {
            Position += Velocity * deltaTime;

            // Keep the ball inside the canvas
            if (Position.X < 0 || Position.X + Radius * 2 > canvasWidth)
                Velocity.X = -Velocity.X; // Bounce off the wall

            if (Position.Y < 0 || Position.Y + Radius * 2 > canvasHeight)
                Velocity.Y = -Velocity.Y; // Bounce off the wall

            // Update the DrawingShape's position
            Canvas.SetLeft(DrawingShape, Position.X);
            Canvas.SetTop(DrawingShape, Position.Y);
        }

        public virtual void ApplyFriction(float deltaTime)
        {
            // To be overridden by specific shapes
        }
    }
}