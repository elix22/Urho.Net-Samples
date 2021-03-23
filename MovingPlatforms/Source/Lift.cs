using Urho;
using Urho.Physics;
using System;


namespace MovingPlatforms
{
    public class Lift : LogicComponent
    {

        Node liftNode_;
        Node liftButtonNode_;

        Vector3 initialPosition_;
        Vector3 finishPosition_;
        Vector3 directionToFinish_;
        float totalDistance_;
        float maxLiftSpeed_ = 5.0f;
        float minLiftSpeed_ = 1.5f;
        float curLiftSpeed_ = 0.0f;

        float buttonPressedHeight_ = 15.0f;
        bool standingOnButton_ = false;

        // states
        LiftButtonStateType liftButtonState_ = LiftButtonStateType.LIFT_BUTTON_UP;
        enum LiftButtonStateType
        {
            LIFT_BUTTON_UP,
            LIFT_BUTTON_POPUP,
            LIFT_BUTTON_DOWN
        };

        LiftStateType liftState_  = LiftStateType.LIFT_STATE_START;
        enum LiftStateType
        {
            LIFT_STATE_START,
            LIFT_STATE_MOVETO_FINISH,
            LIFT_STATE_MOVETO_START,
            LIFT_STATE_FINISH
        };

        public Lift() { }
        public Lift(IntPtr handle) : base(handle) { }

        public void Initialize(Node liftNode, Vector3 finishPosition)
        {
            // get other lift components
            liftNode_ = liftNode;
            liftButtonNode_ = liftNode_.GetChild("LiftButton", true);

            // positions
            initialPosition_ = liftNode_.WorldPosition;
            finishPosition_ = finishPosition;
            directionToFinish_ = Vector3.Normalize(finishPosition_ - initialPosition_);
            totalDistance_ = (finishPosition_ - initialPosition_).Length;

            // events
            liftButtonNode_.NodeCollisionStart += HandleButtonStartCollision;
            liftButtonNode_.NodeCollisionEnd += HandleButtonEndCollision;

        }

        protected override void OnFixedUpdate(PhysicsPreStepEventArgs e)
        {
            base.OnFixedUpdate(e);
            float timeStep = e.TimeStep;

            Vector3 liftPos = liftNode_.Position;
            Vector3 newPos = liftPos;

            // move lift
            if (liftState_ == LiftStateType.LIFT_STATE_MOVETO_FINISH)
            {
                Vector3 curDistance = finishPosition_ - liftPos;
                Vector3 curDirection = Vector3.Normalize(curDistance);
                float dist = curDistance.Length;
                float dotd = Vector3.Dot(directionToFinish_, curDirection);

                if (dotd > 0.0f)
                {
                    // slow down near the end
                    if (dist < 1.0f)
                    {
                        curLiftSpeed_ *= 0.92f;
                    }
                    curLiftSpeed_ = Math.Clamp(curLiftSpeed_, minLiftSpeed_, maxLiftSpeed_);
                    newPos += curDirection * curLiftSpeed_ * timeStep;
                }
                else
                {
                    newPos = finishPosition_;
                    SetTransitionCompleted(LiftStateType.LIFT_STATE_FINISH);
                }
                liftNode_.Position = newPos;
            }
            else if (liftState_ == LiftStateType.LIFT_STATE_MOVETO_START)
            {
                Vector3 curDistance = initialPosition_ - liftPos;
                Vector3 curDirection = Vector3.Normalize(curDistance);
                float dist = curDistance.Length;
                float dotd = Vector3.Dot(directionToFinish_, curDirection);

                if (dotd < 0.0f)
                {
                    // slow down near the end
                    if (dist < 1.0f)
                    {
                        curLiftSpeed_ *= 0.92f;
                    }
                    curLiftSpeed_ = Math.Clamp(curLiftSpeed_, minLiftSpeed_, maxLiftSpeed_);
                    newPos += curDirection * curLiftSpeed_ * timeStep;
                }
                else
                {
                    newPos = initialPosition_;
                    SetTransitionCompleted(LiftStateType.LIFT_STATE_START);
                }
                liftNode_.Position = newPos;
            }

            // reenable button
            if (!standingOnButton_ &&
                liftButtonState_ == LiftButtonStateType.LIFT_BUTTON_DOWN &&
                (liftState_ == LiftStateType.LIFT_STATE_START || liftState_ == LiftStateType.LIFT_STATE_FINISH))
            {
                liftButtonState_ = LiftButtonStateType.LIFT_BUTTON_UP;
                ButtonPressAnimate(false);
            }
        }

        void SetTransitionCompleted(LiftStateType toState)
        {
            liftState_ = toState;

            // from now on stop recieving events  OnFixedUpdate(PhysicsPreStepEventArgs e)
            ReceiveFixedUpdates = false;
            // adjust button
            if (liftButtonState_ == LiftButtonStateType.LIFT_BUTTON_UP)
            {
                ButtonPressAnimate(false);
            }
        }

        void ButtonPressAnimate(bool pressed)
        {
            if (pressed)
            {
                liftButtonNode_.Position = (liftButtonNode_.Position + new Vector3(0, -buttonPressedHeight_, 0));
            }
            else
            {
                liftButtonNode_.Position = (liftButtonNode_.Position + new Vector3(0, buttonPressedHeight_, 0));
            }
        }


        void HandleButtonStartCollision(NodeCollisionStartEventArgs args)
        {
            standingOnButton_ = true;

            if (liftButtonState_ == LiftButtonStateType.LIFT_BUTTON_UP)
            {
                if (liftState_ == LiftStateType.LIFT_STATE_START)
                {
                    liftState_ = LiftStateType.LIFT_STATE_MOVETO_FINISH;
                    liftButtonState_ = LiftButtonStateType.LIFT_BUTTON_DOWN;
                    curLiftSpeed_ = maxLiftSpeed_;

                    // adjust button
                    ButtonPressAnimate(true);
                    // from now on recieve events  OnFixedUpdate(PhysicsPreStepEventArgs e)
                    ReceiveFixedUpdates = true;
                }
                else if (liftState_ == LiftStateType.LIFT_STATE_FINISH)
                {
                    liftState_ = LiftStateType.LIFT_STATE_MOVETO_START;
                    liftButtonState_ = LiftButtonStateType.LIFT_BUTTON_DOWN;
                    curLiftSpeed_ = maxLiftSpeed_;

                    // adjust button
                    ButtonPressAnimate(true);
                     // from now on recieve events  OnFixedUpdate(PhysicsPreStepEventArgs e)
                    ReceiveFixedUpdates = true;
                }

                // play sound and animation
            }
        }

        void HandleButtonEndCollision(NodeCollisionEndEventArgs args)
        {
            standingOnButton_ = false;

            if (liftButtonState_ == LiftButtonStateType.LIFT_BUTTON_DOWN)
            {
                // button animation
                if (liftState_ == LiftStateType.LIFT_STATE_START || liftState_ == LiftStateType.LIFT_STATE_FINISH)
                {
                    liftButtonState_ = LiftButtonStateType.LIFT_BUTTON_UP;
                    ButtonPressAnimate(false);
                }
            }
        }

    }
}
