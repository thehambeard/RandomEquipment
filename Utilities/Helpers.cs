using Kingmaker.Localization;
using System;
using System.Collections.Generic;

namespace RandomEquipment.Utilities
{
    public static class Helpers
    {
        public static LocalizedString CreateString(string key, string value)
        {
            // See if we used the text previously.
            // (It's common for many features to use the same localized text.
            // In that case, we reuse the old entry instead of making a new one.)
            LocalizedString localized;
            if (textToLocalizedString.TryGetValue(value, out localized))
            {
                return localized;
            }
            var strings = LocalizationManager.CurrentPack.Strings;
            String oldValue;
            if (strings.TryGetValue(key, out oldValue) && value != oldValue)
            {
                Main.Mod.Debug($"Info: duplicate localized string `{key}`, different text.");
            }
            strings[key] = value;
            localized = new LocalizedString();
            localized.m_Key = key;
            textToLocalizedString[value] = localized;
            return localized;
        }

        // All localized strings created in this mod, mapped to their localized key. Populated by CreateString.
        static Dictionary<String, LocalizedString> textToLocalizedString = new Dictionary<string, LocalizedString>();
    }
}