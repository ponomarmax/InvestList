import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';

i18n
  .use(initReactI18next)
  .init({
    resources: {
      en: {
        translation: {
          // Add your English translations here
        }
      },
      uk: {
        translation: {
          // Add your Ukrainian translations here
        }
      }
    },
    lng: 'uk', // default language
    fallbackLng: 'uk',
    interpolation: {
      escapeValue: false
    }
  });

export default i18n; 