mergeInto(LibraryManager.library, {
 
	CreateUserWithEmailAndPassword: function(username, email, password, objectName, callback, fallback) {
        
        var parsedUsername = Pointer_stringify(username);
	    var parsedEmail = Pointer_stringify(email);
        var parsedPassword = Pointer_stringify(password);
        var parsedObjectName = Pointer_stringify(objectName);
        var parsedCallback = Pointer_stringify(callback);
        var parsedFallback = Pointer_stringify(fallback);
 
        try {
 
            firebase.auth().createUserWithEmailAndPassword(parsedEmail, parsedPassword).then(function (userCredential) {
                console.log(parsedEmail);
                console.log(parsedObjectName);
                console.log(parsedCallback)
                console.log(parsedFallback)

                const user = userCredential.user;
                console.log(user)

                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, "Success: signed up for " + parsedEmail);
            try {
                user.updateProfile({
                displayName: parsedUsername,
                photoURL: "https://pbs.twimg.com/media/EFKdt0bWsAIfcj9.jpg"
                }).then(function (unused) {
                    "프로필 업데이트 완료?"
                    window.unityInstance.SendMessage('SignUpHandler', 'LoginScreen');
                })
                }
            catch((error) {
                console.log(error);
                });
            
            }).catch(function (error) {
                window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
            });
 
        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)) );
        }
	},
 
    SignInWithEmailAndPassword: function (email, password, objectName, callback, fallback) {
 
        var parsedEmail = Pointer_stringify(email);
        var parsedPassword = Pointer_stringify(password);
        var parsedObjectName = Pointer_stringify(objectName);
        var parsedCallback = Pointer_stringify(callback);
        var parsedFallback = Pointer_stringify(fallback);
 
        try {
 
            firebase.auth().signInWithEmailAndPassword(parsedEmail, parsedPassword).then(function (unused) {
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, "Success: signed in for " + parsedEmail);
                window.unityInstance.SendMessage('LoginHandler', 'LobbyScreen');
            }).catch(function (error) {
                window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)) );
            });
 
        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)) );
        }
    },

    SignInWithGoogle: function (objectName, callback, fallback) {
 
        var parsedObjectName = Pointer_stringify(objectName);
        var parsedCallback = Pointer_stringify(callback);
        var parsedFallback = Pointer_stringify(fallback);
 
        try {
            var provider = new firebase.auth.GoogleAuthProvider();
            firebase.auth().signInWithPopup(provider).then(function (unused) {
                unityInstance.Module.SendMessage(parsedObjectName, parsedCallback, "Success: signed in with Google!");
            }).catch(function (error) {
                unityInstance.Module.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
            });
 
        } catch (error) {
            unityInstance.Module.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },
    
    SignInWithGithub: function (objectName, callback, fallback) {
        var parsedObjectName = Pointer_stringify(objectName);
        var parsedCallback = Pointer_stringify(callback);
        var parsedFallback = Pointer_stringify(fallback);
 
        try {
            var provider = new firebase.auth.GithubAuthProvider();
            firebase.auth().signInWithPopup(provider).then(function (unused) {
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, "Success: signed in with Facebook!");
            }).catch(function (error) {
                window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
            });
 
        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

 
});