/* *******************************************************
 * CCM Account password generator */
function generatePassword()
{
    function shuffleString(inputString)
    {
        let a = inputString.split(""), n = a.length;
        for (var i = n - 1; i > 0; i--) {
            var j = Math.floor(Math.random() * (i + 1));
            var tmp = a[i];
            a[i] = a[j];
            a[j] = tmp;
        }
        return a.join("");
    }

    function getRandomString(characterSet, numberOfCharacters)
    {
        let result = '';
        for (let i = 0; i < numberOfCharacters; i++)
        {
            result += getRandomCharacterFromString(characterSet);
        }
        return result;
    }

    function getRandomCharacterFromString(inputString)
    {
        let randomCharacterIndex = getRandomIntInclusive(0, inputString.length - 1);
        return inputString.charAt(randomCharacterIndex);
    }

    function getRandomIntInclusive(min, max)
    {
        min = Math.ceil(min);
        max = Math.floor(max);
        return Math.floor(Math.random() * (max - min + 1)) + min; //The maximum is inclusive and the minimum is inclusive 
    }

    const totalLength = 16;
    const minNumberOfDigits = 1;
    const minNumberOfSpecial = 1;
    const minNumberOfUpper = 1;
    const minNumberOfLower = 1;
    const numberOfMixedCharacters = totalLength - minNumberOfDigits - minNumberOfSpecial - minNumberOfUpper - minNumberOfLower;
    const upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    const lower = "abcdefghijklmnopqrstuvwxyz";
    const digits = "0123456789";
    const special = '!-_*';
    const all = upper + lower + digits + special;

    let password = "";
    password += getRandomString(special, minNumberOfSpecial);
    password += getRandomString(digits, minNumberOfDigits);
    password += getRandomString(upper, minNumberOfUpper);
    password += getRandomString(lower, minNumberOfLower);
    password += getRandomString(all, numberOfMixedCharacters);
    return shuffleString(password);
}