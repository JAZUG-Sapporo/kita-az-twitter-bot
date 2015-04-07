$(function () {
    $('#tweet-as-the-bot-form').ajaxForm({
        success: function () {
            alert('The text was tweeted successful.');
        },
        error: function () {
            alert('Oops, something wrong...');
        }
    });
});
