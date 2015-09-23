namespace RadialMenuControl
{
    using System;
    using Windows.Foundation;

    public class OctetHelper
    {
        public enum Octet
        {
            NNE = 0,
            NE,
            SE,
            SSE,
            SSW,
            SW,
            NW,
            NNW
        };

        public static Octet GetOctet(double angle)
        {
            if (angle <= 45 * 1)
            {
                return Octet.NNE;
            }
            else if (angle <= 45 * 2)
            {
                return Octet.NE;
            }
            else if (angle <= 45 * 3)
            {
                return Octet.SE;
            }
            else if (angle <= 45 * 4)
            {
                return Octet.SSE;
            }
            else if (angle <= 45 * 5)
            {
                return Octet.SSW;
            }
            else if (angle <= 45 * 6)
            {
                return Octet.SW;
            }
            else if (angle <= 45 * 7)
            {
                return Octet.NW;
            }
            else if (angle <= 45 * 8)
            {
                return Octet.NNW;
            }
            else
            {
                throw new ArgumentOutOfRangeException("angle");
            }
        }

        public static Point Calculate(double radius, double startAngle, double angle)
        {
            var halfAngle = startAngle + angle / 2;

            var octet = GetOctet(halfAngle);
            var quadrantAngle = halfAngle - 90 * (int)octet;

            var adjacent = Math.Cos(Math.PI / 180 * quadrantAngle) * radius;
            var opposite = Math.Sin(Math.PI / 180 * quadrantAngle) * radius;

            switch (octet)
            {
                case Octet.NNE:
                    return new Point(opposite, -1 * adjacent);

                case Octet.NE:
                    return new Point(opposite, -1 * adjacent);

                case Octet.SE:
                    return new Point(adjacent, opposite);

                case Octet.SSE:
                    return new Point(adjacent, opposite);

                case Octet.SSW:
                    return new Point(-1 * opposite, adjacent);

                case Octet.SW:
                    return new Point(-1 * opposite, adjacent);

                case Octet.NW:
                    return new Point(-1 * adjacent, -1 * opposite);

                case Octet.NNW:
                    return new Point(-1 * adjacent, -1 * opposite);

                default:
                    throw new NotSupportedException();
            }
        }
    }
}
