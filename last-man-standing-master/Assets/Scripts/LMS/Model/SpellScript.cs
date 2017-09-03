using System;
using UnityEngine;
using LMS.Battle;

namespace LMS.Model
{
    public class SpellScript : ICloneable
    {

        public SpecifiedSpell specified;
        public PlayerStats PlayerOwner;
        public SkillObject source;
        public int Index;
        public string Name;
        public string Code;
        public string Id;
        public string Description;
        public int ManaCost;
        public int Level;
        public float Cooldown;
        public float Scale;
        public float AreaOfEffect;
        public float _duration = 0;
        public float Duration
        {
            get { return _duration; }
            set
            {
                if (value > 0)
                    _duration = value;
                else
                    _duration = 0;
            }
        }

        private float _currentCoolDown = 0;
        public float CurrentCooldown
        {
            get { return _currentCoolDown; }
            set
            {
                if (value > 0)
                    _currentCoolDown = value;
                else
                    _currentCoolDown = 0;
            }
        }

        public bool IsReadyToUse
        {
            get
            {
                if (PlayerOwner == null) return false;
                //return CurrentCooldown <= 0 && PlayerOwner.Gold >= ManaCost;
                return true;
            }
        }

        public bool IsEnougeMana
        {
            get
            {
                if (PlayerOwner == null) return false;
                //return PlayerOwner.Gold >= ManaCost;
                return true;
            }
        }

        private float _timePerInvoke = 1;
        public float TimePerInvoke
        {
            get { return _timePerInvoke; }
            set { _timePerInvoke = value; }
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public SpellScript(int index, PlayerStats player = null, SkillObject source = null)
        {
            this.Index = index;
            this.PlayerOwner = player;
            this.source = source;
            SetSkill();
        }
        public void SetSkill()
        {
            switch (Index)
            {
                case 1:
                    specified = new KaboomSpell();
                    break;
                case 2:
                    specified = new HealingWardSpell();
                    break;
                case 3:
                    specified = new DiabloSpell();
                    break;
                default:
                    specified = new SpecifiedSpell();
                    break;
            }
            specified.parent = this;
            specified.Init();
            Id = "";
        }

        public void SetCooldownOnCast()
        {
            CurrentCooldown = Cooldown;
        }

        public void OnCast()
        {
            CurrentCooldown = Cooldown;
            specified.OnCast();
        }

        public void OnHitSpell(Vector3 point)
        {
            specified.OnHitSpell(point);
        }

        public virtual void OnFinish()
        {
            specified.OnFinish();
        }

        public virtual void Update(float deltaTime)
        {
            specified.Update(deltaTime);
            if (CurrentCooldown > 0)
                CurrentCooldown -= deltaTime;

        }
    }
}