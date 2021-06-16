/**
 * Set up drag and sortable on a UL HTML list. Add the UL element class.
 *
 * @example Sortable("sortlist", (items) => { });
 *
 * @param {string} - List HTMLElement class
 * @param {Function} - Callback
 * @returns {void} - Listens to nothing, use callback
 */
export function Sortable(targetClass: string, callback: Function) {
    const target = document.getElementsByClassName(targetClass);
    if (target == null || target.length === 0) {
        throw new Error(`Could not bind sortable to target class '.${targetClass}'`);
    }

    for (var index = 0; index < target.length; index++) {
        console.log({targ: target[index]});

        target[index].classList.add("sortable");
        const items = target[index].getElementsByTagName("li");
        if (!items) {
            return;
        }

        var current: HTMLElement;
        callback(items);
        for (let i of items as any) {
            i.draggable = true;

            i.addEventListener("dragstart", function handleStart(this: any, ev: any) {
                current = this;
                for (let it of items as any) {
                    if (it != current) {
                        it.classList.add("hint");
                    }
                }
                current.classList.add("current");

                // Add this element's id to the drag payload so the drop handler will
                // know which element to add to its tree
                console.log("dragStart: dropEffect = " + ev.dataTransfer.dropEffect + " ; effectAllowed = " + ev.dataTransfer.effectAllowed);
                ev.dataTransfer.setData("text", ev.target.id);
                ev.dataTransfer.effectAllowed = "move";
            });

            i.addEventListener("dragenter", function handleEnter(this: any, ev: any) {
                if (this != current) {
                    this.classList.add("active");
                }
            });

            i.addEventListener("dragleave", function handleLeave(this: any, ev: any) {
                this.classList.remove("active");
            });

            i.addEventListener("dragend", function handleEnd() {
                for (let it of items as any) {
                    it.classList.remove("hint");
                    it.classList.remove("current");
                    it.classList.remove("active");
                }
            });

            i.addEventListener("dragover", function handleDragOver(this: any, ev: any) {
                ev.preventDefault();
                ev.dataTransfer.dropEffect = "move"
            });

            i.addEventListener("drop",         function handleDrop(this: any, ev: any) {
                ev.preventDefault();

                // Get the id of the target and add the moved element to the target's DOM
                // var data = ev.dataTransfer.getData("text");
                // console.log(data)
                // ev.target.appendChild(document.getElementById(data));

                if (this != current) {
                    let currentPosition = 0;
                    let positionDrop = 0;
                    for (let it = 0; it < items.length; it++) {
                        // finding where you dragged the item from
                        if (current == items[it]) {
                            currentPosition = it;
                        }
                        // finding the item position that you dropped it on
                        if (this == items[it]) {
                            positionDrop = it;
                        }
                    }
                    if (currentPosition < positionDrop) {
                        this.parentNode.insertBefore(current, this.nextSibling);
                    } else {
                        this.parentNode.insertBefore(current, this);
                    }

                    callback(items);
                }
            });

            // Manual buttons if drag and drop is not working
            let btn = document.createElement("DIV");
            btn.className = "options";

            let up = document.createElement("DIV");
            up.innerHTML = "&#8613;";
            up.className = "btn increase";
            up.addEventListener("click", function(this: any, ev: any) {
                let element = this.parentNode.parentNode;
                if (element.previousElementSibling) {
                    element.parentNode.insertBefore(element, element.previousElementSibling);
                }
                element.classList.add("active");
                setTimeout(function handleEnd() {
                    for (let it of items as any) {
                        it.classList.remove("hint");
                        it.classList.remove("current");
                        it.classList.remove("active");
                    }
                }, 250);
                callback(items);
            });
            btn.append(up);

            let down = document.createElement("DIV");
            down.innerHTML = "&#8615;";
            down.className = "btn decrease";
            down.addEventListener("click", function(this: any, ev: any) {
                let element = this.parentNode.parentNode;
                if (element.nextElementSibling) {
                    element.parentNode.insertBefore(element.nextElementSibling, element);
                }
                element.classList.add("active");
                setTimeout(function handleEnd() {
                    for (let it of items as any) {
                        it.classList.remove("hint");
                        it.classList.remove("current");
                        it.classList.remove("active");
                    }
                }, 250);
                callback(items);
            });
            btn.append(down);

            i.append(btn);
        }
    }
}