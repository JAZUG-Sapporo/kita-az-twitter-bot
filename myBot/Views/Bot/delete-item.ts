$(() => {
    // Delete item via XHR
    $(document).on('click', '.deletable-item-holder .delete',(e) => {
        e.preventDefault();
        var $target = $(e.target);
        if (confirm($target.closest('.deletable-item-holder').data('confirmDelete')) == false) return;
        $.ajax({
            type: 'POST',
            url: $target.attr('href')
        }).done(() => {
            var $deleteTo = $target.closest('.deletable-item');
            $deleteTo.fadeOut('normal',() => { $deleteTo.remove(); });
        }).fail(() => {
            alert('Oops, something wrong...');
        });
    });
}); 