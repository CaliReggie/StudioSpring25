using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [System.Serializable]
    public class PlayerStats
    {
        public string heroName;
    
        public int jmpLimit;
        public float walkSpeed;
        public int jmpHeight;
        public int djmpHeight;
        public float jmpGravScale;

        public AnimatorOverrideController animOverride;
        
        public PlayerStats(string heroName, int jmpLimit, float walkSpeed, int jmpHeight, int djmpHeight, float jmpGravScale, AnimatorOverrideController animOverride)
        {
            this.heroName = heroName;
            this.jmpLimit = jmpLimit;
            this.walkSpeed = walkSpeed;
            this.jmpHeight = jmpHeight;
            this.djmpHeight = djmpHeight;
            this.jmpGravScale = jmpGravScale;
            this.animOverride = animOverride;
        }
        
        public PlayerStats clone() {
            return new PlayerStats(heroName, jmpLimit, walkSpeed, jmpHeight, djmpHeight, jmpGravScale, animOverride);
        }
    }
}