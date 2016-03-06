$(() => {
    function post($target: JQuery) {
        $.ajax({
            type: 'POST',
            url: $target.attr('href')
        }).done(() => {
            var $deleteTo = $target.closest('.deletable-item');
            $deleteTo.fadeOut('normal', () => { $deleteTo.remove(); });
        }).fail(() => {
            alert('Oops, something wrong...');
        });
    }
    // Archive item via XHR
    $(document).on('click', '.deletable-item-holder .archive', (e) => {
        e.preventDefault();
        var $target = $(e.target);
        if (confirm($target.closest('.deletable-item-holder').data('confirmArchive')) == false) return;
        post($target);
    });
    // Restore item via XHR
    $(document).on('click', '.deletable-item-holder .restore', (e) => {
        e.preventDefault();
        var $target = $(e.target);
        if (confirm($target.closest('.deletable-item-holder').data('confirmRestore')) == false) return;
        post($target);
    });
    // Delete item via XHR
    $(document).on('click', '.deletable-item-holder .delete', (e) => {
        e.preventDefault();
        var $target = $(e.target);
        if (confirm($target.closest('.deletable-item-holder').data('confirmDelete')) == false) return;
        post($target);
    });
}); 