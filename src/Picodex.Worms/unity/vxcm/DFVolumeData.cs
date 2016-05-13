using UnityEngine;

using System;
using System.IO;
using System.Collections;

using Picodex.Vxcm;

namespace Picodex.Unity.Vxcm
{
	
	[System.Serializable]
	public sealed class DFVolumeData : VolumeData
	{
        public VXCMVolume volume;

        GeometrySample sample = new GeometrySample();

        public override VolumeRegion Region
        {
            get
            {
                return volume.region;
            }
        }

        /// Gets the color of the specified position.
        /**
		 * \param x The 'x' position of the voxel to get.
		 * \param y The 'y' position of the voxel to get.
		 * \param z The 'z' position of the voxel to get.
		 * \return The color of the voxel.
		 */
        public Color32 GetVoxel(int x, int y, int z)
		{
            volume.Accessor.ProbeValue(new VolumeAddress(x, y, z), ref sample);
            return new Color32(sample.distanceField, 0, 0, 255);
        }
		
		/// Sets the color of the specified position.
		/**
		 * \param x The 'x' position of the voxel to set.
		 * \param y The 'y' position of the voxel to set.
		 * \param z The 'z' position of the voxel to set.
		 * \param quantizedColor The color the voxel should be set to.
		 */
		public void SetVoxel(int x, int y, int z, Color32 color)
		{
            Debug.Assert(false);
		}

		public override void CommitChanges()
		{
			if(!IsVolumeHandleNull())
			{
				if(writePermissions == WritePermissions.ReadOnly)
				{
					throw new InvalidOperationException("Cannot commit changes to read-only voxel database (" + fullPathToVoxelDatabase +")");
				}
			}
		}
		
		public override void DiscardChanges()
		{
		
		}
		
		/// \cond
		protected override void InitializeEmptyVolume(VolumeRegion region)
		{		
			// We check 'mVolumeHandle' instead of 'volumeHandle' as the getter for the latter will in turn call this method.
			Debug.Assert(volume == null, "Volume should be null prior to initializing volume");

			if(!initializeAlreadyFailed) // If it failed before it will fail again - avoid spamming error messages.
			{
				try
				{
                    volume = new VXCMVolume(region,1,-2,2);

                    // Create an empty region of the desired size.
                    //volumeHandle = CubiquityDLL.NewEmptyDFVolume(region.lowerCorner.x, region.lowerCorner.y, region.lowerCorner.z,
                    //	region.upperCorner.x, region.upperCorner.y, region.upperCorner.z, fullPathToVoxelDatabase, DefaultBaseNodeSize);
                }
				catch(VXCMException exception)
				{
					volumeHandle = null;
					initializeAlreadyFailed = true;
					Debug.LogException(exception);
					Debug.LogError("Failed to open voxel database '" + fullPathToVoxelDatabase + "'");
				}
			}
		}
		/// \endcond

		/// \cond
		protected override void InitializeExistingCubiquityVolume()
		{				
			// We check 'mVolumeHandle' instead of 'volumeHandle' as the getter for the latter will in turn call this method.
			Debug.Assert(mVolumeHandle == null, "Volume handle should be null prior to initializing volume");

			if(!initializeAlreadyFailed) // If it failed before it will fail again - avoid spamming error messages.
			{
				try
				{
					// Create an empty region of the desired size.
				//	volumeHandle = CubiquityDLL.NewDFVolumeFromVDB(fullPathToVoxelDatabase, writePermissions, DefaultBaseNodeSize);
				}
				catch(VXCMException exception)
				{
					volumeHandle = null;
					initializeAlreadyFailed = true;
					Debug.LogException(exception);
					Debug.LogError("Failed to open voxel database '" + fullPathToVoxelDatabase + "'");
				}
			}
		}
		/// \endcond
		
		/// \cond
		public override void ShutdownCubiquityVolume()
		{
			if(!IsVolumeHandleNull())
			{
				// We only save if we are in editor mode, not if we are playing.
				bool saveChanges = (!Application.isPlaying) && (writePermissions == WritePermissions.ReadWrite);
				
				if(saveChanges)
				{
					CommitChanges();
				}
				else
				{
					DiscardChanges();
				}
				
				//CubiquityDLL.DeleteVolume(volumeHandle.Value);
				volumeHandle = null;
			}
		}
		/// \endcond
	}
}