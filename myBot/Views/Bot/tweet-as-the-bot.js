/// <reference path="../../scripts/typings/jquery/jquery.d.ts" />
/// <reference path="../../scripts/typings/jquery.form/jquery.form.d.ts" />
var myBot;
(function (myBot) {
    var TweetAsTheBotController = (function () {
        function TweetAsTheBotController($scope) {
            $scope.text = '';
        }
        return TweetAsTheBotController;
    })();
    var app = angular.module('myBot');
    app.controller('TweetAsTheBotController', TweetAsTheBotController);
})(myBot || (myBot = {}));
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
//# sourceMappingURL=tweet-as-the-bot.js.map