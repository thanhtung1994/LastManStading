namespace LMS.Model
{
    public class Unit202 : SpecifiedUnit
    {

        public override void Init()
        {

            base.Init();

            parent.Code = "frostranger";
            parent.Name = "Frost Ranger creep";
            parent.DebugName = string.Format("{0}", parent.Code);
            parent.Attack = 8;
            parent.AttackRange = 400;
            parent.AttackSpeed = 2000;
            parent.MovementSpeed = 175;
            parent.Hp = 100;
            parent.Defense = 3f;
            parent.AniAttackTime = 0.917f;
            parent.Type = UnitScript.UnitType.Ranger;
            parent.ProjectTypeCode = "frostarrow2";
            parent.ProjectileSpeed = 5;

            parent.unitDefineAnimation = new UnitDefineAnimation()
            {
                AniAttack = "attack_1",
                AniAttackCrit = "attack_1",
            };
            parent.CriticalChance = 50;

            // Resources
            parent.Scale = 1;
        }
    }
}