var lastNumber = 0;

function toggleBold() {
    document.execCommand('bold');
}

function addBulletPoints() {
    var element = document.getElementById('description');
    element.innerHTML += "•";
}

function addNumberedList() {
    var element = document.getElementById('description');
    lastNumber++;
    element.innerHTML += lastNumber + ".";
}

function syncDescription() {
    const descriptionContent = document.getElementById('description').innerHTML;
    document.getElementById('PostDescription').value = descriptionContent;
}