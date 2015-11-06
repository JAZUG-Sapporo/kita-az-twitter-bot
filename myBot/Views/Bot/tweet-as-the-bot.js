/// <reference path="../../scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../../scripts/typings/jquery.form/jquery.form.d.ts" />
$(function () {
    $('#tweet-as-the-bot-form')
        .ajaxForm({
        success: function () { alert('The text was tweeted successful.'); },
        error: function () { alert('Oops, something wrong...'); }
    });
});
//# sourceMappingURL=tweet-as-the-bot.js.map