using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using EPlayer = Exiled.API.Features.Player;
using System.Collections.Generic;
using System;
using Exiled.API.Enums;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;

namespace BleedEffect.Handlers
{
    public class Player
    {
        public CoroutineHandle co;
        public Dictionary<int, int> beenShot = new Dictionary<int, int>();
        public Dictionary<int, int> bleeding = new Dictionary<int, int>();
        public List<DamageType> allowedDamageType = new List<DamageType>();
        public bool affectsScps = false;
        public List<RoleTypeId> affectedScps = null;

        public void Init()
        {
            Config config = BleedEffect.Instance.Config;
            if (config.E11RifleEnabled) allowedDamageType.Add(DamageType.E11Sr);
            if (config.LogicerEnabled) allowedDamageType.Add(DamageType.Logicer);
            if (config.MicroEnabled) allowedDamageType.Add(DamageType.MicroHid);
            if (config.Fsp9Enabled) allowedDamageType.Add(DamageType.Fsp9);
            if (config.CrossvecEnabled) allowedDamageType.Add(DamageType.Crossvec);
            if (config.Com15Enabled) allowedDamageType.Add(DamageType.Com15);
            if (config.Com18Enabled) allowedDamageType.Add(DamageType.Com18);
            if (config.GrenadeEnabled) allowedDamageType.Add(DamageType.Explosion);
            if (config.SCP939Enabled) allowedDamageType.Add(DamageType.Scp939);
            if (config.SCP049_2Enabled) allowedDamageType.Add(DamageType.Scp0492);
            if (config.AffectsScps)
            {
                affectsScps = true;
                affectedScps = new List<RoleTypeId>();
                if (config.Affects049) affectedScps.Add(RoleTypeId.Scp049);
                if (config.Affects049_2) affectedScps.Add(RoleTypeId.Scp0492);
                if (config.Affects096) affectedScps.Add(RoleTypeId.Scp096);
                if (config.Affects106) affectedScps.Add(RoleTypeId.Scp106);
                if (config.Affects173) affectedScps.Add(RoleTypeId.Scp173);
                if (config.Affects939) affectedScps.Add(RoleTypeId.Scp939);
            }
        }
        public void OnHurting(HurtingEventArgs ev)
        {
            if (beenShot == null || bleeding == null) return;
            Log.Debug($"Player with id {ev.Player.Id} has taken damage from {ev.DamageHandler.Type}.");
            if (!affectsScps && ev.Player.Role.Team == Team.SCPs) return;
            if (ev.Player.IsGodModeEnabled) return;
            else if (ev.Player.Role.Team == Team.SCPs)
            {
                if (!affectedScps.Contains(ev.Player.Role)) return;
            }
            if (!allowedDamageType.Contains(ev.DamageHandler.Type))
            {
                Log.Debug($"{ev.DamageHandler.Type} has not passed allowed damage types.");
                return;
            }
            Log.Debug($"{ev.DamageHandler.Type} has passed allowed damage types.");
            if (!beenShot.ContainsKey(ev.Player.Id)) beenShot.Add(ev.Player.Id, 1);
            else beenShot[ev.Player.Id] += 1;
            if (beenShot[ev.Player.Id] % BleedEffect.Instance.Config.ShotsToBleed == 0)
            {
                if (beenShot[ev.Player.Id] == BleedEffect.Instance.Config.ShotsToBleed && BleedEffect.Instance.Config.BleedMessage != "") ev.Player.Broadcast(5, $"<color=\"red\">{BleedEffect.Instance.Config.BleedMessage}</color>");
                if (beenShot[ev.Player.Id] > BleedEffect.Instance.Config.ShotsToBleed && BleedEffect.Instance.Config.IncreasedBleedMessage != "") ev.Player.Broadcast(5, $"<color=\"red\">{BleedEffect.Instance.Config.IncreasedBleedMessage}</color>");
                if (!bleeding.ContainsKey(ev.Player.Id)) bleeding.Add(ev.Player.Id, 1);
                else bleeding[ev.Player.Id] += 1;
                if (!BleedEffect.Instance.mainCoroEnabled)
                {
                    BleedEffect.Instance.mainCoroEnabled = true;
                    co = Timing.RunCoroutine(Bleed());
                    BleedEffect.Instance.Coroutines.Add(co);
                }
            }
        }

        public void OnMedicalItemUsed(UsedItemEventArgs ev)
        {
            if (beenShot == null || bleeding == null) return;
            if (ev.Item.Type == ItemType.Adrenaline && !BleedEffect.Instance.Config.AdrenalineStopsBleeding) return;
            if (ev.Item.Type == ItemType.Painkillers && !BleedEffect.Instance.Config.PainkillersStopBleeding) return;
            if (ev.Item.Type == ItemType.Medkit && !BleedEffect.Instance.Config.MedKitStopsBleeding) return;
            if (ev.Item.Type == ItemType.SCP500 && !BleedEffect.Instance.Config.SCP500StopsBleeding) return;
            if (beenShot.ContainsKey(ev.Player.Id)) beenShot.Remove(ev.Player.Id);
            if (bleeding.ContainsKey(ev.Player.Id)) bleeding.Remove(ev.Player.Id);
        }

        public void OnDied(DiedEventArgs ev)
        {
            if (beenShot == null || bleeding == null) return;
            if (beenShot.ContainsKey(ev.Player.Id)) beenShot.Remove(ev.Player.Id);
            if (bleeding.ContainsKey(ev.Player.Id)) bleeding.Remove(ev.Player.Id);
        }

        public void OnLeft(LeftEventArgs ev)
        {
            if (beenShot == null || bleeding == null) return;
            if (beenShot.ContainsKey(ev.Player.Id)) beenShot.Remove(ev.Player.Id);
            if (bleeding.ContainsKey(ev.Player.Id)) bleeding.Remove(ev.Player.Id);
        }

        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (beenShot == null || bleeding == null) return;
            if (beenShot.ContainsKey(ev.Player.Id)) beenShot.Remove(ev.Player.Id);
            if (bleeding.ContainsKey(ev.Player.Id)) bleeding.Remove(ev.Player.Id);
        }


        public IEnumerator<float> Bleed()
        {
            while (bleeding != null && bleeding.Count > 0)
            {
                double HealthPerSec = BleedEffect.Instance.Config.HealthDrainPerSecond;
                double HealthPerSecInc = BleedEffect.Instance.Config.HealthDrainPerSecondIncrease;
                foreach (var ent in bleeding)
                {
                    double amount = HealthPerSec;
                    EPlayer p = EPlayer.Get(ent.Key);
                    if (p.IsGodModeEnabled) 
                    {
                        bleeding.Remove(ent.Key);
                    }
                    if (ent.Value > 1)
                    {
                        if(BleedEffect.Instance.Config.HealthDrainExponential)
                        {
                            double power = ent.Value;
                            if (amount < 1)
                            {
                                power /= BleedEffect.Instance.Config.HealthDrainDivisor;
                            }
                            amount = Math.Pow(HealthPerSec, power);
                        } else
                        {
                            amount += HealthPerSecInc*(ent.Value-1);
                        }
                    }
                    Log.Debug($"Player with id {ent.Key} has drained {amount} health.");
                    if (p.Health - amount <= 0)
                    {
                        bleeding.Remove(ent.Key);
                        beenShot.Remove(ent.Key);
                        p.Kill(DamageType.Bleeding);
                        continue;
                    }
                    p.Health -= (float) amount;
                }
                yield return Timing.WaitForSeconds(1f);
            }
            BleedEffect.Instance.mainCoroEnabled = false;
            Log.Debug($"Stopping Coro {co}");
            BleedEffect.Instance.Coroutines.Remove(co);
            Timing.KillCoroutines(co);
            yield break;
        }
    }
}
