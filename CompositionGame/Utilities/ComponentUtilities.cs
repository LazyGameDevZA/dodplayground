using System.Collections.Generic;
using static CompositionGame.Scene;

namespace CompositionGame.Utilities
{
    internal static class ComponentUtilities
    {
        public static IList<TComponent> FindAllComponentsOfType<TComponent>()
            where TComponent: class, IComponent
        {
            var components = new List<TComponent>(s_GameObjects.Count);

            foreach(var gameObject in s_GameObjects)
            {
                var component = gameObject.GetComponent<TComponent>();

                if(component != null)
                {
                    components.Add(component);
                }
            }

            return components;
        }

        public static TComponent FindOfType<TComponent>()
            where TComponent : class, IComponent
        {
            foreach(var gameObject in s_GameObjects)
            {
                var component = gameObject.GetComponent<TComponent>();

                if(component != null)
                {
                    return component;
                }
            }

            return null;
        }
    }
}
