using Modding.Converters;
using Newtonsoft.Json;

namespace EmoteWheel
{
    public class GlobalSettings
    {
        [JsonConverter(typeof(PlayerActionSetConverter))]
        public KeyBinds Keybinds = new KeyBinds();

        [JsonConverter(typeof(PlayerActionSetConverter))]
        public ButtonBinds ButtonBinds = new ButtonBinds();

    }
}
