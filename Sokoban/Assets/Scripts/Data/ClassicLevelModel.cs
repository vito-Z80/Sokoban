// YApi QuickType插件生成，具体参考文档:https://plugins.jetbrains.com/plugin/18847-yapi-quicktype/documentation

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Data
{
    public partial class ClassicLevelModel
    {
        [JsonProperty("boxes")]
        public Position[] Boxes { get; set; }

        [JsonProperty("walls")]
        public int[][] Walls { get; set; }

        [JsonProperty("buttons")]
        public Position[] Buttons { get; set; }

        [JsonProperty("player")]
        public Position Player { get; set; }
    }

    public class Position
    {
        [JsonProperty("x")]
        public int X { get; set; }

        [JsonProperty("y")]
        public int Y { get; set; }
    }
}
