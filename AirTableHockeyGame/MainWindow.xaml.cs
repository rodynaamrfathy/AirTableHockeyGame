using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows;
using SlimDX;
using System.Numerics;
using System.Windows.Controls;
using System.Reflection.Metadata;

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
            CreatePuck(new Puck(2.0f, 20f));
            CreatePlayerPaddel(new Paddle(10.0f, 30f, 120, 350));
            CreateOnlinePlayerPaddel(new Paddle(10.0f, 30f, 120, 190));

            // Start the free fall simulation
            Task.Run(() => GameLoop());
        }
        private void CreatePuck(Ball shape)
        {
            engine.AddShape(shape);
            renderer.AddShapeToCanvas(shape);
            renderer.UpdateCanvas(shape);
        }
        private void CreatePlayerPaddel(Ball shape)
        {
            shape.DrawingShape.MouseLeftButtonDown += Shape_MouseLeftButtonDown;
            shape.DrawingShape.MouseLeftButtonUp += Shape_MouseLeftButtonUp;
            ballcanvas.MouseMove += Ballcanvas_MouseMove;

            engine.AddShape(shape);
            renderer.AddShapeToCanvas(shape);
            renderer.UpdateCanvas(shape);
        }
        private void CreateOnlinePlayerPaddel(Ball shape)
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


                // Get the position of the puck
                var puckPosition = engine.shapes[0].Position; // Assume this method gets the puck's current position

                // Calculate the direction vector towards the puck
                var directionX = puckPosition.X - initialMousePosition.X;
                var directionY = puckPosition.Y - initialMousePosition.Y;

                // Normalize the direction vector
                var length = Math.Sqrt(directionX * directionX + directionY * directionY);
                if (length > 0)
                {
                    directionX /= length;
                    directionY /= length;
                }

                // Set the velocity towards the puck (adjust speed factor as needed)
                float speed = 5.0f; 
                draggedShape.Velocity = new SlimDX.Vector3((float)(directionX * speed), (float)(directionY * speed), 0);

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
                draggedShape.Velocity = new SlimDX.Vector3(velocityX, velocityY, 0);
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
                draggedShape.UpdatePosition(0.16f, (float)ballcanvas.ActualHeight, (float)ballcanvas.ActualWidth, true);
                renderer.UpdateCanvas(draggedShape); // Redraw the paddle at the new position
            }
        }

        private void GameLoop()
        {
            while (true) // Continuously run the simulation
            {
                if (draggedShape != null && !draggedShape.IsMoving)
                {
                    Dispatcher.Invoke(() =>
                    {
                        float deltaTime = (float)stopwatch.Elapsed.TotalSeconds * 10;
                        stopwatch.Restart();
                        // Update physics and check for collision
                        engine.Update(deltaTime, (float)ballcanvas.ActualHeight, (float)ballcanvas.ActualWidth, true);
                        renderer.UpdateCanvas(); // Redraw the canvas to reflect changes
                    });
                }

                Thread.Sleep(5); // Control update frequency
            }
        }

    }
}