$(document).ready(function () {
    $('#addInputsBtn').click(function () {
        const newIndex = $('.row.mb-3').length;
        const newInputs = `
            <div class="row mb-3">
                <div class="col">
                    <input type="text" class="form-control" name="Links[${newIndex}].AnchorText" placeholder="Anchor Text" />
                </div>
                <div class="col">
                    <input type="text" class="form-control" name="Links[${newIndex}].Hyperlink" placeholder="Hyperlink" />
                </div>
                <div class="col">
                    <input type="hidden" name="Links[${newIndex}].NewsId" value="@ViewData["Id"]" />
                    <input type="checkbox" class="form-check-input" name="Links[${newIndex}].Follow" value="true" />
                    <label class="form-check-label">Follow</label>
                </div>
                <div class="col">
                    <button type="button" class="btn btn-danger removeBtn">Remove</button>
                </div>
            </div>
        `;
        $('#inputsContainer').append(newInputs);
    });

    $(document).on('click', '.removeBtn', function () {
        $(this).closest('.row.mb-3').remove();
    });
});
