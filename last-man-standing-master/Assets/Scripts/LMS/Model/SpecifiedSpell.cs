using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LMS.Model
{
    public class SpecifiedSpell
    {

        public SpellScript parent;

        public virtual void Init()
        {

        }

        public virtual void Update(float deltaTime)
        {

        }

        public virtual void OnHitSpell(Vector3 point)
        {

        }

        public virtual void OnCast()
        {

        }

        public virtual void OnFinish()
        {

        }
    }
}