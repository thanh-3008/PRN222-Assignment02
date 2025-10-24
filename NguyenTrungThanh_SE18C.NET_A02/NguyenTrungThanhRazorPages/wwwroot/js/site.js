// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification

$(document).ready(function () {
    $('.edit-button').on('click', function () {
        var categoryId = $(this).data('id');

        $.ajax({
            url: `/Staff/CategoryManagement?handler=Category&id=${categoryId}`,
            type: 'GET',
            success: function (response) {
                $('#edit-category-id').val(response.categoryId);
                $('#edit-category-name').val(response.categoryName);
                $('#edit-category-description').val(response.categoryDesciption);

                $('#editCategoryModal').modal('show');
            },
            error: function (xhr, status, error) {
            }
        });
    });

    $('#createCategoryModal').on('hidden.bs.modal', function () {
        $(this).find('form')[0].reset();
    });
});