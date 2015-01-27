$(() => {
    var $messagesHolder = $('#messages-holder');

    // * Deleting message is supported by 'delete-item.ts'.

    // Up/Down message order
    $(document).on('click', '#messages-holder .up,#messages-holder .down',(e) => {
        e.preventDefault();
        var $target = $(e.target);

        $.ajax({
            type: 'POST',
            url: $target.attr('href')
        }).done((data: any) => {
            if (data.moved == true) {
                var $targetMessage = $target.closest('.message');
                var $replaceTo: JQuery = ($target.hasClass('up') ? $.fn.prev : $.fn.next).apply($targetMessage);
                $replaceTo.detach();
                ($target.hasClass('up') ? $.fn.insertAfter : $.fn.insertBefore).apply($replaceTo, [$targetMessage]);
            }
        }).fail(() => {
            alert('Oops, something wrong...');
        });
    });
}); 