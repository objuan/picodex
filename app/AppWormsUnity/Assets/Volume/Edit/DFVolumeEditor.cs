using System;

using UnityEngine;
//using UnityEditor;
using Picodex.Vxcm;

namespace Picodex
{
    public enum VolumeEditor_Mode
    {
        Add,
        Substract
    }

    public enum VolumeEditor_ShapeType 
    {
        Circle,
        Rectangle
    }

    public class VolumeEditor_Shape : ScriptableObject
    {
        public VolumeEditor_ShapeType type;
        public float ray = 4;
    }


    [RequireComponent(typeof(DFVolumeFilter))]
    [AddComponentMenu("Vxcm/DFVolumeEditor")]
    [ExecuteInEditMode]
    public class DFVolumeEditor : MonoBehaviour
    {
        public DFVolume volume=null;
        public DFVolumeRenderer volumeRenderer;

        public  Color matColor = Color.white;
        //  public VolumeEditor_Shape shape = new VolumeEditor_Shape();
        public VolumeEditor_ShapeType shape = VolumeEditor_ShapeType.Circle;
        public float ray = 4;
        public VolumeEditor_Mode mode = VolumeEditor_Mode.Add;
        public bool editEnabled;

        public DFVolumeEditor()
        {

        }

        void Start()
        {
            if (!GetComponent<DFVolumeFilter>()) return;
            if (!GetComponent<DFVolumeRenderer>()) return;

            volume = GetComponent<DFVolumeFilter>().volume;
            volumeRenderer = GetComponent<DFVolumeRenderer>();

        
        }

        public void Update()
        {
            if (!Camera.current) return;
           // if (Input.GetMouseButton(0))
            {
                HandleInput();
            }
        }

        void HandleInput()
        {
            if (!editEnabled) return;

            Ray inputRay = Camera.current.ScreenPointToRay(Input.mousePosition);
            VolumeRaycastHit hit;

         //   Debug.Log(inputRay);

            // pick il volume interessato
            if (Picodex.Vxcm.Volume.Raycast(volumeRenderer,inputRay.origin, inputRay.direction, out hit))
            {
                Debug.DrawLine(Camera.current.transform.position, hit.point);

                if (Input.GetMouseButton(0))
                {
                    // modify volume
                  //  if (mode == VolumeEditor_Mode.Add)

                    VolumePrimitive raster = new VolumePrimitive(volume);
                    raster.CutMode = (mode == VolumeEditor_Mode.Substract);
                    raster.IsFilled = (mode == VolumeEditor_Mode.Substract);

                    GeometrySample sample = new GeometrySample();
                    sample.debugColor = new Vector3(1, 0, 0);
                    //raster.Raster(new Vector3(0, 0, 0), 10, sample);

                    //   hit.point
                    Vector3 p = hit.point;
                    p = transform.worldToLocalMatrix.MultiplyPoint(p);

                    if (shape == VolumeEditor_ShapeType.Circle)
                    {
                        raster.RasterSphere(p, ray, sample);
                    }
                    else
                    {
                        Bounds b = new Bounds(p,new Vector3(ray,ray,ray));
                        
                        raster.RasterBox(b.min,b.max, sample);
                    }

                    // UnityUtil.InvalidateObject(gameObject);
                   // EditorUtility.SetDirty(gameObject);
                    //SceneView.RepaintAll();
                }

            }
        }
    }
}
