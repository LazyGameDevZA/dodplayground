using System.Collections.Generic;
using static Common.Constants;

namespace CompositionGame
{
    internal static class Scene
    {
        public static readonly IList<GameObject> s_GameObjects;

        static Scene()
        {
            s_GameObjects = new List<GameObject>(DotCount + BubbleCount + 1);
        }
    }
}
