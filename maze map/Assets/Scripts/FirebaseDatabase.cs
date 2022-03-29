using System.Runtime.InteropServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace FirebaseWebGL.Scripts.FirebaseBridge
{
    public static class FirebaseDatabase
    {
        //CRUD test
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

        //랭킹페이지에 기록 올리기
        [DllImport("__Internal")]
        public static extern void PostGameRecord(string json);

    }
}