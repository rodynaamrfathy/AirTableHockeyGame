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

            // Start the simulation
            Task.Run(() => GameLoop());
        }

        private void CreatePuck(Puck puck)
        {
            engine.AddShape(puck);
            renderer.AddShapeToCanvas(puck);
            renderer.UpdateCanvas(puck);
        }

        private void CreatePlayerPaddle(Paddle paddle)
        {
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

        private bool CheckCollision(Paddle paddle, Puck puck)
        {
            float distance = Vector3.Distance(paddle.Position, puck.Position);
            return distance <= paddle.Radius + puck.Radius;
        }

        private void ApplyImpulse(Paddle paddle, Puck puck)
        {
            // Calculate relative velocity
            Vector3 relativeVelocity = paddle.Velocity - puck.Velocity;

            // Calculate impulse based on masses and relative velocity
            Vector3 impulse = (2 * paddle.Mass / (paddle.Mass + puck.Mass)) * relativeVelocity;

            // Apply impulse to the puck's velocity
            puck.Velocity += impulse;
        }

        private void GameLoop()
        {
            stopwatch.Start();
            while (true) // Continuously run the simulation
            {
                Dispatcher.Invoke(() =>
                {
                    float deltaTime = (float)stopwatch.Elapsed.TotalSeconds * 10;
                    stopwatch.Restart();

                    // Update physics and check for collision
                    engine.Update(deltaTime, (float)ballcanvas.ActualHeight, (float)ballcanvas.ActualWidth);

                    Paddle playerPaddle = engine.GetPaddle();
                    Puck puck = engine.GetPuck();

                    if (CheckCollision(playerPaddle, puck))
                    {
                        ApplyImpulse(playerPaddle, puck); // Apply the impulse on collision
                    }

                    puck.ApplyFriction(deltaTime);

                    renderer.UpdateCanvas(); // Redraw the canvas to reflect changes
                });

                Thread.Sleep(5); // Control update frequency
            }
        }
    }
}