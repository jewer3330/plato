
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Skill
{
    [CreateAssetMenu(fileName = "Skill", menuName = "Skill/NewConfig", order = 1)]
    public class EventNodeScriptableObject : ScriptableObject
    {
        public GameObject skillOwner;
        public List<EventNode> eventNodes = new List<EventNode>();
        public List<PlayAnimationNode> animationNodes = new List<PlayAnimationNode>();
        public List<PlayEffectNode> playEffectNodes = new List<PlayEffectNode>();
        public List<FireBulletNode> fireBulletNodes = new List<FireBulletNode>();
        public List<TriggerAnimationNode> triggerAnimationNodes = new List<TriggerAnimationNode>();
        public List<TriggerEffectNode> triggerEventNodes = new List<TriggerEffectNode>();
        public List<AttackNode> attackNodes = new List<AttackNode>();
        public List<AttackCircleNode> attackCircleNodes = new List<AttackCircleNode>();


    }
}
