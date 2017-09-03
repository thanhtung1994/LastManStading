namespace LMS.Model
{
    public class Heal : SpecifiedSkill
    {
        float percentHp = 0.15f;
        public override void Init()
        {
            base.Init();

            parent.Name = "Heal";
            parent.IconName = "skill_heal";
            parent.Type = SkillScript.SkillType.Active;
            parent.MaxLevel = 1;
            parent.Cooldown = 20;
            parent.CanDrag = false;
        }

        public override void OnCast()
        {
            parent.sourceUnit.Heal(percentHp * parent.sourceUnit.MaxHp);
        }
    }

}