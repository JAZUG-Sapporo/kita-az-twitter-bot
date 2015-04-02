module myBot {
    var app = angular.module('myBot', []);
    app.filter('charcounter',() =>
        (input: string, max: number) => {
            return max - input.length;
        });
} 