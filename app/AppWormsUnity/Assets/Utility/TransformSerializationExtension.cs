using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Picodex
{
        public class StoreTransform
        {
            public Vector3 position;
            public Quaternion rotation;
            public Vector3 localScale;
        }

        public static class TransformSerializationExtension
        {
            public static StoreTransform SaveLocal(this Transform aTransform)
            {
                return new StoreTransform()
                {
                    position = aTransform.localPosition,
                    rotation = aTransform.localRotation,
                    localScale = aTransform.localScale
                };
   
        }

        public static StoreTransform SaveWorld(this Transform aTransform)
        {
                return new StoreTransform()
                {
                    position = aTransform.position,
                    rotation = aTransform.rotation,
                    localScale = aTransform.localScale
                };
        }

        public static void LoadLocal(this Transform aTransform, StoreTransform aData)
        {
            aTransform.localPosition = aData.position;
            aTransform.localRotation = aData.rotation;
            aTransform.localScale = aData.localScale;
        }

        public static void LoadWorld(this Transform aTransform, StoreTransform aData)
        {
            aTransform.position = aData.position;
            aTransform.rotation = aData.rotation;
            aTransform.localScale = aData.localScale;
        }

        public static void setLocalToIdentity(this Transform aTransform)
        {
            aTransform.localPosition = Vector3.zero;
            aTransform.localRotation = Quaternion.identity;
            aTransform.localScale = Vector3.one;
        }

    }
}
