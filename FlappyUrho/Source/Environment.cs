using Urho;
using Urho.Physics;
using System;
using System.Collections.Generic;

namespace FlappyUrho
{
    public class Environment : Component
    {
        protected override void OnUpdate(float timeStep)
        {
            base.OnUpdate(timeStep);

            if (Global.gameState == GameState.GS_PLAY || Global.gameState == GameState.GS_INTRO)
                Node.Rotate(new Quaternion(0.0f, -timeStep * Global.BAR_SPEED * 0.42f, 0.0f));

        }
    }

}