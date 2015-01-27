$(function () {
    var $messagesHolder = $('#messages-holder');
    // Delete message
    $(document).on('click', '#messages-holder .delete', function (e) {
        e.preventDefault();
        if (confirm($messagesHolder.data('confirmDelete')) == false)
            return;
        var $target = $(e.target);
        $.ajax({
            type: 'POST',
            url: $target.attr('href')
        }).done(function () {
            $target.closest('.message').slideUp('fast');
        }).fail(function () {
            alert('Oops, something wrong...');
        });
    });
    // Up/Down message order
    $(document).on('click', '#messages-holder .up,#messages-holder .down', function (e) {
        e.preventDefault();
        var $target = $(e.target);
        $.ajax({
            type: 'POST',
            url: $target.attr('href')
        }).done(function (data) {
            if (data.moved == true) {
                var $targetMessage = $target.closest('.message');
                var $replaceTo = ($target.hasClass('up') ? $.fn.prev : $.fn.next).apply($targetMessage);
                $replaceTo.detach();
                ($target.hasClass('up') ? $.fn.insertAfter : $.fn.insertBefore).apply($replaceTo, [$targetMessage]);
            }
        }).fail(function () {
            alert('Oops, something wrong...');
        });
    });
});
//# sourceMappingURL=messages.js.map