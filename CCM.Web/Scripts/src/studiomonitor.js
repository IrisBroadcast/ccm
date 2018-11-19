// TODO: Slå ihop med codecControl.js. Mycket är likt, men modernare i den här filen.

/* *******************************************************
 * Codec control for studio page */
ccmControllers.controller('studioMonitorController',
    function ($scope, $http, $sce, $interval, $timeout, $window, backendHubProxy) {

        var audioStatusUpdateInterval = 500; // Uppdateringsintervall för ingångar, GPO:er och VU-mätare i ms
        var audioStatusTimeoutHandle;

        var webCamUpdateInterval = 1000; // Uppdateringsintervall för webbkamera-bild. Används för Internet Explorer
        var webCamIntervalHandle;

        var inactivityTimeoutHandle;

        $scope.codecControlHost = $window.codecControlHost;
        $scope.codecControlUserName = $window.codecControlUserName;
        $scope.codecControlPassword = $window.codecControlPassword;
        $scope.studioId = null;
        $scope.sipid = null;
        $scope.isInCall = false;
        $scope.connectedTo = "";
        $scope.nrOfGpos = 0;
        $scope.nrOfMicrophones = 0;
        $scope.codecOnline = false; // True om kodaren är kontaktbar
        $scope.gpos = [];
        $scope.inputs = [];
        $scope.cameraUrl = '';
        $scope.cameraStillImageUrl = '';
        $scope.cameraExists = true;
        $scope.playAudioUrl = '';
        $scope.audioClipNames = '';
        $scope.infoText = '';
        $scope.moreInfoUrl = '';
        $scope.txL = 0;
        $scope.txR = 0;
        $scope.rxL = 0;
        $scope.rxR = 0;
        $scope.adjusting = false; // True om användaren sätter ingångsnivåer. Stoppar pollning efter ljudnivåer.
        $scope.inactivity = false;
        $scope.defaultInputLevel = 0;
        $scope.inactivityTimeout = 15; // Default, i minuter
        $scope.playMessageButtonDisabled = true;

        $scope.optionsDataHubCodec = { reconnectAttempts: 0, socketStatus: false };
        var codecStatusHub = backendHubProxy("/signalr", 'codecStatusHub');

        codecStatusHub.on('codecStatus', function (codecStatus) {
            console.log("Received codec status", codecStatus);
            if (angular.isDefined(codecStatus) && codecStatus.SipAddress.toLowerCase() === $scope.sipAddress.toLowerCase()) {
                console.log("Received codec status for my codec", codecStatus);
                $scope.onCodecStatusUpdate(codecStatus);
            }
        });

        codecStatusHub.start();

        $scope.init = function(model) {
            console.log("* CodecControl Init", model);
            $scope.studioId = model.StudioId;
            $scope.sipAddress = model.CodecSipAddress;
            $scope.nrOfGpos = model.NrOfGpos;
            $scope.nrOfMicrophones = model.NrOfAudioInputs;
            $scope.infoText = model.InfoText;
            $scope.moreInfoUrl = makeUrlAbsolute(model.MoreInfoUrl);
            $scope.playAudioUrl = $sce.trustAsResourceUrl(model.CameraPlayAudioUrl);
            $scope.defaultInputLevel = model.AudioInputDefaultGain;
            $scope.inactivityTimeout = Math.max(model.InactivityTimeout, 1); // 1 eller mer

            // If no camera url is preset, hide Camera in GUI
            if (model.CameraVideoUrl === '' || typeof model.CameraVideoUrl === 'undefined') {
                $scope.cameraExists = false;
            } else {
                $scope.cameraUrl = model.CameraVideoUrl;
                $scope.cameraStillImageUrl = model.CameraImageUrl;

                if ($scope.isInternetExplorer()) {
                    // IE stödjer inte motion jpeg. använd stillbilds-url
                    $scope.cameraUrl = $scope.CameraImageUrl;
                    $scope.startWebCam();
                }
            }

            $scope.audioClipNames = (model.AudioClipNames || "").split(",");

            var microphoneLabels = (model.AudioInputNames || "").split(",");
            var inputs = [];
            for (var i = 0; i < $scope.nrOfMicrophones; i++) {

                if (typeof microphoneLabels[i] === 'undefined') {
                    microphoneLabels.push('IN ' + (i + 1));
                }
                // TODO: Hämta max och min-nivå från CCM
                var input = {
                    id: i,
                    label: microphoneLabels[i].trim(),
                    value: 0,
                    enabled: false,
                    maxDb: 10,
                    minDb: -42
                };
                inputs.push(input);
            }
            $scope.inputs = inputs;

            var gpoLables = (model.GpoNames || "").split(",");
            var gpos = [];
            for (var j = 0; j < $scope.nrOfGpos; j++) {

                if (typeof gpoLables[j] === 'undefined') {
                    gpoLables.push('GPO ' + (j + 1));
                }

                var gpo = { number: j, name: gpoLables[j].trim(), active: false };
                gpos.push(gpo);
            }
            $scope.gpos = gpos;
            console.log($scope.gpos);

            $scope.checkCodecIsOnline().then(function(isOnline) {
                $scope.codecOnline = isOnline;

                if (isOnline) {
                    $scope.updateAudioStatus();
                    $scope.checkCodecStatus();
                } else {
                    $scope.resetInactivityTimer();
                }
            });

        };

        $scope.onCodecStatusUpdate = function (codecStatus) {
            var isInCall = codecStatus.State === 2;
            $scope.isInCall = isInCall;
            $scope.connectedTo = (codecStatus.ConnectedToPresentationName || codecStatus.ConnectedToSipAddress);

            if (isInCall) {
                $scope.stopInactivityTimer();
            } else {
                $scope.resetInactivityTimer();
            }
        };

        $scope.checkCodecIsOnline = function () {
            console.info('Kontrollerar om kodaren är tillgänglig');
            return $http.get($scope.codecControlHost + '/api/codeccontrol/isavailable?sipaddress=' + $scope.sipAddress)
                .then(
                    function (response) {
                        let data = response.data;
                        console.debug('Codec available: ', data);
                        let isAvailable = data.isAvailable;
                        return isAvailable;
                    },
                    function () {
                        console.error('Codec is not available');
                        return false;
                    });
        };

        $scope.checkCodecStatus = function () {
            console.info('Check if codec is in call');
            $http.get('/api/studiomonitorapi/GetCodecStatus?studioId=' + $scope.studioId)
                .then(
                    function (response) {
                        var codecStatus = response.data;
                        console.info('Kodarstatus:', codecStatus);
                        $scope.onCodecStatusUpdate(codecStatus);
                    },
                    function (response) {
                        console.error('Could not get codec status: ', response.data);
                    });
        };

        $scope.resetInactivityTimer = function () {
            $scope.stopInactivityTimer();
            console.log("resetInactivityTimer");
            inactivityTimeoutHandle = $timeout(function () {
                inactivityTimeoutHandle = undefined;
                $scope.onInactivity();
            }, $scope.inactivityTimeout * 60000);
        };

        $scope.stopInactivityTimer = function () {
            console.log("stopInactivityTimer");
            if (angular.isDefined(inactivityTimeoutHandle)) {
                $timeout.cancel(inactivityTimeoutHandle);
            }
        };

        $scope.onInactivity = function () { // When inactivity is detected
            console.log("onInactivity");
            $scope.stopCheckInputValues();
            $scope.stopCheckVuValues();
            $scope.inactivity = true;

            // Cancel web camera stream by navigate img to still image
            $scope.cameraUrl = $scope.cameraStillImageUrl;
            $scope.stopWebCam();
        };

        $scope.setGpo = function (gpo) {
            var active = gpo.active ? false : true;
            $scope.httpPost('/api/codeccontrol/setgpo', { sipaddress: $scope.sipAddress, number: gpo.number, active: active })
                .then(function (data) {
                    console.log("Codec GPO data: ", data);
                    gpo.active = data.active;
                });
        };

        // Input values, reset mixer
        $scope.resetMixer = function() {
            console.info('* Codec Reset mixer snapshot');
            for (var i = 0; i < $scope.nrOfMicrophones; i++) {
                $scope.setGainLevel(i, $scope.defaultInputLevel);
                var on = (i === 0); // First mic on, other off
                $scope.setInputEnabled(i, on);
            }
        };

        $scope.increaseGainLevel = function(input) {
            var newValue = input.value + 1;
            $scope.setGainLevel(input.id, newValue);
        };

        $scope.decreaseGainLevel = function(input) {
            var newValue = input.value - 1;
            $scope.setGainLevel(input.id, newValue);
        };

        $scope.setGainLevel = function (inputNumber, newValue) {
            $scope.stopUpdateAudioStatus();
            console.info("* Codec SetGainLevel", inputNumber, newValue);
            $scope.httpPost('/api/codeccontrol/setinputgain', { sipAddress: $scope.sipAddress, input: inputNumber, level: newValue })
                .then(function (data) {
                    var input = $scope.inputs[inputNumber];
                    input.value = data.gainLevel;
                },
                function (error) {
                    // Do nothing
                    $scope.startCheckInputValues();
                });
        };

        $scope.getAudioStatus = function () {
            return $http.get($scope.codecControlHost + '/api/codeccontrol/getaudiostatus?sipaddress=' + $scope.sipAddress)
                .then(function (response) {
                    let audioStatus = response.data;
                    //console.log("* Codec GetAudioStatus: ", audioStatus);

                    for (var i = 0; i < audioStatus.inputStatus.length; i++) {
                        var inputData = audioStatus.inputStatus[i];
                        var input = $scope.inputs[i];
                        if (input) {
                            input.value = inputData.gainLevel;
                            input.enabled = inputData.enabled;
                        }
                    }

                    var hasActiveGpo = false;

                    for (var j = 0; j < audioStatus.gpos.length; j++) {
                        var gpoData = audioStatus.gpos[j];
                        var gpo = $scope.gpos[gpoData.index];
                        if (gpo) {
                            gpo.active = gpoData.active;
                            hasActiveGpo = hasActiveGpo || gpo.active;
                        }
                    }

                    $scope.playMessageButtonDisabled = hasActiveGpo; // Disable play button when any GPO is active

                    var vuData = response.data.vuValues;
                    //console.info("* got VU-data: ", vuData);

                    // Värdena är i db där 0 = Fullscale +18db, -18 = Test 0dB, -96 = min-nivå
                    $scope.txL = fallback($scope.txL, convertVuToPercentage(vuData.txLeft));
                    $scope.txR = fallback($scope.txR, convertVuToPercentage(vuData.txRight));
                    $scope.rxL = fallback($scope.rxL, convertVuToPercentage(vuData.rxLeft));
                    $scope.rxR = fallback($scope.rxR, convertVuToPercentage(vuData.rxRight));
                });
        };

        $scope.toggleInput = function (input) {
            var enabled = !input.enabled;
            $scope.setInputEnabled(input.id, enabled);
        };

        $scope.setInputEnabled = function (inputNumber, enabled) {
            $scope.httpPost('/api/codeccontrol/setinputenabled', { sipAddress: $scope.sipAddress, input: inputNumber, enabled: enabled })
                .then(function (data) {
                    //console.log("SetInputEnabled", data);
                    $scope.inputs[inputNumber].enabled = data.enabled;
                },
                function (response) {
                    console.error('Codec SetInputEnabled failed');
                });
        };

        $scope.updateAudioStatus = function () {
            $scope.getAudioStatus().then(function () {
                if (!angular.isDefined(audioStatusTimeoutHandle)) {
                    audioStatusTimeoutHandle = $timeout(function () {
                        audioStatusTimeoutHandle = undefined;
                        if (!$scope.inactivity) {
                            $scope.updateAudioStatus();
                        }
                    }, audioStatusUpdateInterval);
                }
            });
        };

        $scope.stopUpdateAudioStatus = function () {
            if (angular.isDefined(audioStatusTimeoutHandle)) {
                $timeout.cancel(audioStatusTimeoutHandle);
                audioStatusTimeoutHandle = undefined;
            }
        };

        // Web cam
        $scope.startWebCam = function () {
            if (angular.isDefined(webCamIntervalHandle)) return;

            webCamIntervalHandle = $interval(function () {
                var newUrl = $scope.cameraStillImageUrl + "&timestamp=" + new Date().valueOf();
                console.log("Uppdaterar bild-url", newUrl);
                $scope.cameraUrl = newUrl;
            }, webCamUpdateInterval);
        };

        $scope.stopWebCam = function () {
            if (angular.isDefined(webCamIntervalHandle)) {
                $interval.cancel(webCamIntervalHandle);
                webCamIntervalHandle = undefined;
            }
        };

        // Other
        $scope.isInternetExplorer = function() {
            return (navigator.appName === 'Microsoft Internet Explorer' ||
                !!(navigator.userAgent.match(/Trident/) || navigator.userAgent.match(/rv:11/)) ||
                (typeof $.browser !== "undefined" && $.browser.msie === 1));
        };

        $scope.$on("$destroy",
            function () {
                console.log("Destroy called");
                $scope.stopUpdateAudioStatus();
                codecStatusHub.stop();
            });

        $scope.httpPost = function (apiPath, data) {
            var authorizationBasic = $window.btoa($scope.codecControlUserName + ':' + $scope.codecControlPassword);
            const headers = { 'Authorization': 'Basic ' + authorizationBasic };

            return $http.post($scope.codecControlHost + apiPath, data, { headers: headers }).then(function (response) {
                return response.data;
            });
        };

    });