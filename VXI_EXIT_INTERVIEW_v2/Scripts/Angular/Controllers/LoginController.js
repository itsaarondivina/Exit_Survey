App.controller("loginController",
    function ($scope, $http, Loader) {
        $scope.loginInfo = {
            Hrid: '',
            BirthDate: ''
        };
        Loader.Close();
        $scope.test = function () {
            alert('Hello');
        }

        $scope.showHridError = false;

        $scope.resetHridValidation = function () {
            document.getElementById("req_hrid").style.display = "none";
        };
        $scope.resetbdayValidation = function () {
            document.getElementById("req_bday").style.display = "none";
        };

        $scope.Login = function () {
            $scope.resetHridValidation();
            $scope.resetbdayValidation();
            if (!$scope.loginInfo.Hrid && !$scope.loginInfo.BirthDate) {
                document.getElementById("req_bday").style.display = "block";
                document.getElementById("req_hrid").style.display = "block";
            } else if (!$scope.loginInfo.Hrid) {
                document.getElementById("req_hrid").style.display = "block";
            } else if (!$scope.loginInfo.BirthDate) {
                document.getElementById("req_bday").style.display = "block";
            } else {
                Loader.Open();
                $http({
                    method: "POST",
                    url: url + 'Home/Login',
                    data: { data: $scope.loginInfo }
                }).then(function successCallback(response) {
                    if (response.data.status) {
                        window.location.replace(url + "Home/Index");
                    }
                    else {
                        Loader.Close();
                        Swal.fire({
                            icon: "error",
                            title: "Oops...",
                            text: "The user name or password is incorrect.",
                        }).then(() => {
                            window.location.reload();
                        });
                    }

                },
                    function errorCallback(result) {
                    });
            }
        }
    })