using System.Collections.Generic;
using SlimDX;

namespace AirTableHockeyGame
{
    public class Engine
    {
        private List<Ball> shapes = new List<Ball>();
        
        public void AddShape(Ball shape)
        {
            shapes.Add(shape);
        }

        public void Update(float deltaTime, float canvasHeight, float canvasWidth)
        {
            foreach (var shape in shapes)
            {
                shape.UpdatePosition(deltaTime, canvasHeight, canvasWidth);
            }
        }

        public Paddle GetPaddle()
        {
            foreach (var shape in shapes)
            {
                if (shape is Paddle paddle)
                {
                    return paddle;
                }
            }
            return null; // If no paddle is found
        }

        public Puck GetPuck()
        {
            foreach (var shape in shapes)
            {
                if (shape is Puck puck)
                {
                    return puck;
                }
            }
            return null; // If no puck is found
        }
    }
}