namespace LMS.Model
{
    public class HeroBladeMaster : SpecifiedUnit
    {

        public override void Init()
        {
            base.Init();

            parent.Code = "blademaster";
            parent.Name = "BladeMaster";
            parent.DebugName = string.Format("{0}", parent.Code);
            parent.Attack = 20;
            parent.AttackRange = 100;
            parent.AttackSpeed = 1500;
            parent.MovementSpeed = 300;
            parent.Hp = 1500;
            parent.Mp = 100;
            parent.Defense = 3f;
            parent.AniAttackTime = 1.083f;
            parent.Type = UnitScript.UnitType.Melee;

            //parent.listSkill.Add(new SkillInfo(1, 1));
            //parent.listSkill.Add(new SkillInfo(2, 1));
            //parent.listSkill.Add(new SkillInfo(3, 1));
            //parent.listSkill.Add(new SkillInfo(13, 1));
            // Resources
            parent.Scale = 1f;

            parent.CallBackOnLoadUnit = (obj) =>
            {
                obj.listSkill.Add(new SkillScript(17, obj));
                obj.listSkill.Add(new SkillScript(901, obj));
                obj.unitAnimation.SetSkin("kiem_trang");
            };

            parent.unitDefineAnimation = new UnitDefineAnimation()
            {
                AniAttack = "attack_2",
                AniAttackCrit = "attack_1",
            };
        }
    }
}