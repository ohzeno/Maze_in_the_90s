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

        //닉네임 중복검사 소셜 회원가입
        [DllImport("__Internal")]
        public static extern void CheckNicknameForSocial(string name);

        //닉네임 변경 중복검사
        [DllImport("__Internal")]
        public static extern void CheckNicknameForChange(string name);

        //랭킹페이지 및 마이페이지에 기록 올리기
        [DllImport("__Internal")]
        public static extern void PostGameRecord(string json);

        //랭킹페이지 및 마이페이지에 기록 올리기
        [DllImport("__Internal")]
        public static extern void PostMyRecord(string json);

        //랭킹페이지에 기록 받아오기
        [DllImport("__Internal")]
        public static extern void SetGameRecord();

        //랭킹페이지 탭 클릭(모드, 맵별 데이터)
        [DllImport("__Internal")]
        public static extern void SetByInfo(string mode, string map);

        //마이페이지에 기록 받아오기(전적)
        [DllImport("__Internal")]
        public static extern void GetRecords(string username);

        //마이페이지에 기록 받아오기(랭킹)
        [DllImport("__Internal")]
        public static extern void GetRanks(string username);

        //캐릭터 변경
        [DllImport("__Internal")]
        public static extern void UpdateCharacter(int charIdx);

    }
}