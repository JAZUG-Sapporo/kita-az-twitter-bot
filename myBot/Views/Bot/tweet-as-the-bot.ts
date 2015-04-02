/// <reference path="../../scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../../scripts/typings/jquery.form/jquery.form.d.ts" />
module myBot {
    class TweetAsTheBotController {
        public constructor($scope: any) {
            $scope.text = '';
        }
    }

    var app = angular.module('myBot');
    app.controller('TweetAsTheBotController', TweetAsTheBotController);
}

$(() => {
    $('#tweet-as-the-bot-form')
        .ajaxForm({
        success: () => { alert('The text was tweeted successful.'); },
        error: () => { alert('Oops, something wrong...'); }
    });
});