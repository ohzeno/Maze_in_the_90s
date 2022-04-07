mergeInto(LibraryManager.library, {

    //�κ� ���Խ� ���������� ��������
    CheckAuthState: function() {
        
        const user = firebase.auth().currentUser;

        if (user) {
            
            //���̾�̽����� ������ ���� ��������

            var userName = user.displayName;
            var character = user.character;

            console.log(typeof userName);
            console.log(typeof character);

            //����Ƽ�� ���� (������) ������
            window.unityInstance.SendMessage('LobbyHandler', 'GetUsername', userName);
            window.unityInstance.SendMessage('LobbyHandler', 'GetCharacter', character);
            
        
        } else {
            console.log('user signed out!');
            window.unityInstance.SendMessage('LobbyHandler', 'LoginScreen');
        }
    
    
    },

    //�α��� - ��ŷ ������ �ڷΰ��� ��ư
    IsLoggedIn: function() {
        
        const user = firebase.auth().currentUser;

        //�α��� ���¸� �κ�
        if (user) {
            window.unityInstance.SendMessage('RankingHandler', 'BackBtn', 1);
            
        //��α��� ���¸� �α���
        } else {
            console.log('user signed out!');
            window.unityInstance.SendMessage('RankingHandler', 'BackBtn', 2);
        }
    
    
    },

    //�ڵ��α��� Ȯ�� 
    CheckAutoLogin: function() {
        
        const user = firebase.auth().currentUser;

        if (user) {
            console.log('autologin!');
            window.unityInstance.SendMessage('LoginHandler', 'LobbyScreen', user.uid);
        
        } else {
            console.log('user signed out!');
        }
    
    
    },
    

    //�̸��Ϸ� ����
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
                
                //Firebase Auth�� ���
                user.updateProfile({
                displayName: parsedUsername,
                email: parsedEmail,
                character: 0
                }).then(function (unused) {
                    console.log('profile update done!!');
                    firebase.auth().signOut();
                    window.unityInstance.SendMessage('SignUpHandler', 'LoginScreen');
                });

                //Realtime Database�� ���
                console.log('db ��� ����!!');
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
    

    //�̸��Ϸ� �α���
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
    
    //���� �α��� �� ȸ������
    LoginWithGoogle: function (objectName, callback, fallback) {
 
    var parsedObjectName = Pointer_stringify(objectName);
    var parsedCallback = Pointer_stringify(callback);
    var parsedFallback = Pointer_stringify(fallback);

      //�α������� ȸ���������� DB Ȯ��
      var userRef = firebase.database().ref('users'); // ��ü �������̺�
      var result = 1; // 0�̸� �α��� 1�̸� ȸ������

      var provider = new firebase.auth.GoogleAuthProvider();
      firebase.auth().signInWithPopup(provider).then(function (unused) {
          
          var user = firebase.auth().currentUser;
          return user;

      }).then(function (user) {

        //�������̺� ������
        userRef.get().then(function(snapshot) {
          snapshot.forEach(function (users) {
                console.log(users.val());
                console.log(users.val().email);
                //�̹� ����, �α��� �õ���
                if (users.val().email == user.email) {
                console.log('user already exists - login');
                result = 0;
                console.log(result);
                }
            });
            if (result == 0){
        //�α���, uid ������ �κ�
        window.unityInstance.SendMessage('LoginHandler', 'LobbyScreen', user.uid);
      }
      else if (result == 1){
        //���� ����, ȸ������ �õ���
        //ȸ������, �г��� �ߺ��˻�
        window.unityInstance.SendMessage('LoginHandler', 'SignUpNicknameCheck')
      }
        });

      
  });
  },

    //���� �α��� �� ȸ������
    LoginWithGithub: function (objectName, callback, fallback) {
 
    var parsedObjectName = Pointer_stringify(objectName);
    var parsedCallback = Pointer_stringify(callback);
    var parsedFallback = Pointer_stringify(fallback);

      //�α������� ȸ���������� DB Ȯ��
      var userRef = firebase.database().ref('users'); // ��ü �������̺�
      var result = 1; // 0�̸� �α��� 1�̸� ȸ������

      var provider = new firebase.auth.GithubAuthProvider();
      firebase.auth().signInWithPopup(provider).then(function (unused) {
          
          var user = firebase.auth().currentUser;
          return user;

      }).then(function (user) {

        //�������̺� ������
        userRef.get().then(function(snapshot) {
          snapshot.forEach(function (users) {
                console.log(users.val());
                console.log(users.val().email);
                //�̹� ����, �α��� �õ���
                if (users.val().email == user.email) {
                console.log('user already exists - login');
                result = 0;
                console.log(result);
                }
            });
            if (result == 0){
        //�α���, uid ������ �κ�
        window.unityInstance.SendMessage('LoginHandler', 'LobbyScreen', user.uid);
      }
      else if (result == 1){
        //���� ����, ȸ������ �õ���
        //ȸ������, �г��� �ߺ��˻�
        window.unityInstance.SendMessage('LoginHandler', 'SignUpNicknameCheck')
      }
        });

      
  });
  },

    //�Ҽ� ���� ������ ���(�г� �ߺ��˻� ��� ��)
    UpdateInfoWithGoogleOrGithub: function (username) {
 
        var parsedUserName = Pointer_stringify(username);
        var user = firebase.auth().currentUser;
        
        //Firebase Auth�� ���
        user.updateProfile({
        displayName: parsedUserName,
        email: user.email,
        character : 0
        });

        //Realtime Database�� ���
        firebase.database().ref('users/' + user.uid).set({
            nickname: parsedUserName,
            email: user.email,
            character : 0
        });

        //�κ�� �̵�
        console.log('profile update done!!');
        window.unityInstance.SendMessage('LoginHandler', 'LobbyScreen', user.uid);
    },

    //���������� �г��� ����
    UpdateNickname: function (username) {
 
        var parsedUserName = Pointer_stringify(username);
        var user = firebase.auth().currentUser;
        console.log(parsedUserName);
        
        //Firebase Auth���� ����
        user.updateProfile({
        displayName: parsedUserName,
        });

        var data = { nickname : parsedUserName };

        //Realtime Database���� ������Ʈ
        firebase.database().ref('users/' + user.uid).update(data);

    },


    //�α׾ƿ�
    SignOut: function() {
        firebase.auth().signOut().then(function (unused) {
            window.unityInstance.SendMessage('LobbyHandler', 'LoginScreen')});
    },


    //���� ����
    UpdateProfilePicture: function(newProfile) {
        var newPfp = Pointer_stringify(newProfile);
        const user = firebase.auth().currentUser;

        var pfData = { profile_picture : newPfp };

        //Firebase Auth���� ������Ʈ
        user.updateProfile({
            photoURL: newPfp
            }).then(function (unused) {
                console.log('profile update done!!');
                window.unityInstance.SendMessage('LobbyHandler', 'ChangePfpSuccess');
            });

        //Realtime Database���� ������Ʈ
        firebase.database().ref('users/' + user.uid).update(pfData);
        
    },

    //��й�ȣ ����(����������)
    UpdatePw: function(newPw) {
        var nextPw = Pointer_stringify(newPw);
        const user = firebase.auth().currentUser;

        user.updatePassword(nextPw).then(function (unused) {
        // Update successful.
        console.log('pw update done!!');
        window.unityInstance.SendMessage('LobbyHandler', 'ChangePwSuccess');
        });
    },

    //��й�ȣ �缳��(�α��� ȭ�鿡�� ��� �ؾ��� ��)
    ResetPassword: function(email) {
        const user = firebase.auth().currentUser;
        var email = Pointer_stringify(email);

        firebase.auth().sendPasswordResetEmail(email).then(function (unused) {
        console.log('pw reset email sent!!');
        window.unityInstance.SendMessage('LoginHandler', 'EmailSentScreen', email);
        });
    },

    //ȸ��Ż��
    DeleteUser: function() {

    const user = firebase.auth().currentUser;

    //Realtime Database���� ����
    firebase.database().ref('users/' + user.uid).remove().then(function(unused) {
            window.unityInstance.SendMessage(parsedObjectName, parsedCallback, "Success: " + parsedPath + " was deleted")});
    
    //Firebase Auth���� ����
    user.delete().then(function (unused) {
        window.unityInstance.SendMessage('LobbyHandler', 'DeleteUserSuccess')});
    
    },

 
});