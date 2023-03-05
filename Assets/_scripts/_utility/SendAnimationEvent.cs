using UnityEngine;

public class SendAnimationEvent : MonoBehaviour
{
    public Player player;

    public void PlayFootstepSound()
    {
        if (player == null) { return; }
        player.PlayFootstep();
    }

    //public void AnimatorEnumEvent(AnimationEvent _event)
    //{
    //    if (player == null) { return; }
    //    player.AnimatorEvent(_event);
    //}

    //public void AnimatorEvent(string _event)
    //{
    //    if (player == null) { return; }
    //    player.AnimatorEvent(_event);
    //}

    //public void AnimatorAttackEvent()
    //{
    //    if (player == null) { return; }
    //    player.AnimatorAttackEvent();
    //}

    //public void PlaySound(string _type="idle")
    //{
    //    if (player == null) { return; }
    //    player.PlaySound(_type);
    //}

    //public void PlaySoundType(EnemySound _type)
    //{
    //    if (player == null) { return; }

    //    player.PlaySoundType(_type);
    //    //player.PlaySound(EnumGroups.EnemySoundToString(_type));
    //}
}