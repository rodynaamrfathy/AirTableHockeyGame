using SlimDX;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
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
        public override void Faceoff()
        {
            Position = new Vector3(142, 345, 0); // Starting position of the paddle
            Canvas.SetTop(DrawingShape, Position.Y);
            Canvas.SetLeft(DrawingShape, Position.X);
        }

        public void RestrictMovement(float canvasHeight, float canvasWidth)
        {
            // Restrict movement vertically
            if (Position.Y - Radius <= 270)
            {
                Position = new Vector3(Position.X, 270 + Radius, 0); // Restrict to player's half
            }
            else if (Position.Y + Radius - 30 >= canvasHeight)
            {
                Position = new Vector3(Position.X, canvasHeight - Radius - 30, 0);
            }

            // Restrict movement horizontally (left and right bounds)
            if (Position.X <= - 30)
            {
                Position = new Vector3(Radius - 30, Position.Y, 0);
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