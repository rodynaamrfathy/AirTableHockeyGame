using SlimDX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Effects;
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
            Position = new Vector3(142, 345, 0);
            Canvas.SetTop(DrawingShape, Position.Y);
            Canvas.SetLeft(DrawingShape, Position.X);
        }


        public void RestrictMovement(float canvasHeight, float canvasWidth)
        {
            if (Position.Y - Radius <= 280)
            {
                Position = new Vector3(Position.X, 280 + Radius, 0);
            }
            else if (Position.Y + Radius >= canvasHeight)
            {
                Position = new Vector3(Position.X, canvasHeight - Radius, 0);
            }
            // Restrict movement to the left boundary
            if (Position.X <= 0)
            {
                Position = new Vector3(Radius, Position.Y, 0);
            }

            // Restrict movement to the right boundary
            else if (Position.X + Radius >= canvasWidth)
            {
                Position = new Vector3(canvasWidth - Radius, Position.Y, 0);
            }
        }


        private Canvas CreatePaddle()
        {
            var paddleMain = new Ellipse
            {
                Fill = new SolidColorBrush(Colors.Green),
                StrokeThickness = 3,
                Width = 50,
                Height = 50,
                Effect = new DropShadowEffect
                {
                    Color = Colors.Black,
                    Direction = 270,
                    ShadowDepth = 5,
                    BlurRadius = 10,
                    Opacity = 0.5
                }
            };

            var paddleHandle = new Ellipse
            {
                Fill = new SolidColorBrush(Colors.Green),
                Width = 15,
                Height = 15,
                Effect = new DropShadowEffect
                {
                    Color = Colors.Black,
                    Direction = 270,
                    ShadowDepth = 5,
                    BlurRadius = 10,
                    Opacity = 0.5
                }
            };

            var paddleCanvas = new Canvas
            {
                Width = 50,
                Height = 50
            };

            Canvas.SetLeft(paddleMain, -17);
            Canvas.SetTop(paddleMain, -17);
            paddleCanvas.Children.Add(paddleMain);

            Canvas.SetLeft(paddleHandle, 0);
            Canvas.SetTop(paddleHandle, 0);
            paddleCanvas.Children.Add(paddleHandle);

            Canvas.SetLeft(paddleCanvas, Position.X); // Set initial position for Paddle
            Canvas.SetTop(paddleCanvas, Position.Y); // Set Y position based on the Paddle's position

            return paddleCanvas;
        }
        public override void UpdatePosition(float deltaTime, float canvasHeight, float canvasWidth, bool IsMoving)
        {
           
        }
    }
}
