$(function () {
    // Archive item via XHR
    $(document).on('click', '.deletable-item-holder .archive', function (e) {
        e.preventDefault();
        var $target = $(e.target);
        if (confirm($target.closest('.deletable-item-holder').data('confirmArchive')) == false)
            return;
        $.ajax({
            type: 'POST',
            url: $target.attr('href')
        }).done(function () {
            var $deleteTo = $target.closest('.deletable-item');
            $deleteTo.fadeOut('normal', function () { $deleteTo.remove(); });
        }).fail(function () {
            alert('Oops, something wrong...');
        });
    });
    // Delete item via XHR
    $(document).on('click', '.deletable-item-holder .delete', function (e) {
        e.preventDefault();
        var $target = $(e.target);
        if (confirm($target.closest('.deletable-item-holder').data('confirmDelete')) == false)
            return;
        $.ajax({
            type: 'POST',
            url: $target.attr('href')
        }).done(function () {
            var $deleteTo = $target.closest('.deletable-item');
            $deleteTo.fadeOut('normal', function () { $deleteTo.remove(); });
        }).fail(function () {
            alert('Oops, something wrong...');
        });
    });
});
//# sourceMappingURL=delete-item.js.map