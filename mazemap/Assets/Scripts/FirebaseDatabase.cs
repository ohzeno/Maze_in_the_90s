using System.Runtime.InteropServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace FirebaseWebGL.Scripts.FirebaseBridge
{
    public static class FirebaseDatabase
    {
        //닉네임 중복검사
        [DllImport("__Internal")]
        public static extern void CheckNickname(string name);

        //랭킹페이지에 기록 올리기
        [DllImport("__Internal")]
        public static extern void PostGameRecord(string json);

        [DllImport("__Internal")]
        public static extern void SetGameRecord();
    }
}