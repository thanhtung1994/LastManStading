namespace LMS.Model
{
    public class HeroLightSorcerer : SpecifiedUnit
    {
        public override void Init()
        {

            base.Init();

            parent.Code = "lightsorcerer";
            parent.Name = "Light Sorcerer";
            parent.DebugName = string.Format("{0}", parent.Code);
            parent.Attack = 18;
            parent.AttackRange = 400;
            parent.AttackSpeed = 2000;
            parent.MovementSpeed = 275;
            parent.Hp = 1000;
            parent.Mp = 100;
            parent.Defense = 3f;
            parent.AniAttackTime = 0.917f;
            parent.Type = UnitScript.UnitType.Ranger;
            parent.ProjectTypeCode = "lightball";
            parent.ProjectileSpeed = 8;

            parent.listSkill.Add(new SkillInfo(21, 1));
            parent.listSkill.Add(new SkillInfo(22, 1));
            parent.listSkill.Add(new SkillInfo(23, 1));
            //parent.listSkill.Add(new SkillInfo(8, 1));

            parent.CallBackOnLoadUnit = (obj) =>
            {
                obj.listSkill.Add(new SkillScript(21, obj));
                obj.listSkill.Add(new SkillScript(22, obj));
                obj.listSkill.Add(new SkillScript(23, obj));
                //obj.listSkill.Add(new SkillScript(8, obj));
                //obj.listSkill.Add(new SkillScript(901, obj));
            };

            parent.unitDefineAnimation = new UnitDefineAnimation()
            {
                AniAttack = "attack_1",
                AniAttackCrit = "attack_2",
            };
            parent.CriticalChance = 50;

            // Resources
            parent.Scale = 1;
        }
    }
}