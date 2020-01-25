using System;

namespace GLSLWallpapers.Helpers {
    public static class MathUtils {
        public static int Clamp(int value, int min, int max) {
            return Math.Max(min, Math.Min(value, max));
        }

        public static double Clamp(double value, double min, double max) {
            return Math.Max(min, Math.Min(value, max));
        }
    }
}
