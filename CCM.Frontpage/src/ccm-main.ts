import StatisticsView from './components/statistics';
import Tool from './utils/Tools';

export class Application {
    constructor() {
        console.log("Initiated application");

        try {
            // Mobile sized menu system
            Tool.$eventByClass("navbar-toggle", "click", (e) => {
                console.log("Pressed: ", e);
                const elements = document.getElementsByClassName("ccm-top-navbar-container");
                for(let i = 0; i < elements.length; i++) {
                    elements[i].classList.toggle("ccm-top-navbar-container--expanded");
                }
            });
        } catch(error) {
            console.error(error);
        }

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

        this.setupFrontpageCols();

        this.setupStatistics();
    }

    private setupFrontpageCols() {

        const min = 150;
        // The max (fr) values for grid-template-columns
        const columnTypeToRatioMap = {
            numeric: 1,
            'text-short': 1.67,
            'text-long': 3.33,
        };

        const table = document.querySelector('table');
        /*
            The following will soon be filled with column objects containing
            the header element and their size value for grid-template-columns
        */
        const columns = [];
        let headerBeingResized;

        // The next three functions are mouse event callbacks

        // Where the magic happens. I.e. when they're actually resizing
        const onMouseMove = (e) => requestAnimationFrame(() => {
            console.log('onMouseMove');

            // Calculate the desired width
            let horizontalScrollOffset = document.documentElement.scrollLeft;
            const width = (horizontalScrollOffset + e.clientX) - headerBeingResized.offsetLeft;

            // Update the column object with the new size value
            const column = columns.find(({ header }) => header === headerBeingResized);
            column.size = Math.max(min, width) + 'px'; // Enforce our minimum

            // For the other headers which don't have a set width, fix it to their computed width
            columns.forEach((column) => {
                if (column.size.startsWith('minmax')) { // isn't fixed yet (it would be a pixel value otherwise)
                    column.size = parseInt(column.header.clientWidth, 10) + 'px';
                }
            });

            /*
                Update the column sizes
                Reminder: grid-template-columns sets the width for all columns in one value
            */
            table.style.gridTemplateColumns = columns
                .map(({ header, size }) => size)
                .join(' ');
        });

        // Clean up event listeners, classes, etc.
        const onMouseUp = () => {
            console.log('onMouseUp');

            window.removeEventListener('mousemove', onMouseMove);
            window.removeEventListener('mouseup', onMouseUp);
            headerBeingResized.classList.remove('header--being-resized');
            headerBeingResized = null;
        };

        // Get ready, they're about to resize
        const initResize = ({ target }) => {
            console.log('initResize');

            headerBeingResized = target.parentNode;
            window.addEventListener('mousemove', onMouseMove);
            window.addEventListener('mouseup', onMouseUp);
            headerBeingResized.classList.add('header--being-resized');
        };

        // Let's populate that columns array and add listeners to the resize handles
        document.querySelectorAll('th').forEach((header) => {
            const max = columnTypeToRatioMap[header.dataset.type] + 'fr';
            columns.push({
                header,
                // The initial size value for grid-template-columns:
                size: `minmax(${min}px, ${max})`,
            });
            header.querySelector('.resize-handle').addEventListener('mousedown', initResize);
        });
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

            Tool.$event("tab-ongoing-btn", "click", (eve) => {
                Tool.$dom("tab-ongoing-content").style.display = "block";
                Tool.$dom("tab-registered-content").style.display = "none";

                Tool.$dom("tab-ongoing-btn").classList.add("active");
                Tool.$dom("tab-registered-btn").classList.remove("active");
            });

            Tool.$event("tab-registered-btn", "click", (eve) => {
                Tool.$dom("tab-ongoing-content").style.display = "none";
                Tool.$dom("tab-registered-content").style.display = "block";

                Tool.$dom("tab-ongoing-btn").classList.remove("active");
                Tool.$dom("tab-registered-btn").classList.add("active");
            });

        } catch(error) {
            console.error(error)
        }
    }

    private setupMenu() {

        function collapseSection(element) {
            // get the height of the element's inner content, regardless of its actual size
            var sectionHeight = element.scrollHeight;

            // temporarily disable all css transitions
            var elementTransition = element.style.transition;
            element.style.transition = '';

            // on the next frame (as soon as the previous style change has taken effect),
            // explicitly set the element's height to its current pixel height, so we
            // aren't transitioning out of 'auto'
            requestAnimationFrame(function() {
                element.style.height = sectionHeight + 'px';
                element.style.transition = elementTransition;

                // on the next frame (as soon as the previous style change has taken effect),
                // have the element transition to height: 0
                requestAnimationFrame(function() {
                    element.style.height = 0 + 'px';
                });
            });

            // mark the section as "currently collapsed"
            element.setAttribute('data-collapsed', 'true');

            window.removeEventListener("scroll", clickOutsideMenu, false);
            Tool.$dom("admin-menu-cover").classList.remove("open");
        }

        function clickOutsideMenu(ev: any) {
            const section = document.querySelector('.section.collapsible');
            collapseSection(section);
        }

        function expandSection(element) {
            // get the height of the element's inner content, regardless of its actual size
            var sectionHeight = element.scrollHeight + 80;

            // have the element transition to the height of its inner content
            element.style.height = sectionHeight + 'px';

            // when the next css transition finishes (which should be the one we just triggered)
            element.addEventListener('transitionend', function(e) {
                // remove this event listener so it only gets triggered once
                console.log(arguments[0])
                element.removeEventListener('transitionend', arguments[0].callee);

                // remove "height" from the element's inline styles, so it can return to its initial value
                //element.style.height = null;
            });

            // mark the section as "currently not collapsed"
            element.setAttribute('data-collapsed', 'false');

            window.addEventListener("scroll", clickOutsideMenu, false);
            Tool.$dom("admin-menu-cover").classList.add("open");
        }

        let collapsableSection = document.querySelector('.section.collapsible');
        if (collapsableSection) {
            collapsableSection.setAttribute('data-collapsed', 'true');
        }

        let navigationAdminBtn = document.querySelector('#admin-navigation-btn');
        if (navigationAdminBtn) {
            navigationAdminBtn.addEventListener('click', function(e) {
                var section = document.querySelector('.section.collapsible');
                var isCollapsed = section.getAttribute('data-collapsed') === 'true';

                if (isCollapsed) {
                    expandSection(section)
                    section.setAttribute('data-collapsed', 'false')
                } else {
                    collapseSection(section)
                }
            });
        }

        Tool.$event("admin-menu-cover", "click", clickOutsideMenu);
    }

    private setupStatistics() {
        const statisticsView = new StatisticsView();
    }
}

export default class Main {
    public static load() {
        console.log("Initiated main");
        const app = new Application();
    }
}

Main.load();
