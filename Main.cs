﻿using ModMaker;
using ModMaker.Utility;
using System;
using System.Reflection;
using UnityEngine;
using UnityModManagerNet;


namespace WrathRandomEquipment
{
#if (DEBUG)
    [EnableReloading]
#endif
    static class Main
    {
        public static LocalizationManager<DefaultLanguage> Local;
        public static ModManager<Core, Settings> Mod;
        public static MenuManager Menu;
        public static UnityModManager.ModEntry ModEntry;

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            Local = new LocalizationManager<DefaultLanguage>();
            Mod = new ModManager<Core, Settings>();
            Menu = new MenuManager();
            ModEntry = modEntry;
            modEntry.OnToggle = OnToggle;
#if (DEBUG)
            modEntry.OnUnload = Unload;
            return true;
        }

        static bool Unload(UnityModManager.ModEntry modEntry)
        {
            Mod.Disable(modEntry, true);
            Menu = null;
            Mod = null;
            Local = null;
            return true;
        }
#else
            return true;
        }
#endif
            static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            if (value)
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                Local.Enable(modEntry);
                Mod.Enable(modEntry, assembly);
                Menu.Enable(modEntry, assembly);
            }
            else
            {
                Menu.Disable(modEntry);
                Mod.Disable(modEntry, false);
                Local.Disable(modEntry);
                ReflectionCache.Clear();
            }
            return true;
        }

        internal static Exception Error(String message)
        {
            Mod.Error(message);
            return new InvalidOperationException(message);
        }
    }
}
