export function get() {
    return document.cookie;
}

export function set(key, value, expiryInDays) {
    var expiryDate = new Date();
    expiryDate.setDate(expiryDate.getDate() + expiryInDays);
    document.cookie = `${key}=${value}; expires=${expiryDate.toUTCString()};`;
}