namespace CompositionGame
{
    internal interface IComponent
    {
        GameObject GameObject { get; set; }

        void Start();

        void Update(float deltaTime);
    }
    
    internal abstract class Component : IComponent
    {
        public GameObject GameObject { get; set; }

        public virtual void Start() {}

        public virtual void Update(float deltaTime) { }
    }
}
