var lastNumber = 0;

function toggleBold() {
    document.execCommand('bold');
}

function addBulletPoints(elementId) {
    var element = document.getElementById(elementId);
    element.innerHTML += "<li>•</li>";
}

function addNumberedList(elementId) {
    var element = document.getElementById(elementId);
    lastNumber++;
    element.innerHTML += "<li>" + lastNumber + ".</li>";
}

function syncDescription() {
    const descriptionContent = document.getElementById('description').innerHTML;
    document.getElementById('PostDescription').value = descriptionContent;
}