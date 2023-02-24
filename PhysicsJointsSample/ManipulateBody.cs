using System;
using System.Collections.Generic;
using System.Text;
using BulletSharp;
using Evergine.Framework;
using Evergine.Framework.Graphics;
using Evergine.Framework.Physics3D;

namespace PhysicsJointsSample
{
    public class ManipulateBody : Behavior
    {
        RigidBody3D selectedRigidBody;
        bool isKinematic;

        protected override void Update(TimeSpan gameTime)
        {
            var camera = this.Managers.RenderManager?.ActiveCamera3D;

            var mouse = camera.Display?.MouseDispatcher;
            if (mouse != null)
            {
                var btn = mouse.ReadButtonState(Evergine.Common.Input.Mouse.MouseButtons.Left);
                if (btn == Evergine.Common.Input.ButtonState.Pressing)
                {
                    var pos = mouse.Position.ToVector2();

                    camera.CalculateRay(ref pos, out var ray);

                    var hitResult = this.Managers.PhysicManager3D.RayCast(ref ray, 100);
                    if (hitResult.Succeeded)
                    {
                        var bodyComponent = hitResult.PhysicBody.BodyComponent;


                        if (bodyComponent is RigidBody3D rigidBody)
                        {
                            this.selectedRigidBody = rigidBody;
                            this.isKinematic = rigidBody.PhysicBodyType == RigidBodyType3D.Kinematic;
                        }
                    }
                }
                else if (btn == Evergine.Common.Input.ButtonState.Pressed)
                {
                    if (this.selectedRigidBody != null)
                    {
                        var delta = -mouse.PositionDelta.ToVector2();
                        var delta3D = new Evergine.Mathematics.Vector3(0, delta.Y, delta.X);

                        if (this.isKinematic)
                        {
                            this.selectedRigidBody.Transform3D.Position += delta3D * 0.01f;  
                        }
                        else
                        {
                            this.selectedRigidBody.ApplyImpulse(delta3D* 0.1f);
                        }
                    }
                }
                else if (btn == Evergine.Common.Input.ButtonState.Releasing)
                {
                    this.selectedRigidBody = null;
                }
            }
        }
    }
}
