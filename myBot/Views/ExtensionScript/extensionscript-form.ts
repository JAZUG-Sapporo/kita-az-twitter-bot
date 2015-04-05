module myBot {
    export var testRunURL: string;

    var app = angular.module('myBot');

    class ExtensionScriptFormController {
        private $scope: any;
        private $http: ng.IHttpService;

        public constructor($scope: any, $http: ng.IHttpService) {
            this.$scope = $scope;
            this.$http = $http;
            this.$scope.scriptText = $('#original-text').val() || '';
            this.$scope.language = $('#original-language').val() || '';
            this.$scope.testRunResult = { status: 'Not Run' };
        }

        public ExecuteTestRun(): void {
            this.$scope.testRunResult = { status: 'Running' };
            this.$http.post(testRunURL, { scriptText: this.$scope.scriptText, language: this.$scope.language })
                .success(result => this.$scope.testRunResult = result)
                .error((_: any, status: number) => this.$scope.testRunResult = { status: 'Error', errmsg: 'HTTP ' + status });
        }
    }

    app.controller('ExtensionScriptFormController', ExtensionScriptFormController);
} 