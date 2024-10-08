using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using AirTableHockeyGame;
using SlimDX;

namespace AirTableHockeyGame
{
    internal class Engine
    {
        public List<Ball> shapes;

        public Engine()
        {
            shapes = new List<Ball>();
        }

        public void AddShape(Ball shape)
        {
            shapes.Add(shape);
        }

        public Ball GetShapeFromDrawing(Shape drawingShape)
        {
            return shapes.FirstOrDefault(s => s.DrawingShape == drawingShape);
        }

        public void Update(float deltaTime, float canvasHeight, float canvasWidth, bool IsMoving)
        {
            bool AreCollided;
            foreach (var check in shapes)
            {
                foreach (var shape in shapes)
                {
                    if(shape is Puck puck)
                        puck.UpdatePosition(deltaTime, canvasHeight, canvasWidth, IsMoving);

                    if (check != shape && check is Ball checkBall && shape is Ball shapeBall)
                    {
                        AreCollided = checkBall.AreCollidedBallToBall(shapeBall);
                        if (AreCollided)
                        {
                            checkBall.HandleOverlap(shapeBall);
                            checkBall.ResolveBallToBallCollison(shapeBall);
                        }
                    }
                }
            }
        }
    }
}