
using UnityEngine;

namespace Picodex.Vxcm
{
    [System.Serializable]
    /// A three-dimensional vector type with integer components.
    /**
	 * This class is commonly used for representing positions of voxels inside volumes.
	 */
    public struct VolumeAddress
    {
        /// The 'x' component of the vector.
        public int x;
        /// The 'y' component of the vector.
        public int y;
        /// The 'z' component of the vector.
        public int z;
        /// A vector with all components set to zero.
        public static readonly VolumeAddress zero = new VolumeAddress(0, 0, 0);
        /// A vector with all components set to one.
        public static readonly VolumeAddress one = new VolumeAddress(1, 1, 1);
        /// A unit vector pointing along the positive z axis.
        public static readonly VolumeAddress forward = new VolumeAddress(0, 0, 1);
        /// A unit vector pointing along the negative z axis.
        public static readonly VolumeAddress back = new VolumeAddress(0, 0, -1);
        /// A unit vector pointing along the positive y axis.
        public static readonly VolumeAddress up = new VolumeAddress(0, 1, 0);
        /// A unit vector pointing along the negative y axis.
        public static readonly VolumeAddress down = new VolumeAddress(0, -1, 0);
        /// A unit vector pointing along the negative x axis.
        public static readonly VolumeAddress left = new VolumeAddress(-1, 0, 0);
        /// A unit vector pointing along the positive x axis.
        public static readonly VolumeAddress right = new VolumeAddress(1, 0, 0);

        /// Constructs a vector from the supplied components.
        public VolumeAddress(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// Constructs a vector from the supplied components (z is set to zero).
        public VolumeAddress(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.z = 0;
        }

        /// Constructs a vector by copying the supplied vector.
        public VolumeAddress(VolumeAddress a)
        {
            this.x = a.x;
            this.y = a.y;
            this.z = a.z;
        }

        /// Constructs a vector by copying the supplied vector and casting the components to ints.
        public VolumeAddress(Vector3 a)
        {
            this.x = (int)a.x;
            this.y = (int)a.y;
            this.z = (int)a.z;
        }

        /// Accesses the component at the specified index.
        public int this[int key]
        {
            get
            {
                switch (key)
                {
                    case 0:
                        {
                            return x;
                        }
                    case 1:
                        {
                            return y;
                        }
                    case 2:
                        {
                            return z;
                        }
                    default:
                        {
                            Debug.LogError("Invalid VolumeAddress index value of: " + key);
                            return 0;
                        }
                }
            }
            set
            {
                switch (key)
                {
                    case 0:
                        {
                            x = value;
                            return;
                        }
                    case 1:
                        {
                            y = value;
                            return;
                        }
                    case 2:
                        {
                            z = value;
                            return;
                        }
                    default:
                        {
                            Debug.LogError("Invalid VolumeAddress index value of: " + key);
                            return;
                        }
                }
            }
        }


        /// Scales 'a' by 'b' and returns the result.
        public static VolumeAddress Scale(VolumeAddress a, VolumeAddress b)
        {
            return new VolumeAddress(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        /// Computes the distance between two positions.
        public static float Distance(VolumeAddress a, VolumeAddress b)
        {
            return Mathf.Sqrt(DistanceSquared(a, b));
        }

        /// Computes the squared distance between two positions.
        public static int DistanceSquared(VolumeAddress a, VolumeAddress b)
        {
            int dx = b.x - a.x;
            int dy = b.y - a.y;
            int dz = b.z - a.z;
            return dx * dx + dy * dy + dz * dz;
        }

        /// A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.
        public override int GetHashCode()
        {
            // Microsoft use XOR in their example here: http://msdn.microsoft.com/en-us/library/ms173147.aspx
            // We also use shifting, as the compoents are typically small and this should reduce collisions.
            return x ^ (y << 8) ^ (z << 16);
        }

        /// Determines whether the specified System.Object is equal to the current Cubiquity.VolumeAddress.
        public override bool Equals(object other)
        {
            if (!(other is VolumeAddress))
            {
                return false;
            }
            VolumeAddress vector = (VolumeAddress)other;
            return x == vector.x &&
                   y == vector.y &&
                   z == vector.z;
        }

        /// Returns a System.String that represents the current Cubiquity.VolumeAddress.
        public override string ToString()
        {
            return string.Format("VolumeAddress({0} {1} {2})", x, y, z);
        }

        /// Determines whether a specified instance of VolumeAddress is equal to another specified VolumeAddress.
        public static bool operator ==(VolumeAddress a, VolumeAddress b)
        {
            return a.x == b.x &&
                   a.y == b.y &&
                   a.z == b.z;
        }

        /// Determines whether a specified instance of VolumeAddress is not equal to another specified VolumeAddress.
        public static bool operator !=(VolumeAddress a, VolumeAddress b)
        {
            return a.x != b.x ||
                   a.y != b.y ||
                   a.z != b.z;
        }
        public static VolumeAddress operator -(VolumeAddress a)
        {
            return new VolumeAddress(-a.x, -a.y, -a.z);
        }
        /// Component-wise subtraction of one vector from another, returning a new vector as the result.
        public static VolumeAddress operator -(VolumeAddress a, VolumeAddress b)
        {
            return new VolumeAddress(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        /// Component-wise addition of one vector to another, returning a new vector as the result.
        public static VolumeAddress operator +(VolumeAddress a, VolumeAddress b)
        {
            return new VolumeAddress(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        /// Component-wise multiplication of one vector with another, returning a new vector as the result.
        public static VolumeAddress operator *(VolumeAddress a, int d)
        {
            return new VolumeAddress(a.x * d, a.y * d, a.z * d);
        }

        public static VolumeAddress operator *(VolumeAddress a, float d)
        {
            return new VolumeAddress((int)(a.x * d), (int)(a.y * d), (int)(a.z * d));
        }

        /// Component-wise division of one vector with another, returning a new vector as the result.
        public static VolumeAddress operator *(int d, VolumeAddress a)
        {
            return new VolumeAddress(a.x * d, a.y * d, a.z * d);
        }

        /// Cast a VolumeAddress to Unity's Vector3 type.
        public static explicit operator Vector3(VolumeAddress v)
        {
            return new Vector3(v.x, v.y, v.z);
        }

        /// Cast Unity's Vector3 type to a VolumeAddress
        public static explicit operator VolumeAddress(Vector3 v)
        {
            return new VolumeAddress(v);
        }

        /// Component-wise minimum of one vector and another, returning a new vector as the result.
        public static VolumeAddress Min(VolumeAddress lhs, VolumeAddress rhs)
        {
            return new VolumeAddress(Mathf.Min(lhs.x, rhs.x), Mathf.Min(lhs.y, rhs.y), Mathf.Min(lhs.z, rhs.z));
        }

        /// Component-wise maximum of one vector and another, returning a new vector as the result.
        public static VolumeAddress Max(VolumeAddress lhs, VolumeAddress rhs)
        {
            return new VolumeAddress(Mathf.Max(lhs.x, rhs.x), Mathf.Max(lhs.y, rhs.y), Mathf.Max(lhs.z, rhs.z));
        }

        /// Component-wise clamping of one vector and another, returning a new vector as the result.
        public static VolumeAddress Clamp(VolumeAddress value, VolumeAddress min, VolumeAddress max)
        {
            return new VolumeAddress(Mathf.Clamp(value.x, min.x, max.x), Mathf.Clamp(value.y, min.y, max.y), Mathf.Clamp(value.z, min.z, max.z));
        }
    }
}
