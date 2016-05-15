﻿#region LGPL License

/*
Axiom Graphics Engine Library
Copyright © 2003-2011 Axiom Project Team

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
//     <id value="$Id:$"/>
// </file>

#endregion SVN Version Information

#region Namespace Declarations

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Axiom.Serialization;

#endregion Namespace Declarations

namespace Axiom.Graphics
{
	partial class GpuProgramParameters
	{
		/// <summary>
		/// class collecting together the information for named constants.
		/// </summary>
		public class GpuNamedConstants
		{
			/// <summary>
			/// Total size of the float buffer required
			/// </summary>
			public int FloatBufferSize;

			/// <summary>
			/// Total size of the int buffer required
			/// </summary>
			public int IntBufferSize;

			/// <summary>
			/// Dictionary of parameter names to GpuConstantDefinition
			/// </summary>
			public Dictionary<string, GpuConstantDefinition> GpuConstantDefinitions = new Dictionary<string, GpuConstantDefinition>();

			private static bool _generateAllConstantDefinitionArrayEntries;

			/// <summary>
			/// Indicates whether all array entries will be generated and added to the definitions map
			/// </summary>
			public static bool GenerateAllConstantDefinitionEntries { get { return _generateAllConstantDefinitionArrayEntries; } set { _generateAllConstantDefinitionArrayEntries = value; } }

			/// <summary>
			/// Generate additional constant entries for arrays based on a base definition.
			/// </summary>
			/// <param name="paramName"></param>
			/// <param name="baseDef"></param>
			/// <remarks>
			/// Array uniforms will be added just with their base name with no array
			/// suffix. This method will add named entries for array suffixes too
			///	so individual array entries can be addressed. Note that we only
			///	individually index array elements if the array size is up to 16
			///	entries in size. Anything larger than that only gets a [0] entry
			///	as well as the main entry, to save cluttering up the name map. After
			///	all, you can address the larger arrays in a bulk fashion much more
			///	easily anyway.
			/// </remarks>
			public void GenerateConstantDefinitionArrayEntries( String paramName, GpuConstantDefinition baseDef )
			{
				// Copy definition for use with arrays
				GpuConstantDefinition arrayDef = baseDef;
				arrayDef.ArraySize = 1;
				string arrayName = string.Empty;

				// Add parameters for array accessors
				// [0] will refer to the same location, [1+] will increment
				// only populate others individually up to 16 array slots so as not to get out of hand,
				// unless the system has been explicitly configured to allow all the parameters to be added

				// paramName[0] version will always exist
				int maxArrayIndex = 1;
				if( baseDef.ArraySize <= 16 || _generateAllConstantDefinitionArrayEntries )
				{
					maxArrayIndex = baseDef.ArraySize;
				}

				for( int i = 0; i < maxArrayIndex; i++ )
				{
					arrayName = paramName + "[" + i + "]";
					// increment location
					arrayDef.PhysicalIndex += arrayDef.ElementSize;
					GpuConstantDefinitions.Add( arrayName, arrayDef );
				}
				// note no increment of buffer sizes since this is shared with main array def
			}

			/// <summary>
			/// Saves constant definitions to a file, compatible with GpuProgram::setManualNamedConstantsFile.
			/// </summary>
			/// <param name="filename"></param>
			public void Save( string filename )
			{
				GpuNamedConstantsSerializer ser = new GpuNamedConstantsSerializer();
				ser.ExportNamedConstants( this, filename );
			}

			/// <summary>
			/// Loads constant definitions from a stream, compatible with GpuProgram::setManualNamedConstantsFile.
			/// </summary>
			/// <param name="stream"></param>
			public void Load( Stream stream )
			{
				GpuNamedConstantsSerializer ser = new GpuNamedConstantsSerializer();
				ser.ImportNamedConstants( stream, this );
			}
		}

		/// <summary>
		/// Simple class for loading / saving GpuNamedConstants
		/// </summary>
		public class GpuNamedConstantsSerializer : Serializer
		{
			public void ExportNamedConstants( GpuNamedConstants pConsts, string filename )
			{
#warning implement Endian.Native.
				this.ExportNamedConstants( pConsts, filename, Endian.Little );
			}

			public void ExportNamedConstants( GpuNamedConstants pConsts, string filename, Endian endianMode )
			{
				throw new NotImplementedException();
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="stream"></param>
			/// <param name="pDest"></param>
			public void ImportNamedConstants( Stream stream, GpuNamedConstants pDest )
			{
				throw new NotImplementedException();
			}
		}
	}
}