var myBot;
(function (myBot) {
    var app = angular.module('myBot', []);
    app.filter('charcounter', function () { return function (input, max) {
        var regxp = /((https?:\/\/)?([a-z.]+\.[a-z]{2,3}))([^a-z.]|$)/ig;
        input = input.replace(regxp, function () { return new Array(22 + 1).join('-') + arguments[4]; });
        return max - input.length;
    }; });
    var TweetTextController = (function () {
        function TweetTextController($scope) {
            $scope.text = $('#original-text').val() || '';
        }
        return TweetTextController;
    })();
    app.controller('TweetTextController', TweetTextController);
})(myBot || (myBot = {}));
//# sourceMappingURL=mybot-common.js.map