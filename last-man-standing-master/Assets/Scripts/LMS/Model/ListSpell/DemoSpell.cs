using UnityEngine;
using LMS.Battle;

namespace LMS.Model
{
    public class KaboomSpell : SpecifiedSpell
    {
        int damage = 150;
        public override void Init()
        {
            base.Init();

            parent.Code = "kaboom";
            parent.Scale = 2;
            parent.Cooldown = 10;
            parent.AreaOfEffect = 1;
            parent.ManaCost = 150;
        }

        public override void OnHitSpell(Vector3 point)
        {
            base.OnHitSpell(point);

            // Hit target
            Collider2D[] hits = Physics2D.OverlapCircleAll(parent.source.transform.position.ToVector2(), parent.AreaOfEffect);
            foreach (Collider2D col in hits)
            {
                if (BattleManager.ColliderIsCharacter(col, parent.source.gameObject))
                {
                    if (BattleManager.IsEnemyAndAlive(col.gameObject, parent.PlayerOwner.TeamIndex))
                        col.GetComponent<UnitObject>().TakeDamage(damage, parent.PlayerOwner);
                }
            }
        }
    }

    public class HealingWardSpell : SpecifiedSpell
    {
        int healAmount = 20;
        public override void Init()
        {
            base.Init();

            parent.Code = "healingward";
            parent.Scale = 4;
            parent.Cooldown = 10;
            parent.Duration = 5;
            parent.TimePerInvoke = 0.5f;
            parent.AreaOfEffect = 2;
            parent.ManaCost = 100;
        }

        public override void OnCast()
        {
            base.OnCast();
            _timeSinceLastUpdate = 0;
            _duration = 0;
        }

        float _timeSinceLastUpdate = 0;
        float _duration = 0;
        public override void Update(float deltaTime)
        {
            _duration += deltaTime;
            _timeSinceLastUpdate += deltaTime;
            if (_timeSinceLastUpdate >= parent.TimePerInvoke)
            {
                _timeSinceLastUpdate -= parent.TimePerInvoke;

                // Heal alias
                Collider2D[] hits = Physics2D.OverlapCircleAll(parent.source.transform.position.ToVector2(), parent.AreaOfEffect);
                foreach (Collider2D col in hits)
                {
                    if (BattleManager.ColliderIsCharacter(col, parent.source.gameObject))
                    {
                        if (BattleManager.IsAlias(col.gameObject, parent.PlayerOwner.TeamIndex)
                            && BattleManager.IsAlive(col.gameObject))
                            col.GetComponent<UnitObject>().Heal(healAmount);
                    }
                }
            }

            if (_duration >= parent.Duration)
            {
                parent.source.OnFinish();
            }
        }
    }

    public class DiabloSpell : SpecifiedSpell
    {
        int damageInvoke = 35;
        public override void Init()
        {
            base.Init();

            parent.Code = "diablo";
            parent.Scale = 4;
            parent.Cooldown = 10;
            parent.Duration = 7;
            parent.TimePerInvoke = 0.5f;
            parent.AreaOfEffect = 2;
            parent.ManaCost = 100;
        }

        public override void OnCast()
        {
            base.OnCast();
            _timeSinceLastUpdate = 0;
            _duration = 0;
        }

        float _timeSinceLastUpdate = 0;
        float _duration = 0;
        public override void Update(float deltaTime)
        {
            _duration += deltaTime;
            _timeSinceLastUpdate += deltaTime;
            if (_timeSinceLastUpdate >= parent.TimePerInvoke)
            {
                _timeSinceLastUpdate -= parent.TimePerInvoke;

                // Heal alias
                Collider2D[] hits = Physics2D.OverlapCircleAll(parent.source.transform.position.ToVector2(), parent.AreaOfEffect);
                foreach (Collider2D col in hits)
                {
                    if (BattleManager.ColliderIsCharacter(col, parent.source.gameObject))
                    {
                        if (BattleManager.IsEnemy(col.gameObject, parent.PlayerOwner.TeamIndex)
                            && BattleManager.IsAlive(col.gameObject))
                            col.GetComponent<UnitObject>().TakeDamage(damageInvoke, parent.PlayerOwner);
                    }
                }
            }

            if (_duration >= parent.Duration)
            {
                parent.source.OnFinish();
            }
        }
    }
    public class PoisonSwamp : SpecifiedSpell
    {
        int damageInvoke = 35;
        public override void Init()
        {
            base.Init();

            parent.Code = "PoisonSwamp";
            parent.Scale = 4;
            parent.Cooldown = 10;
            parent.Duration = 7;
            parent.TimePerInvoke = 0.5f;
            parent.AreaOfEffect = 2;
            parent.ManaCost = 100;
        }

        public override void OnCast()
        {
            base.OnCast();
            _timeSinceLastUpdate = 0;
            _duration = 0;
        }

        float _timeSinceLastUpdate = 0;
        float _duration = 0;
        public override void Update(float deltaTime)
        {
            _duration += deltaTime;
            _timeSinceLastUpdate += deltaTime;
            if (_timeSinceLastUpdate >= parent.TimePerInvoke)
            {
                _timeSinceLastUpdate -= parent.TimePerInvoke;

                // Heal alias
                Collider2D[] hits = Physics2D.OverlapCircleAll(parent.source.transform.position.ToVector2(), parent.AreaOfEffect);
                foreach (Collider2D col in hits)
                {
                    if (BattleManager.ColliderIsCharacter(col, parent.source.gameObject))
                    {
                        if (BattleManager.IsEnemy(col.gameObject, parent.PlayerOwner.TeamIndex)
                            && BattleManager.IsAlive(col.gameObject))
                            col.GetComponent<UnitObject>().TakeDamage(damageInvoke, parent.PlayerOwner);
                    }
                }
            }

            if (_duration >= parent.Duration)
            {
                parent.source.OnFinish();
            }
        }
    }
}