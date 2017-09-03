namespace LMS.Model
{
    public class HeroFrostRanger : SpecifiedUnit
    {
        public override void Init()
        {

            base.Init();

            parent.Code = "frostranger";
            parent.Name = "Frost Ranger";
            parent.DebugName = string.Format("{0}", parent.Code);
            parent.Attack = 18;
            parent.AttackRange = 400;
            parent.AttackSpeed = 2000;
            parent.MovementSpeed = 275;
            parent.Hp = 1000;
            parent.Defense = 3f;
            parent.AniAttackTime = 0.917f;
            parent.Type = UnitScript.UnitType.Ranger;
            parent.ProjectTypeCode = "frostarrow2";
            parent.ProjectileSpeed = 8;

            parent.listSkill.Add(new SkillInfo(6, 1));
            parent.listSkill.Add(new SkillInfo(7, 1));
            parent.listSkill.Add(new SkillInfo(8, 1));

            parent.CallBackOnLoadUnit = (obj) =>
            {
                obj.listSkill.Add(new SkillScript(6, obj));
                obj.listSkill.Add(new SkillScript(7, obj));
                obj.listSkill.Add(new SkillScript(8, obj));
                obj.listSkill.Add(new SkillScript(901, obj));
            };

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