
/* *******************************************************
 * Codec control for front page popup */
ccmControllers.controller('sipInfoController', function ($scope, $http, $interval, $uibModalInstance, sipid, sipAddress) {

    var audioStatusUpdateInterval = 1000; // Uppdate interval for inputs, GPO:s and VU-meters in ms
    var audioStatusUpdateHandle;

    $scope.codecControlHost = window.codecControlHost;
    $scope.userName = window.codecControlUserName;
    $scope.password = window.codecControlPassword;
    $scope.codecOnline = false;  // 'true' if codec is reachable
    $scope.sipid = sipid;
    $scope.sipAddress = sipAddress;
    $scope.info = {};
    $scope.txL = -96;
    $scope.txR = -96;
    $scope.rxL = -96;
    $scope.rxR = -12;
    $scope.rebootconfirm1 = false;
    $scope.rebootconfirm2 = false;
    $scope.canAx = "ActiveXObject" in window;
    $scope.presetToLoad = '';
    $scope.selectedLine = '';
    $scope.gpos = [];
    $scope.inputs = [];
    $scope.lines = [];
    $scope.lineStatusArea = '';
    $scope.audioModeArea = '';
    $scope.signalrConnection = null;

    console.info('Getting codec information for ' + sipAddress);

    $scope.setCodecIsOnline = function (codecIsOnline) {
        // Codec is reachable
        console.info('Codec is online:', codecIsOnline);
        $scope.codecOnline = codecIsOnline;

        if (codecIsOnline) {
            $scope.getAvailableGpos();
            $scope.getAudioStatus();
            $scope.startAudioStatus();
            $scope.startAudioUpdateHub();
        }
    };

    $scope.startSignalrConnection = function () {
        if (!$scope.signalrConnection) console.error('SignalrConnection must be configured before it can be started.');

        console.log('Initiating Signalr connection', $scope.signalrConnection);

        $scope.signalrConnection.start()
            .then(function (result) {
                console.log("Signalr started");

                $scope.signalrConnection.invoke("Subscribe", $scope.sipAddress).catch(function (err) {
                    return console.error(err.toString());
                });
            })
            .catch(function (err) {
                setTimeout($scope.startSignalrConnection, 3000);
                return console.error(err.toString());
            });
    };
    
   $scope.startAudioStatus = function () {
        if (angular.isDefined(audioStatusUpdateHandle)) return;

        audioStatusUpdateHandle = $interval(function () {
            $scope.getAudioStatus();
        },
        audioStatusUpdateInterval);
    };

    $scope.stopAudioStatus = function () {
        if (angular.isDefined(audioStatusUpdateHandle)) {
            $interval.cancel(audioStatusUpdateHandle);
            audioStatusUpdateHandle = undefined;
        }
    };

    $scope.startAudioUpdateHub = function () {
        $scope.signalrConnection = new signalR.HubConnectionBuilder()
            .withUrl($scope.codecControlHost + "/audioStatusHub")
            .configureLogging(signalR.LogLevel.Information)
            .build();

        $scope.signalrConnection.on("AudioStatus", function (sipAddress, audioStatus) {
            //console.log("AudioStatus received", sipAddress, audioStatus);
            if (sipAddress === $scope.sipAddress) {
                $scope.updateAudioStatus(audioStatus);
            }
        });

        $scope.signalrConnection.onclose(function (e) {
            console.log('Signalr connection closed.');
            setTimeout($scope.startSignalrConnection, 3000);
        });

        $scope.startSignalrConnection();
    }

    // API
    $scope.getAvailableGpos = function () {
        console.info('Getting GPOs');
        $http.get($scope.codecControlHost + '/api/codeccontrol/getavailablegpos?sipaddress=' + $scope.sipAddress)
            .then(function (response) { // on success
                console.info('Available GPOs', response.data);
                $scope.gpos = response.data.gpos;
            },
            function () { // on error
                $scope.gpos = [];
            });
    };

    $scope.getLineStatus = function (line) {
        $http.get($scope.codecControlHost + '/api/codeccontrol/getlinestatus?sipaddress=' + $scope.sipAddress)
            .then(function (response) {
                var data = response.data;
                console.log(data);
                var msg = "LineStatus: " + data.lineStatus + ". DisconnectReason: " + data.disconnectReasonDescription + " (" + data.disconnectReasonCode + ").\r\n";
                $scope.lineStatusArea = msg + $scope.lineStatusArea;
            });
    };

    $scope.getAudioMode = function () {
        $http.get($scope.codecControlHost + '/api/codeccontrol/GetAudioMode?sipaddress=' + $scope.sipAddress).then(function (response) {
            var data = response.data;
            var msg;
            if (data.Error) {
                msg = data.Error;
            } else {
                msg = "AudioMode: Encoder=" + data.encoderAudioModeDescription + " (" + data.encoderAudioMode + ")" +
                    ", Decoder=" + data.decoderAudioModeDescription + " (" + data.decoderAudioMode + ")";
            }
            $scope.audioModeArea = msg + "\r\n" + $scope.audioModeArea;
        });
    };

    $scope.setGpo = function (gpo) {
        var newState = gpo.active ? false : true;
        $scope.httpPost('/api/codeccontrol/setgpo', { sipaddress: $scope.sipAddress, number: gpo.number, active: newState })
            .then(function (data) {
                console.log("Codec GPO data: ", data);
                gpo.active = data.active;
            });
    };

    $scope.getAudioStatus = function () {
        console.info('Getting codec audio status');

        $http.get($scope.codecControlHost + '/api/codeccontrol/getaudiostatus?sipaddress=' + $scope.sipAddress).then(function (response) {
            var audioStatus = response.data;
            $scope.updateAudioStatus(audioStatus);
        });

    };

    $scope.updateAudioStatus = function (audioStatus) {
        for (var i = 0; i < audioStatus.inputStatus.length; i++) {
            $scope.setInputValue(audioStatus.inputStatus[i]);
        }

        for (var j = 0; j < audioStatus.gpos.length; j++) {
            var gpoData = audioStatus.gpos[j];
            var gpo = $scope.gpos[gpoData.index];
            if (gpo) {
                gpo.active = gpoData.active;
            }
        }

        // Values is presented in dB where 0 = Fullscale +18db, -18 = Test 0dB, -96 = min-level
        $scope.txL = fallback($scope.txL, convertVuToPercentage(audioStatus.vuValues.txLeft));
        $scope.txR = fallback($scope.txR, convertVuToPercentage(audioStatus.vuValues.txRight));
        $scope.rxL = fallback($scope.rxL, convertVuToPercentage(audioStatus.vuValues.rxLeft));
        $scope.rxR = fallback($scope.rxR, convertVuToPercentage(audioStatus.vuValues.rxRight));
    };

    $scope.setInputValue = function (inputStatus) {
        var input = $scope.inputs[inputStatus.index];

        if (input) {
            if (inputStatus.error) {
                input.error = inputStatus.error;
                input.disabled = 'disabled';
            } else {
                input.value = inputStatus.gainLevel;
                input.enabled = inputStatus.enabled;
                input.disabled = '';
            }
        }
    };

    $scope.toggleInputEnabled = function (input) {
        var enabled = input.enabled ? false : true;
        let data = { sipAddress: $scope.sipAddress, input: input.id, enabled: enabled };
        $scope.httpPost('/api/codeccontrol/setinputenabled', data).then(function (data) {
            let isEnabled = data.enabled;
            console.log("SetInputEnabled", input.id, isEnabled);
            input.enabled = isEnabled;
        });
    };
    
    $scope.setGainLevel = function (input, value) {
        $scope.httpPost('/api/codeccontrol/setinputgain', { sipAddress: $scope.sipAddress, input: input.id, level: input.value + value })
            .then(function (data) {
                input.value = data.gainLevel;
            });
    };

    $scope.reboot = function () {
        this.rebootconfirm1 = false;
        this.rebootconfirm2 = false;
        $scope.httpPost('/api/codeccontrol/reboot', { sipaddress: $scope.sipAddress });
    };

    $scope.checkCodecAvailable = function () {
        console.info('Checking if codec is online');
        $http.get($scope.codecControlHost + '/api/codeccontrol/isavailable?sipaddress=' + $scope.sipAddress)
            .then(function (response) {
                var data = response.data;
                console.debug('Codec available: ', data);
                let isAvailable = data.isAvailable;
                $scope.setCodecIsOnline(isAvailable);
            })
            .catch(function (err) {
                console.error(err);
            });
    };

    $scope.openAdmin = function (link, width, height, scrollbars) {
        if (width <= 0) {
            width = 1000;
        }

        if (height <= 0) {
            height = 620;
        }

        window.open(link, '_blank', 'width=' + width + ',height=' + height + ',status=no,menubar=no,resizable=no,scrollbars=' + (scrollbars ? 'yes' : 'no') + ',toolbar=no,location=no,directories=no');
        return false;
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

    $scope.$on("$destroy", function () {
        console.log("Destroy called.");

        // Stop interval checking Audio status
        $scope.stopAudioStatus();

        // Stop subscription
        if ($scope.signalrConnection) {
            $scope.signalrConnection.invoke("Unsubscribe", null)
                .then(function() {
                    console.log("Unsubscribed");
                })
                .catch(function(err) {
                        return console.error(err.toString());
                });
        }
    });
    
    $http.get('/api/registeredsipdetails/GetRegisteredSipInfo?id=' + $scope.sipid).then(function (response) {
        let info = response.data;
        console.info('Codec information', info);
        $scope.info = info;

        if (info.IsAuthenticated && info.CodecControl)
        {
            if (info.Inputs)
            {
                var inputs = [];
                for (var i = 0; i < info.Inputs; i++) {
                    var input = { id: i, number: i + 1, value: 0, enabled: false, disabled: 'disabled' };
                    inputs.push(input);
                }
                $scope.inputs = inputs;
            }

            if (info.Lines)
            {
                var lines = [];
                for (var j = 0; j < info.Lines; j++) {
                    var line = { id: j, number: j + 1 };
                    lines.push(line);
                }
                $scope.lines = lines;

                if (lines.length > 0) {
                    $scope.selectedLine = lines[0];
                }
            }

            $scope.checkCodecAvailable();
        }
        else
        {
            console.warn('Codec-control is not Authorized');
        }
    },
    function () {
        console.info("Codec information is missing");
    });

    // Utilities
    $scope.httpPost = function (apiPath, data) {
        var authorizationBasic = window.btoa($scope.userName + ':' + $scope.password);
        const headers = { 'Authorization': 'Basic ' + authorizationBasic };

        return $http.post($scope.codecControlHost + apiPath, data, { headers: headers }).then(function (response) {
            return response.data;
        });
    };

});
