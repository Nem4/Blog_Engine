
const BASE_URL = 'https://localhost:44305/api';

/**
 * Expose des méthodes utilitaires en lien avec l'environnement de l'application
 */
class Utils {
    /**
     * Retourne l'url d'une action selon l'environnement local ou non local
     * @param {string} action L'uri représentant l'action du contrôleur
     */
    static url(action) {
        return `${BASE_URL}${action}`;
    }
}

export { Utils };
