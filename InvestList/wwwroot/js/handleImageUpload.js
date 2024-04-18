document.addEventListener( 'DOMContentLoaded', function () {
    // Add event listener for photo removal
    document.getElementById( 'removePhotoButton' ).addEventListener( 'click', removePhoto );

    // Add event listener for image file input change
    document.getElementById( 'imageFileInput' ).addEventListener( 'change', handleImageChange );

    // Check if ImageBase64 is already populated
    var imageBase64Value = document.getElementById( 'ImageBase64' ).value;
    if ( imageBase64Value && imageBase64Value.trim() !== '' ) {
        displayImage( imageBase64Value );
        showRemovePhotoButton();
    }
} );

function removePhoto() {
    clearFileInput();
    hideUploadedImage();
    clearHiddenFieldValue();
    hideRemovePhotoButton();
}

function clearFileInput() {
    var fileInput = document.getElementById( 'imageFileInput' );
    fileInput.value = '';
}

function hideUploadedImage() {
    var uploadedImage = document.getElementById( 'uploadedImage' );
    uploadedImage.style.display = 'none';
}

function clearHiddenFieldValue() {
    document.getElementById( 'ImageBase64' ).value = '';
}

function hideRemovePhotoButton() {
    var removePhotoButton = document.getElementById( 'removePhotoButton' );
    removePhotoButton.style.display = 'none';
}

function handleImageChange() {
    var fileInput = this;
    var file = fileInput.files[0];

    if ( file ) {
        var reader = new FileReader();
        reader.onload = function ( e ) {
            var img = new Image();
            img.src = e.target.result;

            img.onload = function () {
                // Resize and convert the image
                var canvas = document.createElement( 'canvas' );
                var ctx = canvas.getContext( '2d' );
                canvas.width = 300;
                canvas.height = 300;
                ctx.drawImage( img, 0, 0, 300, 300 );
                var compressedBase64 = canvas.toDataURL( 'image/jpeg', 0.7 ).split( ',' )[1];

                // Display the image
                displayImage( compressedBase64 );

                // Show the Remove Photo button
                showRemovePhotoButton();
            };
        };
        reader.readAsDataURL( file );
    }
}

function displayImage( base64Value ) {
    var uploadedImage = document.getElementById( 'uploadedImage' );
    uploadedImage.src = "data:image;base64," + base64Value;
    uploadedImage.style.display = 'block';

    // Update the hidden field value
    document.getElementById( 'ImageBase64' ).value = base64Value;
}

function showRemovePhotoButton() {
    var removePhotoButton = document.getElementById( 'removePhotoButton' );
    removePhotoButton.style.display = 'block';
}
