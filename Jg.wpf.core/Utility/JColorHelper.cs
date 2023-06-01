using System.Windows.Media;
using Jg.wpf.core.Extensions.Types;

namespace Jg.wpf.core.Utility
{
    public static class JColorHelper
    {
        public static Color ToColor(JColor color)
        {
            var windowColor = new Color
            {
                A = 255,
                R = (byte)color.R,
                G = (byte)color.G,
                B = (byte)color.B
            };

            return windowColor;
        }
    }
}
