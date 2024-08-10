// var lastNumber = parseInt(localStorage.getItem('lastNumber')) || 0;

// function insertTextAtCursor(text) {
//     var sel = window.getSelection();
//     if (sel.rangeCount) {
//         var range = sel.getRangeAt(0);
//         range.deleteContents();

//         var textNode = document.createTextNode(text);
//         range.insertNode(textNode);

//         range.setStartAfter(textNode);
//         range.setEndAfter(textNode);
//         sel.removeAllRanges();
//         sel.addRange(range);
//     }
// }

// function toggleBold() {
//     document.execCommand('bold');
// }

// function addBulletPoints() {
//     insertTextAtCursor("• ");
// }

// function addNumberedList() {
//     lastNumber++;
//     insertTextAtCursor(lastNumber + ". ");
//     localStorage.setItem('lastNumber', lastNumber);
// }

// function syncDescription() {
//     const descriptionContent = document.getElementById('description').innerHTML;
//     document.getElementById('PostDescription').value = descriptionContent;
// }

const toolbarOptions = [
    [{ 'font': [] }],
    [{ size: ['small', false, 'large', 'huge'] }],
    [{ 'color': [] }, { 'background': [] }],  

    ['bold', 'italic', 'underline', 'strike'],
    [{ list: 'ordered' }, { list: 'bullet' }],

    ['link'],
    ['clean'],
]

const quill = new Quill('#editor', {
    modules: {
        toolbar: toolbarOptions
    },
    theme: 'snow',
});

quill.root.innerHTML = document.getElementById('PostDescription').value;

document.querySelector('form').onsubmit = function() {
    document.getElementById('PostDescription').value = quill.root.innerHTML;
};