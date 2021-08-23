import Tools from "./Tools";

/**
 * Set up a color picker for an input field
 *
 * @example ColorPicker("color-picker");
 *
 * @param {string} - Color picker HTMLElement class
 */
export function ColorPicker(targetClass: string, previewClass: string = null) {
    const targets = document.getElementsByClassName(targetClass);
    if (targets == null || targets.length === 0) {
        throw new Error(`Could not bind color picker to target class '.${targetClass}'`);
    }

    for (var index = 0; index < targets.length; index++) {
        console.log({targ: targets[index]});

        const currentColor = targets[index].getAttribute("value");
        console.log(currentColor);
        if(currentColor.indexOf("#") > -1) {
            console.log("All good with the hash");
        } else {
            targets[index].setAttribute("value", `#${currentColor}`);
        }
        Tools.$event(targets[index], "input", (key) => {
            let elem = previewClass != null ? document.getElementById(previewClass) : document.getElementsByTagName("body")[0];
            elem.style.backgroundColor = key.target.value;
        });
        let elem = previewClass != null ? document.getElementById(previewClass) : document.getElementsByTagName("body")[0];
        elem.style.backgroundColor = targets[index].getAttribute("value");
    }

	// Background adjustment
	// let color1 = Math.floor((Math.random() * 256));
	// let color2 = Math.floor((Math.random() * 256));
	// let color3 = Math.floor((Math.random() * 256));
	// let color = "#" + color1.toString(16) + color2.toString(16) + color3.toString(16);
	// let colorText = "#" + (255 - color1).toString(16) + (255 - color2).toString(16) + (255 - color3).toString(16);
	// let elem = document.getElementsByTagName("body")[0];
	// elem.style.backgroundColor = color;
	// elem.style.color = colorText;
}