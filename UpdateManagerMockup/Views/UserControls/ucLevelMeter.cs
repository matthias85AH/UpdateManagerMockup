using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Input;
using Avalonia.Threading;

namespace UpdateManagerMockup.Views.UserControls
{
    public class ucLevelMeter : Control
    {
        public static readonly StyledProperty<double> LevelProperty = AvaloniaProperty.Register<ucFrequencyPlot, double>(nameof(Level), 70.0d);
        public double Level { get => GetValue(LevelProperty); set => SetValue(LevelProperty, value); }

        public static readonly StyledProperty<bool> DemoModeProperty = AvaloniaProperty.Register<ucFrequencyPlot, bool>(nameof(DemoModeProperty), false);
        public bool DemoMode { get => GetValue(DemoModeProperty); set => SetValue(DemoModeProperty, value); }

        private int numRects = 30;

        private double gapRatio = 0.1;

        private IPen _pen;

        private Brush _brushGray = new SolidColorBrush(Color.FromRgb(61, 61, 61));
        private Brush _brushGreen = new SolidColorBrush(Color.FromRgb(205, 255, 26));
        private Brush _brushOrange = new SolidColorBrush(Color.FromRgb(255, 155, 65));
        private Brush _brushRed = new SolidColorBrush(Color.FromRgb(249, 84, 42));

        private System.Diagnostics.Stopwatch _timeKeeper = System.Diagnostics.Stopwatch.StartNew();

        public ucLevelMeter()
        {
            _pen = new Pen(new SolidColorBrush(Colors.DarkGray), lineCap: PenLineCap.Round);
        }

        public override void Render(DrawingContext context)
        {
            var localBounds = new Rect(new Size(this.Bounds.Width, this.Bounds.Height));
            var clip = context.PushClip(this.Bounds);
            context.FillRectangle(Brushes.Black, localBounds);

            double rectPlusGapHeight = this.Bounds.Height / numRects;

            for (int i = 0; i < numRects; i++)
            {
                double x = 6.0d;
                double y = rectPlusGapHeight * i;
                double width = this.Bounds.Width - 10.0d;
                double height = rectPlusGapHeight * (1.0d - gapRatio);

                Brush currentBrush = null;

                // Determine whether the rectangle gets filled
                if (Level > ((numRects - (double)i)/numRects) * 100.0d)
                {
                    // Determine Color
                    if (i / (double)numRects < 0.2d)
                        currentBrush = _brushRed;
                    else if (i / (double)numRects < 0.4d)
                        currentBrush = _brushOrange;
                    else 
                        currentBrush = _brushGreen; 
                }
                else
                {
                    currentBrush = _brushGray;
                }

                context.FillRectangle(currentBrush, new Rect(x, y, width, height), 5.0f);
            }

            clip.Dispose();

            if (DemoMode)
            {
                Level = 70.0 + Math.Sin(_timeKeeper.ElapsedMilliseconds / 250.0d) * 20.0;
            }

            // oh and draw again when you can, no rush, right? 
            Dispatcher.UIThread.Post(InvalidateVisual, DispatcherPriority.Background);
        }
    }
}
