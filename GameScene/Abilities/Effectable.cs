using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlueAndWhite.Entities;
using BlueAndWhite.Objects;

namespace BlueAndWhite.Abilities
{
    [System.Serializable]
    public class Effectable : MonoBehaviour 
    {
        /*
        private string _effectName;
        public string effectName { get { return _effectName; } set { _effectName = value; } }
        private string _effectDescription;
        public string effectDescription { get { return _effectDescription; } set { _effectDescription = value; } }
        private int _effectID;
        public int effectID { get { return _effectID; } set { _effectID = value; } }
        */
        public enum EffectName
        {
            Burning,
            Healing
        }
        public List<Effect> currentEffects;

        /*
        public void ApplyEffect(Effect effect)
        {
            currentEffects.Add(effect);
        }
        public void RemoveEffect(Effect effect)
        {
            currentEffects.Remove(effect);
        }

        public void HandleEffect()
        {

        }
        */

        public void Burning(Entity target, int effectTurns, int effectLevel)
        {
            target.attackPoint.AddModifier(new StatModifier(-10, StatModifierType.Flat, this)); 

            if (target.currentEffects[0].effectTurns > 0)
            {
                target.currentHp.value -= 10f;
            }
        }
    }
}
