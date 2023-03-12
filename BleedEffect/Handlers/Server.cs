using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using System.Collections.Generic;
using Exiled.Events.EventArgs.Server;

namespace BleedEffect.Handlers
{
    public class Server
    {
        public void OnRoundEnded(RoundEndedEventArgs ev)
        {
            BleedEffect.Instance.mainCoroEnabled = false;
            foreach (CoroutineHandle handle in BleedEffect.Instance.Coroutines)
            {
                Log.Debug($"Killed coro {handle}");
                Timing.KillCoroutines(handle);
            }

            BleedEffect.Instance.Coroutines = new List<CoroutineHandle>();

            BleedEffect.Instance.player.bleeding = null;
            BleedEffect.Instance.player.beenShot = null;
        }

        public void OnRoundStarted()
        {
            BleedEffect.Instance.player.bleeding = new Dictionary<int, int>();
            BleedEffect.Instance.player.beenShot = new Dictionary<int, int>();
        }
    }
}
