using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void BeHit(CombatInteractionConfig interactionConfig,CharacterBase characterBase);
}
