

var connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();

connection.start()

var dataId = $("#sendBtn").attr("data-id");

console.log(connection)

$("#sendBtn").click(function (e) {

    //e.preventDefault();

    var messageinput = $("#textarea").val();
    console.log(messageinput)

    connection.invoke("SendMessage", messageinput).catch(function (err) {
        return console.error(err.tostring());
    });

    connection.on("ReceiveMessage", function (user,message, id) {
        if (user.id != dataId) {
            $(`<li class="d-flex justify-content-between mb-4">
                      <div class="card mask-custom w-100">
                        <div class="card-header d-flex justify-content-between p-3"
                          style="border-bottom: 1px solid rgba(255,255,255,.3);">
                          <p class="fw-bold mb-0">${user.firstName + " " + user.lastName}</p >
                          <p class="text-light small mb-0"><i class="far fa-clock"></i> 13 mins ago</p>
                        </div>
                        <div class="card-body">
                          <p class="mb-0">
                           ${message}
                          </p>
                        </div>
                      </div>
                      <img src="~/img/${user.imgUrl}" alt="avatar"
                        class="rounded-circle d-flex align-self-start ms-3 shadow-1-strong" width="60">
                    </li>`).appendTo($('.messageBox ul'));

        } else {
            $(` <li class="d-flex justify-content-between mb-4">
                      <img src="~/img/${user.imgUrl}" alt="avatar"
                        class="rounded-circle d-flex align-self-start me-3 shadow-1-strong" width="60">
                      <div class="card mask-custom">
                        <div class="card-header d-flex justify-content-between p-3"
                          style="border-bottom: 1px solid rgba(255,255,255,.3);">
                          <p class="fw-bold mb-0">${user.firstName + " " + user.lastName}</p>
                          <p class="text-light small mb-0"><i class="far fa-clock"></i> 12 mins ago</p>
                        </div>
                        <div class="card-body">
                          <p class="mb-0">
                           ${message}
                          </p>
                        </div>
                      </div>
                    </li>`).appendTo($('.messageBox ul'));
        }

        $("#textarea").val("");



    })
})