using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

using UnityEngine;
using UnityEngine.Rendering;

using Picodex.Vxcm;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Picodex
{
    [AddComponentMenu("Vxcm/DFVolumeExtra")]
    [ExecuteInEditMode]
    public class DFVolumeExtra : MonoBehaviour
    {
        //DFVolume volume;
        //PathFinder pathFinder;

        //[System.NonSerialized]
        //public VolumePath volumePath;

        //// 
        //Vector3i gridToObjectOffset;
        //Matrix4x4 localToWorldMatrix;

        public VolumeNavigator volumeNavigator;

        void Start()
        {
            if (GetComponent<DFVolumeFilter>() == null) return;
            if (GetComponent<DFVolumeRenderer>() == null) return;

            //volume = GetComponent<DFVolumeFilter>().volume;
            //gridToObjectOffset = -volume.objectToGridOffset;
            //localToWorldMatrix = transform.localToWorldMatrix;

            //pathFinder = new PathFinder(volume);
            //pathFinder.OnEnd += PathFinder_OnEnd;

         
            //volumePath = new VolumePath();
        }

        // multithread
        //private void PathFinder_OnEnd(PathFinder pathFinder)
        //{
        //    VolumePathPoint p;
        //    volumePath.Clear();
        //    foreach (Vector3i point in pathFinder.path)
        //    {
        //        p = new VolumePathPoint();
        //        p.set(point, gridToObjectOffset, localToWorldMatrix);
        //        volumePath.pointList.Add(p);
        //    }
        //}

        //public void CreatePathW(Vector3 fromPoint, Vector3 toPoint)
        //{
        //    Vector3 _fromPointObj = transform.worldToLocalMatrix.MultiplyPoint(fromPoint);
        //    Vector3 _toPointObj = transform.worldToLocalMatrix.MultiplyPoint(toPoint);

        //    Vector3i _fromPoint = new Vector3i(_fromPointObj) + volume.objectToGridOffset;
        //    Vector3i _toPoint = new Vector3i(_toPointObj) + volume.objectToGridOffset;
        //    //  Vector3i gridToObjectOffset = -volume.objectToGridOffset;

        //    VolumePath path = new VolumePath();
        //    pathFinder.Start(new Vector3i(_fromPoint), new Vector3i(_toPoint));

        //    //VolumePathPoint p;
        //    //foreach (Vector3i point in pathFinder.path)
        //    //{
        //    //    p = new VolumePathPoint();
        //    //    p.set(point, gridToObjectOffset,transform.localToWorldMatrix);
        //    //    path.pointList.Add(p);
        //    //}
        //    // return path;
        //}
                                  
        //public void SetNavTarget(Vector3 toPoint)
        //{
        //    Vector3 _fromPointObj = transform.worldToLocalMatrix.MultiplyPoint(fromPoint);
        //    Vector3 _toPointObj = transform.worldToLocalMatrix.MultiplyPoint(toPoint);

        //    Vector3i _fromPoint = new Vector3i(_fromPointObj) + volume.objectToGridOffset;
        //    Vector3i _toPoint = new Vector3i(_toPointObj) + volume.objectToGridOffset;

        //    navigator.

        //    Vector3i point;
        //    volumePath.Clear();
        //    VolumePathPoint p;
        //    for (var i = 0; i < nearOffset.Length; i++)
        //    {
        //        point = new Vector3i(fromPoint + nearOffset[i]);

        //        byte val = volume.Accessor.GetDistanceField(point);
        //        return val > 0 && val <= isoValue;//TODO

        //        // 
        //        p = new VolumePathPoint();
        //        p.set(point, gridToObjectOffset, localToWorldMatrix);
        //        volumePath.pointList.Add(p);
        //    }
        //  //  volume.Accessor.GetDistanceField();
        //}
    }
}