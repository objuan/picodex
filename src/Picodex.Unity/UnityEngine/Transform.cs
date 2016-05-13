using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine
{
    public enum TransformSpace
    {
        /// <summary>
        ///     Transform is relative to the local space.
        /// </summary>
        Local = 0,

        /// <summary>
        ///     Transform is relative to the space of the parent node.
        /// </summary>
        Parent,

        /// <summary>
        ///     Transform is relative to world space.
        /// </summary>
        World
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

        //private bool inheritScale = true;
        //private bool inheritOrientation = true;
        //private bool isYawFixed = true;
        //private Vector3 yawFixedAxis = new Vector3(0,1,0);

        public Vector3 up { 
            get 
            { 
                Vector3 yAxis = Vector3.UnitY;
                yAxis =rotation * yAxis;
                return yAxis;
            } 
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


        // locals
        // Position of the transform relative to the parent transform. 

        public Vector3 localPosition
        {
            get
            {
                return _node.Position;
            }
            set
            {
                _node.Position = value;
            }
        }

        //The rotation of the transform relative to the parent transform's rotation. 
        public Quaternion localRotation
        {
            get
            {
                return _node.Orientation;
            }
            set
            {
                _node.Orientation = value;
            }
        }

        // The scale of the transform relative to the parent. 
        public Vector3 localScale
        {
            get
            {
                return _node.Scale;
            }
            set
            {
                _node.Scale = value;
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
                    return _node.DerivedOrientation;
			    }
			    set
			    {
                    _node.DerivedOrientation = value;
			    }
        }

        virtual public Vector3 position
        {
            get
            {
                return _node.DerivedPosition;
            }
            set
            {
                _node.DerivedPosition = value;
            }
        }

        virtual public Vector3 scale
        {
            get
            {
                return _node.DerivedScale;
            }
            set
            {
                _node.DerivedScale = value;
            }
        }


        
        //that transforms a point from local space into world space (Read Only). 
        virtual public Matrix4x4 localToWorldMatrix
        {
            get
            {
                return _node.FullTransform;
            }
        }
        // Matrix that transforms a point from world space into local space (Read Only). 
        virtual public Matrix4x4 worldToLocalMatrix
        {
            get
            {
                //TODO
                return _node.FullTransform.Inverse();
            }
        }


        public Transform()
        {
            _node = UnityEngine.Platform.UnityContext.Singleton.CurrentScene._sceneManager.CreateSceneNode();

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
        public void LookAt(Transform target)// = Vector3.up)
        {
            LookAt(target.position, Vector3.up);
        }
        public void LookAt(Transform target, Vector3 worldUp)// = Vector3.up)
        {
            LookAt(target.position, worldUp);
        }
        //public void LookAt(Vector3 target, TransformSpace relativeTo, Vector3 localDirection)
        //{
        //    SetDirection(target - this.position, relativeTo, localDirection);
        //}

        //public void LookAt(Vector3 target, TransformSpace relativeTo)
        //{
        //    LookAt(target, relativeTo, Vector3.NegativeUnitZ);
        //}
        public void LookAt(Vector3 target)
        {
            _node.LookAt(target, Axiom.Core.TransformSpace.World, Vector3.forward);
            // LookAt(target, TransformSpace.World, worldUp);
        }

        public void LookAt(Vector3 target, Vector3 worldUp)
        {
            _node.LookAt(target, Axiom.Core.TransformSpace.Local, Vector3.forward);
            // //TODO ??
            _node.LookAt(worldUp, Axiom.Core.TransformSpace.Local, Vector3.up);
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
            SetDirection(x, y, z, TransformSpace.Local, Vector3.CameraForward);
        }

        public void SetDirection(float x, float y, float z, TransformSpace relativeTo)
        {
            SetDirection(x, y, z, relativeTo, Vector3.CameraForward);
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
            _node.SetDirection(vec, (Axiom.Core.TransformSpace)(int)relativeTo, localDirection);
        }

        public void SetDirection(Vector3 vec)
        {
            SetDirection(vec, TransformSpace.Local, Vector3.CameraForward);
        }

        public void SetDirection(Vector3 vec, TransformSpace relativeTo)
        {
            SetDirection(vec, relativeTo, Vector3.CameraForward);
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
            _node.ScaleBy(factor);
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
            _node.Translate(translate, (Axiom.Core.TransformSpace)(int)relativeTo);
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
            _node.Rotate(rot, (Axiom.Core.TransformSpace)(int)relativeTo);
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
            this.parent = parent;
            //if (parent != null)
            //{
            //    parent.childs.Add(this);
            //}
        }

      //  SetSiblingIndex Sets the sibling index. 
        //TransformDirection Transforms direction from local space to world space. 
        //TransformPoint Transforms position from local space to world space. 
        //TransformVector Transforms vector from local space to world space. 
        //Translate 


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

        //public void NeedUpdate(  ) // bool forceParentUpdate
        //{
        //    hasChanged = true;
        //    //needParentUpdate = true;
        //    //needChildUpdate = true;
        //    needTransformUpdate = true;
        //    //needRelativeTransformUpdate = true;

        //    //// make sure we are not the root node
        //    //if( parent != null && ( !isParentNotified || forceParentUpdate ) )
        //    //{
        //    //    parent.RequestUpdate( this );
        //    //    isParentNotified = true;
        //    //}

        //    //// all children will be updated shortly
        //    //childrenToUpdate.Clear();
        //}

    

        // ===========================
        // Axiom link
        // ===========================
        protected virtual void _LinkTo(Transform parent)
        {
            _parent = parent;
            if (_node.Parent!=null)
            {
                _node.Parent.RemoveChild(_node);
            }
           // else
           //     _node = gameObject.scene._sceneManager.CreateSceneNode();

            if (parent != null)
            {
                parent._node.AddChild(_node);
                //parent._AddChild(this);
                parent.childs.Add(this);

                gameObject._BroadcastMessageToComponent("_OnLinkTo", parent, SendMessageOptions.DontRequireReceiver);

                try
                {
                    //gameObject._OnAttach(gameObject);
//                    gameObject._OnAttach();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }

      
    }
}
