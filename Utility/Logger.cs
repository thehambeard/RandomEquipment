using Kingmaker.UI.Models.Log;
using Kingmaker.UI.Models.Log.CombatLog_ThreadSystem;
using Kingmaker.UI.Models.Log.CombatLog_ThreadSystem.LogThreads.Common;
using System;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using static ModMenu.ModMenu;
using static WrathRandomEquipment.REModMenu.RandomEquipmentSettings;

namespace WrathRandomEquipment.Utility
{
    internal static class Logger
    {
        public static void VLog(string s)
        {
            if (GetSettingValue<bool>(GetKey("bool-verbose-logging")))
                Main.Mod.Log(s);
        }

        public static void CLog(string s)
        {
            using (GameLogContext.Scope)
            {
                var messageLog = LogThreadService.Instance.m_Logs[LogChannelType.Common].FirstOrDefault(x => x is MessageLogThread);

                if (GetSettingValue<bool>(GetKey("bool-combat-logging")))
                    messageLog?.AddMessage(new($"[RE]: {s}", Color.black, PrefixIcon.RightArrow));
            }
        }

        public class ProcessLog : IDisposable
        {
            private Stopwatch stopWatch = new Stopwatch();
            private string message;
            public ProcessLog(string message)
            {
                this.message = message;
                stopWatch.Start();
            }

            public void Dispose()
            {
                stopWatch.Stop();
                Main.Mod.Debug($"{message}: [{stopWatch.Elapsed:ss\\.ff}]");
            }
        }
    }
}
