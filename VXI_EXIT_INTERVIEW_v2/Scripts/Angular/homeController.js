App.controller("homeController",
    function ($scope, $http, Loader, $timeout) {
        Loader.Close();

        $scope.answerList = [];

        $scope.goSurvey = function () {
            Loader.Open();
            window.location.replace(url + "Home/SurveyPage");
        }

        $scope.Loaditems = function () {
            Loader.Open();

            $http({
                method: "GET",
                url: url + 'Home/GetSurvey'
            }).then(function successCallback(result) {
                $scope.Question = result.data.questionList;
                $scope.Answer = result.data.answerList;

                console.log("Question: ", $scope.Question);
                console.log("Answer: ", $scope.Answer);
                Loader.Close();
            }, function errorCallback(res) {

            });
        }

        $scope.getAnswerList = function () {
            var answer = [];
            var questionCount = $(".question-container").length;

            for (var i = 0; i < questionCount; i++) {
                var questionSelector = $(".question-container:eq(" + i + ")"),
                    questionType = questionSelector.data("type"),
                    questionId = questionSelector.data("questionid");

                var answerCount = $(".answer[data-questionid=" + questionId + "]").length;

                if (questionType == 1) {
                    for (var a = 0; a < answerCount; a++) {
                        var userAnswer = $(".answer[data-questionid='" + questionId + "']:eq(" + a + ")").val();

                        if (userAnswer.length != 0 && userAnswer != null) {
                            var answerInfo = {
                                answer: $(".answer[data-questionid='" + questionId + "']:eq(" + a + ")").val(),
                                answerId: $(".answer[data-questionid='" + questionId + "']:eq(" + a + ")").data('id'),
                                questionId: questionId
                            }

                            answer.push(answerInfo);
                        }
                    }
                }

                else if (questionType == 3) {
                    for (var b = 0; b < answerCount; b++) {
                        var isChecked = $(".answer[data-questionid='" + questionId + "']:eq(" + b + ")").is(':checked');

                        if (isChecked) {
                            var answerInfo = {
                                answer: $(".answer[data-questionid='" + questionId + "']:eq(" + b + ")").val(),
                                answerId: $(".answer[data-questionid='" + questionId + "']:eq(" + b + ")").data('id'),
                                questionId: questionId
                            }

                            answer.push(answerInfo);
                        }
                    }
                }

                else if (questionType != 3 && questionType != 1) {
                    var answerInfo = {
                        answer: $(".answer[data-questionid='" + questionId + "']:checked").val(),
                        answerId: $(".answer[data-questionid='" + questionId + "']:checked").data('id'),
                        questionId: questionId
                    }

                    answer.push(answerInfo);
                }
            }

            return answer;
        }

        $scope.SubmitSurvey = function () {
            Loader.Open();
            var answerLists = $scope.getAnswerList();

            $http({
                method: "POST",
                url: url + 'Home/saveSurvey',
                data: answerLists
            }).then(function successCallback(result) {
                console.log(result);
                Loader.Close();
            }, function errorCallback(res) {

            });
        };




    })