namespace _MainGame.Scripts.Utilities.Behaviours
{
    public abstract class AutoApplicationBehaviour : ApplicationBehaviour
    {
        protected virtual void Awake()
        {
            Setup();
        }
        protected virtual void Start()
        {
            LateSetup();
        }
    }
}