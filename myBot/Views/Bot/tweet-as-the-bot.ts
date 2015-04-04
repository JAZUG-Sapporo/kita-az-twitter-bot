/// <reference path="../../scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../../scripts/typings/jquery.form/jquery.form.d.ts" />
$(() => {
    $('#tweet-as-the-bot-form')
        .ajaxForm({
        success: () => { alert('The text was tweeted successful.'); },
        error: () => { alert('Oops, something wrong...'); }
    });
});