$(document).ready(function () {

    const connection = new signalR.HubConnectionBuilder().withUrl("/apphubtypesafe").configureLogging(signalR.LogLevel.Information)
        .withAutomaticReconnect(1000, 2000, 5000, 10000)
        .build();



    const broadCastMessageToIndividualClient = "BroadCastMessageToIndividualClient";
    const receiveMessageToIndividualClient = "ReceiveMessageToIndividualClient";

    const connectionIdForClient = "connectionIdForClient";

    const GetNickName = "GetNickName";
    const clientler = "clientler";



    async function start() {

        try {

            await connection.start().then(console.log("Hub ile bağlantı kuruldu !"));
        } catch (error) {
            setTimeout(() => start(), 2000);
        }


    }
    start();






    //subscribes



    connection.on(receiveMessageToIndividualClient, (message) => {
        $("#messageContainer").append(`
            <div class="d-flex justify-content-start mb-4">
                <div class="img_cont_msg">
                    <img src="https://static.turbosquid.com/Preview/001292/481/WV/_D.jpg" class="rounded-circle user_img_msg">
                </div>
                <div class="msg_cotainer">
              
                    ${message}
                    <span class="msg_time_send"></span>
                </div>
            </div>
        `);

    });

    let fullname = "";
    let _username = "";
    connection.on(connectionIdForClient, (connectionId, username, fname) => {
        fullname = fname;
         /* $("#connectionId").html(`${connectionId}`); */
        _username = username;
       
        invokeGetNickname();
    });

   
    function invokeGetNickname() {
        const nickName = _username;
        const fullName = fullname;
        connection.invoke(GetNickName, nickName,fullName).catch(err => console.log("hata", err));
    };


    connection.on(clientler, clients => {
        $("#_clients").html("");
        $.each(clients, (index, item) => {
            const user = $(".users").first().clone();
            user.css("display", "list-item");
            const cloneId = `clone_${index + 1}`;
            user.find("a").attr("id", cloneId);
            const btnId = `btn_${index + 1}`;
            user.attr("id", btnId);
            user.find(".spanid").text(item.fullName);  // Kullanıcı adını güncelle            //Burda döngüye kendi adımı da eklemeliyim gelince bak
            user.find("#mailadres").text(item.nickName);  // Kullanıcı adını güncelle
            $("#_clients").append(user);
        });
       
    });

   
  /*  $(document).on('click', '#btn_1', function () {
        const kadi = $(this).find(".spanid").text();  // Tıklanan #chatstart içindeki .spanid değerini al
        console.log(kadi);
        $("#kullaniciadi").html(kadi);  // kadi değerini kullaniciadi ID'sine yazdır
    });
   */

    $(document).on('click', '.users', function () {
        const id = $(this).attr('id');  // Tıklanan öğenin id değerini al
       
        const k_adi = $(this).find(".spanid").text();  // Tıklanan öğe içindeki .spanid değerini al
        const mail_adi = $(this).find("#mailadres").text();  // Tıklanan öğe içindeki .spanid değerini al  
        
        $("#kullaniciadi").html(k_adi);  // kadi değerini kullaniciadi ID'sine yazdır
        $("#connectionId").html(mail_adi);  // kadi değerini kullaniciadi ID'sine yazdır
        $("#messageContainer").html("");
    });




    //buttons


    $(".send_btn").click(function () {
        const message = $(".type_msg").val();
        const targetNickname = $("#connectionId").text();

        $("#messageContainer").append(`
            

            <div class="d-flex justify-content-end mb-4">
							<div class="msg_cotainer_send">
                         
								${message}
                                
								<span class="msg_time"></span>
							</div>
							<div class="img_cont_msg">
								<img src="https://avatars.hsoubcdn.com/ed57f9e6329993084a436b89498b6088?s=256" class="rounded-circle user_img_msg">
							</div>
						</div>
        `);

        $(".type_msg").val("");

        connection.invoke(broadCastMessageToIndividualClient, message,targetNickname).catch(err => console.log("hata", err));
    });



});
