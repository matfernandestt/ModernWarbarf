using UnityEngine;

public class MonsterMushroom : BaseMovementAI
{
    private void Update()
    {
        Movement();
        ApplyMovement();
    }
}
