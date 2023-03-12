using Exiled.API.Enums;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using Player = Exiled.Events.Handlers.Player;
using Server = Exiled.Events.Handlers.Server;
using MEC;

namespace BleedEffect
{
    public class BleedEffect : Plugin<Config>
    {
        private static readonly Lazy<BleedEffect> LazyInstance = new Lazy<BleedEffect>(() => new BleedEffect());
        public static BleedEffect Instance => LazyInstance.Value;

        public override PluginPriority Priority { get; } = PluginPriority.Medium;
        public override string Name { get; } = "BleedEffect";
        public override string Author { get; } = "Steven4547466";
        public override Version Version { get; } = new Version(1, 1, 0);
        public override Version RequiredExiledVersion { get; } = new Version(2, 1, 2);
        public override string Prefix { get; } = "BleedEffect";

        public List<CoroutineHandle> Coroutines = new List<CoroutineHandle>();

        private BleedEffect() { }

        public Handlers.Player player { get; set; }
        public Handlers.Server server { get; set; }

        public bool mainCoroEnabled { get; set; }

        public override void OnEnabled()
        {
            if (BleedEffect.Instance.Config.IsEnabled == false) return;
            base.OnEnabled();
            Log.Info("BleedEffect enabled.");
            RegisterEvents();
            Coroutines = new List<CoroutineHandle>();
        }

        public override void OnDisabled()
        {
            base.OnDisabled();
            Log.Info("BleedEffect disabled.");
            UnregisterEvents();
        }

        public override void OnReloaded()
        {
            base.OnReloaded();
            Log.Info("BleedEffect reloading.");
        }

        public void RegisterEvents()
        {
            player = new Handlers.Player();
            player.Init();

            Player.Hurting += player.OnHurting;
            Player.Died += player.OnDied;
            Player.ChangingRole += player.OnChangingRole;
            Player.Left += player.OnLeft;
            Player.UsedItem += player.OnMedicalItemUsed;

            server = new Handlers.Server();
            Server.RoundEnded += server.OnRoundEnded;
            Server.RoundStarted += server.OnRoundStarted;
        }

        public void UnregisterEvents()
        {
            Log.Info("Events unregistered");
            mainCoroEnabled = false;
            Player.Hurting -= player.OnHurting;
            Player.Died -= player.OnDied;
            Player.ChangingRole -= player.OnChangingRole;
            Player.Left -= player.OnLeft;
            Player.UsedItem -= player.OnMedicalItemUsed;

            player = null;

            Server.RoundEnded -= server.OnRoundEnded;
            Server.RoundStarted -= server.OnRoundStarted;

            server = null;

            foreach (CoroutineHandle handle in Coroutines)
            {
                Log.Debug($"Killed coro {handle}");
                Timing.KillCoroutines(handle);
            }

            Coroutines = null;
        }
    }
}
