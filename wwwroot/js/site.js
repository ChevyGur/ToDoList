const uri = '/Task';
let tasks = [];

function getUser() {

}

function getItems(token)
{
    alert(token + "             " + "eowww")
    fetch(`https://localhost:7123/Task/${token}`)
        .then(response => response.json())
        .then(data => {  alert(token + "             " + "eowww")
            _displayItems(data)
        }
        )
        .catch(error => console.error('Unable to get items.', error));
}
function getItemById() {
    const id = document.getElementById('get-item').value;
    fetch(`https://localhost:7123/Task/${id}`)
        .then(response => response.json())
        .then(data => {
            showItem(data);
        }
        )
        .catch(error => console.error('Unable to get items.', error));
}

function showItem(data) {
    const name = document.getElementById('name');
    const isDone = document.getElementById('isDone');
    name.innerText = data.name;
    isDone.innerText = data.isDone;

}

function addItem() {
    const addNameTextbox = document.getElementById('add-name');
    const item = {
        isDone: false,
        name: addNameTextbox.value.trim()
    };

    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(response => response.json())
        .then(() => {
            getItems(token);
            addNameTextbox.value = '';
        })
        .catch(error => console.error('Unable to add item.', error));
}

function deleteItem(id) {
    fetch(`${uri}/?id=${id}`, {
        method: 'DELETE'
    })
        .then(() => {
            getItems(token)
            debugger
        })
        .catch(error => console.error('Unable to delete item.', error));
}

function displayEditForm(id) {
    const item = tasks.find(item => item.id === id);

    document.getElementById('edit-name').value = item.name;
    document.getElementById('edit-id').value = item.id;
    document.getElementById('edit-isDone').checked = item.isDone;
    document.getElementById('editForm').style.display = 'block';
}

function updateItem() {
    const itemId = document.getElementById('edit-id').value;
    const item = {
        Id: parseInt(itemId, 10),
        IsDone: document.getElementById('edit-isDone').checked,
        Name: document.getElementById('edit-name').value.trim()
    };

    fetch(`${uri}/${itemId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(() => getItems(token))
        .catch(error => console.error('Unable to update item.', error));

    closeInput();

    return false;
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function _displayCount(itemCount) {
    const name = (itemCount === 1) ? 'task' : 'task kinds';

    document.getElementById('counter').innerText = `${itemCount} ${name}`;
}

function _displayItems(data) {
    const tBody = document.getElementById('tasks');
    tBody.innerHTML = '';

    _displayCount(data.length);

    const button = document.createElement('button');

    data.forEach(item => {
        let isDoneCheckbox = document.createElement('input');
        isDoneCheckbox.type = 'checkbox';
        isDoneCheckbox.disabled = true;
        isDoneCheckbox.checked = item.isDone;

        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${item.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteItem(${item.id})`);

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        td1.appendChild(isDoneCheckbox);

        let td2 = tr.insertCell(1);
        let textNode = document.createTextNode(item.name);
        td2.appendChild(textNode);

        let td3 = tr.insertCell(2);
        td3.appendChild(editButton);

        let td4 = tr.insertCell(3);
        td4.appendChild(deleteButton);
    });

    tasks = data;
}
//////////////////////
var token = "";
function Login() {
    const name = document.getElementById('name');
    const password = document.getElementById('password');
    var myHeaders = new Headers();
    myHeaders.append(
        "Authorization",
        "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ0eXBlIjoiQWRtaW4iLCJleHAiOjE2NzM3MzA4OTgsImlzcyI6Imh0dHBzOi8vdXNlci1kZW1vLmNvbSIsImF1ZCI6Imh0dHBzOi8vdXNlci1kZW1vLmNvbSJ9.o_nqpm7TbvRK0ad5MNbNXYXqNlKLn3E3vhTfBT25S14"
    );
    myHeaders.append("Content-Type", "application/json");
    var raw = JSON.stringify({
        Id: 0,
        Name: name.value.trim(),
        IsAdmin: false,
        Password: password.value.trim()
    })
    var requestOptions = {
        method: "POST",
        headers: myHeaders,
        body: raw,
        redirect: "follow",
    };

    fetch("https://localhost:7123/User/Login", requestOptions)
        .then((response) => response.text())
        .then((result) => {
            if (result.includes("401")) {
                name.value = "";
                password.value = "";
                alert("not exist!!")
            } else {
                token = result;
                alert(token)
                location.href = "task.html";
                getItems(token);
            }
        }).catch((error) => alert("error", error));
    
    // addFuncByStatus();
};

// function addFuncByStatus() {
//     fetch("https://localhost:7123", requestOptions)
//         .then((response) => response.text())
//         .then((result) => {
//             if (result.includes("401")) {
//                 name.value = "";
//                 password.value = "";
//                 alert("not exist!!")
//             } else {
//                 token = result;
//                 // var handler = new JwtSecurityTokenHandler();
//                 // var decodedValue = handler.ReadJwtToken(token);
//                 // alert(decodedValue.Id)
//                 location.href = "task.html"
//             }
//         }).catch((error) => alert("error", error));
// }