using SlimDX;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AirTableHockeyGame
{
    internal class Ball
    {
        public Vector3 Position { get; set; }
        public Vector3 Velocity { get; set; }
        public float BouncingFactor { get; set; } = 0.7f;
        public UIElement DrawingShape { get; set; }
        public float Mass { get; set; }
        public bool IsMoving { get; set; }
        public float Radius { get; set; }

        public Ball(float mass, float radius)
        {
            // Set the initial position of the ball
            Velocity = new Vector3(0, 0, 0);
            Radius = radius;
            Mass = mass;
        }


        public Vector3 FrictionForce()
        {
            float frictionCoefficient = 0.1f;
            return new Vector3(-frictionCoefficient * Velocity.X, 0, -frictionCoefficient * Velocity.Z);
        }

        public List<Vector3> TotalForces()
        {
            List<Vector3> forces = new List<Vector3>
            {
                FrictionForce()
            };
            return forces;
        }


        public bool AreCollidedBallToBall(Ball ball2)
        {
            if (ball2 == null) return false;

            // Compute the center of each ball
            float XCenterball1 = this.Position.X + this.Radius;
            float YCenterball1 = this.Position.Y + this.Radius;
            float XCenterball2 = ball2.Position.X + ball2.Radius;
            float YCenterball2 = ball2.Position.Y + ball2.Radius;

            float diffX = XCenterball1 - XCenterball2;
            float diffY = YCenterball1 - YCenterball2;
            float distance = (float)Math.Sqrt(diffX * diffX + diffY * diffY);
            float r1 = this.Radius;
            float r2 = ball2.Radius;

            return distance <= (r1 + r2);
        }

        public void HandleOverlap(Ball ball2)
        {
            if (ball2 == null) return;

            // Compute the center of each ball
            float XCenterball1 = this.Position.X + this.Radius;
            float YCenterball1 = this.Position.Y + this.Radius;
            float XCenterball2 = ball2.Position.X + ball2.Radius;
            float YCenterball2 = ball2.Position.Y + ball2.Radius;

            Vector3 overlapVector = new Vector3(XCenterball1 - XCenterball2, YCenterball1 - YCenterball2, 0);
            float distance = overlapVector.Length();
            float overlap = (this.Radius + ball2.Radius) - distance;

            if (overlap > 0)
            {
                Vector3 separationDirection = Vector3.Normalize(overlapVector);
                Vector3 displacement = separationDirection * (overlap / 2.0f);

                this.Position += displacement;
                ball2.Position -= displacement;
            }
        }


        public void ResolveBallToBallCollison(Ball ball)
        {
            Vector3 Normal = new Vector3(this.Position.X - ball.Position.X, this.Position.Y - ball.Position.Y, 0);
            float NormNormal = (float)Math.Sqrt(Math.Pow(Normal.X, 2) + Math.Pow(Normal.Y, 2));
            Vector3 UnitNormal = new Vector3(Normal.X / NormNormal, Normal.Y / NormNormal, 0);
            Vector3 UnitTangent = new Vector3(-UnitNormal.Y, UnitNormal.X, 0);

            float V1n = Vector3.Dot(UnitNormal, ball.Velocity) * ball.BouncingFactor;
            float V1t = Vector3.Dot(UnitTangent, ball.Velocity); // unchanged before and after collision 
            float V2n = Vector3.Dot(UnitNormal, this.Velocity) * this.BouncingFactor;
            float V2t = Vector3.Dot(UnitTangent, this.Velocity); // unchanged before and after collision 

            float MassSum = this.Mass + ball.Mass;
            float MassDiff = ball.Mass - this.Mass;

            float V1nAfterCollision = ((V1n * MassDiff) + (2 * this.Mass * V2n)) / MassSum;
            float V2nAfterCollision = ((V2n * MassDiff) + (2 * ball.Mass * V1n)) / MassSum;

            Vector3 V1nAfterCollisionVector = new Vector3(UnitNormal.X * V1nAfterCollision, UnitNormal.Y * V1nAfterCollision, UnitNormal.Z * V1nAfterCollision);
            Vector3 V2nAfterCollisionVector = new Vector3(UnitNormal.X * V2nAfterCollision, UnitNormal.Y * V2nAfterCollision, UnitNormal.Z * V2nAfterCollision);
            Vector3 V1tVector = new Vector3(UnitTangent.X * V1t, UnitTangent.Y * V1t, UnitTangent.Z * V1t);
            Vector3 V2tVector = new Vector3(UnitTangent.X * V2t, UnitTangent.Y * V2t, UnitTangent.Z * V2t);

            // update final velocities

            this.Velocity = new Vector3(V2nAfterCollisionVector.X + V2tVector.X, V2nAfterCollisionVector.Y + V2tVector.Y, V2nAfterCollisionVector.Z + V2tVector.Z);
            ball.Velocity = new Vector3(V1nAfterCollisionVector.X + V1tVector.X, V1nAfterCollisionVector.Y + V1tVector.Y, V1nAfterCollisionVector.Z + V1tVector.Z);

        }

        public virtual void UpdatePosition(float deltaTime, float canvasHeight, float canvasWidth, bool IsMoving)
        {

        }
    }
}