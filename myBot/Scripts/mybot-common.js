var myBot;
(function (myBot) {
    var app = angular.module('myBot', []);
    app.filter('charcounter', function () { return function (input, max) {
        return max - input.length;
    }; });
})(myBot || (myBot = {}));
//# sourceMappingURL=mybot-common.js.map