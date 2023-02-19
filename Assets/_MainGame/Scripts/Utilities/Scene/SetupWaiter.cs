using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace _MainGame.Scripts.Utilities.Scene
{
    public static class SetupWaiter
    {
        public static async UniTask WaitForDoneSetup(IEnumerable<IHeavySetup> setups)
        {
            await UniTask.WaitWhile(() => setups.Any(s => !s.HasDoneSetup));
        }
    }
}