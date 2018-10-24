using System.Collections.Generic;
using static OOGame.Scene;

namespace OOGame.Utilities
{
    internal static class GameObjectUtilities
    {
        public static IList<TObject> FindAllObjectsOfType<TObject>()
            where TObject : GameObject
        {
            var gameObjects = new List<TObject>();

            foreach(var gameObject in s_GameObjects)
            {
                if(gameObject is TObject tObject)
                {
                    gameObjects.Add(tObject);
                }
            }

            return gameObjects;
        }
    }
}
