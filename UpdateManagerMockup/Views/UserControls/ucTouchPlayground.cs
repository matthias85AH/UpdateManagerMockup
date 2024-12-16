using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Input;
using Avalonia.Threading;
using Avalonia.Controls.Shapes;
using System.Linq;

namespace UpdateManagerMockup.Views.UserControls
{
    public class ucTouchPlayground : Control
    {
        private List<Point> points = new();

        private int _activePointID = -1;

        private IPen _penBorder;

        private List<Brush> _brushes = new();

        public ucTouchPlayground()
        {
            _penBorder = new Pen(Brushes.White, thickness: 3.0d);

            points.Add(new Point(50,50));
            points.Add(new Point(150,50));
            points.Add(new Point(150,150));
            points.Add(new Point(50,150));

            _brushes.Add(new SolidColorBrush(Color.FromRgb(0, 163, 200)));
            _brushes.Add(new SolidColorBrush(Color.FromRgb(200, 113, 0)));
            _brushes.Add(new SolidColorBrush(Color.FromRgb(120, 200, 0)));
            _brushes.Add(new SolidColorBrush(Color.FromRgb(147, 0, 200)));
        }

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            base.OnPointerMoved(e);
            if (_activePointID >= 0)
            {
                points[_activePointID] = e.GetPosition(this);
            }
            //_cursorPoint = e.GetPosition(this);
            Dispatcher.UIThread.Post(InvalidateVisual, DispatcherPriority.Background);
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            e.Handled = true;
            e.Pointer.Capture(this);

            // Find point with shortest distance
            var pointsOrderedByDistance = points.OrderBy(p => Point.Distance(p, e.GetPosition(this))).ToList();
            if (Point.Distance(pointsOrderedByDistance[0], e.GetPosition(this)) < 30.0 )
            {
                _activePointID = points.IndexOf(pointsOrderedByDistance[0]);
            }
            
            base.OnPointerPressed(e);
        }

        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            e.Pointer.Capture(null);
            _activePointID = -1;
            base.OnPointerReleased(e);
        }

        public override void Render(DrawingContext context)
        {
            var localBounds = new Rect(new Size(this.Bounds.Width, this.Bounds.Height));
            var clip = context.PushClip(this.Bounds);
            context.FillRectangle(Brushes.Black, localBounds);

            for (int i = 0; i < points.Count; i++)
                context.DrawLine(_penBorder, points[i], points[(i+1) % points.Count]);

            for (int i = 0; i < points.Count; i++)
                context.DrawEllipse(_brushes[i], _penBorder, points[i], 20, 20);

            clip.Dispose();
        }
    }
}
