namespace LMS.Model
{
    public class HeroScout : SpecifiedUnit
    {
        public override void Init()
        {

            base.Init();

            parent.Code = "scout";
            parent.Name = "Scout";
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
            parent.ProjectTypeCode = "dart";
            parent.ProjectileSpeed = 15;

            //parent.listSkill.Add(new SkillInfo(46, 1));
            parent.listSkill.Add(new SkillInfo(47, 1));
            //parent.listSkill.Add(new SkillInfo(48, 1));

            parent.CallBackOnLoadUnit = (obj) =>
            {
                //obj.listSkill.Add(new SkillScript(46, obj));
                obj.listSkill.Add(new SkillScript(47, obj));
                //obj.listSkill.Add(new SkillScript(48, obj));
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