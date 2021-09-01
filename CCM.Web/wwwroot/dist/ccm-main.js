/******/ (function(modules) { // webpackBootstrap
/******/ 	// The module cache
/******/ 	var installedModules = {};
/******/
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/
/******/ 		// Check if module is in cache
/******/ 		if(installedModules[moduleId]) {
/******/ 			return installedModules[moduleId].exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = installedModules[moduleId] = {
/******/ 			i: moduleId,
/******/ 			l: false,
/******/ 			exports: {}
/******/ 		};
/******/
/******/ 		// Execute the module function
/******/ 		modules[moduleId].call(module.exports, module, module.exports, __webpack_require__);
/******/
/******/ 		// Flag the module as loaded
/******/ 		module.l = true;
/******/
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/
/******/
/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = modules;
/******/
/******/ 	// expose the module cache
/******/ 	__webpack_require__.c = installedModules;
/******/
/******/ 	// define getter function for harmony exports
/******/ 	__webpack_require__.d = function(exports, name, getter) {
/******/ 		if(!__webpack_require__.o(exports, name)) {
/******/ 			Object.defineProperty(exports, name, { enumerable: true, get: getter });
/******/ 		}
/******/ 	};
/******/
/******/ 	// define __esModule on exports
/******/ 	__webpack_require__.r = function(exports) {
/******/ 		if(typeof Symbol !== 'undefined' && Symbol.toStringTag) {
/******/ 			Object.defineProperty(exports, Symbol.toStringTag, { value: 'Module' });
/******/ 		}
/******/ 		Object.defineProperty(exports, '__esModule', { value: true });
/******/ 	};
/******/
/******/ 	// create a fake namespace object
/******/ 	// mode & 1: value is a module id, require it
/******/ 	// mode & 2: merge all properties of value into the ns
/******/ 	// mode & 4: return value when already ns object
/******/ 	// mode & 8|1: behave like require
/******/ 	__webpack_require__.t = function(value, mode) {
/******/ 		if(mode & 1) value = __webpack_require__(value);
/******/ 		if(mode & 8) return value;
/******/ 		if((mode & 4) && typeof value === 'object' && value && value.__esModule) return value;
/******/ 		var ns = Object.create(null);
/******/ 		__webpack_require__.r(ns);
/******/ 		Object.defineProperty(ns, 'default', { enumerable: true, value: value });
/******/ 		if(mode & 2 && typeof value != 'string') for(var key in value) __webpack_require__.d(ns, key, function(key) { return value[key]; }.bind(null, key));
/******/ 		return ns;
/******/ 	};
/******/
/******/ 	// getDefaultExport function for compatibility with non-harmony modules
/******/ 	__webpack_require__.n = function(module) {
/******/ 		var getter = module && module.__esModule ?
/******/ 			function getDefault() { return module['default']; } :
/******/ 			function getModuleExports() { return module; };
/******/ 		__webpack_require__.d(getter, 'a', getter);
/******/ 		return getter;
/******/ 	};
/******/
/******/ 	// Object.prototype.hasOwnProperty.call
/******/ 	__webpack_require__.o = function(object, property) { return Object.prototype.hasOwnProperty.call(object, property); };
/******/
/******/ 	// __webpack_public_path__
/******/ 	__webpack_require__.p = "/";
/******/
/******/
/******/ 	// Load entry module and return exports
/******/ 	return __webpack_require__(__webpack_require__.s = "./src/ccm-main.ts");
/******/ })
/************************************************************************/
/******/ ({

/***/ "./src/ccm-main.ts":
/*!*************************!*\
  !*** ./src/ccm-main.ts ***!
  \*************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
eval("\r\nObject.defineProperty(exports, \"__esModule\", { value: true });\r\nvar Tools_1 = __webpack_require__(/*! ./utils/Tools */ \"./src/utils/Tools.ts\");\r\nvar Application = /** @class */ (function () {\r\n    function Application() {\r\n        console.log(\"Initiated application\");\r\n        // try {\r\n        //     // Mobile sized menu system\r\n        //     Tool.$eventByClass(\"navbar-toggle\", \"click\", (e) => {\r\n        //         console.log(\"Pressed: \", e);\r\n        //         const elements = document.getElementsByClassName(\"ccm-top-navbar-container\");\r\n        //         for(let i = 0; i < elements.length; i++) {\r\n        //             elements[i].classList.toggle(\"ccm-top-navbar-container--expanded\");\r\n        //         }\r\n        //     });\r\n        // } catch(error) {\r\n        //     console.error(error);\r\n        // }\r\n        try {\r\n            // Dropdowns and accordions\r\n            var accordions = document.querySelectorAll(\"[data-toggle]\");\r\n            accordions.forEach(function (item) {\r\n                if (item.dataset.toggle == \"dropdown\") {\r\n                    Tools_1.default.$event(item, \"click\", function (e) {\r\n                        e.preventDefault();\r\n                        e.stopPropagation();\r\n                        if (e.target.parentNode) {\r\n                            e.target.parentNode.classList.toggle(\"open\");\r\n                        }\r\n                    });\r\n                }\r\n                else {\r\n                    Tools_1.default.$event(item, \"click\", function (eve) {\r\n                        eve.preventDefault();\r\n                        eve.stopPropagation();\r\n                        if (item.getAttribute(\"href\")) {\r\n                            var col = item.href;\r\n                            var idd = col.substr(col.indexOf(\"#\") + 1, col.lenght);\r\n                            if (idd) {\r\n                                Tools_1.default.$dom(idd).classList.toggle(\"open\");\r\n                            }\r\n                        }\r\n                    });\r\n                }\r\n            });\r\n        }\r\n        catch (error) {\r\n            console.error(error);\r\n        }\r\n        this.setupStartpage();\r\n        this.setupMenu();\r\n        this.setupTabs();\r\n        (function () {\r\n            var searchbar = Tools_1.default.$dom(\"searchField\");\r\n            if (searchbar) {\r\n                searchbar.focus();\r\n            }\r\n        })();\r\n    }\r\n    Application.prototype.setupStartpage = function () {\r\n        try {\r\n            Tools_1.default.$event(\"tab-region-btn\", \"click\", function (eve) {\r\n                Tools_1.default.$dom(\"tab-codectype-filter\").style.display = \"none\";\r\n                Tools_1.default.$dom(\"tab-category-filter\").style.display = \"none\";\r\n                Tools_1.default.$dom(\"tab-region-filter\").style.display = \"block\";\r\n                Tools_1.default.$dom(\"tab-region-btn\").classList.add(\"active\");\r\n                Tools_1.default.$dom(\"tab-category-btn\").classList.remove(\"active\");\r\n                Tools_1.default.$dom(\"tab-codectype-btn\").classList.remove(\"active\");\r\n            });\r\n            Tools_1.default.$event(\"tab-codectype-btn\", \"click\", function (eve) {\r\n                Tools_1.default.$dom(\"tab-codectype-filter\").style.display = \"block\";\r\n                Tools_1.default.$dom(\"tab-category-filter\").style.display = \"none\";\r\n                Tools_1.default.$dom(\"tab-region-filter\").style.display = \"none\";\r\n                Tools_1.default.$dom(\"tab-region-btn\").classList.remove(\"active\");\r\n                Tools_1.default.$dom(\"tab-category-btn\").classList.remove(\"active\");\r\n                Tools_1.default.$dom(\"tab-codectype-btn\").classList.add(\"active\");\r\n            });\r\n            Tools_1.default.$event(\"tab-category-btn\", \"click\", function (eve) {\r\n                Tools_1.default.$dom(\"tab-codectype-filter\").style.display = \"none\";\r\n                Tools_1.default.$dom(\"tab-region-filter\").style.display = \"none\";\r\n                Tools_1.default.$dom(\"tab-category-filter\").style.display = \"block\";\r\n                Tools_1.default.$dom(\"tab-codectype-btn\").classList.remove(\"active\");\r\n                Tools_1.default.$dom(\"tab-region-btn\").classList.remove(\"active\");\r\n                Tools_1.default.$dom(\"tab-category-btn\").classList.add(\"active\");\r\n            });\r\n        }\r\n        catch (error) {\r\n            console.error(error);\r\n        }\r\n    };\r\n    Application.prototype.setupMenu = function () {\r\n        function collapseSection(element) {\r\n            // get the height of the element's inner content, regardless of its actual size\r\n            var sectionHeight = element.scrollHeight;\r\n            // temporarily disable all css transitions\r\n            var elementTransition = element.style.transition;\r\n            element.style.transition = \"\";\r\n            // on the next frame (as soon as the previous style change has taken effect),\r\n            // explicitly set the element's height to its current pixel height, so we\r\n            // aren't transitioning out of 'auto'\r\n            requestAnimationFrame(function () {\r\n                element.style.height = sectionHeight + \"px\";\r\n                element.style.transition = elementTransition;\r\n                // on the next frame (as soon as the previous style change has taken effect),\r\n                // have the element transition to height: 0\r\n                requestAnimationFrame(function () {\r\n                    element.style.height = 0 + \"px\";\r\n                });\r\n            });\r\n            // mark the section as \"currently collapsed\"\r\n            element.setAttribute(\"data-collapsed\", \"true\");\r\n            window.removeEventListener(\"scroll\", clickOutsideMenu, false);\r\n            Tools_1.default.$dom(\"admin-menu-cover\").classList.remove(\"open\");\r\n        }\r\n        function clickOutsideMenu(ev) {\r\n            var section = document.querySelector(\".section.collapsible\");\r\n            collapseSection(section);\r\n        }\r\n        function expandSection(element) {\r\n            // get the height of the element's inner content, regardless of its actual size\r\n            var sectionHeight = element.scrollHeight + 80;\r\n            // have the element transition to the height of its inner content\r\n            element.style.height = sectionHeight + \"px\";\r\n            // when the next css transition finishes (which should be the one we just triggered)\r\n            element.addEventListener(\"transitionend\", function (e) {\r\n                // remove this event listener so it only gets triggered once\r\n                console.log(arguments[0]);\r\n                element.removeEventListener(\"transitionend\", arguments[0].callee);\r\n                // remove \"height\" from the element's inline styles, so it can return to its initial value\r\n                //element.style.height = null;\r\n            });\r\n            // mark the section as \"currently not collapsed\"\r\n            element.setAttribute(\"data-collapsed\", \"false\");\r\n            // determine if the menu is filling the whole screen then disable scroll\r\n            if (window.innerHeight >= sectionHeight) {\r\n                window.addEventListener(\"scroll\", clickOutsideMenu, false);\r\n            }\r\n            Tools_1.default.$dom(\"admin-menu-cover\").classList.add(\"open\");\r\n        }\r\n        var collapsableSection = document.querySelector(\".section.collapsible\");\r\n        if (collapsableSection) {\r\n            collapsableSection.setAttribute(\"data-collapsed\", \"true\");\r\n        }\r\n        var navigationAdminBtn = document.querySelector(\"#admin-navigation-btn\");\r\n        if (navigationAdminBtn) {\r\n            navigationAdminBtn.addEventListener(\"click\", function (e) {\r\n                var section = document.querySelector(\".section.collapsible\");\r\n                var isCollapsed = section.getAttribute(\"data-collapsed\") === \"true\";\r\n                if (isCollapsed) {\r\n                    expandSection(section);\r\n                    section.setAttribute(\"data-collapsed\", \"false\");\r\n                }\r\n                else {\r\n                    collapseSection(section);\r\n                }\r\n            });\r\n        }\r\n        Tools_1.default.$event(\"admin-menu-cover\", \"click\", clickOutsideMenu);\r\n    };\r\n    Application.prototype.setupTabs = function () {\r\n        var tabs = Array.from(document.querySelectorAll('[data-target-tab]'));\r\n        if (!tabs || tabs.length === 0) {\r\n            console.warn(\"No tabs found on this page\");\r\n            return;\r\n        }\r\n        tabs.forEach(function (tab) {\r\n            Tools_1.default.$event(tab, \"click\", function (el) {\r\n                try {\r\n                    var tabId_1 = this.dataset.targetTab;\r\n                    if (!tabId_1) {\r\n                        console.warn(\"No target tab could be found to display for '\" + tabId_1 + \"'\");\r\n                        return;\r\n                    }\r\n                    var parent_1 = Array.from(this.parentNode.parentNode.children);\r\n                    if (!parent_1) {\r\n                        console.warn(\"Could not find parent tab pane\");\r\n                        return;\r\n                    }\r\n                    parent_1.forEach(function (lm) {\r\n                        var li = lm.querySelector(\"a\").dataset.targetTab;\r\n                        if (!li) {\r\n                            console.warn(\"No parent li element\");\r\n                            return;\r\n                        }\r\n                        if (li === tabId_1) {\r\n                            lm.classList.add(\"active\");\r\n                            Tools_1.default.$dom(li).classList.add(\"active\");\r\n                        }\r\n                        else {\r\n                            lm.classList.remove(\"active\");\r\n                            Tools_1.default.$dom(li).classList.remove(\"active\");\r\n                        }\r\n                    });\r\n                }\r\n                catch (err) {\r\n                    console.error(err);\r\n                }\r\n            });\r\n        });\r\n    };\r\n    return Application;\r\n}());\r\nexports.Application = Application;\r\nvar Main = /** @class */ (function () {\r\n    function Main() {\r\n    }\r\n    Main.load = function () {\r\n        console.log(\"Initiated main\");\r\n        var app = new Application();\r\n    };\r\n    return Main;\r\n}());\r\nexports.default = Main;\r\nMain.load();\r\n\n\n//# sourceURL=webpack:///./src/ccm-main.ts?");

/***/ }),

/***/ "./src/utils/Tools.ts":
/*!****************************!*\
  !*** ./src/utils/Tools.ts ***!
  \****************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
eval("\r\nObject.defineProperty(exports, \"__esModule\", { value: true });\r\nvar Tools = /** @class */ (function () {\r\n    function Tools() {\r\n        this.elementNotAvailable = [];\r\n        this.elementsInAnimation = [];\r\n    }\r\n    Tools.GetUrlParameters = function (name, url) {\r\n        if (!url) {\r\n            url = window.location.href;\r\n        }\r\n        name = name.replace(/[\\[\\]]/g, '\\\\$&');\r\n        var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)');\r\n        var results = regex.exec(url);\r\n        if (!results) {\r\n            return '';\r\n        }\r\n        if (!results[2]) {\r\n            return '';\r\n        }\r\n        return decodeURIComponent(results[2].replace(/\\+/g, ' '));\r\n    };\r\n    Tools.$event = function (elementName, evnt, funct) {\r\n        try {\r\n            var element = elementName;\r\n            if (typeof elementName == \"string\") {\r\n                element = this.$dom(elementName);\r\n            }\r\n            if (element.attachEvent) {\r\n                return element.attachEvent('on' + evnt, funct);\r\n            }\r\n            else {\r\n                return element.addEventListener(evnt, funct, false);\r\n            }\r\n        }\r\n        catch (error) {\r\n            console.warn(\"Could not attach event: \" + error + \" for element \" + elementName);\r\n            return document.createElement(\"div\");\r\n        }\r\n    };\r\n    Tools.$eventByClass = function (elementClass, evnt, funct) {\r\n        try {\r\n            var elements = document.getElementsByClassName(elementClass);\r\n            for (var index = 0; index < elements.length; index++) {\r\n                if (elements[index].attachEvent) {\r\n                    return elements[index].attachEvent('on' + evnt, funct);\r\n                }\r\n                else {\r\n                    return elements[index].addEventListener(evnt, funct, false);\r\n                }\r\n            }\r\n        }\r\n        catch (error) {\r\n            console.warn(\"Could not attach event: \" + error + \" for elements with class \" + elementClass);\r\n            return document.createElement(\"div\");\r\n        }\r\n    };\r\n    Tools.$fetchView = function (url, parameters) {\r\n        return fetch(url + \"?\" + new URLSearchParams(parameters).toString(), {\r\n            method: \"POST\",\r\n            body: JSON.stringify(parameters)\r\n        })\r\n            .then(function (response) {\r\n            if (response.ok) {\r\n                return response.text();\r\n            }\r\n            else {\r\n                throw new Error(response.status + \" - \" + response.statusText);\r\n            }\r\n        });\r\n    };\r\n    Tools.$attr = function (key, value, html) {\r\n        if (html === void 0) { html = false; }\r\n        // Updates every element with the 'data-key' attribute\r\n        html = html || false;\r\n        try {\r\n            var dataset = document.querySelectorAll(\"[data-key]\");\r\n            if (html) {\r\n                dataset.forEach(function (item) {\r\n                    if (item.dataset.key === key) {\r\n                        window.requestAnimationFrame(function () {\r\n                            item.innerHTML = value;\r\n                        });\r\n                    }\r\n                });\r\n            }\r\n            else {\r\n                dataset.forEach(function (item) {\r\n                    if (item.dataset.key === key) {\r\n                        window.requestAnimationFrame(function () {\r\n                            item.textContent = value;\r\n                        });\r\n                    }\r\n                });\r\n            }\r\n            return true;\r\n        }\r\n        catch (e) {\r\n            console.error(e);\r\n            return false;\r\n        }\r\n    };\r\n    Tools.$dom = function (id) {\r\n        // if (this.elementNotAvailable.includes(id)) {\r\n        //     return null;\r\n        // }\r\n        if (id !== null && id !== \"\" && id !== undefined) {\r\n            try {\r\n                if (document.getElementById(id) !== null) {\r\n                    return document.getElementById(id);\r\n                }\r\n                else {\r\n                    console.warn(\"Can't access element with id: \" + id);\r\n                    // this.elementNotAvailable.push(id);\r\n                    return null;\r\n                }\r\n            }\r\n            catch (e) {\r\n                console.warn(\"Can't access element with id: \" + id);\r\n                // this.elementNotAvailable.push(id);\r\n                return null;\r\n            }\r\n        }\r\n        else {\r\n            console.warn(\"Can't access element with id: \" + id);\r\n            // this.elementNotAvailable.push(id);\r\n            return null;\r\n        }\r\n    };\r\n    return Tools;\r\n}());\r\nexports.default = Tools;\r\n\n\n//# sourceURL=webpack:///./src/utils/Tools.ts?");

/***/ })

/******/ });