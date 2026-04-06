using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationID
{
    public static readonly int HasInputID = Animator.StringToHash("HasInput");
    public static readonly int MovementID = Animator.StringToHash("Movement");
    public static readonly int IsRunID = Animator.StringToHash("IsRun");
    public static readonly int JumpID = Animator.StringToHash("Jump");
    public static readonly int IsDeadID = Animator.StringToHash("IsDead");
    public static readonly int DetalAngleID = Animator.StringToHash("DetalAngle");
    public static readonly int VerticalVelocityID = Animator.StringToHash("VerticalVelocity");
    public static readonly int IsFallID = Animator.StringToHash("IsFall");
    public static readonly int IsGroundID = Animator.StringToHash("IsGround");
}
