using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

using UnityEngine;

using Picodex.Math;
using Picodex.Vxcm;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Picodex.Volume
{
    [ExecuteInEditMode]
    public class DFVolume : MonoBehaviour
    {
        [SerializeField]
        private DFVolumeData mData = null;

        public DFVolumeData data
        {
            get { return this.mData; }
            set
            {
                if (this.mData != value)
                {
                    UnregisterVolumeData();
                    this.mData = value;
                    RegisterVolumeData();

                    Resolution = mData.Region.size;

                    // Delete the octree, so that next time Update() is called a new octree is constructed to match the new volume data.
                    RequestFlushInternalData();
                }
            }
        }
        [SerializeField]
        private VolumeAddress Resolution;


        protected void OnValidate()
        {
            if (Resolution != mData.Region.size)
            {
                this.mData.Resize(Resolution);
            }
        }

        /// Indicates whether the mesh representation is currently up to date with the volume data.
        /**
		 * Note that this property may fluctuate rapidly during real-time editing as the system tries to keep up with the users
		 * modifications, and also that it may lag a few frames behind the true syncronization state.
		 * 
		 * \sa OnMeshSyncComplete, OnMeshSyncLost
		 */
        public bool isMeshSyncronized
        {
            get { return mIsMeshSyncronized; }
            protected set
            {
                // Check if the state of the mesh sync variable has actually changed.
                if (mIsMeshSyncronized != value)
                {
                    // If so update it.
                    mIsMeshSyncronized = value;

                    // And fire the appropriate event. The isMeshSyncronized flag works in edit mode,
                    // but we only fire the events in play mode (unless we find an edit-mode use too?)
                    if (Application.isPlaying)
                    {
                        if (mIsMeshSyncronized)
                        {
                            if (OnMeshSyncComplete != null) { OnMeshSyncComplete(); }
                        }
                        else
                        {
                            if (OnMeshSyncLost != null) { OnMeshSyncLost(); }
                        }
                    }
                }
            }
        }
        private bool mIsMeshSyncronized = false;

        /// Delegate type used by OnMeshSyncComplete() and OnMeshSyncLost()
        public delegate void MeshSyncAction();
        /// This event is fired once the mesh representation is up-to-date with the volume data.
        /**
		 * The process of keeping the mesh data syncronized to the volume data is computationally expensive, and it is quite possible for the
		 * mesh to lag behind. This is particularly common when fresh volume data is first assigned as it can take a few seconds for the initial
		 * mesh to be generated. If you wish to wait for the mesh to be generated before (e.g.) spawning your player object then you can use
		 * this event for this purpose.
		 * 
		 * The mesh can also lag beind during intensive editing operations, and this can cause a series of OnMeshSyncComplete events to occur
		 * as the system repeatedly catches up. Therefore, in the previous player-spawning example you would probably want to disconnect the
		 * event after the first one has occured.
		 * 
		 * Please see MeshSyncHandler.cs in the provided examples for a demonstration of usage.
		 * 
		 * \sa isMeshSyncronized, OnMeshSyncLost
		 */
        public event MeshSyncAction OnMeshSyncComplete;

        /// This event is fired if the mesh representation is no longer up-to-date with the volume data.
        /**
		 * Syncronization between the mesh and the volume data will be lost when you first assign new volume data, and also during editing
		 * operations.
		 * 
		 * Please see MeshSyncHandler.cs in the provided examples for a demonstration of usage.
		 * 
		 * \sa isMeshSyncronized, OnMeshSyncComplete
		 */
        public event MeshSyncAction OnMeshSyncLost;

        /// Sets an upper limit on the rate at which the mesh representation is updated to match the volume data.
        /**
		 * %Cubiquity continuously checks whether the the mesh representation (used for rendering and physics) is synchronized with the underlying
		 * volume data. Such synchronization can be lost whenever the volume data is modified, and %Cubiquity will then regenerate the mesh. This
		 * regeneration process can take some time, and so typically you want to spread the regeneration over a number of frames.
		 *
		 * Internally %Cubiquity breaks down the volume into a number regions each corresponding to an octree node, and these can be resynchronized
		 * individually. Therefore this property controls how many of the octree nodes will be resynchronized each frame. A small value will result
		 * in a better frame rate when modifications are being performed, but at the possible expense of the rendered mesh noticeably lagging behind 
		 * the modifications which are being performed.
		 * 
		 * NOTE: This property is currently hidden from the user until we have a better understanding of how it should behave. For example, should
		 * that same value be used in edit mode vs. play mode? What if there is/isn't a collision mesh? Or what if we want to syncronize every 'x'
		 * updates rather than 'x' times per update?
		 */
        /// \cond
        protected uint maxSyncOperationsInPlayMode = 4;
        protected uint maxSyncOperationsInEditMode = 16; // Can be higher than in play mode as we have no collision mehses
                                                         /// \endcond

        // The root node of our octree. It is protected so that derived classes can use it, but users
        // are not supposed to create derived classes themselves so we hide this property from the docs.
        /// \cond
        [System.NonSerialized]
        protected GameObject rootOctreeNodeGameObject;
        /// \endcond

        // Used to check when the game object changes layer, so we can move the children to match.
        private int previousLayer = -1;

        // Used to catch the user using the same volume data for multiple volumes (which they should not do).
        // It's not a really robust approach but it works well enough and only serves to issue a warning anyway.
        private static Dictionary<int, int> volumeDataAndVolumes = new Dictionary<int, int>();

        private bool flushRequested;

        // ------------------------------------------------------------------------------
        // These editor-only functions are used to emulate repeated calls to Update() in edit mode. Setting the '[ExecuteInEditMode]' attribute does cause
        // Update() to be called automatically in edit mode, but it only happens in response to user-driven events such as moving the mouse in the editor
        // window. We want to support background loading of our terrain and so we hook into the 'EditorApplication.update' event for this purpose.
        // ------------------------------------------------------------------------------
#if UNITY_EDITOR

        private int editModeUpdates = 0;

        // Public so that we can manually drive it from the editor as required,
        // but user code should not do this so it's hidden from the docs.
        /// \cond
        public void ForceUpdate()
        {
            Update();
        }

        /// \cond
        public void EditModeUpdateHandler() // Public so we can call it from Editor scripts
        {
          //  Picodex.DebugUtils.Assert(Application.isPlaying == false, "EditModeUpdateHandler() should never be called in play mode!");

            if (enabled)
            {
                if (isMeshSyncronized)
                {
                    if(editModeUpdates % 20 == 0)
                    {
                        //Debug.Log("Low freq update");
                        ForceUpdate();
                        //SceneView.RepaintAll();
                    }
                }
                else
                {
                    //Debug.Log("High freq update");
                    ForceUpdate();
                    //SceneView.RepaintAll();
                }
            }

            editModeUpdates++;
        }
        /// \endcond
#endif
        // ------------------------------------------------------------------------------

        void Awake()
        {
            RegisterVolumeData();
        }

        void OnEnable()
        {
            // We have taken steps to make sure that our octree does not get saved to disk or persisted between edit/play mode,
            // but it will still survive if we just disable and then enable the volume. This is because the OnDisable() and
            // OnEnable() methods do now allow us to modify our game object hierarchy. Note that this disable/enable process
            // may also happen automatically such as during a script reload? Requesting a flush of the octree is the safest option.
            RequestFlushInternalData();
        }

        void OnDisable()
        {
            // Ideally the VolumeData would handle it's own initialization and shutdown, but it's OnEnable()/OnDisable() methods don't seems to be
            // called when switching between edit/play mode if it has been turned into an asset. Therefore we do it here as well just to be sure.
            if (data != null)
            {
             //   data.ShutdownCubiquityVolume();
            }
        }

        void OnDestroy()
        {
            UnregisterVolumeData();
        }

        // Public as the editor sometimes needs to flush the internal data,
        // but user code should not do this so it's hidden from the docs.
        /// \cond
        public void RequestFlushInternalData()
        {
            flushRequested = true;
        }
        /// \endcond

        private void FlushInternalData()
        {
            // It should be enough to delete the root octree node in this function but we're seeing cases 
            // of octree nodes surviving the transition between edit and play modes. I'm not quite sure 
            // why, but the approach below of deleting all child objects seems to solve the problem.

            // Find all the child objects 
            List<GameObject> childObjects = new List<GameObject>();
            foreach (Transform childTransform in gameObject.transform)
            {
                childObjects.Add(childTransform.gameObject);
            }

            // Destroy all children
            foreach (GameObject childObject in childObjects)
            {
                //Impl.Utility.DestroyOrDestroyImmediate(childObject);
            }

            rootOctreeNodeGameObject = null;
        }

      //  protected abstract bool SynchronizeVolume(uint maxSyncOperations);

        private void Update()
        {
            if (flushRequested)
            {
                FlushInternalData();
                flushRequested = false;

                // It seems prudent to return at this point, and leave the actual updating to the next call of this function.
                // This is because we've just destroyed a bunch of stuff by flushing and Unity actually defers Destroy() until
                // later in the frame. It actually seems t work ok without the return, but it makes me feel a little safer.
                return;
            }

            // Check whether the gameObject has been moved to a new layer.
            if (gameObject.layer != previousLayer)
            {
                // If so we update the children to match and then clear the flag.
                gameObject.SetLayerRecursively(gameObject.layer);
                previousLayer = gameObject.layer;
            }

            // Set shader parameters.
            DFVolumeRenderer volumeRenderer = gameObject.GetComponent<DFVolumeRenderer>();
            if (volumeRenderer != null)
            {
                if (volumeRenderer.material != null)
                {
                    VolumeAddress volumeSize = (data.Region.max - data.Region.min);
                    volumeSize.x++; volumeSize.y++; volumeSize.z++;
                    volumeRenderer.material.SetVector("_VolumeSize", (Vector3)volumeSize);

                    volumeRenderer.material.SetMatrix("_World2Volume", transform.worldToLocalMatrix);
                }
            }

            //if (data != null && data.volumeHandle.HasValue)
            //{
            //    // When we are in game mode we limit the number of nodes which we update per frame, to maintain a nice.
            //    // framerate The Update() method is called repeatedly and so over time the whole mesh gets syncronized. 
            //    if (Application.isPlaying)
            //    {
            //        isMeshSyncronized = SynchronizeVolume(maxSyncOperationsInPlayMode);
            //    }
            //    else
            //    {
            //        isMeshSyncronized = SynchronizeVolume(maxSyncOperationsInEditMode);
            //    }
            //}
        }
        /// \endcond

        /* \param data The volume data which should be attached to the construced volume.

         * \param addRenderer Specifies whether a renderer component should be added so that the volume is displayed.

         * \param addCollider Specifies whether a collider component should be added so that the volume can participate in collisions.

         */
		public static GameObject CreateGameObject(DFVolumeData data, bool addRenderer, bool addCollider)
        {
            // Create our main game object representing the volume.
            GameObject volumeGameObject = new GameObject("DFVolume");

            //Add the required volume component.
            DFVolume dfVolume = volumeGameObject.GetOrAddComponent<DFVolume>();

            // Set the provided data.
            dfVolume.data = data;

            // Add the renderer and collider if desired.
            if (addRenderer) { volumeGameObject.AddComponent<DFVolumeRenderer>(); }
          //  if (addCollider) { volumeGameObject.AddComponent<DFVolumeCollider>(); }

            // Return the created object
            return volumeGameObject;
        }

        // It seems that we need to implement this function in order to make the volume pickable in the editor.
        // It's actually the gizmo which get's picked which is often bigger than than the volume (unless all
        // voxels are solid). So somtimes the volume will be selected by clicking on apparently empty space.
        // We shold try and fix this by using raycasting to check if a voxel is under the mouse cursor?
        void OnDrawGizmos()
        {
            if (data != null)
            {
                // Compute the size of the volume.
                int width = (data.Region.max.x - data.Region.min.x) + 1;
                int height = (data.Region.max.y - data.Region.min.y) + 1;
                int depth = (data.Region.max.z - data.Region.min.z) + 1;
                float offsetX = width / 2;
                float offsetY = height / 2;
                float offsetZ = depth / 2;

                // The origin is at the centre of a voxel, but we want this box to start at the corner of the voxel.
                Vector3 halfVoxelOffset = new Vector3(0.5f, 0.5f, 0.5f);

                // + new Vector3(offsetX, offsetY, offsetZ)
                // Draw an invisible box surrounding the olume. This is what actually gets picked.
                Gizmos.color = new Color(1.0f, 1.0f, 0.0f, 0.9f);
                Gizmos.DrawWireCube(transform.position , new Vector3(width, height, depth)); //  - halfVoxelOffset
            }
        }

        // Public so that we can manually drive it from the editor as required,
        // but user code should not do this so it's hidden from the docs.
        /// \cond
        public void OnGUI()
        {
            // This code doesn't belong in Volume? There should
            // probably be one global copy of this, not one per volume.

            /*GUILayout.BeginArea(new Rect(10, 10, 300, 300));
            GUI.skin.label.alignment = TextAnchor.MiddleLeft;
            string debugPanelMessage = "Cubiquity Debug Panel\n";
            if(isMeshSyncronized)
            {
                debugPanelMessage += "Mesh sync: Completed";
            }
            else
            {
                debugPanelMessage += "Mesh sync: In progress...";
            }
            GUILayout.Box(debugPanelMessage);
            GUILayout.EndArea();*/
        }
        /// \endcond

        private void RegisterVolumeData()
        {
            if (mData != null)
            {
                int volumeID = GetInstanceID();
                int volumeDataID = mData.GetInstanceID();

                int existingVolumeID;
                if (volumeDataAndVolumes.TryGetValue(volumeDataID, out existingVolumeID))
                {
                    if (existingVolumeID != volumeID)
                    {
                        // It's being used by a different instance, so warn the user.
                        // In play mode the best we can do is give the user the instance IDs.
                        string volumeName = "Instance ID = " + volumeID;
                        string existingVolumeName = "Instance ID = " + existingVolumeID;
                        string volumeDataName = "Instance ID = " + volumeDataID;

                        // But in the editor we can try and find names for them.
#if UNITY_EDITOR
						UnityEngine.Object volume = EditorUtility.InstanceIDToObject(volumeID);
						if(volume) volumeName = volume.name;

                        UnityEngine.Object existingVolume = EditorUtility.InstanceIDToObject(existingVolumeID);
						if(existingVolume) existingVolumeName = existingVolume.name;

						volumeDataName = AssetDatabase.GetAssetPath(volumeDataID);
#endif

                        // Let the user know what has gone wrong.
                        string warningMessage = "Multiple use of volume data detected! Did you attempt to duplicate or clone an existing volume? " +
                            "Each volume data should only be used by a single volume - please see the Cubiquity for Unity3D user manual and API documentation for more information. " +
                            "\nBoth '" + existingVolumeName + "' and '" + volumeName + "' reference the volume data called '" + volumeDataName + "'." +
                            "\nNote: If you see this message regarding an asset which you have already deleted then you may need to close the scene and/or restart Unity.";
                        Debug.LogWarning(warningMessage);
                    }
                }
                else
                {
                    volumeDataAndVolumes.Add(volumeDataID, volumeID);
                }
            }
        }

        private void UnregisterVolumeData()
        {
            if (mData != null)
            {
                // Remove the volume data entry from our duplicate-checking dictionary.
                // This could fail, e.g. if the user does indeed create two volumes with the same volume data
                // then deleting the first will remove the entry which then won't exist when deleting the second.
                volumeDataAndVolumes.Remove(mData.GetInstanceID());
            }
        }

    }
}