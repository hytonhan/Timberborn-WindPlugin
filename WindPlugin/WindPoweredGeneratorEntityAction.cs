using Timberborn.PowerGenerating;
using TimberbornAPI.EntityActionSystem;
using UnityEngine;

namespace WindPlugin
{
    public class WindPoweredGeneratorEntityAction : IEntityAction
    {
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
            
            if(windPoweredGenerator.name.StartsWith("LargeWindmill"))
            {
                windPoweredGenerator.MinRequiredWindStrength = WindPlugin.MinRequiredLargeWindmillWindStrength;
            }
            else if(windPoweredGenerator.name.StartsWith("Windmill"))
            {
                windPoweredGenerator.MinRequiredWindStrength = WindPlugin.MinRequiredWindmillWindStrength;
            }
        }
    }
}
