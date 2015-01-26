$(() => {
    var $holder = $('#bot-status-holder');

    var changeStatus = (e: JQueryEventObject, toEnable: boolean) => {
        e.preventDefault();
        if (confirm($(e.target).data('confirm')) == false) return;

        $.ajax({
            type: 'POST',
            url: $holder.attr('action'),
            data: { enabled: toEnable }
        }).done(() => {
            $holder.removeClass('bot-enabled-' + (!toEnable));
            $holder.addClass('bot-enabled-' + toEnable);
        }).fail(() => {
            alert('Oops, something wrong...');
        });
    };

    $('#btn-make-enable').click(e => changeStatus(e, true));
    $('#btn-make-disable').click(e => changeStatus(e, false));

}); 