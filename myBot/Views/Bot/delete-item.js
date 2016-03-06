$(function () {
    function post($target) {
        $.ajax({
            type: 'POST',
            url: $target.attr('href')
        }).done(function () {
            var $deleteTo = $target.closest('.deletable-item');
            $deleteTo.fadeOut('normal', function () { $deleteTo.remove(); });
        }).fail(function () {
            alert('Oops, something wrong...');
        });
    }
    // Archive item via XHR
    $(document).on('click', '.deletable-item-holder .archive', function (e) {
        e.preventDefault();
        var $target = $(e.target);
        if (confirm($target.closest('.deletable-item-holder').data('confirmArchive')) == false)
            return;
        post($target);
    });
    // Restore item via XHR
    $(document).on('click', '.deletable-item-holder .restore', function (e) {
        e.preventDefault();
        var $target = $(e.target);
        if (confirm($target.closest('.deletable-item-holder').data('confirmRestore')) == false)
            return;
        post($target);
    });
    // Delete item via XHR
    $(document).on('click', '.deletable-item-holder .delete', function (e) {
        e.preventDefault();
        var $target = $(e.target);
        if (confirm($target.closest('.deletable-item-holder').data('confirmDelete')) == false)
            return;
        post($target);
    });
});
//# sourceMappingURL=delete-item.js.map