using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using Picodex.Vxcm;

namespace Picodex
{

    [CreateAssetMenu(fileName = "Volume", menuName = "Picodex/Volume", order = 1)]
    public class DFVolume : VXCMVolume
	{

  //      protected void InitializeEmptyVolume(Vector3i size)
  //      {
  //           Debug.Assert(volume == null, "Volume should be null prior to initializing volume");

  //          if (!initializeAlreadyFailed) // If it failed before it will fail again - avoid spamming error messages.
  //          {
  //              try
  //              {
  //                  volume = new VXCMVolume(size, 1, -2, 2);
  //              }
  //              catch (VXCMException exception)
  //              {
  //                  //volumeHandle = null;
  //                  initializeAlreadyFailed = true;
  //                  Debug.LogException(exception);
  //                  Debug.LogError("Failed to open voxel database '" + fullPathToVoxelDatabase + "'");
  //              }
  //          }
  //      }

  //      /**
		// * Writes the current state of the voxels into the voxel database.
		// * 
		// * 
		// */
  //      //public virtual void CommitChanges();

  //      //public virtual void DiscardChanges();

  //      private void Awake()
		//{
		//	// Make sure the Cubiquity library is installed.
		//	//Installation.ValidateAndFix();

		//	RegisterPath();
		//}
		
		//private void OnEnable()
		//{	
		//	// We should reset this flag from time-to-time incase the user has fixed the issue. This 
		//	// seems like an appropriate place as the user may fix the issue aand then reload the scene.
		//	initializeAlreadyFailed = false;

		//	// It would seem intuitive to open and initialise the voxel database from this function. However, there seem to be 
		//	// some problems with this approach.
		//	//		1. OnEnable() is (sometimes?) called when simply clicking on the asset in the project window. In this scenario
		//	// 		we don't really want/need to connect to the database.
		//	//		2. OnEnable() does not seem to be called when a volume asset is dragged onto an existing volume, and this is
		//	// 		the main way of setting which data a volume should use.
		//}
		
		//private void OnDisable()
		//{
		//	// Note: For some reason this function is not called when transitioning between edit/play mode if this scriptable 
		//	// object has been turned into an asset. Therefore we also call Initialize...()/Shutdown...() from the Volume class.
		//	// Aditional: Would we rather do this in OnDestoy()? It would give better behaviour with read-only volumes as these
		//	// can still have temporary changes which are lost when the volume is shutdown. It may be that the user would prefer
		//	// such temporary changes to survive a disable/enable cycle.
		//	//ShutdownCubiquityVolume();
		//}
		
		//private void OnDestroy()
		//{
		//	// If the voxel database was created in the temporary location
		//	// then we can be sure the user has no further use for it.
		//	if(basePath == VoxelDatabasePaths.Temporary)
		//	{
		//		File.Delete(fullPathToVoxelDatabase);
				
		//		if(File.Exists(fullPathToVoxelDatabase))
		//		{
		//			Debug.LogWarning("Failed to delete voxel database from temporary cache");
		//		}
		//	}

		//	UnregisterPath();
		//}

		//private void RegisterPath()
		//{
		//	// This function may be called before the object is poroperly initialised, in which case
		//	// the path may be empty. There's no point in checking for duplicate entries of an empty path.
		//	if(!String.IsNullOrEmpty(relativePathToVoxelDatabase))
		//	{
		//		int instanceID = GetInstanceID();

		//		// Find out wherther the path is aready being used by an instance of VolumeData.
		//		int existingInstanceID;
		//		if(pathsAndAssets.TryGetValue(fullPathToVoxelDatabase, out existingInstanceID))
		//		{
		//			// It is being used, but this function could be called multiple tiomes so maybe it's being used by us?
		//			if(existingInstanceID != instanceID)
		//			{
		//				// It's being used by a different instance, so warn the user.
		//				// In play mode the best we can do is give the user the instance IDs.
		//				string assetName = "Instance ID = " + instanceID;
		//				string existingAssetName = "Instance ID = " + existingInstanceID;

		//				// But in the editor we can try and find assets for them.
		//				#if UNITY_EDITOR
		//				assetName = AssetDatabase.GetAssetPath(instanceID);
		//				existingAssetName = AssetDatabase.GetAssetPath(existingInstanceID);
		//				#endif

		//				// Let the user know what has gone wrong.
		//				string warningMessage = "Duplicate volume data detected! Did you attempt to duplicate or clone an existing asset? " +
		//					"If you want multiple volume data instances to share a voxel database then they must all be set to read-only. " +
		//					"Please see the Cubiquity for Unity3D user manual and API documentation for more information. " +
		//					"\nBoth '" + existingAssetName + "' and '" + assetName + "' reference the voxel database called '" + fullPathToVoxelDatabase + "'." +
		//					"\nIt is recommended that you delete/destroy '" + assetName + "'." +
		//					"\nNote: If you see this message regarding an asset which you have already deleted then you may need to close the scene and/or restart Unity.";
		//				Debug.LogWarning(warningMessage);
		//			}
		//		}
		//		else
		//		{
		//			// No VolumeData instance is using this path yet, so register it for ourselves. However, we only need to register if the volume data
		//			// has write permissions, because multiple volume datas can safely share a single voxel database in read-only mode. Note that this 
		//			// logic is not bulletproof because, for example, a volume data could open a .vdb in read-only mode (hence not registering it) and
		//			// another could then open it in read-write mode. But it would be caught if the volume datas were created in the other order. In
		//			// general we are mostly concerned with the user duplicating in the Unity editor, for which this logic should be sufficient.
		//			if(writePermissions == WritePermissions.ReadWrite)
		//			{
		//				pathsAndAssets.Add(fullPathToVoxelDatabase, instanceID);
		//			}
		//		}
		//	}
		//}

		//private void UnregisterPath()
		//{
		//	// Remove the path entry from our duplicate-checking dictionary.
		//	// This could fail, e.g. if the user does indeed create two instance with the same filename
		//	// then deleting the first will remove the entry which then won't exist when deleting the second.
		//	pathsAndAssets.Remove(fullPathToVoxelDatabase);
		//}

		///// \cond
		////protected abstract void InitializeEmptyVolume(VolumeRegion region);
		////protected abstract void InitializeExistingCubiquityVolume();
		////public abstract void ShutdownCubiquityVolume();
		///// \endcond
	}
}
