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

function copyEditorContent() {
    document.getElementById('PostDescription').value = quill.root.innerHTML;
}