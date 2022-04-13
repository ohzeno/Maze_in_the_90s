mergeInto(LibraryManager.library, {
 
    //닉네임 중복검사
    CheckNickname: function(name) {
    
    //가입 시도하는 닉네임
    var parsedNick = Pointer_stringify(name);
    console.log(parsedNick);
    // 뒤져볼 경로
    var nameRef = firebase.database().ref('users'); //전체 유저
    // 결과 (0 -> 중복 있음 가입 불가능, 1 -> 중복 없음 가입 가능)
    var result = 1

    firebase.database().ref(nameRef).once('value').then(function(list) {
    //하나씩 읽음
     list.forEach(function (user) {
        console.log(user.val());
        console.log(user.val().nickname);

        //중복 있음
        if (user.val().nickname == parsedNick){
            console.log('중복');
            result = 0
        }

        //아무것도 입력하지 않음
        if (parsedNick == ""){
            console.log('nothing....');
            result = 3
        }
      });
      //검사 끝 유니티로 결과 보냄
      console.log(result);
      console.log(typeof result);
      window.unityInstance.SendMessage('SignUpHandler', 'CheckedName', result);
     });

   },

   //닉네임 중복검사(소셜)
    CheckNicknameForSocial: function(name) {
    
    //가입 시도하는 닉네임
    var parsedNick = Pointer_stringify(name);
    console.log(parsedNick);
    // 뒤져볼 경로
    var nameRef = firebase.database().ref('users'); //전체 유저
    // 결과 (0 -> 중복 있음 가입 불가능, 1 -> 중복 없음 가입 가능)
    var result = 1

    firebase.database().ref(nameRef).once('value').then(function(list) {
    //하나씩 읽음
     list.forEach(function (user) {
        console.log(user.val());
        console.log(user.val().nickname);

        //중복 있음
        if (user.val().nickname == parsedNick){
            console.log('중복');
            result = 0
        }
        //아무것도 입력하지 않음
        if (parsedNick == ""){
            console.log('nothing....');
            result = 3
        }
      });
      //검사 끝 유니티로 결과 보냄
      console.log(result);
      console.log(typeof result);
      window.unityInstance.SendMessage('LoginHandler', 'CheckedNameForSocial', result);
     });

   },

   //닉네임 변경 중복검사
   CheckNicknameForChange: function(name) {
    
    //변경 시도하는 닉네임
    var parsedNick = Pointer_stringify(name);
    console.log(parsedNick);

    //현재 유저닉네임
    var currNick = firebase.auth().currentUser.displayName;

    // 뒤져볼 경로
    var nameRef = firebase.database().ref('users'); //전체 유저
    // 결과 (0 -> 중복 있음 변경 불가능, 1 -> 중복 없음 변경 가능, 2 -> 변경 없음)
    var result = 1

    firebase.database().ref(nameRef).once('value').then(function(list) {
    //하나씩 읽음
     list.forEach(function (user) {
        console.log(user.val());
        console.log(user.val().nickname);

        //중복 있음
        if (user.val().nickname == parsedNick){
            console.log('overlap');
            result = 0
        }

        //현재 닉네임과 같음
        if (currNick == parsedNick){
            console.log('overlap current nickname');
            result = 2
        }

        //아무것도 입력하지 않음
        if (parsedNick == ""){
            console.log('nothing....');
            result = 3
        }

      });
      //검사 끝 유니티로 결과 보냄
      console.log(result);
      console.log(typeof result);
      window.unityInstance.SendMessage('LobbyHandler', 'CheckedNameForChange', result);
     });

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
    const players = obj.totalPlayers;
    const rank = obj.rank;    
    const name = obj.nickName;
    const time = obj.time;
    const mode = obj.gameMode;
    const map = obj.gameMap;


    //DB 저장 경로는 rank/모드(1/2)/맵/닉네임
    //저장할 데이터는 {time : 걸린 시간}
    var rankRef = firebase.database().ref('rank/' + mode + '/' + map + '/' + name); //전체 랭킹
    var recordRef = firebase.database().ref('record/' + name); //마이페이지 유저별 게임전적

    //랭킹페이지 { name: 어쩌구, time: 12 }
    var rankData = new Object();
    rankData.name = name;
    rankData.time = time;

    //마이페이지 { mode: 어쩌구, map: 어쩌구, players: 몇명, rank: 몇등, time: 시간 }
    var recordData = new Object();
    recordData.mode = mode;
    recordData.map = map;
    recordData.players = players;
    recordData.rank = rank;
    recordData.time = time;

    //전체 랭킹 테이블 업데이트(해당 유저의 기록이 이미 있는 경우 더 짧은 기록으로 대체할 것)
    rankRef.get().then(function(snapshot) {
    //해당 경로에 기록이 이미 있음
    if (snapshot.exists()) {
        console.log(snapshot.val());

        //시간 비교
        //기존에 있는 기록이 같거나 더 짧다면 갱신하지 않음
        if (snapshot.val().time <= time){
            console.log('time not replaced...');
        } 
        //기록 갱신했으면 데이터 보내서 대체함
        else{
            firebase.database().ref(rankRef).update(rankData).then(function(unused) {
            console.log('time replaced!');
            });
        }

        //해당 경로에 기록이 없음(해당 모드, 맵에서 첫 게임인 경우)
    } else {
        firebase.database().ref(rankRef).update(rankData).then(function(unused) {
        console.log('rank post completed!');
        });
        
    
    }
    });

   },

   //유저전적 한 번만 보내기
    PostMyRecord: function(json) {
        //unity에서 string으로 받은 json
        var parsedJSON = Pointer_stringify(json);
        
        //string을 json 오브젝트로 변환
        var obj = JSON.parse(parsedJSON);
    
        //오브젝트에서 필요한 value 값을 찾아서 변수로 만듬
        const players = obj.totalPlayers;
        const rank = obj.rank;    
        const name = obj.nickName;
        const time = obj.time;
        const mode = obj.gameMode;
        const map = obj.gameMap;


        //DB 저장 경로는 rank/모드(1/2)/맵/닉네임
        //저장할 데이터는 {time : 걸린 시간}
        var recordRef = firebase.database().ref('record/' + name); //마이페이지 유저별 게임전적

        //마이페이지 { mode: 어쩌구, map: 어쩌구, players: 몇명, rank: 몇등, time: 시간 }
        var recordData = new Object();
        recordData.mode = mode;
        recordData.map = map;
        recordData.players = players;
        recordData.rank = rank;
        recordData.time = time;


        //유저 전적 테이블 업데이트(여기는 덮어쓰기 없음 계속 추가)
        firebase.database().ref(recordRef).push().set(recordData).then(function(unused) {
        console.log('record post completed!');
        });
   },


   //랭킹페이지 탭 구분
   SetByInfo: function(mode, map) {

   //파이어베이스로 보내기 위해 string으로 파싱
   var parsedMode = Pointer_stringify(mode);
   var parsedMap = Pointer_stringify(map);
   console.log(parsedMode);
   console.log(parsedMap);

    //해당하는 경로의 TOP 10 랭킹 값 읽어오기
    firebase.database().ref('rank/' + parsedMode + '/' + parsedMap).orderByChild('time').limitToFirst(10).once('value').then(function(list) {
    console.log(list.length);

    //정렬된 데이터를 가져오기 위해서 하나하나씩 읽음
     list.forEach(function (score) {
        console.log(score.val());
        //게임데이터를 다시 유니티로 보냄
        window.unityInstance.SendMessage('RankingHandler', 'SetUp', JSON.stringify(score.val()));
        });
     });
   },

   GetRecords: function(username) {
        var parsedUsername = Pointer_stringify(username);

        //해당하는 유저의 전적 읽어오기
        firebase.database().ref('record/' + parsedUsername).once('value').then(function(list) {
        //정렬된 데이터를 가져오기 위해서 하나하나씩 읽음
        list.forEach(function (record) {
        console.log(record.val());
        //게임데이터를 다시 유니티로 보냄
        window.unityInstance.SendMessage('MyPageHandler', 'SetUp', JSON.stringify(record.val()));
        });
     });
   },

   GetRanks: function(username) {
        var parsedUsername = Pointer_stringify(username);

        // 뒤져볼 경로
        var rankRef = firebase.database().ref('rank'); //전체 유저

        firebase.database().ref(rankRef).once('value').then(function(mode) {
        mode.forEach(function (map) {
        console.log(mode.val());
        console.log(mode.val().name);
        });
      });
   },

   UpdateCharacter: function(charIdx) {

        console.log("updating character");
        console.log(charIdx);
        console.log(typeof charIdx);

        var user = firebase.auth().currentUser;

        var data = { character : charIdx };

        //Realtime Database에서 업데이트
        firebase.database().ref('users/' + user.uid).update(data);
        console.log("character update complete!");
   },


});