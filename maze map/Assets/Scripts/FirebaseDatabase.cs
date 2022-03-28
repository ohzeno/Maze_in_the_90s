using System.Runtime.InteropServices;

namespace FirebaseWebGL.Scripts.FirebaseBridge
{
    public static class FirebaseDatabase
    {
        [DllImport("__Internal")]
        public static extern void PostJSON(string path, string value, string objectName, string callback, string fallback);

        [DllImport("__Internal")]
        public static extern void GetJSON(string path, string objectName, string callback, string fallback);

        [DllImport("__Internal")]
        public static extern void PushJSON(string path, string value, string objectName, string callback, string fallback);

        [DllImport("__Internal")]
        public static extern void UpdateJSON(string path, string value, string objectName, string callback, string fallback);

        [DllImport("__Internal")]
        public static extern void DeleteJSON(string path, string objectName, string callback, string fallback);
    }
}