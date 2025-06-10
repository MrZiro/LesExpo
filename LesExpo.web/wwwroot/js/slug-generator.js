/**
 * Converts a string to a URL-friendly slug
 * Handles Turkish characters properly by converting them to their Latin equivalents
 * @param {string} text - The text to convert to a slug
 * @returns {string} - The generated slug
 */
function generateSlug(text) {
    if (!text) return '';
    
    // Define Turkish character mappings
    const turkishChars = {
        'ı': 'i', 'ğ': 'g', 'ü': 'u', 'ş': 's', 'ö': 'o', 'ç': 'c',
        'İ': 'I', 'Ğ': 'G', 'Ü': 'U', 'Ş': 'S', 'Ö': 'O', 'Ç': 'C'
    };
    
    // Replace Turkish characters with Latin equivalents
    let slug = text.trim();
    Object.keys(turkishChars).forEach(char => {
        slug = slug.replace(new RegExp(char, 'g'), turkishChars[char]);
    });
    
    // Convert to lowercase and replace spaces with hyphens
    slug = slug.toLowerCase()
        .replace(/[^\w\s-]/g, '')     // Remove special characters
        .replace(/\s+/g, '-')         // Replace spaces with hyphens
        .replace(/-+/g, '-')          // Replace multiple hyphens with single hyphen
        .replace(/^-+/, '')           // Remove leading hyphens
        .replace(/-+$/, '');          // Remove trailing hyphens
    
    return slug;
} 