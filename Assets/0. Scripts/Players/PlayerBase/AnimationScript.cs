using System;
using UnityEngine;

namespace Scripts
{
    /// <summary>
    /// Handle animation for the player
    /// Works in conjunction with the PlayerLoader to load the correct animation controller
    /// </summary>
    public class AnimationScript : MonoBehaviour
    {
        private Animator _anim;
        private Collision _sColl;

        private PlayerLoader _sLoader;
        private AnimatorOverrideController _animOverride;

        #region Unity Events
        private void Start()
        {
            _anim = GetComponent<Animator>();
            _sColl = GetComponentInParent<Collision>();
            //_sLoader = GetComponentInParent<PlayerLoader>();
            
            if (!_anim) Debug.LogWarning("AnimationScript: Missing Animator!");
            
            // Check if it has been loaded. If yes then then apply it now.
            //if(_sLoader.Loaded) ChangeAnim(_sLoader.curPlayerStats.animOverride);
            // Subscribe to event after it has been loaded.
            //_sLoader.OnLoaded += HandleEventLoaded; 
        }
        
        private void Update()
        {
            _anim.SetBool("onground", _sColl.onGround);
        }
        #endregion

        /// <summary>
        /// Animation for when the player has been loaded
        /// </summary>
        private void HandleEventLoaded()
        {
            ChangeAnim(_sLoader.curPlayerStats.animOverride);
        }

        /// <summary>
        /// Set the current state of the animation
        /// </summary>
        /// <param name="s">State to set to</param>
        public void SetState(int s)
        {
            _anim.SetInteger("State", s);
        }

        // <summary>
        // Turn on the trigger in animator
        // </summary>
        // :param Trigger: The trigger to turn on
        public void SetTrigger(string Trigger)
        {
            _anim.SetTrigger(Trigger);
        }

        /// <summary>
        /// Change the current animation override controller
        /// </summary>
        /// <param name="newAnim">AnimationOverrideController to change to</param>
        public void ChangeAnim(AnimatorOverrideController newAnim)
        {
            _animOverride = newAnim;
            _anim.runtimeAnimatorController = newAnim;
        }
        
        /// <summary>
        /// Set the "animating" bool in the animator
        /// </summary>
        /// <param name="Bool">Bool to set animmating to</param>
        public void SetAnimating(bool Bool)
        {
            _anim.SetBool("animating", Bool);
        }

        /// <summary>
        /// Set the speed of the animaton
        /// </summary>
        /// <param name="speed">Speed</param>
        public void SetSpeed(float speed)
        {
            _anim.speed = speed;
        }
        
        public void SetBool(string name, bool value)
        {
            _anim.SetBool(name, value);
        }
    }
}
