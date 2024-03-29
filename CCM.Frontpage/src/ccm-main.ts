import Tool from './utils/Tools';

export class Application {
    constructor() {
        console.log("Initiated application");

        // try {
        //     // Mobile sized menu system
        //     Tool.$eventByClass("navbar-toggle", "click", (e) => {
        //         console.log("Pressed: ", e);
        //         const elements = document.getElementsByClassName("ccm-top-navbar-container");
        //         for(let i = 0; i < elements.length; i++) {
        //             elements[i].classList.toggle("ccm-top-navbar-container--expanded");
        //         }
        //     });
        // } catch(error) {
        //     console.error(error);
        // }

        try {
            // Dropdowns and accordions
            let accordions = document.querySelectorAll("[data-toggle]");
            accordions.forEach((item) => {
                if ((item as any).dataset.toggle == "dropdown") {
                    Tool.$event(item, "click", (e) => {
                        e.preventDefault();
                        e.stopPropagation();
                        if (e.target.parentNode) {
                            e.target.parentNode.classList.toggle("open");
                        }
                    });
                } else {
                    Tool.$event(item, "click", (eve) => {
                        eve.preventDefault();
                        eve.stopPropagation();
                        if ((item as any).getAttribute("href")) {
                            const col = (item as any).href;
                            const idd = col.substr(col.indexOf("#") + 1, col.lenght);
                            if (idd) {
                                Tool.$dom(idd).classList.toggle("open");
                                eve.target.classList.toggle("open");
                            }
                        }
                    });
                }
            });
        } catch(error) {
            console.error(error);
        }

        this.setupStartpage();

        this.setupMenu();

        this.setupTabs();

        (function() {
            const searchbar = Tool.$dom("searchField");
            if (searchbar) {
                searchbar.focus();
            }
        })();
    }

    private setupStartpage() {
        try {
            Tool.$event("tab-region-btn", "click", (eve) => {
                Tool.$dom("tab-codectype-filter").style.display = "none";
                Tool.$dom("tab-category-filter").style.display = "none";
                Tool.$dom("tab-region-filter").style.display = "block";

                Tool.$dom("tab-region-btn").classList.add("active");
                Tool.$dom("tab-category-btn").classList.remove("active");
                Tool.$dom("tab-codectype-btn").classList.remove("active");
            });

            Tool.$event("tab-codectype-btn", "click", (eve) => {
                Tool.$dom("tab-codectype-filter").style.display = "block";
                Tool.$dom("tab-category-filter").style.display = "none";
                Tool.$dom("tab-region-filter").style.display = "none";

                Tool.$dom("tab-region-btn").classList.remove("active");
                Tool.$dom("tab-category-btn").classList.remove("active");
                Tool.$dom("tab-codectype-btn").classList.add("active");
            });

            Tool.$event("tab-category-btn", "click", (eve) => {
                Tool.$dom("tab-codectype-filter").style.display = "none";
                Tool.$dom("tab-region-filter").style.display = "none";
                Tool.$dom("tab-category-filter").style.display = "block";

                Tool.$dom("tab-codectype-btn").classList.remove("active");
                Tool.$dom("tab-region-btn").classList.remove("active");
                Tool.$dom("tab-category-btn").classList.add("active");
            });

        } catch(error) {
            console.error(error)
        }
    }

    private setupMenu() {

        function collapseSection(element) {
            // get the height of the element's inner content, regardless of its actual size
            const sectionHeight = element.scrollHeight;

            // temporarily disable all css transitions
            const elementTransition = element.style.transition;
            element.style.transition = "";

            // on the next frame (as soon as the previous style change has taken effect),
            // explicitly set the element's height to its current pixel height, so we
            // aren't transitioning out of 'auto'
            requestAnimationFrame(function() {
                element.style.height = sectionHeight + "px";
                element.style.transition = elementTransition;

                // on the next frame (as soon as the previous style change has taken effect),
                // have the element transition to height: 0
                requestAnimationFrame(function() {
                    element.style.height = 0 + "px";
                });
            });

            // mark the section as "currently collapsed"
            element.setAttribute("data-collapsed", "true");

            window.removeEventListener("scroll", clickOutsideMenu, false);
            Tool.$dom("admin-menu-cover").classList.remove("open");
        }

        function clickOutsideMenu(ev: any) {
            const section = document.querySelector(".section.collapsible");
            collapseSection(section);
        }

        function expandSection(element) {
            // get the height of the element's inner content, regardless of its actual size
            const sectionHeight = element.scrollHeight + 80;

            // have the element transition to the height of its inner content
            element.style.height = sectionHeight + "px";

            // when the next css transition finishes (which should be the one we just triggered)
            element.addEventListener("transitionend", function(e) {
                // remove this event listener so it only gets triggered once
                console.log(arguments[0])
                element.removeEventListener("transitionend", arguments[0].callee);

                // remove "height" from the element's inline styles, so it can return to its initial value
                //element.style.height = null;
            });

            // mark the section as "currently not collapsed"
            element.setAttribute("data-collapsed", "false");

            // determine if the menu is filling the whole screen then disable scroll
            if (window.innerHeight >= sectionHeight) {
                window.addEventListener("scroll", clickOutsideMenu, false);
            }

            Tool.$dom("admin-menu-cover").classList.add("open");
        }

        function clickOnAdminMenu(e) {
            const section = document.querySelector(".section.collapsible");
            const isCollapsed = section.getAttribute("data-collapsed") === "true";

            if (isCollapsed) {
                expandSection(section)
                section.setAttribute("data-collapsed", "false")
            } else {
                collapseSection(section)
            }
        }

        let collapsableSection = document.querySelector(".section.collapsible");
        if (collapsableSection) {
            collapsableSection.setAttribute("data-collapsed", "true");
        }

        let navigationAdminBtn = document.querySelector("#admin-navigation-btn");
        if (navigationAdminBtn) {
            //navigationAdminBtn.addEventListener("click", clickOnAdminMenu);
            Tool.$event(navigationAdminBtn, "click", clickOnAdminMenu);
        }

        Tool.$event("admin-menu-cover", "click", clickOutsideMenu);
    }

    private setupTabs() {
        const tabs = Array.from(document.querySelectorAll('[data-target-tab]'));
        if (!tabs || tabs.length === 0) {
            console.warn("No tabs found on this page");
            return;
        }

        tabs.forEach((tab) => {
            Tool.$event(tab, "click", function(el) {
                try {
                    const tabId = this.dataset.targetTab;
                    if (!tabId) {
                        console.warn(`No target tab could be found to display for '${tabId}'`);
                        return;
                    }

                    const parent = Array.from(this.parentNode.parentNode.children);
                    if (!parent) {
                        console.warn("Could not find parent tab pane");
                        return;
                    }

                    parent.forEach((lm) => {
                        const li = (lm as any).querySelector("a").dataset.targetTab;
                        if (!li) {
                            console.warn("No parent li element");
                            return;
                        }
                        if (li === tabId) {
                            (lm as any).classList.add("active");
                            Tool.$dom(li).classList.add("active");
                        } else {
                            (lm as any).classList.remove("active");
                            Tool.$dom(li).classList.remove("active");
                        }
                    });
                } catch(err) {
                    console.error(err)
                }
            });
        });
    }
}

export default class Main {
    public static load() {
        console.log("Initiated main");
        const app = new Application();
    }
}

Main.load();
