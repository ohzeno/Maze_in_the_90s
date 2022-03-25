using UnityEngine;
using UnityEngine.UI;
using FirebaseWebGL.Scripts.FirebaseBridge;
using TMPro;

namespace FirebaseWebGL.Examples.Auth
{
    public class LobbyHandler : MonoBehaviour
    {

        public void SignOut() =>
           FirebaseAuth.SignOut();



        public void LoginScreen()
        {
            GameManager.instance.ChangeScene("LogIn");
        }


    }

}