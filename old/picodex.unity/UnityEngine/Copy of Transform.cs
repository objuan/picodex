using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine
{
    public enum TransformSpace
    {
        Parent = 0,
        World,
        Local
    }


    // albero di trasformazioni

    public class Transform : Component, System.Collections.IEnumerable
    {
        internal Axiom.Core.SceneNode _node = null;

        private List<Transform> childs = new List<Transform>();
        private Transform _parent = null;// The parent of the transform. 

        // LINK a Trasform with a parent trasform
        public Transform parent
        {
            get { return _parent; }
            set {
                _LinkTo(value);
            }
        }

        public int childCount { get { return childs.Count; } }// The number of children the Transform has. 

        //public IEnumerator<T> GetEnumerator()  where T : Transform 
        //{
        //    return null;
        //   // return childs.GetEnumerator<Transform>();
        //}
        public System.Collections.IEnumerator GetEnumerator()
        {
            return childs.GetEnumerator();
        }

        // Returns the topmost transform in the hierarchy. 
        public Transform root {
            get
            {
                Transform parent = this.parent;
                while (parent.parent != null) parent = parent.parent;
                return parent;
            }
        }

        private bool inheritScale = true;
        private bool inheritOrientation = true;
        private bool isYawFixed = true;
        private Vector3 yawFixedAxis = new Vector3(0,1,0);


        private bool needTransformUpdate=false;
        public bool hasChanged = true;// Has the transform changed since the last time the flag was set to 'false'? 

        Vector3 _localScale;
        Vector3 _localPosition;
        Quaternion _localRotation;

        public Vector3 forward;// The blue axis of the transform in world space. 
        //lossyScale The global scale of the object (Read Only). 
        public Vector3 right;       //The red axis of the transform in world space. 
        public Vector3 up;          // The green axis of the transform in world space. 

        private Vector3 derivedScale;
        private Quaternion derivedRotation;   // The rotation of the transform in world space stored as a Quaternion. 
        private Vector3 derivedPosition;     // The position of the transform in world space. 


        private Matrix4x4 _localToWorldMatrix;
        private Matrix4x4 _worldToLocalMatrix;

        // locals
        // Position of the transform relative to the parent transform. 

        public Vector3 localPosition
        {
            get
            {
                return _localPosition;
            }
            set
            {
                _localPosition = value;
                NeedUpdate();
            }
        }

        //The rotation of the transform relative to the parent transform's rotation. 
        public Quaternion localRotation
        {
            get
            {
                return _localRotation;
            }
            set
            {
                _localRotation = value;
                NeedUpdate();
            }
        }

        // The scale of the transform relative to the parent. 
        public Vector3 localScale
        {
            get
            {
                return _localScale;
            }
            set
            {
                _localScale = value;
                NeedUpdate();
            }
        }

        // The rotation as Euler angles in degrees. 
        public Vector3 eulerAngles
        {
            get
            {
                return rotation.eulerAngles;
            }
        }
        // The rotation as Euler angles in degrees relative to the parent transform's rotation. 
        public Vector3 localEulerAngles
        {
            get
            {
                return localRotation.eulerAngles;
            }
        }

        public Quaternion rotation
        {
        	    get
			    {
				    if( hasChanged ) UpdateFromParent();
                    return derivedRotation;
			    }
			    set
			    {
				    if( inheritOrientation && parent != null )
				    {
                        localRotation = parent.rotation.Inverse() * value;
				    }
				    else
				    {
                        localRotation = value;
				    }

                    localRotation.Normalize(); // avoid drift
				    NeedUpdate();
			    }
        }

        virtual public Vector3 position
        {
            get
            {
                if (hasChanged)
                {
                    UpdateFromParent();
                }

                return derivedPosition;
            }
            set
            {
                if (parent != null)
                {
                    localPosition = parent.rotation.Inverse() * (value - parent.position) / parent.scale;
                }
                else
                {
                    localPosition = value;
                }

                NeedUpdate();
            }
        }

        virtual public Vector3 scale
        {
            get
            {
                if (hasChanged)
                {
                    UpdateFromParent();
                }

                return derivedScale;
            }
            set
            {
                if (inheritScale & parent != null)
                {
                    localScale = value / parent.scale;
                }
                else
                {
                    localScale = value;
                }

                NeedUpdate();
            }
        }


        
        //that transforms a point from local space into world space (Read Only). 
        virtual public Matrix4x4 localToWorldMatrix
        {
            get
            {
                //if needs an update from parent or it has been updated from parent
                //yet this hasn't been called after that yet
                if (needTransformUpdate)
                {
                    //derived properties may call Update() if needsParentUpdate is true and this will set needTransformUpdate to true
                    MakeTransform(this.position, this.scale, this.rotation, ref _localToWorldMatrix);
                    _worldToLocalMatrix = _localToWorldMatrix.Inverse();
                    //dont need to update this again until next invalidation
                    needTransformUpdate = false;
                }

                return _localToWorldMatrix;
            }
        }
        // Matrix that transforms a point from world space into local space (Read Only). 
        virtual public Matrix4x4 worldToLocalMatrix
        {
            get
            {
                if (needTransformUpdate)
                {
                    //derived properties may call Update() if needsParentUpdate is true and this will set needTransformUpdate to true
                    MakeTransform(this.position, this.scale, this.rotation, ref _localToWorldMatrix);
                    _worldToLocalMatrix = _localToWorldMatrix.Inverse();
                    //dont need to update this again until next invalidation
                    needTransformUpdate = false;
                }
                return _worldToLocalMatrix;
            }
        }


        public Transform()
        {
            _localRotation = derivedRotation =  Quaternion.Identity;
            _localPosition = derivedPosition = Vector3.Zero;
            _localScale = derivedScale = Vector3.UnitScale;
            up = Vector3.UnitY;
            right = Vector3.UnitX;
            forward = Vector3.UnitZ;
            _localToWorldMatrix = Matrix4x4.Identity;
            _worldToLocalMatrix = Matrix4x4.Identity;
        }

        // Unparents all children. 
        public void DetachChildren ()
        {
            foreach (Transform trx in childs)
                trx.parent = null;
            childs.Clear();
        }

        //Finds a child by name and returns it. 
        public new Transform Find(string name)
        {
            foreach (Transform trx in childs)
            {
                if (trx.name == name) return trx;
            }
            return null;
        }

       // Returns a transform child by index. 
        public Transform GetChild(int index)
        {
            return childs[index];
        }

        // Is this transform a child of parent? 
        public bool IsChildOf(Transform transform)
        {
            return parent == transform;
        }

       // GetSiblingIndex Gets the sibling index. 
       // InverseTransformDirection Transforms a direction from world space to local space. The opposite of Transform.TransformDirection. 
       // InverseTransformPoint Transforms position from world space to local space. 
      //  InverseTransformVector Transforms a vector from world space to local space. The opposite of Transform.TransformVector. 

         // Rotates the transform so the forward vector points at /target/'s current position. 
        public void LookAt(Transform target, Vector3 worldUp)// = Vector3.up)
        {
            throw new Exception("not yet");
          //  SetDirection(target - this.position, relativeTo, localDirection);
        }

        public void LookAt(Vector3 target, TransformSpace relativeTo, Vector3 localDirection)
        {
            SetDirection(target - this.position, relativeTo, localDirection);
        }

        public void LookAt(Vector3 target, TransformSpace relativeTo)
        {
            LookAt(target, relativeTo, Vector3.NegativeUnitZ);
        }

        public void LookAt(Vector3 target, Vector3 worldUp)
        {
            throw new Exception("not yet");
           // LookAt(target, TransformSpace.World, worldUp);
        }

        // ======================================================================

        public void SetDirection(float x, float y, float z, TransformSpace relativeTo, Vector3 localDirectionVector)
        {
            Vector3 dir;
            dir.x = x;
            dir.y = y;
            dir.z = z;
            SetDirection(dir, relativeTo, localDirectionVector);
        }

        public void SetDirection(float x, float y, float z)
        {
            SetDirection(x, y, z, TransformSpace.Local, Vector3.NegativeUnitZ);
        }

        public void SetDirection(float x, float y, float z, TransformSpace relativeTo)
        {
            SetDirection(x, y, z, relativeTo, Vector3.NegativeUnitZ);
        }

        /// <summary>
        ///		Sets the node's direction vector ie it's local -z.
        /// </summary>
        /// <remarks>
        ///		Note that the 'up' vector for the orientation will automatically be
        ///		recalculated based on the current 'up' vector (i.e. the roll will
        ///		remain the same). If you need more control, use the <see cref="Orientation"/>
        ///		property.
        /// </remarks>
        /// <param name="vec">The direction vector.</param>
        /// <param name="relativeTo">The space in which this direction vector is expressed.</param>
        /// <param name="localDirection">The vector which normally describes the natural direction
        ///		of the node, usually -Z.
        ///	</param>
        public void SetDirection(Vector3 vec, TransformSpace relativeTo, Vector3 localDirection)
        {
            // Do nothing if given a zero vector
            if (vec == Vector3.Zero)
            {
                return;
            }

            // Adjust vector so that it is relative to local Z
            Vector3 zAdjustVec;

            if (localDirection == Vector3.NegativeUnitZ)
            {
                zAdjustVec = -vec;
            }
            else
            {
                Quaternion localToUnitZ = localDirection.GetRotationTo(Vector3.UnitZ);
                zAdjustVec = localToUnitZ * vec;
            }

            zAdjustVec.Normalize();

            Quaternion targetOrientation;

            if (isYawFixed)
            {
                Vector3 xVec = yawFixedAxis.Cross(zAdjustVec);
                xVec.Normalize();

                Vector3 yVec = zAdjustVec.Cross(xVec);
                yVec.Normalize();

                targetOrientation = Quaternion.FromAxes(xVec, yVec, zAdjustVec);
            }
            else
            {
                Vector3 xAxis, yAxis, zAxis;

                // Get axes from current quaternion
                // get the vector components of the derived orientation vector
                this.rotation.ToAxes(out xAxis, out yAxis, out zAxis);

                Quaternion rotationQuat;

                if ((zAxis + zAdjustVec).LengthSquared < 0.00000001f)
                {
                    // Oops, a 180 degree turn (infinite possible rotation axes)
                    // Default to yaw i.e. use current UP
                    rotationQuat = Quaternion.FromAngleAxis(Utility.PI, yAxis);
                }
                else
                {
                    // Derive shortest arc to new direction
                    rotationQuat = zAxis.GetRotationTo(zAdjustVec);
                }

                targetOrientation = rotationQuat * localRotation;
            }

            if (relativeTo == TransformSpace.Local || parent != null)
            {
                localRotation = targetOrientation;
            }
            else
            {
                if (relativeTo == TransformSpace.Parent)
                {
                    localRotation = targetOrientation * parent.localRotation.Inverse();
                }
                else if (relativeTo == TransformSpace.World)
                {
                    localRotation = targetOrientation * parent.rotation.Inverse();
                }
            }
        }

        public void SetDirection(Vector3 vec)
        {
            SetDirection(vec, TransformSpace.Local, Vector3.NegativeUnitZ);
        }

        public void SetDirection(Vector3 vec, TransformSpace relativeTo)
        {
            SetDirection(vec, relativeTo, Vector3.NegativeUnitZ);
        }

        /// <summary>
        /// Scales the node, combining its current scale with the passed in scaling factor.
        /// </summary>
        /// <remarks>
        ///	This method applies an extra scaling factor to the node's existing scale, (unlike setScale
        ///	which overwrites it) combining its current scale with the new one. E.g. calling this
        ///	method twice with Vector3(2,2,2) would have the same effect as setScale(Vector3(4,4,4)) if
        /// the existing scale was 1.
        ///
        ///	Note that like rotations, scalings are oriented around the node's origin.
        ///</remarks>
        /// <param name="scale"></param>
        virtual public void ScaleBy(Vector3 factor)
        {
            scale = scale * factor;
            NeedUpdate();
        }

        /// <summary>
        /// Moves the node along the cartesian axes.
        ///
        ///	This method moves the node by the supplied vector along the
        ///	world cartesian axes, i.e. along world x,y,z
        /// </summary>
        /// <param name="scale">Vector with x,y,z values representing the translation.</param>
        virtual public void Translate(Vector3 translate)
        {
            Translate(translate, TransformSpace.Parent);
        }

        /// <summary>
        /// Moves the node along the cartesian axes.
        ///
        ///	This method moves the node by the supplied vector along the
        ///	world cartesian axes, i.e. along world x,y,z
        /// </summary>
        /// <param name="scale">Vector with x,y,z values representing the translation.</param>
        virtual public void Translate(Vector3 translate, TransformSpace relativeTo)
        {
            switch (relativeTo)
            {
                case TransformSpace.Local:
                    // position is relative to parent so transform downwards
                    position += localRotation * translate;
                    break;

                case TransformSpace.World:
                    if (parent != null)
                    {
                        position += (parent.rotation.Inverse() * translate) / parent.scale;
                    }
                    else
                    {
                        position += translate;
                    }

                    break;

                case TransformSpace.Parent:
                    position += translate;
                    break;
            }

            NeedUpdate();
        }

        /// <summary>
        /// Moves the node along arbitrary axes.
        /// </summary>
        /// <remarks>
        ///	This method translates the node by a vector which is relative to
        ///	a custom set of axes.
        ///	</remarks>
        /// <param name="axes">3x3 Matrix containg 3 column vectors each representing the
        ///	X, Y and Z axes respectively. In this format the standard cartesian axes would be expressed as:
        ///		1 0 0
        ///		0 1 0
        ///		0 0 1
        ///		i.e. The Identity matrix.
        ///	</param>
        /// <param name="move">Vector relative to the supplied axes.</param>
        virtual public void Translate(Matrix3 axes, Vector3 move)
        {
            Vector3 derived = axes * move;
            Translate(derived, TransformSpace.Parent);
        }

        /// <summary>
        /// Moves the node along arbitrary axes.
        /// </summary>
        /// <remarks>
        ///	This method translates the node by a vector which is relative to
        ///	a custom set of axes.
        ///	</remarks>
        /// <param name="axes">3x3 Matrix containg 3 column vectors each representing the
        ///	X, Y and Z axes respectively. In this format the standard cartesian axes would be expressed as:
        ///		1 0 0
        ///		0 1 0
        ///		0 0 1
        ///		i.e. The Identity matrix.
        ///	</param>
        /// <param name="move">Vector relative to the supplied axes.</param>
        virtual public void Translate(Matrix3 axes, Vector3 move, TransformSpace relativeTo)
        {
            Vector3 derived = axes * move;
            Translate(derived, relativeTo);
        }


        // Applies a rotation of eulerAngles.z degrees around the z axis, eulerAngles.x degrees around the x axis, and eulerAngles.y degrees around the y axis (in that order). 

        public void Rotate(Vector3 eulerAngles)
        {
            Rotate(Quaternion.Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z), TransformSpace.Local);
        }


        public void Rotate(Quaternion rot, TransformSpace relativeTo)
        {
            rot.Normalize(); // avoid drift

            switch (relativeTo)
            {
                case TransformSpace.Parent:
                    // Rotations are normally relative to local axes, transform up
                    localRotation = rot * localRotation;
                    break;

                case TransformSpace.World:
                    localRotation = localRotation * derivedRotation.Inverse() * rot * derivedRotation;
                    break;

                case TransformSpace.Local:
                    // Note the order of the mult, i.e. q comes after
                    localRotation = localRotation * rot;
                    break;
            }

            NeedUpdate();
        }

        /// <summary>
        /// Rotate the node around the X-axis.
        /// </summary>
        /// <param name="degrees"></param>
        virtual public void Pitch(float degrees, TransformSpace relativeTo)
        {
            Rotate(Vector3.UnitX, degrees, relativeTo);
        }

        /// <summary>
        /// Rotate the node around the X-axis.
        /// </summary>
        /// <param name="degrees"></param>
        virtual public void Pitch(float degrees)
        {
            Rotate(Vector3.UnitX, degrees, TransformSpace.Local);
        }

        /// <summary>
        /// Rotate the node around the Z-axis.
        /// </summary>
        /// <param name="degrees"></param>
        virtual public void Roll(float degrees, TransformSpace relativeTo)
        {
            Rotate(Vector3.UnitZ, degrees, relativeTo);
        }

        /// <summary>
        /// Rotate the node around the Z-axis.
        /// </summary>
        /// <param name="degrees"></param>
        virtual public void Roll(float degrees)
        {
            Rotate(Vector3.UnitZ, degrees, TransformSpace.Local);
        }

        /// <summary>
        /// Rotate the node around the Y-axis.
        /// </summary>
        /// <param name="degrees"></param>
        virtual public void Yaw(float degrees, TransformSpace relativeTo)
        {
            Rotate(Vector3.UnitY, degrees, relativeTo);
        }

        /// <summary>
        /// Rotate the node around the Y-axis.
        /// </summary>
        /// <param name="degrees"></param>
        virtual public void Yaw(float degrees)
        {
            Rotate(Vector3.UnitY, degrees, TransformSpace.Local);
        }

        /// <summary>
        /// Rotate the node around an arbitrary axis.
        /// </summary>
        virtual public void Rotate(Vector3 axis, float degrees, TransformSpace relativeTo)
        {
            Quaternion q = Quaternion.FromAngleAxis(Utility.DegreesToRadians((float)degrees), axis);
            Rotate(q, relativeTo);
        }

        /// <summary>
        /// //  
        /// </summary>
        virtual public void Rotate(Vector3 axis, float degrees)
        {
            Rotate(axis, degrees, TransformSpace.Local);
        }

        // RotateAround Rotates the transform about axis passing through point in world coordinates by angle degrees. 

        public void RotateAround(Vector3 point, Vector3 axis, float angleDegrees)
        {
            Rotate(axis, angleDegrees, TransformSpace.Local);
        }

       // Rotate 
      
        //SetAsFirstSibling Move the transform to the start of the local transform list. 
        //SetAsLastSibling Move the transform to the end of the local transform list. 

        //  Set the parent of the transform. 
        public void SetParent(Transform parent, bool worldPositionStays)
        {
            _parent = parent;
            if (parent != null)
            {
                parent.childs.Add(this);
            }
        }

      //  SetSiblingIndex Sets the sibling index. 
        //TransformDirection Transforms direction from local space to world space. 
        //TransformPoint Transforms position from local space to world space. 
        //TransformVector Transforms vector from local space to world space. 
        //Translate 

        private void UpdateFromParent()
        {
            if (hasChanged)
            {
                if (parent != null)
                {
                    // Update orientation
                    Quaternion parentOrientation = parent.derivedRotation;
                    if (inheritOrientation)
                    {
                        // combine local orientation with parents
                        derivedRotation = parentOrientation * localRotation;
                    }
                    else
                    {
                        // no inheritance
                        derivedRotation = localRotation;
                    }

                    // update scale
                    Vector3 parentScale = parent.derivedScale;
                    if (inheritScale)
                    {
                        // set own scale, just combine as equivalent axes, no shearing
                        derivedScale = parentScale * localScale;
                    }
                    else
                    {
                        // do not inherit parents scale
                        derivedScale = localScale;
                    }

                    // Change position vector based on parent's orientation & scale
                    derivedPosition = parentOrientation * (parentScale * localPosition);

                    // add parents positition to local altered position
                    derivedPosition += parent.derivedPosition;
                }
                else
                {
                    // Root node, no parent
                    derivedRotation = localRotation;
                    derivedPosition = localPosition;
                    derivedScale = localScale;
                }

                // multpliy each times the current orientation
                right = derivedRotation * Vector3.UnitX;
                up = derivedRotation * Vector3.UnitY;
                forward = derivedRotation * Vector3.UnitZ;

                hasChanged = false;

                needTransformUpdate = true;
            }

            //    needParentUpdate = false;
            //    needTransformUpdate = true;
            //    needRelativeTransformUpdate = true;

            //    if (suppressUpdateEvent == false)
            //    {
            //        OnUpdatedFromParent();
            //    }
            //}
        }

        /// <summary>
        /// Internal method for building a Matrix4 from orientation / scale / position.
        /// </summary>
        /// <remarks>
        ///	Transform is performed in the order scale, rotate, translation, i.e. translation is independent
        ///	of orientation axes, scale does not affect size of translation, rotation and scaling are always
        ///	centered on the origin.
        ///	</remarks>
        /// <param name="position"></param>
        /// <param name="scale"></param>
        /// <param name="orientation"></param>
        /// <returns></returns>
        protected void MakeTransform(Vector3 position, Vector3 scale, Quaternion orientation, ref Matrix4x4 destMatrix)
        {
            // Ordering:
            //    1. Scale
            //    2. Rotate
            //    3. Translate

            // Parent scaling is already applied to derived position
            // Own scale is applied before rotation
            Matrix3 rot3x3;
            Matrix3 scale3x3;
            rot3x3 = orientation.ToRotationMatrix();
            scale3x3 = Matrix3.Zero;
            scale3x3.m00 = scale.x;
            scale3x3.m11 = scale.y;
            scale3x3.m22 = scale.z;

            destMatrix = rot3x3 * scale3x3;
            destMatrix.Translation = position;
        }

        public void NeedUpdate(  ) // bool forceParentUpdate
		{
            hasChanged = true;
            //needParentUpdate = true;
            //needChildUpdate = true;
            needTransformUpdate = true;
            //needRelativeTransformUpdate = true;

            //// make sure we are not the root node
            //if( parent != null && ( !isParentNotified || forceParentUpdate ) )
            //{
            //    parent.RequestUpdate( this );
            //    isParentNotified = true;
            //}

            //// all children will be updated shortly
            //childrenToUpdate.Clear();
		}

        public Matrix3 LocalAxes
        {
            get
            {
                // get the 3 unit Vectors
                Vector3 xAxis = Vector3.UnitX;
                Vector3 yAxis = Vector3.UnitY;
                Vector3 zAxis = Vector3.UnitZ;

                // multpliy each times the current orientation
                xAxis = localRotation * xAxis;
                yAxis = localRotation * yAxis;
                zAxis = localRotation * zAxis;

                return new Matrix3(xAxis, yAxis, zAxis);
            }
        }

        public Matrix3 Axes
        {
            get
            {
                // get the 3 unit Vectors
                Vector3 xAxis = Vector3.UnitX;
                Vector3 yAxis = Vector3.UnitY;
                Vector3 zAxis = Vector3.UnitZ;

                // multpliy each times the current orientation
                xAxis = rotation * xAxis;
                yAxis = rotation * yAxis;
                zAxis = rotation * zAxis;

                return new Matrix3(xAxis, yAxis, zAxis);
            }
        }


        // ===========================
        // Axiom link
        protected virtual void _LinkTo(Transform parent)
        {
            _parent = parent;
            if (_node != null && _node.Parent!=null)
            {
                _node.Parent.RemoveChild(_node);
            }
            else
                _node = gameObject.scene._sceneManager.CreateSceneNode();

            if (parent != null)
            {
                parent._node.AddChild(_node);
                //parent._AddChild(this);
                parent.childs.Add(this);

                try
                {
                    //gameObject._OnAttach(gameObject);
                    gameObject._OnAttach();
                }
                catch (Exception e)
                {


                    Debug.LogException(e);
                }
            }
        }

      
    }
}
