
namespace LMS.Model
{
    /// <summary>
    /// Index = 7
    /// </summary>
    public class HeroGolem : SpecifiedUnit
    {

        public override void Init()
        {
            base.Init();

            parent.Code = "golem";
            parent.Name = "Golem";
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

            parent.listSkill.Add(new SkillInfo(36, 1));
            parent.listSkill.Add(new SkillInfo(37, 1));
            parent.listSkill.Add(new SkillInfo(38, 1));
            // Resources
            parent.Scale = 1f;

            parent.CallBackOnLoadUnit = (obj) =>
            {
                obj.listSkill.Add(new SkillScript(36, obj));
                obj.listSkill.Add(new SkillScript(37, obj));
                obj.listSkill.Add(new SkillScript(38, obj));
            };

            parent.unitDefineAnimation = new UnitDefineAnimation()
            {
                AniAttack = "attack_1",
                AniAttackCrit = "attack_2",
            };
        }
    }
}