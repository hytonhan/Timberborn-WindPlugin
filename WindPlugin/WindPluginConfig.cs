using TimberApi.ConfigSystem;

namespace WindPlugin
{
    public class WindPluginConfig : IConfig
    {
        public string ConfigFileName => "WindChanger";

        public float MinRequiredWindmillWindStrength = 0.3f;
        public float MinRequiredLargeWindmillWindStrength = 0.2f;
        public float MinWindStrength = 0f;
        public float MaxWindStrength = 1f;
        public float MinWindTimeInHours = 5f;
        public float MaxWindTimeInHours = 12f;
    }
}
