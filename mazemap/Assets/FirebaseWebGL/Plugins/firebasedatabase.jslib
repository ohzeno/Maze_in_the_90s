mergeInto(LibraryManager.library, {
 
    PostJSON: function(path, value, objectName, callback, fallback) {
        var parsedPath = Pointer_stringify(path);
        var parsedValue = Pointer_stringify(value);
        var parsedObjectName = Pointer_stringify(objectName);
        var parsedCallback = Pointer_stringify(callback);
        var parsedFallback = Pointer_stringify(fallback);
 
        try {
 
            firebase.database().ref(parsedPath).set(parsedValue).then(function(unused) {
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, "Success: " + parsedValue + " was posted to " + parsedPath);
            });
 
        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    GetJSON: function(path, objectName, callback, fallback) {
        var parsedPath = Pointer_stringify(path);
        var parsedObjectName = Pointer_stringify(objectName);
        var parsedCallback = Pointer_stringify(callback);
        var parsedFallback = Pointer_stringify(fallback);
 
        try {
 
            firebase.database().ref(parsedPath).once('value').then(function(snapshot) {
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, JSON.stringify(snapshot.val()));
            });
 
        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    PushJSON: function(path, value, objectName, callback, fallback) {
        var parsedPath = Pointer_stringify(path);
        var parsedValue = Pointer_stringify(value);
        var parsedObjectName = Pointer_stringify(objectName);
        var parsedCallback = Pointer_stringify(callback);
        var parsedFallback = Pointer_stringify(fallback);
 
        try {
 
            firebase.database().ref(parsedPath).push().set(parsedValue).then(function(unused) {
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, "Success: " + parsedValue + " was pushed to " + parsedPath);
            });
 
        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    UpdateJSON: function(path, value, objectName, callback, fallback) {
        var parsedPath = Pointer_stringify(path);
        var parsedValue = Pointer_stringify(value);
        var parsedObjectName = Pointer_stringify(objectName);
        var parsedCallback = Pointer_stringify(callback);
        var parsedFallback = Pointer_stringify(fallback);
 
        var postData = { Score : parsedValue };
 
        try {
 
            firebase.database().ref(parsedPath).update(postData).then(function(unused) {
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, "Success: " + parsedValue + " was updated in " + parsedPath);
            });
 
        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    DeleteJSON: function(path, objectName, callback, fallback) {
        var parsedPath = Pointer_stringify(path);
        var parsedObjectName = Pointer_stringify(objectName);
        var parsedCallback = Pointer_stringify(callback);
        var parsedFallback = Pointer_stringify(fallback);
 
        try {
 
            firebase.database().ref(parsedPath).remove().then(function(unused) {
                window.unityInstance.SendMessage(parsedObjectName, parsedCallback, "Success: " + parsedPath + " was deleted");
            });
 
        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    //게임 끝났을 때 유니티에서 받은 데이터 파이어베이스로 보내기(저장만)
    PostGameRecord: function(json) {
    //unity에서 string으로 받은 json
    var parsedJSON = Pointer_stringify(json);
        
    //string
    console.log(parsedJSON);
    console.log(typeof parsedJSON);

    //string을 json 오브젝트로 변환
    var obj = JSON.parse(parsedJSON);
    console.log(obj);
    console.log(typeof obj);
    
    //오브젝트에서 필요한 value 값을 찾아서 변수로 만듬
    //const mode = obj.gameMode;
    //const map = obj.gameMap;
    const name = obj.nickName;
    const time = obj.time;

    //DB 저장 경로는 rank/모드(0/1)/맵/닉네임
    //저장할 데이터는 {time : 걸린 시간}
    var rankRef = firebase.database().ref('rank/' + '0' + '/' + 'forest1' + '/' + name); //전체 랭킹
    var recordRef = firebase.database().ref('record/' + name + '/' + '0' + '/' + 'forest1'); //유저 게임전적

    //{ name: 어쩌구, time: 12 }
    var postData = new Object();
    postData.name = name;
    postData.time = time;

    // 전체 랭킹 테이블과 유저 전적 테이블을 업데이트
    firebase.database().ref(rankRef).update(postData).then(function(unused) {
        console.log('rank post completed!');
    });

    firebase.database().ref(recordRef).update(postData).then(function(unused) {
        console.log('record post completed!');
    });

   },

   //랭킹페이지 진입했을 때
   SetGameRecord: function() {
    //TOP 10 랭킹 값 읽어오기
    firebase.database().ref('rank/' + '0' + '/' + 'forest1').orderByChild('time').limitToFirst(10).once('value').then(function(list) {
    //정렬된 데이터를 가져오기 위해서 하나하나씩 읽음
     list.forEach(function (score) {
        console.log(score.val());
        //게임데이터를 다시 유니티로 보냄
        window.unityInstance.SendMessage('RankingHandler', 'SetUp', JSON.stringify(score.val()));
        });
     });
   },

});