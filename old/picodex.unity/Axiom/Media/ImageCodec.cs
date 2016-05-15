#region LGPL License

/*
Axiom Graphics Engine Library
Copyright � 2003-2011 Axiom Project Team

The overall design, and a majority of the core engine and rendering code 
contained within this library is a derivative of the open source Object Oriented 
Graphics Engine OGRE, which can be found at http://ogre.sourceforge.net.  
Many thanks to the OGRE team for maintaining such a high quality project.

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
*/

#endregion

#region SVN Version Information

// <file>
//     <license see="http://axiom3d.net/wiki/index.php/license.txt"/>
//     <id value="$Id: ImageCodec.cs 2940 2012-01-05 12:25:58Z borrillis $"/>
// </file>

#endregion SVN Version Information

#region Namespace Declarations

using System;

#endregion Namespace Declarations

namespace Axiom.Media
{
	/// <summary>
	/// Summary description for ImageCodec.
	/// </summary>
	abstract public class ImageCodec : ICodec
	{
		public ImageCodec() {}

		#region ICodec Members

		// Note: Redefining as abstract to force subclasses to implement, since interface methods must still be included
		// in abstract base classes
		abstract public object Decode( System.IO.Stream input, System.IO.Stream output, params object[] args );

		abstract public void Encode( System.IO.Stream input, System.IO.Stream output, params object[] args );

		abstract public void EncodeToFile( System.IO.Stream input, string fileName, object codecData );

		abstract public String Type { get; }

		#endregion

		public class ImageData
		{
			public int width;
			public int height;
			public int depth;
			public int size;
			public ImageFlags flags;
			public int numMipMaps;
			public PixelFormat format;
		}
	}
}