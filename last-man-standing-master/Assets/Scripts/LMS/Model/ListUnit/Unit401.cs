namespace LMS.Model
{
    public class Unit401 : SpecifiedUnit
    {

        public override void Init()
        {
            base.Init();

            parent.Code = "frostranger";
            parent.Name = "Frost Ranger";
            parent.DebugName = string.Format("{0}", parent.Code);
            parent.Attack = 30;
            parent.AttackRange = 400;
            parent.AttackSpeed = 3000;
            parent.MovementSpeed = 275;
            parent.Hp = 2500;
            parent.Defense = 10f;
            parent.AniAttackTime = 0.917f;
            parent.Type = UnitScript.UnitType.Ranger;
            parent.ProjectTypeCode = "frostarrow2";
            parent.ProjectileSpeed = 8;

            parent.CriticalChance = 50;
            parent.unitDefineAnimation = new UnitDefineAnimation()
            {
                AniAttack = "attack_1",
                AniAttackCrit = "attack_1",
            };

            parent.CallBackOnLoadUnit = (obj) =>
            {
                obj.listSkill.Add(new SkillScript(1001, obj));
                obj.listSkill.Add(new SkillScript(1002, obj));
                //obj.listSkill.Add(new SkillScript(2, obj));
                //obj.listSkill.Add(new SkillScript(3, obj));
            };
            
            parent.Scale = 1.5f;
        }
    }
}