var lastNumber = 0;

function toggleBold() {
    document.execCommand('bold');
}

function addBulletPoints() {
    var element = document.getElementById('description');
    element.innerHTML += "<li>•</li>";
}

function addNumberedList() {
    var element = document.getElementById('description');
    lastNumber++;
    element.innerHTML += "<li>" + lastNumber + ".</li>";
}

function syncDescription() {
    const descriptionContent = document.getElementById('description').innerHTML;
    document.getElementById('PostDescription').value = descriptionContent;
}