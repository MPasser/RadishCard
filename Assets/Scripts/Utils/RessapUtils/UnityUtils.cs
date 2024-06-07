using UnityEngine;

namespace Ressap.Utils {
    public static class UnityUtils {
        /// <summary>
        /// Returns the color parse from a hexadecimal number in the format "RRGGBB".
        /// </summary>
        public static Color GetHexColor(int hex) {
            // Others may ask, why you need this function when there already exists ColorUtility.TryParseHtmlString?
            // Well, that function can not be used on initiating a class member of Color;
            float maxVal = 255;

            int b = 0xFF & hex;

            int g = 0xFF00 & hex;
            g >>= 8;

            int r = 0xFF0000 & hex;
            r >>= 16;

            return new Color(r / maxVal, g / maxVal, b / maxVal, 1);
        }

        public static bool TrueOfPossibility(float possibility) {
            if (possibility < 0) {
                possibility = 0;
            }
            if (possibility > 1) {
                possibility = 1;
            }

            float rand = Random.Range(0f, 1f);

            return rand < possibility;
        }
    }
}