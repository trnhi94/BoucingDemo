namespace _MainGame.Scripts.Utilities.Behaviours.Initialize
{
    public interface IInitialize
    {
        bool HasInitialized { get; }
        void Initialize();
    }
}