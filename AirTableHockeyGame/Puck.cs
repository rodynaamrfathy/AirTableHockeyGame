using SlimDX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AirTableHockeyGame
{
    internal class Puck : Ball
    {
        public Puck(float mass, Color color, float radius, Vector3 Pos) : base(mass, color, radius, Pos)
        {

        }
        public override void UpdatePosition(float deltaTime, float canvasHeight, float canvasWidth, bool IsMoving)
        {
            // Update position based on velocity
            Position += Velocity * deltaTime;

            // Check if the shape hits the top of the canvas
            if (Position.Y <= 0)
            {
                Position = new Vector3(Position.X, 0, Position.Z);
                Velocity = new Vector3(Velocity.X, -Velocity.Y * BouncingFactor, Velocity.Z);
            }

            // Check if the shape hits the right edge of the canvas
            if (Position.X + Radius * 2 >= canvasWidth)
            {
                Position = new Vector3(canvasWidth - Radius * 2, Position.Y, Position.Z);
                Velocity = new Vector3(-Velocity.X * BouncingFactor, Velocity.Y, Velocity.Z);
            }

            // Check if the shape hits the left edge of the canvas
            if (Position.X <= 0)
            {
                Position = new Vector3(0, Position.Y, Position.Z);
                Velocity = new Vector3(-Velocity.X * BouncingFactor, Velocity.Y, Velocity.Z);
            }

            // Check if the shape hits the bottom of the canvas
            if (Position.Y + Radius * 2 >= canvasHeight)
            {
                Position = new Vector3(Position.X, canvasHeight - Radius * 2, Position.Z);
                Velocity = new Vector3(Velocity.X, -Velocity.Y * BouncingFactor, Velocity.Z);

                // If the absolute value of velocity is small, stop the shape
                if (Math.Abs(Velocity.Y) < 1 && Math.Abs(Velocity.X) < 1)
                {
                    IsMoving = false; // Assuming there's an IsMoving property in Shapes
                    Velocity = Vector3.Zero;
                }
            }

            // Update the drawing shape position if it's not null
            if (this.DrawingShape != null)
            {
                Canvas.SetTop(DrawingShape, Position.Y);
                Canvas.SetLeft(DrawingShape, Position.X);
            }
        }
    }
}
