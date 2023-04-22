using BepInEx;
using HarmonyLib;
using Timberborn.WindSystem;
using System.Reflection;
//using TimberbornAPI;
using Timberborn.PowerGenerating;
using System;
using TimberApi.ModSystem;
using TimberApi.ConsoleSystem;
using Timberborn.EntitySystem;
using Timberborn.BaseComponentSystem;

namespace WindPlugin
{
    /// <summary>
    /// This class holds all the initial stuff to do in startup
    /// </summary>
    [HarmonyPatch]
    public class WindPlugin : IModEntrypoint
    {
        public static WindPluginConfig Config;

        public void Entry(IMod mod, IConsoleWriter consoleWriter)
        {

            Config = mod.Configs.Get<WindPluginConfig>();
            var harmony = new Harmony("hytone.plugins.windplugin");
            harmony.PatchAll();

            consoleWriter.LogInfo("WindPlugin is loaded.");
        }

        /// <summary>
        /// A prefix method to override default wind values with 
        /// custom values from config
        /// </summary>
        /// <returns></returns>
        [HarmonyPrefix]
        [HarmonyPatch(typeof(WindService), nameof(WindService.Load))]
        static bool OverrideWindValues()
        {
            FieldInfo minWindStrengthField = typeof(WindService).GetField(nameof(WindService.MinWindStrength), BindingFlags.Static | BindingFlags.NonPublic);
            minWindStrengthField.SetValue(null, Config.MinWindStrength);
            FieldInfo maxWindStrengthField = typeof(WindService).GetField(nameof(WindService.MaxWindStrength), BindingFlags.Static | BindingFlags.NonPublic);
            maxWindStrengthField.SetValue(null, Config.MaxWindStrength);
            FieldInfo minWindTimeInHours = typeof(WindService).GetField(nameof(WindService.MinWindTimeInHours), BindingFlags.Static | BindingFlags.NonPublic);
            minWindTimeInHours.SetValue(null, Config.MinWindTimeInHours);
            FieldInfo maxWindTimeInHours = typeof(WindService).GetField(nameof(WindService.MaxWindTimeInHours), BindingFlags.Static | BindingFlags.NonPublic);
            maxWindTimeInHours.SetValue(null, Config.MaxWindTimeInHours);
            return false;
        }


        [HarmonyPatch(typeof(EntityService), "Instantiate", typeof(BaseComponent), typeof(Guid))]
        class MinWindStrengthPatch
        {
            public static void Postfix(BaseComponent __result)
            {
                if(__result.name.StartsWith("LargeWindmill"))
                {
                    var gen = __result.GetComponentFast<WindPoweredGenerator>();
                    gen._minRequiredWindStrength = Config.MinRequiredLargeWindmillWindStrength;
                }
                else if(__result.name.StartsWith("Windmill"))
                {
                    var gen = __result.GetComponentFast<WindPoweredGenerator>();
                    gen._minRequiredWindStrength = Config.MinRequiredWindmillWindStrength;
                }
            }
        }
    }
}
