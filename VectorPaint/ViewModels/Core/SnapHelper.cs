using Avalonia;

namespace VectorPaint.ViewModels.Core;

public static class SnapHelper
{
    public static double SnapValue(double value, double snap)
    {
        if (snap == 0.0)
        {
            return value;
        }
        var c = value % snap;
        var r = c >= snap / 2.0 ? value + snap - c : value - c;
        return r;
    }

    public static Point SnapPoint(Point point, double snapX = 5, double snapY = 5, bool enabled = true)
    {
        if (enabled)
        {
            var pointX = SnapValue(point.X, snapX);
            var pointY = SnapValue(point.Y, snapY);
            return new Point(pointX, pointY);
        }

        return point;
    }
}
