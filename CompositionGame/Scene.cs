using System.Collections.Generic;

namespace CompositionGame
{
    internal static class Scene
    {
        public static readonly IList<GameObject> s_GameObjects;

        static Scene()
        {
            s_GameObjects = new List<GameObject>();
        }
    }
}
