namespace LMS.Model
{
    public class Unit201 : SpecifiedUnit
    {

        public override void Init()
        {
            base.Init();

            parent.Code = "golem";
            parent.Name = "golem";
            parent.DebugName = string.Format("{0}", parent.Code);
            parent.Attack = 5;
            parent.AttackRange = 100;
            parent.AttackSpeed = 1500;
            parent.MovementSpeed = 200;
            parent.Hp = 120;
            parent.Mp = 100;
            parent.Defense = 3f;
            parent.AniAttackTime = 1.6f;
            parent.Type = UnitScript.UnitType.Melee;

            // Resources
            parent.Scale = 1f;

            parent.CallBackOnLoadUnit = (obj) =>
            {
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