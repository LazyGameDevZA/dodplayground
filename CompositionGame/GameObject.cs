using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace CompositionGame
{
    internal class GameObject
    {
        private readonly string name;

        private readonly IList<IComponent> components;

        public GameObject(string name)
        {
            this.name = name;
            this.components = new List<IComponent>();
        }

        public TComponent GetComponent<TComponent>() where TComponent : class, IComponent 
        {
            foreach(var component in this.components)
            {
                if(component is TComponent tComponent)
                {
                    return tComponent;
                }
            }

            return null;
        }

        public void AddComponent<TComponent>(TComponent component) where TComponent : class, IComponent
        {
            component.GameObject = this;
            this.components.Add(component);
        }

        public void Start()
        {
            foreach(var component in this.components)
            {
                component.Start();
            }
        }

        public void Update(float deltaTime)
        {
            foreach(var component in this.components)
            {
                component.Update(deltaTime);
            }
        }
    }
}
