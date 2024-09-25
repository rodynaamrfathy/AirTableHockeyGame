using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows;
using SlimDX;
using System.Numerics;
using System.Windows.Controls;

namespace AirTableHockeyGame
{
    public partial class MainWindow : Window
    {
        private Engine engine = new Engine();
        private Renderer renderer;
        private bool isDragged = false;
        private Stopwatch stopwatch;
        private Ball draggedShape;
        private Point initialMousePosition;
        private DateTime initialMouseDownTime;

        public MainWindow()
        {
            InitializeComponent();
            renderer = new Renderer(ballcanvas);
            stopwatch = new Stopwatch();

            // Create shapes
            CreatePuck(new Puck(4.0f, 20f)); // Puck object
            CreatePlayerPaddle(new Paddle(2.0f, 30f)); // Player Paddle
            //CreateOnlinePlayerPaddle(new Paddle(2.0f, 30f)); // Uncomment when adding multiplayer functionality

            // Start the free-fall simulation
            Task.Run(() => GameLoop());
        }

        private void CreatePuck(Ball shape)
        {
            engine.AddShape(shape);
            renderer.AddShapeToCanvas(shape);
            renderer.UpdateCanvas(shape);
        }

        private void CreatePlayerPaddle(Ball shape)
        {
            shape.DrawingShape.MouseLeftButtonDown += Shape_MouseLeftButtonDown;
            shape.DrawingShape.MouseLeftButtonUp += Shape_MouseLeftButtonUp;
            ballcanvas.MouseMove += Ballcanvas_MouseMove;

            engine.AddShape(shape);
            renderer.AddShapeToCanvas(shape);
            renderer.UpdateCanvas(shape);
        }

        private void CreateOnlinePlayerPaddle(Ball shape)
        {
            engine.AddShape(shape);
            renderer.AddShapeToCanvas(shape);
            renderer.UpdateCanvas(shape);
        }

        private void Shape_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            draggedShape = engine.GetShapeFromDrawing(sender as Shape);
            if (draggedShape != null)
            {
                isDragged = true;
                draggedShape.DrawingShape.CaptureMouse();
                initialMousePosition = e.GetPosition(ballcanvas);
                initialMouseDownTime = DateTime.Now;
                stopwatch.Reset();
            }
        }

        private void Shape_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (draggedShape != null && isDragged)
            {
                isDragged = false;
                draggedShape.DrawingShape.ReleaseMouseCapture();
                var finalMousePosition = e.GetPosition(ballcanvas);
                var timeTaken = DateTime.Now - initialMouseDownTime;

                // Calculate the velocity based on mouse movement and the time taken
                var velocityX = (float)(finalMousePosition.X - initialMousePosition.X) / (float)timeTaken.TotalSeconds;
                var velocityY = (float)(finalMousePosition.Y - initialMousePosition.Y) / (float)timeTaken.TotalSeconds;

                // Set the paddle's velocity
                draggedShape.Velocity = new SlimDX.Vector3(velocityX / 10, velocityY / 10, 0);
                stopwatch.Start(); // Start the stopwatch for updating physics
            }
        }

        private void Ballcanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragged && draggedShape != null)
            {
                var mousePos = e.GetPosition(ballcanvas);
                // Update the paddle's position directly when dragging
                draggedShape.Position = new SlimDX.Vector3((float)(mousePos.X - draggedShape.Radius),
                                                           (float)(mousePos.Y - draggedShape.Radius), 0);
                renderer.UpdateCanvas(draggedShape); // Redraw the paddle at the new position
            }
        }

        private void GameLoop()
        {
            while (true) // Continuously run the simulation
            {
                Dispatcher.Invoke(() =>
                {
                    float deltaTime = (float)stopwatch.Elapsed.TotalSeconds * 10; // Adjust deltaTime for smoother updates
                    stopwatch.Restart();
                    // Update physics and check for collision
                    engine.Update(deltaTime, (float)ballcanvas.ActualHeight, (float)ballcanvas.ActualWidth, true);
                    renderer.UpdateCanvas(); // Redraw the canvas to reflect changes
                });

                Thread.Sleep(5); // Control update frequency (adjust if needed)
            }
        }
    }
}