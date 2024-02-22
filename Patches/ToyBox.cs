using HarmonyLib;
using Kingmaker.UI.MVVM._VM.Loot;
using Kingmaker.UI.MVVM;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UniRx;
using UnityModManagerNet;
using WrathRandomEquipment.RandomEquipment;
using static UnityModManagerNet.UnityModManager.Param;
using ModMaker.Utility;


namespace WrathRandomEquipment.Patches
{
    public static class ToyBox
    {
        public static class TryOpenMassLootPatch
        {
            public static void Patch()
            {
                var toyBox = UnityModManager.FindMod("0ToyBox0");
                
                if (toyBox == null)
                    return;

                try
                {
                    var lootHelperClass = toyBox.Assembly.GetType("ToyBox.LootHelper") ?? throw new NullReferenceException();
                    var openWindowMethod = lootHelperClass.GetMethod("OpenMassLoot", BindingFlags.Public | BindingFlags.Static) ?? throw new NullReferenceException();
                    var harmony = new Harmony($"{Main.ModEntry.Info.Id}-ToyBoxOptional");

                    harmony.Patch(openWindowMethod, transpiler: new HarmonyMethod(typeof(TryOpenMassLootPatch), nameof(OpenMassLootTranspiler)));
                }
                catch (Exception ex)
                {
                    Main.Mod.Error("Failed to patch ToyBox OpenMassLoot. Using Open Mass Loot button will not generate random loot.");
                    Main.Mod.Error(ex.Message + ex.StackTrace);

                }
            }

            public static void ProcessAreaRandomLoot(IEnumerable<LootWrapper> areaLoot)
            {
                if (areaLoot == null)
                    return;

                EntityLootHandler handler = new(false);

                foreach (var loot in areaLoot) 
                {
                    if (loot.InteractionLoot != null)
                    {
                        handler.OnBeforeInteract(null, loot.InteractionLoot);
                        loot.InteractionLoot.IsViewed = true;
                    }
                }
            }

            static IEnumerable<CodeInstruction> OpenMassLootTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator ilGen)
            {
                var lootVMCodes = new CodeInstruction[]
                {
                    new(OpCodes.Ldc_I4_2),
                    new(OpCodes.Ldloc_1),
                    new(OpCodes.Ldnull),
                    new(OpCodes.Ldloc_0)
                };

                var lootVMIndex = instructions.FindCodes(lootVMCodes);

                if (lootVMIndex >= 0)
                {
                    var patchCodes = new CodeInstruction[]
                    {
                        new(OpCodes.Ldloc_1),
                        new(OpCodes.Call,
                            new Action<IEnumerable<LootWrapper>>(ProcessAreaRandomLoot).Method)
                    };

                    return instructions.InsertRange(lootVMIndex, patchCodes, true).Complete();
                }
                else
                {
                    Main.Mod.Error("OpenMassLootTranspiler Transpile Failed.");
                    return instructions;
                }
            }
        }
    }
}
