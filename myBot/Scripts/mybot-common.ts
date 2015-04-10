module myBot {
    var app = angular.module('myBot', []);

    app.filter('charcounter',() =>
        (input: string, max: number) => {
            var regxp = /((https?:\/\/)?([a-z0-9.\-_]+\.[a-z]{2,3})([a-z0-9.%\-_\+/~])*(\?[a-z0-9=&%\-\+_!/~]*)?(\#[a-z0-9=&%\-\+_!/~]*)?)([^a-z.]|$)/ig;
            input = input.replace(regxp,() => {
                // DEBUG: console.dir(arguments);
                return new Array(22 + 1).join('-') + arguments[7];
            });
            return max - input.length;
        });

    class TweetTextController {
        public constructor($scope: any) {
            $scope.text = $('#original-text').val() || '';
        }
    }

    app.controller('TweetTextController', TweetTextController);
} 