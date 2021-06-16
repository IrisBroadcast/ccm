/**
 * Set up a UL list and input for filtering function. User data attribute 'data-to-filter' and add 'id' for content list to filter.
 *
 * @example FilterList("sortlist");
 *
 * @param {string} - List HTMLElement class
 * @returns {void} - Listens to nothing
 */
export function FilterList(targetClass: string) {
    const targets = document.getElementsByClassName(targetClass);
    if (targets == null || targets.length === 0) {
        throw new Error(`Could not bind filter lists to target class '.${targetClass}'`);
    }

    for (let index = 0; index < targets.length; index++) {
        console.log({targ: targets[index]});
        console.log(targets[index].getAttribute("data-to-filter"))

        const itemsId = targets[index].getAttribute("data-to-filter");
        if (!itemsId && !targets[index]) {
            return;
        }
        const items = document.getElementById(itemsId).getElementsByTagName('li');
        targets[index].addEventListener('keyup', function(ev: any) {
            const text = ev.target.value;
            let pat = new RegExp(text, 'i');
            for (let i=0; i < items.length; i++) {
                const item = items[i];
                if (pat.test(item.innerText)) {
                    item.classList.remove("hidden");
                } else {
                    item.classList.add("hidden");
                }
            }
        });

        console.log(`Binding filtering input to list '${itemsId}'`);
    }
}