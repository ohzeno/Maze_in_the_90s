mergeInto(LibraryManager.library, {

    //로비 진입시 유저프로필 가져오기
    CheckAuthState: function() {
        
        const user = firebase.auth().currentUser;

        if (user) {
            
            //파이어베이스에서 프로필 정보 가져오기

            console.log("arrived Lobby!")
            var userName = user.displayName;
            console.log(userName);
            console.log(typeof userName);

            //유니티로 정보 보내기
            window.unityInstance.SendMessage('LobbyHandler', 'GetUsername', userName);
            var nameRef = firebase.database().ref('users'); //전체 유저

            firebase.database().ref(nameRef).once('value').then(function(list) {

            list.forEach(function (user) {
            console.log(user.val());
            console.log(user.val().nickname);
            console.log(user.val().character);
            console.log(typeof user.val().character);

            if (user.val().nickname == userName) {
                console.log("nickname found!!!!!!!!!!!!!!!!!!!!!!");
                var str = (user.val().character).toString();
                console.log(str);
                window.unityInstance.SendMessage('LoginHandler', 'GetCharacter', str);
            }
            });
        });    
        
        } else {
            console.log('user signed out!');
            window.unityInstance.SendMessage('LobbyHandler', 'LoginScreen');
        }
    },

    //로그인 - 랭킹 페이지 뒤로가기 버튼
    IsLoggedIn: function() {
        
        const user = firebase.auth().currentUser;

        //로그인 상태면 로비
        if (user) {
            window.unityInstance.SendMessage('RankingHandler', 'BackBtn', 1);
            
        //비로그인 상태면 로그인
        } else {
            console.log('user signed out!');
            window.unityInstance.SendMessage('RankingHandler', 'BackBtn', 2);
        }
    
    
    },

    //자동로그인 확인 
    CheckAutoLogin: function() {
        
        const user = firebase.auth().currentUser;

        if (user) {
            console.log('autologin!');
            window.unityInstance.SendMessage('LoginHandler', 'LobbyScreen', user.uid);
        
        } else {
            console.log('user signed out!');
        }
    
    
    },
    

    //이메일로 가입
	CreateUserWithEmailAndPassword: function(username, email, password, objectName, callback) {
        
        var parsedUsername = Pointer_stringify(username);
	    var parsedEmail = Pointer_stringify(email);
        var parsedPassword = Pointer_stringify(password);
        var parsedObjectName = Pointer_stringify(objectName);
        var parsedCallback = Pointer_stringify(callback);
 
        try {
 
            firebase.auth().createUserWithEmailAndPassword(parsedEmail, parsedPassword).then(function (userCredential) {
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, "Success: signed up for " + parsedEmail);
                var user = userCredential.user;

                console.log(user);
                return user;

            }).then(function (user) {

                console.log('profile update start!!');
                console.log(user);
                
                //Firebase Auth에 등록
                user.updateProfile({
                displayName: parsedUsername,
                email: parsedEmail
                }).then(function (unused) {
                    console.log('profile update done!!');
                    firebase.auth().signOut();
                    window.unityInstance.SendMessage('SignUpHandler', 'LoginScreen');
                });

                //Realtime Database에 등록
                console.log('db 등록 시작!!');
                firebase.database().ref('users/' + user.uid).set(
                {
                    nickname: parsedUsername,
                    email: parsedEmail,
                    character : 0
                });

                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, "Success: signed up for " + parsedEmail);
                
            }).catch(function (error) {
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
            });
 
        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedCallback, JSON.stringify(error, Object.getOwnPropertyNames(error)) );
        }
	},
    

    //이메일로 로그인
    SignInWithEmailAndPassword: function (email, password, objectName, callback, fallback) {
 
        var parsedEmail = Pointer_stringify(email);
        var parsedPassword = Pointer_stringify(password);
        var parsedObjectName = Pointer_stringify(objectName);
        var parsedCallback = Pointer_stringify(callback);
        var parsedFallback = Pointer_stringify(fallback);
 
        try {
 
            firebase.auth().signInWithEmailAndPassword(parsedEmail, parsedPassword).then(function (unused) {
                
                var user = firebase.auth().currentUser;
                console.log(user);

                window.unityInstance.SendMessage('LoginHandler', 'LobbyScreen', user.uid);
                
                unityInstance.Module.SendMessage(parsedObjectName, parsedCallback, "Success: signed in for " + parsedEmail);

            }).catch(function (error) {
                window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)) );
            });
 
        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)) );
        }
    },
    
    //구글 로그인 및 회원가입
    LoginWithGoogle: function (objectName, callback, fallback) {
 
    var parsedObjectName = Pointer_stringify(objectName);
    var parsedCallback = Pointer_stringify(callback);
    var parsedFallback = Pointer_stringify(fallback);

      //로그인인지 회원가입인지 DB 확인
      var userRef = firebase.database().ref('users'); // 전체 유저테이블
      var result = 1; // 0이면 로그인 1이면 회원가입
      var charIdx = null;
      var textIdx = null;

      var provider = new firebase.auth.GoogleAuthProvider();
      firebase.auth().signInWithPopup(provider).then(function (unused) {
          
          var user = firebase.auth().currentUser;
          return user;

      }).then(function (user) {

        //유저테이블 뒤져봄
        userRef.get().then(function(snapshot) {
          snapshot.forEach(function (users) {
                console.log(users.val());
                console.log(users.val().email);
                console.log(users.val().character);
                //이미 가입, 로그인 시도임
                if (users.val().email == user.email) {
                console.log('user already exists - login');
                result = 0;
                charIdx = users.val().character;
                console.log(typeof charIdx);
                textIdx = charIdx.toString();
                console.log(textIdx);
                }
            });
            if (result == 0){
        //로그인, uid 가지고 로비
        window.unityInstance.SendMessage('LoginHandler', 'LobbyScreen', user.uid);
        window.unityInstance.SendMessage('LoginHandler', 'GetCharacter', textIdx);
      }
      else if (result == 1){
        //유저 없음, 회원가입 시도임
        //회원가입, 닉네임 중복검사
        window.unityInstance.SendMessage('LoginHandler', 'SignUpNicknameCheck')
      }
        });

      
  });
  },

    //깃헙 로그인 및 회원가입
    LoginWithGithub: function (objectName, callback, fallback) {
 
    var parsedObjectName = Pointer_stringify(objectName);
    var parsedCallback = Pointer_stringify(callback);
    var parsedFallback = Pointer_stringify(fallback);

      //로그인인지 회원가입인지 DB 확인
      var userRef = firebase.database().ref('users'); // 전체 유저테이블
      var result = 1; // 0이면 로그인 1이면 회원가입
      var charIdx = null;
      var textIdx = null;

      var provider = new firebase.auth.GithubAuthProvider();
      firebase.auth().signInWithPopup(provider).then(function (unused) {
          
          var user = firebase.auth().currentUser;
          return user;

      }).then(function (user) {

        //유저테이블 뒤져봄
        userRef.get().then(function(snapshot) {
          snapshot.forEach(function (users) {
                console.log(users.val());
                console.log(users.val().email);
                console.log(users.val().character);
                //이미 가입, 로그인 시도임
                if (users.val().email == user.email) {
                console.log('user already exists - login');
                result = 0;
                charIdx = users.val().character;
                console.log(typeof charIdx);
                textIdx = charIdx.toString();
                console.log(textIdx);
                }
            });
            if (result == 0){
        //로그인, uid 가지고 로비
        window.unityInstance.SendMessage('LoginHandler', 'LobbyScreen', user.uid);
        window.unityInstance.SendMessage('LoginHandler', 'GetCharacter', textIdx);
      }
      else if (result == 1){
        //유저 없음, 회원가입 시도임
        //회원가입, 닉네임 중복검사
        window.unityInstance.SendMessage('LoginHandler', 'SignUpNicknameCheck')
      }
        });

      
  });
  },

//소셜 가입 프로필 등록(닉넴 중복검사 통과 후)
UpdateInfoWithGoogleOrGithub: function (username) {
 
    var parsedUserName = Pointer_stringify(username);
    var user = firebase.auth().currentUser;
    console.log(user);
    console.log(parsedUserName);
    
    //Firebase Auth에 등록
    user.updateProfile({
    displayName: parsedUserName,
    }).then(function (unused) {
            console.log('displayName changed');
            console.log(user.displayName);
        }).then(function (unused) {
            //Realtime Database에 등록
            firebase.database().ref('users/' + user.uid).set({
            nickname: parsedUserName,
            email: user.email,
            character : 0
            })}).then(function (unused) {
            //로비로 이동

            console.log('profile update done!!');
            window.unityInstance.SendMessage('LoginHandler', 'LobbyScreen', user.uid);
            
            });
},

    //마이페이지 닉네임 변경
    UpdateNickname: function (username) {
 
        var parsedUserName = Pointer_stringify(username);
        var user = firebase.auth().currentUser;
        console.log(parsedUserName);
        
        //Firebase Auth에서 업뎃
        user.updateProfile({
        displayName: parsedUserName,
        });

        var data = { nickname : parsedUserName };

        //Realtime Database에서 업데이트
        firebase.database().ref('users/' + user.uid).update(data);

    },


    //로그아웃
    SignOut: function() {
        firebase.auth().signOut().then(function (unused) {
            window.unityInstance.SendMessage('LobbyHandler', 'LoginScreen')});
    },


    //프사 변경
    UpdateProfilePicture: function(newProfile) {
        var newPfp = Pointer_stringify(newProfile);
        const user = firebase.auth().currentUser;

        var pfData = { profile_picture : newPfp };

        //Firebase Auth에서 업데이트
        user.updateProfile({
            photoURL: newPfp
            }).then(function (unused) {
                console.log('profile update done!!');
                window.unityInstance.SendMessage('LobbyHandler', 'ChangePfpSuccess');
            });

        //Realtime Database에서 업데이트
        firebase.database().ref('users/' + user.uid).update(pfData);
        
    },

    //비밀번호 변경(마이페이지)
    UpdatePw: function(newPw) {
        var nextPw = Pointer_stringify(newPw);
        const user = firebase.auth().currentUser;

        user.updatePassword(nextPw).then(function (unused) {
        // Update successful.
        console.log('pw update done!!');
        window.unityInstance.SendMessage('LobbyHandler', 'ChangePwSuccess');
        });
    },

    //비밀번호 재설정(로그인 화면에서 비번 잊었을 때)
    ResetPassword: function(email) {
        const user = firebase.auth().currentUser;
        var email = Pointer_stringify(email);

        firebase.auth().sendPasswordResetEmail(email).then(function (unused) {
        console.log('pw reset email sent!!');
        window.unityInstance.SendMessage('LoginHandler', 'EmailSentScreen', email);
        });
    },

    //회원탈퇴
    DeleteUser: function() {

    const user = firebase.auth().currentUser;

    //Realtime Database에서 삭제
    firebase.database().ref('users/' + user.uid).remove().then(function(unused) {
            window.unityInstance.SendMessage(parsedObjectName, parsedCallback, "Success: " + parsedPath + " was deleted")});
    
    //Firebase Auth에서 삭제
    user.delete().then(function (unused) {
        window.unityInstance.SendMessage('LobbyHandler', 'DeleteUserSuccess')});
    
    },

 
});
