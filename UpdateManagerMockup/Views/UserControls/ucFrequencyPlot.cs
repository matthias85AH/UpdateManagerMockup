using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Input;
using Avalonia.Threading;

namespace UpdateManagerMockup.Views.UserControls
{
    public class ucFrequencyPlot : Control
    {
        public static readonly StyledProperty<double> FrequencyProperty = AvaloniaProperty.Register<ucFrequencyPlot, double>(nameof(Frequency), 1.0d);
        public double Frequency { get => GetValue(FrequencyProperty); set => SetValue(FrequencyProperty, value); }

        public static readonly StyledProperty<double> AmplitudeProperty = AvaloniaProperty.Register<ucFrequencyPlot, double>(nameof(Amplitude), 1.0d);
        public double Amplitude { get => GetValue(AmplitudeProperty); set => SetValue(AmplitudeProperty, value); }

        private int numGridLinesX = 10;
        private int numGridLinesY = 5;

        private int numWavePoints = 300;

        private IPen _penGrid;
        private IPen _penWave;

        private System.Diagnostics.Stopwatch _timeKeeper = System.Diagnostics.Stopwatch.StartNew();

        public ucFrequencyPlot()
        {
            _penGrid = new Pen(new SolidColorBrush(Colors.DarkGray), lineCap: PenLineCap.Round);
            _penWave = new Pen(new SolidColorBrush(Color.FromRgb(0, 163, 200)), thickness: 5.0d);
        }

        public override void Render(DrawingContext context)
        {
            var localBounds = new Rect(new Size(this.Bounds.Width, this.Bounds.Height));
            var clip = context.PushClip(this.Bounds);
            //context.DrawRectangle(Brushes.Black, _pen, localBounds, 1.0d);
            context.FillRectangle(Brushes.Black, localBounds);

            // Draw Grid
            double gridXDelta = localBounds.Width / (numGridLinesX + 1);
            for (int gridLineId = 0; gridLineId < numGridLinesX; gridLineId++)
            {
                double xCoord = gridXDelta + gridLineId * gridXDelta;
                context.DrawLine(_penGrid, new Point(xCoord, 0.0d), new Point(xCoord, localBounds.Height));
            }

            double gridYDelta = localBounds.Height / (numGridLinesY + 1);
            for (int gridLineId = 0; gridLineId < numGridLinesY; gridLineId++)
            {
                double yCoord = gridYDelta + gridLineId * gridYDelta;
                context.DrawLine(_penGrid, new Point(0.0d, yCoord), new Point(localBounds.Width, yCoord));
            }

            // Create a new StreamGeometry to store the updated line path
            var waveGeometry = new StreamGeometry();

            // Open the StreamGeometry for writing
            using (var cntx = waveGeometry.Open())
            {
                // Move to the first point (this is like StartPoint)
                cntx.BeginFigure(new Point(0.0d, localBounds.Height / 2.0), false); // false means no fill

                // Add line segments to the StreamGeometry
                for (int pID = 1; pID < numWavePoints; pID++)
                {
                    double xCoord = pID * localBounds.Width / numWavePoints;
                    double yCoord = (localBounds.Height / 2.0d) + Math.Sin(pID / (10.0 + 0.2d * (100-Frequency)) + _timeKeeper.ElapsedMilliseconds / 250.0d) * Amplitude * 0.008 * localBounds.Height / 2.0d;
                    cntx.LineTo(new Point(xCoord, yCoord));
                }
            }

            context.DrawGeometry(null, _penWave, waveGeometry);

            clip.Dispose();

            // oh and draw again when you can, no rush, right? 
            Dispatcher.UIThread.Post(InvalidateVisual, DispatcherPriority.Background);
        }
    }
}
