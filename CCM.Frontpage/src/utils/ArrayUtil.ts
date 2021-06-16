export default class ArrayUtil {

    /**
     * Makes sure the object is an array.
     * Returnes the object if it's an array, otherwise an
     * array with the object as the only element.
     * @param o Input object
     */
    public static toArray<T>(o: any): Array<T> {
        if (!o) {
            return Array<T>();
        }
        if (o instanceof Array) {
            return o;
        } else {
            return [o];
        }
    }

}
