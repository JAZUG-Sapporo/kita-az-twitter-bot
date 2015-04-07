var myBot;
(function (myBot) {
    myBot.testRunURL;
    var app = angular.module('myBot');
    var ExtensionScriptFormController = (function () {
        function ExtensionScriptFormController($scope, $http) {
            this.$scope = $scope;
            this.$http = $http;
            this.$scope.scriptText = $('#original-text').val() || '';
            this.$scope.language = $('#original-language').val() || '';
            this.$scope.testRunResult = { status: 'Not Run' };
        }
        ExtensionScriptFormController.prototype.ExecuteTestRun = function () {
            var _this = this;
            this.$scope.testRunResult = { status: 'Running' };
            this.$http.post(myBot.testRunURL, { scriptText: this.$scope.scriptText, language: this.$scope.language }).success(function (result) { return _this.$scope.testRunResult = result; }).error(function (_, status) { return _this.$scope.testRunResult = { status: 'Error', errmsg: 'HTTP ' + status }; });
        };
        return ExtensionScriptFormController;
    })();
    app.controller('ExtensionScriptFormController', ExtensionScriptFormController);
})(myBot || (myBot = {}));
