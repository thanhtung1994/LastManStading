
namespace LMS.Model
{
    public class HeroSamurai : SpecifiedUnit
    {

        public override void Init()
        {
            base.Init();

            parent.Code = "samurai";
            parent.Name = "Samurai";
            parent.DebugName = string.Format("{0}", parent.Code);
            parent.Attack = 20;
            parent.AttackRange = 125;
            parent.AttackSpeed = 2000;
            parent.MovementSpeed = 275;
            parent.Hp = 1100;
            parent.Mp = 100;
            parent.Defense = 3f;
            parent.AniAttackTime = 1.1f;
            parent.Type = UnitScript.UnitType.Melee;
            parent.CriticalChance = 50;

            parent.listSkill.Add(new SkillInfo(27, 1));
            //parent.listSkill.Add(new SkillInfo(2, 1));
            //parent.listSkill.Add(new SkillInfo(3, 1));
            //parent.listSkill.Add(new SkillInfo(13, 1));
            // Resources
            parent.Scale = 1f;

            parent.CallBackOnLoadUnit = (obj) =>
            {
                obj.listSkill.Add(new SkillScript(27, obj));
                //obj.listSkill.Add(new SkillScript(2, obj));
                //obj.listSkill.Add(new SkillScript(3, obj));
                //obj.listSkill.Add(new SkillScript(901, obj));
            };

            parent.unitDefineAnimation = new UnitDefineAnimation()
            {
                AniAttack = "attack_1",
                AniAttackCrit = "attack_2",
            };
        }
    }
}