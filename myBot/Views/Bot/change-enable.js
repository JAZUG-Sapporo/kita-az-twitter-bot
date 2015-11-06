$(function () {
    var $holder = $('#bot-status-holder');
    var changeStatus = function (e, toEnable) {
        e.preventDefault();
        if (confirm($(e.target).data('confirm')) == false)
            return;
        $.ajax({
            type: 'POST',
            url: $holder.attr('action'),
            data: { enabled: toEnable }
        }).done(function () {
            $holder.removeClass('bot-enabled-' + (!toEnable));
            $holder.addClass('bot-enabled-' + toEnable);
        }).fail(function () {
            alert('Oops, something wrong...');
        });
    };
    $('#btn-make-enable').click(function (e) { return changeStatus(e, true); });
    $('#btn-make-disable').click(function (e) { return changeStatus(e, false); });
});
//# sourceMappingURL=change-enable.js.map