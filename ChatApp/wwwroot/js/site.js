
let createRoomBtn = document.getElementById('create-room-btn');
let createRoomModal = document.getElementById('create-room-modal');

createRoomBtn.addEventListener('click',
    function() {
        createRoomModal.classList.add('active');
    });

function closeModal() {
    createRoomModal.classList.remove('active');
}


let makeEdits = document.getElementById('make-edit-with-message');
let createMessageModel = document.getElementById('create-message-modal');

makeEdits.addEventListener('click',
    function makeEdits() {
        window.createMessageModal.classList.add('active');
    });


function closeMessageModal() {
    window.createMessageModal.classList.remove('active');
}
