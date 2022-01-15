"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (chat, user, message, dateTime) {
    var li = document.createElement("li");
    li.className = "list-group-item";
    document.getElementById("messagesList" + chat).appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `(${dateTime}) ${user} says ${message}`;
    document.getElementById("messageInput").value = "";

    var chat = document.getElementById("messagesList" + chat);
    var count = chat.getElementsByTagName("li").length;

    if (count > 50) {
        chat.removeChild(chat.getElementsByTagName("li")[0]);
    }

});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    
    var chat = $('#chats-tabs div.active').attr('id');

    var user = document.getElementById("userInput").innerHTML;    
    var message = document.getElementById("messageInput").value;

    message = message.trim();

    if (message.length == 0)
        return;
    connection.invoke("SendMessage", chat, user, message).catch(function (err) {
        return console.error(err.toString());
    });    
    event.preventDefault();
});
