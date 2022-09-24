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
using UnityEngine;

namespace WindPlugin
{
    /// <summary>
    /// This class holds all the initial stuff to do in startup
    /// </summary>
    [BepInPlugin("hytone.plugins.windplugin", "WindPlugin", "2.0.0")]
    //[BepInDependency("com.timberapi.timberapi")]
    //[BepInProcess("Timberborn.exe")]
    [HarmonyPatch]
    public class WindPlugin : BaseUnityPlugin, IModEntrypoint
    {
        public static new BepInEx.Logging.ManualLogSource? Logger;
        public static float MinRequiredWindmillWindStrength = 0.3f;
        public static float MinRequiredLargeWindmillWindStrength = 0.2f;

        private static float _minWindStrength = 0f;
        private static float _maxWindStrength = 1f;
        private static float _minWindTimeInHours = 5f;
        private static float _maxWindTimeInHours = 12f;

        /// <summary>
        /// The entry point. Handle configs and registrations and such
        /// </summary>
        public void Entry(IMod mod, IConsoleWriter consoleWriter)
        {
            //Logger = base.Logger;
            MinRequiredWindmillWindStrength = Config.Bind(
                "WindMills",
                "MinRequiredWindmillWindStrength",
                0.3f,
                "The minimum wind strength when a regular Windmill will generate power").Value;
            MinRequiredLargeWindmillWindStrength = Config.Bind(
                "WindMills",
                "MinRequiredLargeWindmillWindStrength",
                0.2f,
                "The maximum wind strength when a Large Windmill will generate power").Value;
            _minWindStrength = Config.Bind(
                "Wind",
                "MinWindStrength",
                0f,
                "The minimum wind strength on a scale of 0-1.").Value;
            _maxWindStrength = Config.Bind(
                "Wind",
                "MaxWindStrength",
                1f,
                "The maximum wind strength on a scale of 0-1.").Value;
            _minWindTimeInHours = Config.Bind(
                "Wind",
                "MinWindTimeInHours",
                5f,
                "The minimum time it takes for wind to change in hours.").Value;
            _maxWindTimeInHours = Config.Bind(
                "Wind",
                "MaxWindTimeInHours",
                12f,
                "The maximum time it takes for wind to change in hours.").Value;

            var harmony = new Harmony("hytone.plugins.windplugin");
            harmony.PatchAll();

            //DependencyRegistry.AddConfigurator(new WindPluginConfigurator());
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
            minWindStrengthField.SetValue(null, _minWindStrength);
            FieldInfo maxWindStrengthField = typeof(WindService).GetField(nameof(WindService.MaxWindStrength), BindingFlags.Static | BindingFlags.NonPublic);
            maxWindStrengthField.SetValue(null, _maxWindStrength);
            FieldInfo minWindTimeInHours = typeof(WindService).GetField(nameof(WindService.MinWindTimeInHours), BindingFlags.Static | BindingFlags.NonPublic);
            minWindTimeInHours.SetValue(null, _minWindTimeInHours);
            FieldInfo maxWindTimeInHours = typeof(WindService).GetField(nameof(WindService.MaxWindTimeInHours), BindingFlags.Static | BindingFlags.NonPublic);
            maxWindTimeInHours.SetValue(null, _maxWindTimeInHours);
            return false;
        }

        [HarmonyPatch(typeof(EntityService), "Instantiate", typeof(GameObject), typeof(Guid))]
        class MinWindStrengthPatch
        {
            public static void Postfix(GameObject __result)
            {
                if(__result.name.StartsWith("LargeWindmill"))
                {
                    var gen = __result.GetComponent<WindPoweredGenerator>();
                    gen._minRequiredWindStrength = MinRequiredLargeWindmillWindStrength;
                }
                else if(__result.name.StartsWith("Windmill"))
                {
                    var gen = __result.GetComponent<WindPoweredGenerator>();
                    gen._minRequiredWindStrength = MinRequiredWindmillWindStrength;
                }
            }
        }
    }
}
