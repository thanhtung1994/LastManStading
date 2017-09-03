namespace LMS.Model
{
    public class HeroFallenAngel : SpecifiedUnit
    {

        public override void Init()
        {

            base.Init();

            parent.Code = "fallenangel";
            parent.Name = "Fallen Angel";
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

            parent.listSkill.Add(new SkillInfo(11, 1));
            parent.listSkill.Add(new SkillInfo(12, 1));
            parent.listSkill.Add(new SkillInfo(13, 1));

            parent.CallBackOnLoadUnit = (obj) =>
            {
                obj.listSkill.Add(new SkillScript(11, obj));
                obj.listSkill.Add(new SkillScript(12, obj));
                obj.listSkill.Add(new SkillScript(13, obj));
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