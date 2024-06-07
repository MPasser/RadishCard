using System.Collections.Generic;

namespace Ressap.Utils {
    public static class ArrayUtils {
        /// <summary>
        /// Fill list with the same element.
        /// </summary>
        public static void Fill<T>(this T[] arr, T element = default(T)) {
            if (null == arr || arr.Length <= 0) {
                return;
            }
            for (int i = 0; i < arr.Length; i++) {
                arr[i] = element;
            }
        }



        /// <summary>
        /// Returns a list only has single element.
        /// </summary>
        public static List<T> SingletonList<T>(T element = default(T)) {
            return new List<T>() { element };
        }



        /// <summary>
        /// Returns more readable string for the list.
        /// </summary>
        public static string List2String<T>(IEnumerable<T> arr) {
            return List2String(arr, ", ", "[", "]");
        }
        public static string List2String<T>(IEnumerable<T> arr, string delimiter, string prefix, string suffix) {
            if (null == arr) {
                return "null";
            }
            IEnumerator<T> enumerator = arr.GetEnumerator();

            if (!enumerator.MoveNext()) {
                return "empty";
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(prefix);

            if (null == enumerator.Current) {
                sb.Append("null");
            } else {
                sb.Append(enumerator.Current.ToString());
            }

            while (enumerator.MoveNext()) {
                sb.Append(delimiter);

                if (null == enumerator.Current) {
                    sb.Append("null");
                } else {
                    sb.Append(enumerator.Current.ToString());
                }
            }

            sb.Append(suffix);

            return sb.ToString();
        }



        public static T Max<T>(this IEnumerable<T> arr, IComparer<T> comparer) {
            return getMaxOrMin<T>(arr, comparer, true);
        }
        public static T Min<T>(this IEnumerable<T> arr, IComparer<T> comparer) {
            return getMaxOrMin<T>(arr, comparer, false);
        }
        private static T getMaxOrMin<T>(IEnumerable<T> arr, IComparer<T> comparer, bool isMax) {
            T val = default(T);

            if (null == arr) {
                return val;
            }
            IEnumerator<T> enumerator = arr.GetEnumerator();

            if (!enumerator.MoveNext()) {
                return val;
            }

            val = enumerator.Current;

            if (isMax) {
                while (enumerator.MoveNext()) {
                    if (comparer.Compare(enumerator.Current, val) > 0) {
                        val = enumerator.Current;
                    }
                }
            } else {
                while (enumerator.MoveNext()) {
                    if (comparer.Compare(enumerator.Current, val) < 0) {
                        val = enumerator.Current;
                    }
                }
            }

            return val;
        }
    }
}