using Spine;
using Spine.Unity;
using System;
using UnityEngine;
using Core;

namespace Animation
{
    [RequireComponent(typeof(SkeletonAnimation))]
    public class SpineAnimatorController : AnimationController
    {
        [SpineAnimation, SerializeField] private string _idleAnimation;
        [SpineAnimation, SerializeField] private string _walkAnimation;
        [SpineAnimation, SerializeField] private string _runAnimation;
        [SpineAnimation, SerializeField] private string _jumpAnimation;
        [SpineAnimation, SerializeField] private string _attackAnimation;
        private SkeletonAnimation _skeletonAnimation;

        private void Start() => _skeletonAnimation = GetComponent<SkeletonAnimation>();

        protected override void PlayAnimation(AnimationType animationType)
        {
            string animationName = GetAnimationName(animationType);
            if (_skeletonAnimation.AnimationState.GetCurrent(0).Animation.Name == animationName)
                return;
        }

        private string GetAnimationName(AnimationType animationType)
        {
            switch (animationType)
            {
                case AnimationType.Walk: 
                    return _walkAnimation;
                case AnimationType.Idle:
                    return _idleAnimation;
                case AnimationType.Slide:
                    return _runAnimation;
                case AnimationType.Jump:
                    return _jumpAnimation;
                case AnimationType.Atack:
                    return _attackAnimation;
                default:
                    return _idleAnimation;

            }
        }
    }
}
