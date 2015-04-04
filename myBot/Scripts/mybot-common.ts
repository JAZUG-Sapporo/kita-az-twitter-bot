module myBot {
    var app = angular.module('myBot', []);

    app.filter('charcounter',() =>
        (input: string, max: number) => {
            var regxp = /((https?:\/\/)?([a-z.]+\.[a-z]{2,3}))([^a-z.]|$)/ig;
            input = input.replace(regxp,() => new Array(22 + 1).join('-') + arguments[4]);
            return max - input.length;
        });

    class TweetTextController {
        public constructor($scope: any) {
            $scope.text = $('#original-text').val() || '';
        }
    }

    app.controller('TweetTextController', TweetTextController);
} 