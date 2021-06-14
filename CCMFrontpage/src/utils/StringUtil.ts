export default class StringUtil {

    public static isNotNullOrEmpty(value: any): boolean {
        if (typeof value !== "undefined") {
            if( value || value === false || value === 0 ) {
                return true;
            }
            if (Array.isArray(value) && value.length > 0) {
                return true;
            }
        }
        return false;
    }

    public static isNullOrEmpty(value: any): boolean {
        if (typeof value !== "undefined") {
            if( value || value === false || value === 0 ) {
                return false;
            }
            if (Array.isArray(value) && value.length > 0) {
                return false;
            }
        }
        return true;
    }

    public static isNumber(num: string): boolean {
        return /^\d+$/.test(num);
    }

}
