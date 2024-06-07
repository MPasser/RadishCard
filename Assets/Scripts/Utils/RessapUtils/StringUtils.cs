namespace Ressap.Utils {
    public static class StringUtils {
        public static string ToRomanNumStr(this int num) {
            if (num <= 0 || num >= 4000) {
                UnityEngine.Debug.Log($"[{nameof(StringUtils)}] {nameof(ToRomanNumStr)}: Cannot transfering number = {num}, only support number in [1, 3999]");
                return num.ToString();
            }


            System.Text.StringBuilder roman = new System.Text.StringBuilder();

            int[] elems = new int[] { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };
            for (int i = 0; i < elems.Length; i++) {
                while (0 != num / elems[i]) {
                    roman.Append(toRomanNumStr(elems[i]));
                    num = num - elems[i];
                }
            }

            return roman.ToString();

            string toRomanNumStr(int n) {
                switch (n) {
                    case (1): return "I";
                    case (4): return "IV";
                    case (5): return "V";
                    case (9): return "IX";
                    case (10): return "X";
                    case (40): return "XL";
                    case (50): return "L";
                    case (90): return "XC";
                    case (100): return "C";
                    case (400): return "CD";
                    case (500): return "D";
                    case (900): return "CM";
                    case (1000): return "M";
                    default: return "";
                }
            }
        }
    }
}
