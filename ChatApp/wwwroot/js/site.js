
let createRoomBtn = document.getElementById('create-room-btn');
let createRoomModal = document.getElementById('create-room-modal');

createRoomBtn.addEventListener('click',
    function() {
        createRoomModal.classList.add('active');
    });

function closeModal() {
    createRoomModal.classList.remove('active');
}

const messages = document.querySelectorAll(".chat-body");
messages.forEach(m => m.addEventListener("contextmenu", e => e.preventDefault()));

document.addEventListener(
    "contextmenu",
    function (event) {
        const message = event.target.parentElement;

        if (!message.classList.contains('message')) {
            closeModals();
        }
    },
    false
);

function closeModals() {
    const menus = document.querySelectorAll(".message");
    menus.forEach(m => m.children[m.children.length - 1].classList.add('d-none'));
}


function openContextMenu(element) {
    const menu = element.children[element.children.length - 1];
    closeModals();
    if (!menu.classList.contains('d-none')) {
        menu.classList.add('d-none');
        return false;
    }
    menu.classList.remove('d-none');
    return true;
}


