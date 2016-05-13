using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Picodex.Vxcm
{
    public class VXCMException : Exception
    {
        public VXCMException() 
        {

        }
        public VXCMException(String message) : base(message)
        {

        }

        public VXCMException(String message, Exception inner) : base(message, inner)
        {

        }
    }
}
