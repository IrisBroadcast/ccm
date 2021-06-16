'use strict';

/* *******************************************************
 * JS Application */
var ccmApp = angular.module('ccm', ['ccmControllers', 'ui.bootstrap', 'ngStorage']);

ccmApp.filter('timeAgo', function () {
    return function (started, currentTime) {
        let secNum = parseInt((currentTime - started) / 1000, 10);
        if (secNum < 0) {
            return '---';
        }

        let days = Math.floor(secNum / (24 * 60 * 60));
        let hours = Math.floor(secNum / (60 * 60)) % 24;
        let minutes = Math.floor(secNum / 60) % 60;
        let seconds = secNum % 60;

        if (hours < 10) {
            hours = "0" + hours;
        }
        if (minutes < 10) {
            minutes = "0" + minutes;
        }
        if (seconds < 10) {
            seconds = "0" + seconds;
        }
        if (days > 1) {
            return days + " dagar " + hours + ':' + minutes + ':' + seconds; // TODO: Internationalisera detta
        } else if (days === 1) {
            return days + " dag " + hours + ':' + minutes + ':' + seconds;
        } else {
            return hours + ':' + minutes + ':' + seconds;
        }
    };
});

ccmApp.filter('max', function () {
    return function (value, max) {
        if (max < 0) {
            max = 0;
        }
        return value > max ? max : value;
    };
});

ccmApp.filter('callTimeWarning', function () {
    // {{'text'|callTimeWarning:warningLevel(10000):call.start:call.end}}
    return function (type, warningLevel, callStart, callEnd) {
        const diff = new Date(callEnd) - new Date(callStart);
        if (parseInt(diff) <= warningLevel) {
            if (type === 'class') {
                return 'list-ccm--warning-calltime';
            } else {
                return true;
            }
        } else {
            return false;
        }
    };
});

ccmApp.directive('shortcut', function () {
    return {
        restrict: 'E',
        replace: true,
        scope: true,
        link: function postLink(scope, iElement, iAttrs) {
            jQuery(document).on('keypress', function (e) {
                scope.$apply(scope.keyPressed(e));
            });
        }
    };
});

ccmApp.factory('backendHubProxy', ['$rootScope', function ($rootScope) {

    function backendFactory(serverUrl, hubName) {

        //var connection = $.hubConnection(serverUrl);

        const connection = new signalR.HubConnectionBuilder()
            .withUrl('/' + hubName)
            .withAutomaticReconnect()
            .build();
        console.log(connection);

        //connection.logging = true;

        connection.on('error', function (error) {
            console.error('* SignalR error: ', error);
        });

        connection.on('connectionSlow', function () {
            console.log("* SignalR connectionSlow");
        });

        connection.on('disconnected', function () {
            console.log("* SignalR disconnected");
            setTimeout(function () {
                connection.start();
            }, 2000);
        });

        return {
            on: function (eventName, callback) {
                connection.on(eventName,
                    function (result) {
                        $rootScope.$apply(function () {
                            if (callback) {
                                callback(result);
                            }
                        });
                    });
            },
            start: function () {
                connection.start()
                    .then(function () {
                        console.log("* Connected to SignalR hub. Connection id=" + connection.id);
                    })
                    .catch(function (error) {
                        console.log('* Could not connect to hubName: ', hubName, ', serverUrl: ', serverUrl, error);
                    });
            },
            stop: function () {
                connection.stop(false, true);
            },
            status: function (callback) {
                // Get connection status
                connection.on('starting',
                    function () {
                        $rootScope.$apply(function () {
                            if (callback) {
                                callback({ action: "starting", current: 5 });
                            }
                        });
                    });
                connection.onclose(
                    function () {
                        $rootScope.$apply(function () {
                            if (callback) {
                                callback({ action: "disconnected", current: 4 });
                            }
                        });
                    });
            }
        };
    }

    return backendFactory;
}]);

ccmApp.directive('vuMeter', function () {
    return {
        restrict: 'E',
        scope: { level: '=level' },
        template: "<div class='progress vertical'>" +
            "<div class='progress-bar green' style='bottom: 0%;' ng-style='{ height: (level | max:70) + \"%\"}'></div>" +
            "<div class='progress-bar yellow' style='bottom: 70%;' ng-style='{height: (level-70 | max:15) + \"%\"}'></div>" +
            "<div class='progress-bar red' style='bottom: 85%;' ng-style='{height: (level-85 | max:15) + \"%\"}'></div>" +
            "</div>"
    };
});

ccmApp.component("loadingOverlay", {
    template: "<div class='overlay'><div class='spinner'/></div>"
});

var ccmControllers = angular.module('ccmControllers', []);

/* *******************************************************
 * CCM Frontpage */
ccmControllers.controller('overviewController', function ($scope, $http, $interval, $uibModal, backendHubProxy, $sessionStorage) {

    var timerHandle_updateCallDuration;

    $scope.isFiltered = false;

    $scope.currentTime = new Date();

    $scope.startCallDurationTimer = function () {
        if (angular.isDefined(timerHandle_updateCallDuration)) return;

        timerHandle_updateCallDuration = $interval(function () {
            $scope.currentTime = new Date();
        }, 1000);
    };

    $scope.stopCallDurationTimer = function () {
        if (angular.isDefined(timerHandle_updateCallDuration)) {
            $interval.cancel(timerHandle_updateCallDuration);
            timerHandle_updateCallDuration = undefined;
        }
    };

    var viewIsFiltered = function () {
        if ($scope.codecType || $scope.region || $scope.category || $scope.searchString) {
            return true;
        } else {
            return false;
        }
    };

    var setOngoingCalls = function (ongoingCalls) {
        // Updates start-time based on duration and local time/clock
        // call-length is presented in a correct way even if there is different
        // times on server and client

        ongoingCalls.forEach(function (item) {
            var startTime = new Date() - (item.durationSeconds * 1000);
            item.startedLocal = new Date(startTime);
        });

        if (ongoingCalls.length > 0) {
            $scope.startCallDurationTimer();
        } else {
            $scope.stopCallDurationTimer();
        }

        $scope.onGoingCalls = ongoingCalls;
    };

    var onOldCalls = function (oldCalls) {
        if (viewIsFiltered()) {
            // If filter is applied get filtered data from server and show
            $scope.refreshOldFiltered();
        } else {
            $scope.oldCalls = oldCalls;
        }
    };

    /* *******************************************************
     * Hub Sockets */
    var ccmDataHub = backendHubProxy("", 'WebGuiHub');

    ccmDataHub.on('codecsOnline',
        function (data) {
            console.log("Received codecs online update: ", data.length);
            $scope.registeredSips = data;
        });

    ccmDataHub.on('ongoingCalls',
        function (data) {
            console.log("Received ongoing calls update: ", data.length);
            setOngoingCalls(data);
        });

    ccmDataHub.on('oldCalls',
        function (data) {
            console.log("Received old calls update: ", data.length);
            onOldCalls(data);
        });

    ccmDataHub.start();

    $scope.socketStatusHubGui = { reconnectAttempts: 0, socketStatus: false };

    ccmDataHub.status(
        function (state) {
            console.log("* Status changed: ", state);
            if (state.current === 1) {
                // Connecting
                $scope.socketStatusHubGui.reconnectAttempts = 0;
                $scope.socketStatusHubGui.socketStatus = true;
            } else if (state.current === 2) {
                // Connected ish
                $scope.socketStatusHubGui.reconnectAttempts = 0;
                $scope.socketStatusHubGui.socketStatus = false;
            } else if (state.current === 3) {
                // Reconnecting
                $scope.socketStatusHubGui.reconnectAttempts += 1;
                $scope.socketStatusHubGui.socketStatus = false;
            } else if (state.current === 4) {
                // Disconnected
                $scope.socketStatusHubGui.socketStatus = false;
            } else if (state.current === 5) {
                // Starting
                $scope.socketStatusHubGui.reconnectAttempts += 1;
                $scope.socketStatusHubGui.socketStatus = false;
            } else {
                // Unknown
                $scope.socketStatusHubGui.socketStatus = false;
            }
        });

    /* *******************************************************
     * Lists and data, filtering */
    $scope.registeredSipsFilter = function (item) {
        if ($scope.region && item.regionName !== $scope.region) {
            return false;
        }

        if ($scope.codecType && item.codecTypeName !== $scope.codecType) {
            return false;
        }

        if ($scope.category && item.locationCategory !== $scope.category) {
            return false;
        }

        if ($scope.searchString) {
            var search = $scope.searchString.toLowerCase();
            if ($scope.containsString(item.displayName, search)) {
                return true;
            }
            if ($scope.containsString(item.sip, search)) {
                return true;
            }
            if ($scope.containsString(item.userName, search)) {
                return true;
            }
            if ($scope.containsString(item.userComment, search)) {
                return true;
            }
            if ($scope.containsString(item.location, search)) {
                return true;
            }
            if ($scope.containsString(item.locationShortName, search)) {
                return true;
            }
            return false;
        }
        return true;
    };

    $scope.callFilter = function (item) {
        if ($scope.region &&
            item.fromRegionName !== $scope.region &&
            item.toRegionName !== $scope.region) {
            return false;
        }

        if ($scope.codecType &&
            item.fromCodecTypeName !== $scope.codecType &&
            item.toCodecTypeName !== $scope.codecType) {
            return false;
        }

        if ($scope.category &&
            item.fromLocationCategory !== $scope.category &&
            item.toLocationCategory !== $scope.category) {
            return false;
        }

        if ($scope.searchString) {
            var search = $scope.searchString.toLowerCase();
            if ($scope.containsString(item.fromDisplayName, search)) {
                return true;
            }
            if ($scope.containsString(item.toDisplayName, search)) {
                return true;
            }
            if ($scope.containsString(item.fromSip, search)) {
                return true;
            }
            if ($scope.containsString(item.toSip, search)) {
                return true;
            }
            if ($scope.containsString(item.fromLocationName, search)) {
                return true;
            }
            if ($scope.containsString(item.toLocationName, search)) {
                return true;
            }
            if ($scope.containsString(item.fromLocationShortName, search)) {
                return true;
            }
            if ($scope.containsString(item.toLocationShortName, search)) {
                return true;
            }
            if ($scope.containsString(item.fromUserDisplayName, search)) {
                return true;
            }
            if ($scope.containsString(item.toUserDisplayName, search)) {
                return true;
            }
            return false;
        }
        return true;
    };

    $scope.containsString = function (item, searchString) {
        return !!(item && item.toLowerCase().indexOf(searchString) > -1);
    };

    $scope.refreshOngoing = function () {
        $http.post('/api/OngoingCall')
            .then(function (response) {
                setOngoingCalls(response.data);
            },
            function () {
                console.error("No answer from '/api/OngoingCall'");
            });
    };

    $scope.refreshOld = function () {
        $http.post('/api/OldCall')
            .then(function (response) {
                console.log(response)
                $scope.oldCalls = response.data;
            },
            function (response) {
                console.error("No answer from '/api/OldCall'");
            });
    };

    $scope.refreshOldFiltered = function () {
        const region = $scope.region ? $scope.region : "";
        const codecType = $scope.codecType ? $scope.codecType : "";
        console.log("Getting filtered cancelled/hungup calls, region: ", region, ", type: ", codecType, ", search: ", $scope.searchString);
        $http.get('/api/OldCallFiltered?region=' + encodeURI(region) + '&codecType=' + encodeURI(codecType) + '&search=' + encodeURI($scope.searchString))
            .then(function (response) {
                const oldCalls = response.data;
                console.log("Recieved cancelled/hungup calls from server: ", oldCalls.length);
                $scope.oldCalls = oldCalls;
            },
                function (error) {
                    console.error("No answer from '/api/OldCallFiltered'", error);
                });
    };

    $scope.refreshRegistered = function () {
        $http.post('/api/RegisteredCodec')
            .then(function (response) {
                $scope.registeredSips = response.data;
            },
                function (error) {
                    console.error("No answer from '/api/RegisteredCodec'", error);
                });
    };

    $scope.changeOngoingCallSorting = function (sortProperty) {
        $scope.setSorting($scope.ongoingCallSort, sortProperty);
    };

    $scope.changeRegisteredSorting = function (sortProperty) {
        $scope.setSorting($scope.registeredSort, sortProperty);
    };

    $scope.setSorting = function (sort, sortProperty) {
        if (sort.column === sortProperty) {
            sort.descending = !sort.descending;
        } else {
            sort.column = sortProperty;
            sort.descending = false;
        }
        console.log("Sort: ", sort.column, ", descending: ", sort.descending);
    };

    $scope.resetViewFilters = function () {
        console.log("Resetting filters");
        $scope.region = "";
        $scope.regionName = "";
        $scope.codecType = "";
        $scope.codecTypeName = "";
        $scope.searchString = "";
        $scope.category = "";
        $scope.categoryName = "";
    };

    $scope.setFilterRegion = function (region) {
        $scope.region = region;
        $scope.regionName = region;
        $('#regions li').removeClass('active');
        $('#regions li:contains(' + $scope.regionName + ')').addClass('active');
        $sessionStorage.region = $scope.region;
    };

    $scope.setFilterCodecType = function (codecType) {
        $scope.codecType = codecType;
        $scope.codecTypeName = codecType;
        // TODO: make these correct and maybe just angular fully..
        $('#codecTypes li').removeClass('active');
        $('#codecTypes li:contains(' + $scope.codecTypeName + ')').addClass('active');
        $sessionStorage.codecType = $scope.codecType;
    };

    $scope.setFilterCategory = function (category) {
        $scope.category = category;
        $scope.categoryName = category;
        $('#codecTypes li').removeClass('active');
        $('#codecTypes li:contains(' + $scope.codecTypeName + ')').addClass('active');
        $sessionStorage.category = $scope.category;
    };

    $scope.showSipInfo = function (id, sipAddress, $event) {
        $event.preventDefault();
        $uibModal.open({
            templateUrl: 'sipInfo.html',
            controller: 'sipInfoController',
            resolve:
            {
                sipid: function () { return id; },
                sipAddress: function () { return sipAddress; }
            }
        }).result.catch(function (res) {
            console.log('Closed modal: ', res);
            if (!(res === 'cancel' || res === 'escape key press')) {
                //throw res;
            }
        });
    };

    $scope.editComment = function (id) {
        let url = '/home/EditRegisteredSipComment/' + id;
        $('#registeredSipModal').modal({ remote: url });
    };

    $scope.$on("$destroy", function () {
        console.log("> Destroy called, stopCallDurationTimer");
        $scope.stopCallDurationTimer();
    });

    // Sort columns init
    $scope.registeredSort = { column: 'DisplayName', descending: false };
    $scope.ongoingCallSort = { column: 'DurationSeconds', descending: false };

    // Set filter inits from session
    $scope.setFilterRegion($sessionStorage.region);
    $scope.setFilterCodecType($sessionStorage.codecType);
    $scope.searchString = '';
    $scope.searchKeyUp = function (keyCode) {
        if (keyCode === 27) {
            // when escape = 27 is pressed clear field
            $scope.searchString = "";
        }
    };

    $scope.refreshOngoing();
    $scope.refreshRegistered();

    if (viewIsFiltered()) {
        $scope.refreshOldFiltered();
    } else {
        $scope.refreshOld();
    }

    function debounce(func, wait, immediate) {
        var timeout;
        return function () {
            var context = this, args = arguments;
            var later = function () {
                timeout = null;
                if (!immediate) func.apply(context, args);
            };
            var callNow = immediate && !timeout;
            clearTimeout(timeout);
            timeout = setTimeout(later, wait);
            if (callNow) func.apply(context, args);
        };
    };

    var refreshOldFilteredDebounce = debounce(function () {
        $scope.refreshOldFiltered();
    }, 800);

    $scope.$watch('codecType',
        function (newValue, oldValue) {
            if (newValue === oldValue) {
                return;
            }
            console.log('Codec type changed', $scope.codecType);
            refreshOldFilteredDebounce();
            checkIfFiltered();
        });

    $scope.$watch('category',
        function (newValue, oldValue) {
            if (newValue === oldValue) {
                return;
            }
            console.log('Category changed', $scope.category);
            refreshOldFilteredDebounce();
            checkIfFiltered();
        });

    $scope.$watch('region',
        function (newValue, oldValue) {
            if (newValue === oldValue) {
                return;
            }
            console.log('Region changed', $scope.region);
            refreshOldFilteredDebounce();
            checkIfFiltered();
        });

    $scope.$watch('searchString',
        function (newValue, oldValue) {
            if (newValue === oldValue) {
                return;
            }
            console.log('Search string changed', $scope.searchString);
            refreshOldFilteredDebounce();
            checkIfFiltered();
        });

    var checkIfFiltered = function() {
        $scope.isFiltered = viewIsFiltered();
    }

    // Night-mode, full-overview , shift + i = 73, ctrl + i = 9
    $scope.uiStateNightmode = false;
    $scope.keyPressed = function (e) {
        if (e.which === 9) {
            console.log("Triggered nightmode");
            $scope.uiStateNightmode = !$scope.uiStateNightmode;
        }
    };
});

var makeUrlAbsolute = function (url) {
    if (typeof url == 'undefined' || url === null) {
        return "";
    }
    return url.match(/^[a-zA-Z]+:\/\//) ? url : 'http://' + url;
}

// var convertRange = function (value, returnPercent, limitMin, limitMax) {
//     if (value === null && value === undefined) {
//         return 0;
//     }
//     var yMin = limitMin || -128, // -128 actual limit
//         yMax = limitMax || 0;

//     if (value <= yMin) {
//         value = yMin;
//     }

//     var percent = (value - yMin) / (yMax - yMin);

//     if (returnPercent === undefined || returnPercent) {
//         return percent * 100;//((xMax - xMin) + xMin); var xMax = 100; var xMin = 1;
//     }
//     else {
//         return percent.toFixed(2);
//     }
// }