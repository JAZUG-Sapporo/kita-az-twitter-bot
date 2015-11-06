$(function () {
    var $messagesHolder = $('#messages-holder');
    // * Deleting message is supported by 'delete-item.ts'.
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