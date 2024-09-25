using SlimDX;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AirTableHockeyGame
{
    internal class Puck : Ball
    {
        public Puck(float mass, float radius) : base(mass, radius)
        {
            DrawingShape = CreatePuck();
            Faceoff();
        }

        private Ellipse CreatePuck()
        {
            var puck = new Ellipse
            {
                Fill = new SolidColorBrush(Colors.Black),
                Width = Radius * 2,
                Height = Radius * 2,
                Effect = new System.Windows.Media.Effects.DropShadowEffect
                {
                    Color = Colors.Black,
                    Direction = 270,
                    ShadowDepth = 3,
                    BlurRadius = 5,
                    Opacity = 0.5
                }
            };
            return puck;
        }

        public override void Faceoff()
        {
            Position = new Vector3(130, 280, 0);
            Canvas.SetTop(DrawingShape, Position.Y);
            Canvas.SetLeft(DrawingShape, Position.X);
        }

        public override void UpdatePosition(float deltaTime, float canvasHeight, float canvasWidth, bool IsMoving)
        {
            if (IsMoving)
            {
                // Update position based on velocity
                Position += Velocity * deltaTime;

                // Check for collisions with walls and bounce
                if (Position.Y <= 0) // Top wall
                {
                    Position = new Vector3(Position.X, 0, Position.Z);
                    Velocity = new Vector3(Velocity.X, -Velocity.Y * BouncingFactor, Velocity.Z);
                }

                if (Position.X + Radius * 2 >= canvasWidth) // Right wall
                {
                    Position = new Vector3(canvasWidth - Radius * 2, Position.Y, Position.Z);
                    Velocity = new Vector3(-Velocity.X * BouncingFactor, Velocity.Y, Velocity.Z);
                }

                if (Position.X <= 0) // Left wall
                {
                    Position = new Vector3(0, Position.Y, Position.Z);
                    Velocity = new Vector3(-Velocity.X * BouncingFactor, Velocity.Y, Velocity.Z);
                }

                if (Position.Y + Radius * 2 >= canvasHeight) // Bottom wall
                {
                    Position = new Vector3(Position.X, canvasHeight - Radius * 2, Position.Z);
                    Velocity = new Vector3(Velocity.X, -Velocity.Y * BouncingFactor, Velocity.Z);

                    if (Math.Abs(Velocity.Y) < 1 && Math.Abs(Velocity.X) < 1)
                    {
                        IsMoving = false; // Stop moving if speed is minimal
                        Velocity = Vector3.Zero;
                    }
                }

                // Apply friction
                ApplyFriction(deltaTime);

                // Update the drawing shape position
                if (this.DrawingShape != null)
                {
                    Canvas.SetTop(DrawingShape, Position.Y);
                    Canvas.SetLeft(DrawingShape, Position.X);
                }
            }
        }

        private void ApplyFriction(float deltaTime)
        {
            float frictionCoefficient = 0.995f; // Adjust this for different levels of friction
            Velocity *= frictionCoefficient; // Reduce velocity over time
        }
    }
}