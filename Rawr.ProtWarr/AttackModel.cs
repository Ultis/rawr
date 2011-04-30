using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Rawr.ProtWarr
{
    public class AttackModel
    {
        private Player Player;
        private DefendTable DefendTable;

        public AbilityModelList Abilities = new AbilityModelList();

        private AttackModelMode _attackModelMode;
        public AttackModelMode AttackModelMode
        {
            get { return _attackModelMode; }
            set { _attackModelMode = value; Calculate(); }
        }

        private RageModelMode _rageModelMode;
        public RageModelMode RageModelMode
        {
            get { return _rageModelMode; }
            set { _rageModelMode = value; Calculate(); }
        }

        public string Name { get; private set; }
        public string ShortName 
        {
            get { return Name.Replace("Sword and Board", "SnB").Replace("Revenge + Shockwave", "Rev + Shockwave"); }
        }
        public string Description { get; private set; }
        public float ThreatPerSecond { get; private set; }
        public float DamagePerSecond { get; private set; }
        public float WeaponSpeed { get; private set; }
        public float WeaponAttacksPerSecond { get; private set; }
        public float HitsPerSecond { get; private set; }
        public float CritsPerSecond { get; private set; }

        private void Calculate()
        {
            Dictionary<Ability, float> modelAbilities = new Dictionary<Ability, float>();
            float modelLength = 0.0f;
            float modelThreat = 0.0f;
            float modelDamage = 0.0f;
            float modelCrits = 0.0f;
            float modelHits = 0.0f;
            float modelWeaponAttacks = 0.0f;

            WeaponSpeed = 1.0f;
            if (Player.Character.MainHand != null)
                WeaponSpeed = Player.Character.MainHand.Speed;

            // Rotation Auto-Detection
            if (AttackModelMode == AttackModelMode.Optimal)
            {
                if (Player.Talents.Shockwave == 1 && Player.Talents.SwordAndBoard == 3 && Player.Talents.Devastate == 1)
                    if (Player.Talents.ImprovedRevenge > 0)
                        AttackModelMode = AttackModelMode.FullProtectionRevenge;
                    else
                        AttackModelMode = AttackModelMode.FullProtection;
                else if (Player.Talents.SwordAndBoard == 3 && Player.Talents.Devastate == 1)
                    if (Player.Talents.ImprovedRevenge > 0)
                        AttackModelMode = AttackModelMode.SwordAndBoardRevenge;
                    else
                        AttackModelMode = AttackModelMode.SwordAndBoard;
                else if (Player.Talents.Devastate == 1)
                    if (Player.Talents.ImprovedRevenge > 0)
                        AttackModelMode = AttackModelMode.DevastateRevenge;
                    else
                        AttackModelMode = AttackModelMode.Devastate;
                else
                    AttackModelMode = AttackModelMode.Basic;                   
            }

            switch (AttackModelMode)
            {
                case AttackModelMode.Basic:
                {
                    // Basic Rotation
                    // Shield Slam -> Revenge -> Sunder Armor -> Sunder Armor
                    Name        = "Basic Cycle";
                    Description = "Shield Slam -> Revenge -> Sunder Armor -> Sunder Armor";
                    modelLength = 6.0f;
                    modelAbilities.Add(Ability.ShieldSlam, 1.0f);
                    modelAbilities.Add(Ability.Revenge, 1.0f);
                    modelAbilities.Add(Ability.SunderArmor, 2.0f);
                    break;
                }
                case AttackModelMode.Devastate:
                {
                    // Devastate Rotation
                    // Shield Slam -> Devastate -> Devastate -> Devastate
                    Name = "Devastate";
                    Description = "Shield Slam -> Devastate -> Devastate -> Devastate";
                    modelLength = 6.0f;
                    modelAbilities.Add(Ability.ShieldSlam, 1.0f);
                    modelAbilities.Add(Ability.Devastate, 3.0f);
                    break;
                }
                case AttackModelMode.DevastateRevenge:
                {
                    // Devastate + Revenge Rotation
                    // Shield Slam -> Revenge -> Devastate -> Devastate
                    Name = "Devastate + Revenge";
                    Description = "Shield Slam -> Revenge -> Devastate -> Devastate";
                    modelLength = 6.0f;
                    modelAbilities.Add(Ability.ShieldSlam, 1.0f);
                    modelAbilities.Add(Ability.Revenge, 1.0f);
                    modelAbilities.Add(Ability.Devastate, 2.0f);
                    break;
                }
                case AttackModelMode.SwordAndBoard:
                case AttackModelMode.SwordAndBoardRevenge:
                case AttackModelMode.FullProtection:
                case AttackModelMode.FullProtectionRevenge:
                {
                    // Sword And Board Rotation
                    // Shield Slam > Revenge > Devastate
                    // The distribution of abilities in the model is as follows:
                    // 1.0 * Shield Slam + 0.73 * Revenge + 1.4596 * Devastate
                    // -or-
                    // Shield Slam > Revenge > Devastate @ 3s Shield Slam Cooldown > Shockwave > Devastate
                    // The distribution of abilities in the model is as follows:
                    // 1.0 * Shield Slam + 0.73 * Revenge + 1.133 * Devastate + 0.3266 * (Concussion Blow/Shockwave/Devastate)
                    // The cycle length is 4.7844s, abilities per cycle is 3.1896
                    Name = "Sword and Board";
                    Description = "Shield Slam > ";
                    if (AttackModelMode == AttackModelMode.SwordAndBoardRevenge || AttackModelMode == AttackModelMode.FullProtectionRevenge)
                    {
                        Name += " + Revenge";
                        Description += "Revenge > Devastate";
                    }
                    else
                        Description += "Devastate";
                    if (AttackModelMode == AttackModelMode.FullProtection || AttackModelMode == AttackModelMode.FullProtectionRevenge)
                    {
                        Name += " + Shockwave";
                        Description += "\n@ 1.5s Shield Slam Cooldown: Shockwave > Devastate";
                    }

                    modelLength = 4.7844f;
                    modelAbilities.Add(Ability.ShieldSlam, 1.0f);
                    modelAbilities.Add(Ability.Devastate, 1.133f);
                    
                    // Add Revenge, if applicable
                    if (AttackModelMode == AttackModelMode.SwordAndBoardRevenge || AttackModelMode == AttackModelMode.FullProtectionRevenge)
                        modelAbilities.Add(Ability.Revenge, 0.73f);
                    else
                        modelAbilities[Ability.Devastate] += 0.73f;
                    
                    // Add Shockwave, if applicable
                    if (AttackModelMode == AttackModelMode.FullProtection || AttackModelMode == AttackModelMode.FullProtectionRevenge)
                    {
                        modelAbilities.Add(Ability.Shockwave, (0.3266f / 3.0f));
                        modelAbilities[Ability.Devastate] += (0.3266f / 3.0f) * 2.0f;
                    }
                    else
                        modelAbilities[Ability.Devastate] += 0.3266f;

                    break;
                }
            }

            // Accumulate base model statistics
            foreach (KeyValuePair<Ability, float> modelAbility in modelAbilities)
            {
                AbilityModel ability = Abilities[modelAbility.Key];
                modelThreat += ability.Threat * modelAbility.Value;
                modelDamage += ability.Damage * modelAbility.Value;
                if (ability.Damage > 0.0f)
                {
                    modelHits += ability.HitPercentage * modelAbility.Value;
                    modelCrits += ability.CritPercentage * modelAbility.Value;
                }
                if (ability.IsWeaponAttack)
                    modelWeaponAttacks += modelAbility.Value;
            }

            // Heroic Strike
            float heroicStrikePercentage = Math.Max(0.0f, Math.Min(1.0f, Player.Options.HeroicStrikeFrequency));
            float heroicStrikeCount = (modelLength / 3.0f) * heroicStrikePercentage;

            modelThreat += Abilities[Ability.HeroicStrike].Threat * heroicStrikeCount;
            modelDamage += Abilities[Ability.HeroicStrike].Damage * heroicStrikeCount;
            modelHits   += Abilities[Ability.HeroicStrike].HitPercentage * heroicStrikeCount;
            modelCrits  += Abilities[Ability.HeroicStrike].CritPercentage * heroicStrikeCount;
            modelWeaponAttacks += heroicStrikeCount;

            // Simple GCD-Based Latency Adjustment
            // modelLength *= Lookup.GlobalCooldownSpeed(true) / Lookup.GlobalCooldownSpeed(false);
            // Note: Removed due to action queue in Cataclysm--should no longer be needed

            // Weapon Damage Swings
            float weaponSwings = modelLength / Lookup.WeaponSpeed(Player);
            modelThreat += Abilities[Ability.None].Threat * weaponSwings;
            modelDamage += Abilities[Ability.None].Damage * weaponSwings;
            modelCrits  += Abilities[Ability.None].CritPercentage * weaponSwings;
            modelHits   += Abilities[Ability.None].HitPercentage * weaponSwings;
            modelWeaponAttacks += weaponSwings;

            // Deep Wounds
            AbilityModel deepWounds = Abilities[Ability.DeepWounds];
            modelThreat += deepWounds.Threat * modelCrits;
            modelDamage += deepWounds.Damage * modelCrits;

            // Misc. Power Gains
            modelThreat += DefendTable.AnyBlock * (modelLength / Lookup.TargetWeaponSpeed(Player)) * 25.0f * Player.Talents.ShieldSpecialization;

            // Final Per-Second Calculations
            ThreatPerSecond             = modelThreat / modelLength;
            DamagePerSecond             = modelDamage / modelLength;
            WeaponAttacksPerSecond      = modelWeaponAttacks / modelLength;
            HitsPerSecond               = modelHits / modelLength;
            CritsPerSecond              = modelCrits / modelLength;
        }

        public AttackModel(Player player, AttackModelMode attackModelMode)
            : this(player, attackModelMode, RageModelMode.Infinite)
        {
        }

        public AttackModel(Player player, AttackModelMode attackModelMode, RageModelMode rageModelMode)
        {
            Player           = player;
            DefendTable      = new DefendTable(Player);
            _attackModelMode = attackModelMode;
            _rageModelMode   = rageModelMode;

            Abilities.Add(Ability.None, Player);
            Abilities.Add(Ability.Cleave, Player);
            Abilities.Add(Ability.ConcussionBlow, Player);
            Abilities.Add(Ability.DeepWounds, Player);
            Abilities.Add(Ability.Devastate, Player);
            Abilities.Add(Ability.HeroicStrike, Player);
            Abilities.Add(Ability.HeroicThrow, Player);
            Abilities.Add(Ability.Rend, Player);
            Abilities.Add(Ability.Revenge, Player);
            Abilities.Add(Ability.ShieldSlam, Player);
            Abilities.Add(Ability.Shockwave, Player);
            Abilities.Add(Ability.Slam, Player);
            Abilities.Add(Ability.SunderArmor, Player);
            Abilities.Add(Ability.ThunderClap, Player);

            Calculate();
        }
    }
}
