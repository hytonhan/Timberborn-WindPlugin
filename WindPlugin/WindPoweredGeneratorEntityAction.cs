using System.Reflection;
using Timberborn.PowerGenerating;
using TimberbornAPI.EntityActionSystem;
using UnityEngine;

namespace WindPlugin
{
    public class WindPoweredGeneratorEntityAction : IEntityAction
    {
        private readonly string _minWindFieldName1 = "MinRequiredWindStrength";
        private readonly string _minWindFieldName2 = "_minRequiredWindStrength";

        /// <summary>
        /// This method can be used to change the behaviour of any entity in the game.
        /// Now we're using it to overwrite the values of Windmills' minimum wind strength
        /// </summary>
        /// <param name="entity"></param>
        public void ApplyToEntity(GameObject entity)
        {
            WindPoweredGenerator windPoweredGenerator = entity.GetComponent<WindPoweredGenerator>();
            if (windPoweredGenerator == null)
            {
                return;
            }

            if (windPoweredGenerator.name.StartsWith("LargeWindmill"))
            {
                var field = GetMinWindStrengthField(windPoweredGenerator);
                field.SetValue(windPoweredGenerator, WindPlugin.MinRequiredLargeWindmillWindStrength);
            }
            else if (windPoweredGenerator.name.StartsWith("Windmill"))
            {
                var field = GetMinWindStrengthField(windPoweredGenerator);
                field.SetValue(windPoweredGenerator, WindPlugin.MinRequiredWindmillWindStrength);
            }
        }

        private FieldInfo GetMinWindStrengthField(WindPoweredGenerator windPoweredGenerator)
        {
            var field = windPoweredGenerator.GetType()
                                             .GetField(_minWindFieldName1,
                                                       BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            if (field == null)
            {
                field = windPoweredGenerator.GetType()
                                             .GetField(_minWindFieldName2,
                                                       BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            }
            return field;
        }
    }
}
