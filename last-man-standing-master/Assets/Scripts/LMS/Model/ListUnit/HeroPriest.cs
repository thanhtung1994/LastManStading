namespace LMS.Model
{
    /// <summary>
    /// Index = 8
    /// </summary>
    public class HeroPriest : SpecifiedUnit
    {
        public override void Init()
        {

            base.Init();

            parent.Code = "priest";
            parent.Name = "Priest";
            parent.DebugName = string.Format("{0}", parent.Code);
            parent.Attack = 18;
            parent.AttackRange = 400;
            parent.AttackSpeed = 2000;
            parent.MovementSpeed = 275;
            parent.Hp = 1000;
            parent.Defense = 3f;
            parent.CriticalChance = 50;
            parent.AniAttackTime = 0.917f;
            parent.Type = UnitScript.UnitType.Ranger;
            parent.ProjectTypeCode = "banana";
            parent.ProjectileSpeed = 8;

            parent.listSkill.Add(new SkillInfo(31, 1));
            parent.listSkill.Add(new SkillInfo(32, 1));
            parent.listSkill.Add(new SkillInfo(33, 1));

            parent.CallBackOnLoadUnit = (obj) =>
            {
                obj.listSkill.Add(new SkillScript(31, obj));
                obj.listSkill.Add(new SkillScript(32, obj));
                obj.listSkill.Add(new SkillScript(33, obj));
                //obj.listSkill.Add(new SkillScript(901, obj));
            };

            parent.unitDefineAnimation = new UnitDefineAnimation()
            {
                AniAttack = "attack_1",
                AniAttackCrit = "attack_1",
            };

            // Resources
            parent.Scale = 1;
        }
    }
}