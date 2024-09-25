using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows;
using SlimDX;
using System.Windows.Controls;

namespace AirTableHockeyGame
{
    public partial class MainWindow : Window
    {
        private Engine engine = new Engine();
        private Renderer renderer;
        private bool isDragged = false;
        private Paddle draggedPaddle;
        private Point initialMousePosition;
        private Stopwatch stopwatch;

        public MainWindow()
        {
            InitializeComponent();
            renderer = new Renderer(ballcanvas);
            stopwatch = new Stopwatch();

            // Create shapes
            CreatePuck(new Puck(4.0f, 20f));
            CreatePlayerPaddle(new Paddle(2.0f, 30f));
            CreateOnlinePlayerPaddle(new Paddle(2.0f, 30f));

            // Start the game loop
            Task.Run(() => GameLoop());
        }

        private void CreatePuck(Ball shape)
        {
            engine.AddShape(shape);
            renderer.AddShapeToCanvas(shape);
            renderer.UpdateCanvas(shape);
        }

        private void CreatePlayerPaddle(Paddle paddle)
        {
            paddle.DrawingShape.MouseLeftButtonDown += Shape_MouseLeftButtonDown;
            paddle.DrawingShape.MouseLeftButtonUp += Shape_MouseLeftButtonUp;
            ballcanvas.MouseMove += Ballcanvas_MouseMove;

            engine.AddShape(paddle);
            renderer.AddShapeToCanvas(paddle);
            renderer.UpdateCanvas(paddle);
        }

        private void CreateOnlinePlayerPaddle(Paddle paddle)
        {
            engine.AddShape(paddle);
            renderer.AddShapeToCanvas(paddle);
            renderer.UpdateCanvas(paddle);
        }

        // Event: Mouse down on paddle
        private void Shape_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Ellipse paddleShape)
            {
                draggedPaddle = engine.GetPaddle(); // Get the player's paddle
                isDragged = true;
                initialMousePosition = e.GetPosition(ballcanvas); // Get initial mouse position
                paddleShape.CaptureMouse(); // Capture the mouse to keep receiving events
            }
        }

        // Event: Mouse up, stop dragging
        private void Shape_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isDragged && draggedPaddle != null)
            {
                isDragged = false;
                draggedPaddle = null;
                if (sender is Ellipse paddleShape)
                {
                    paddleShape.ReleaseMouseCapture(); // Release mouse capture
                }
            }
        }

        // Event: Mouse movement to move the paddle
        private void Ballcanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragged && draggedPaddle != null)
            {
                Point currentMousePosition = e.GetPosition(ballcanvas);
                Vector delta = currentMousePosition - initialMousePosition; // Calculate the movement delta

                // Update paddle position based on mouse movement
                draggedPaddle.Position = new Vector3(
                    (float)(draggedPaddle.Position.X + delta.X),
                    (float)(draggedPaddle.Position.Y + delta.Y),
                    0
                );

                // Restrict movement to the canvas bounds
                draggedPaddle.RestrictMovement((float)ballcanvas.ActualHeight, (float)ballcanvas.ActualWidth);

                // Update initial mouse position for next move
                initialMousePosition = currentMousePosition;

                // Update the canvas to reflect paddle's new position
                renderer.UpdateCanvas(draggedPaddle);
            }
        }

        private void GameLoop()
        {
            stopwatch.Start();
            while (true) // Continuously run the simulation
            {
                float deltaTime = (float)stopwatch.Elapsed.TotalSeconds;
                stopwatch.Restart();

                Dispatcher.Invoke(() =>
                {
                    // Update physics and check for collisions
                    engine.Update(deltaTime, (float)ballcanvas.ActualHeight, (float)ballcanvas.ActualWidth);
                    renderer.UpdateCanvas(); // Redraw the canvas to reflect changes
                });

                Thread.Sleep(5); // Control update frequency
            }
        }
    }
}