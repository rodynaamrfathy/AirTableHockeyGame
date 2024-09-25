using AirTableHockeyGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace AirTableHockeyGame
{
    internal class Renderer
    {
        private Canvas canvas;

        public Renderer(Canvas canvas)
        {
            this.canvas = canvas;
        }

        public void AddShapeToCanvas(Ball shape)
        {
            canvas.Children.Add(shape.DrawingShape);
        }

        public void UpdateCanvas(Ball shape)
        {
            Canvas.SetLeft(shape.DrawingShape, shape.Position.X);
            Canvas.SetTop(shape.DrawingShape, shape.Position.Y);
        }

        public void SetInitialShapePosition(Ball shape)
        {
            Canvas.SetLeft(shape.DrawingShape, (float)0.5 * (canvas.ActualWidth + shape.Radius));
            Canvas.SetTop(shape.DrawingShape, (float)canvas.ActualHeight - shape.Radius);
        }

        public void UpdateCanvas()
        {
            foreach (UIElement child in canvas.Children)
            {
                if (child is Shape drawingShape)
                {
                    var shape = canvas.Children.OfType<Ball>().FirstOrDefault(s => s.DrawingShape == drawingShape);
                    if (shape != null)
                    {
                        UpdateCanvas(shape);
                    }
                }
            }
        }
    }



}
