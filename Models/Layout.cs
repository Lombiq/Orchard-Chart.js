namespace Lombiq.ChartJs.Models
{
    public class Layout
    {
        public LayoutPadding Padding { get; set; }

        public Layout(double padding = 10) =>
            Padding = new LayoutPadding
            {
                Top = padding,
                Right = padding,
                Bottom = padding,
                Left = padding,
            };

        public Layout(double top, double right, double bottom, double left) =>
            Padding = new LayoutPadding
            {
                Top = top,
                Right = right,
                Bottom = bottom,
                Left = left,
            };

        public class LayoutPadding
        {
            public double Top { get; set; }
            public double Right { get; set; }
            public double Bottom { get; set; }
            public double Left { get; set; }
        }
    }
}
