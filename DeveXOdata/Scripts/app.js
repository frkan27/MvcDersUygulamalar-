/// <reference path="angular.js" />


var app=angular.module("myApp",["dx"]);

app.controller("testCtrl",function($scope){
$scope.dataGridOptions={
dataSource:customers,
}

});