import Tools from "./Tools";

/**
 * Set up a form support for generating passwords
 *
 * @example PasswordGenerator();
 *
 * @param {string} - List HTMLElement class
 * @returns {void} - Listens to nothing
 */
export default class PasswordGenerator {

    constructor() {

        Tools.$event("generatePasswordCheckBox", "change", (item) => {
            let checked = item.target.checked || false;
            this.showGeneratedPassword(checked);
        });

        Tools.$event("changePasswordCheckBox", "change", (item) => {
            let checked = item.target.checked || false;
            this.toggleChangePasswordFieldsVisibility(checked);
        });

        Tools.$event(window, "load", () => {
            // Edit previous item
            let checkBoxChangePwd = Tools.$dom("changePasswordCheckBox");
            if (checkBoxChangePwd) {
                console.warn("You are editing, so nothing to show yet");
                this.toggleChangePasswordFieldsVisibility(false);
                return;
            }

            // Create new item
            let checkBox = Tools.$dom("generatePasswordCheckBox");
            if (!checkBox) {
                console.warn("No generate password check box");
                return;
            }

            const isPasswordSet = (Tools.$dom("PasswordDefault") as any).value;
            if (isPasswordSet === "") {
                let check = (checkBox as any).checked || false;
                checkBox.setAttribute("checked", "true");
                // checkBox.prop('checked', true).change();
                this.showGeneratedPassword(true);
                console.log(checkBox, check)
            }
        });
    }

    private async generatePassword(onPasswordReceived) {
        try {
            const response = await fetch("/api/passwordgenerator");
            const data = await response.json();
            onPasswordReceived(data.password);
        } catch(error) {
            console.log(error);
        }
    }

    private showGeneratedPassword(checked) {
        this.setGeneratedPasswordFieldVisibility(checked);
        this.setPasswordFieldsVisibility(!checked);

        if (checked) {
            this.generatePassword((password) => {
                this.setPasswordFields(password);
            });
        } else {
            this.setPasswordFields("");
        }
    }

    private setPasswordFields(password) {
        Tools.$dom("PasswordDefault").setAttribute("value", password);
        Tools.$dom("PasswordConfirm").setAttribute("value", password);
        Tools.$dom("generatedPassword").setAttribute("value", password);
    }

    private setGeneratedPasswordFieldVisibility(show) {
        Tools.$dom("generatedPasswordWrapper").classList.toggle('hidden', !show);
    }

    private setPasswordFieldsVisibility(show) {
        Tools.$dom("passwordFieldsWrapper").classList.toggle('hidden', !show);
    }

    private toggleChangePasswordFieldsVisibility(show) {
        Tools.$dom("changePasswordFieldsWrapper").classList.toggle('hidden', !show);
    }

    public generateUserPassword() {
        const length = 13;
        const alfabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!#%";

        let password = "";
        for (var i = 0; i < length; ++i) {
            password += alfabet.charAt(Math.floor(Math.random() * alfabet.length));
        }

        return password;
    }
}
