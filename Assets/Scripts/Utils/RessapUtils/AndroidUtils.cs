using UnityEngine;

namespace Ressap.Utils {
    public static class AndroidUtils {
        #region Toast

        /// <summary>
        /// Show a toast message immediately.
        /// </summary>
        public static void ShowToast(string content) {
            using AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            using AndroidJavaClass Toast = new AndroidJavaClass("android.widget.Toast");
            currentActivity.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                Toast.CallStatic<AndroidJavaObject>("makeText", currentActivity, content, Toast.GetStatic<int>("LENGTH_LONG")).Call("show");
            }));
        }

        #endregion

        #region Email

        /// <summary>
        /// Call the mail-processing application in the device, and prepare to send email.
        /// </summary>
        /// <param name="toAddr">URI of the address that intend to send to, usually start with "mailto:"</param>
        public static void SendEmail(string toAddr, string title, string content) {
            using AndroidJavaClass IntentJC = new AndroidJavaClass("android.content.Intent");
            using AndroidJavaObject intentJO = new AndroidJavaObject("android.content.Intent");
            intentJO.Call<AndroidJavaObject>("setAction", IntentJC.GetStatic<string>("ACTION_SENDTO"));

            using (AndroidJavaClass UriJC = new AndroidJavaClass("android.net.Uri")) {
                AndroidJavaObject uriJO = UriJC.CallStatic<AndroidJavaObject>("parse", toAddr);
                intentJO.Call<AndroidJavaObject>("setData", uriJO);
            }

            intentJO.Call<AndroidJavaObject>("putExtra", IntentJC.GetStatic<string>("EXTRA_SUBJECT"), title);
            intentJO.Call<AndroidJavaObject>("putExtra", IntentJC.GetStatic<string>("EXTRA_TEXT"), content);

            using AndroidJavaClass UnityPlayerJC = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = UnityPlayerJC.GetStatic<AndroidJavaObject>("currentActivity");
            currentActivity.Call("startActivity", intentJO);
        }

        #endregion



        #region Locale

        /// <summary>
        /// Returns the language code of this Locale. using ISO 639 standard. http://www.loc.gov/standards/iso639-2/php/code_list.php
        /// </summary>
        /// <returns>The language code, or the empty string if none is defined.</returns>
        public static string GetLanguage() {
            return CallLocaleJOFunc<string>("getLanguage");
        }

        /// <summary>
        /// Returns a name for the locale's language that is appropriate for display to the user.
        /// </summary>
        /// <returns>The name of the display language.</returns>
        public static string GetDisplayLanguage() {
            return CallLocaleJOFunc<string>("getDisplayLanguage");
        }

        /// <summary>
        /// Returns the country/region code for the default locale, which should either be the empty string, an uppercase ISO 3166 2-letter code, or a UN M.49 3-digit code. https://www.iso.org/obp/ui/
        /// </summary>
        public static string GetCountry() {
            return CallLocaleJOFunc<string>("getCountry");
        }

        private static ReturnType CallLocaleJOFunc<ReturnType>(string methodName, params object[] args) {
            ReturnType result = default;
            using (AndroidJavaClass LocaleJC = new AndroidJavaClass("java.util.Locale")) {
                using AndroidJavaObject localeJO = LocaleJC.CallStatic<AndroidJavaObject>("getDefault");
                result = localeJO.Call<ReturnType>(methodName, args);
            }
            return result;
        }

        #endregion

        #region WIFI

        /// <summary>
        /// Check if the device is connected to WIFI.
        /// </summary>
        public static bool IsWifiConnected() {
            using AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            using AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            using AndroidJavaClass ContextJC = new AndroidJavaClass("android.content.Context");
            using AndroidJavaObject connServiceJO = currentActivity.Call<AndroidJavaObject>("getSystemService", ContextJC.GetStatic<string>("CONNECTIVITY_SERVICE"));
            AndroidJavaObject activeNetwork = connServiceJO.Call<AndroidJavaObject>("getActiveNetworkInfo");
            if (activeNetwork == null) {
                return false;
            }
            using AndroidJavaClass ConnMgr = new AndroidJavaClass("android.net.ConnectivityManager");
            return activeNetwork.Call<int>("getType") == ConnMgr.GetStatic<int>("TYPE_WIFI");
        }

        #endregion
    }
}
