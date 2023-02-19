namespace _MainGame.Scripts.Utilities.Behaviours.Initialize
{
    /// <summary>
    /// Used for object is inactive at first => need to be booted from other source
    /// </summary>
    public abstract class InitializedApplicationBehaviour : ApplicationBehaviour, IInitialize
    {
        public bool HasInitialized { get; private set; }
        public void Initialize()
        {
            HasInitialized = true;
            Setup();
            LateSetup();
        }
    }
}