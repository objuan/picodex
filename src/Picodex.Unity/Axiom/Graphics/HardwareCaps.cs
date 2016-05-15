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
//     <id value="$Id: HardwareCaps.cs 2940 2012-01-05 12:25:58Z borrillis $"/>
// </file>

#endregion SVN Version Information

#region Namespace Declarations

using Axiom.Core;
using Axiom.Scripting;

#endregion Namespace Declarations

namespace Axiom.Graphics
{
	/// <summary>
	/// Enumeration of GPU vendors.
	/// </summary>
	public enum GPUVendor
	{
		[ScriptEnum( "Unknown" )] Unknown = 0,

		[ScriptEnum( "Nvidia" )] Nvidia = 1,

		[ScriptEnum( "Ati" )] Ati = 2,

		[ScriptEnum( "Intel" )] Intel = 3,

		[ScriptEnum( "S3" )] S3 = 4,

		[ScriptEnum( "Matrox" )] Matrox = 5,

		[ScriptEnum( "3DLabs" )] _3DLabs = 6,

		[ScriptEnum( "Sis" )] Sis = 7,

		[ScriptEnum( "Imagination Technologies" )] ImaginationTechnologies = 8,

		// Apple Software Renderer
		[ScriptEnum( "Apple" )] Apple = 9,

		[ScriptEnum( "Nokia" )] Nokia = 10,
	};

	/// <summary>
	/// 	This serves as a way to query information about the capabilies of a 3D API and the
	/// 	users hardware configuration.  A RenderSystem should create and initialize an instance
	/// 	of this class during startup so that it will be available for use ASAP for checking caps.
	/// </summary>
	public class RenderSystemCapabilities
	{
		#region Fields and Properties

		/// <summary>
		///    Flag enum holding the bits that identify each supported feature.
		/// </summary>
		private Capabilities _caps;

		#region TextureUnitCount Property

		/// <summary>
		///    Max number of texture units available on the current hardware.
		/// </summary>
		private int _textureUnitCount;

		/// <summary>
		///		Reports on the number of texture units the graphics hardware has available.
		/// </summary>
		public int TextureUnitCount { get { return _textureUnitCount; } set { _textureUnitCount = value; } }

		#endregion TextureUnitCount Property

		#region WorlMatrixCount Property

		/// <summary>
		///    Max number of world matrices supported.
		/// </summary>
		private int _worldMatrixCount;

		/// <summary>
		///    Max number of world matrices supported by the hardware.
		/// </summary>
		public int WorldMatrixCount { get { return _worldMatrixCount; } set { _worldMatrixCount = value; } }

		#endregion WorlMatrixCount Property

		#region MaxVertexProgramVersion Property

		/// <summary>
		///    The best vertex program version supported by the hardware.
		/// </summary>
		private string _maxVertexProgramVersion;

		/// <summary>
		///    Best vertex program version supported by the hardware.
		/// </summary>
		public string MaxVertexProgramVersion { get { return _maxVertexProgramVersion; } set { _maxVertexProgramVersion = value; } }

		#endregion MaxVertexProgramVersion Property

		#region VertexProgramConstantFloatCount Property

		/// <summary>
		///    The number of floating point constants the current hardware supports for vertex programs.
		/// </summary>
		private int _vertexProgramConstantFloatCount;

		/// <summary>
		///    Max number of floating point constants supported by the hardware for vertex programs.
		/// </summary>
		public int VertexProgramConstantFloatCount { get { return _vertexProgramConstantFloatCount; } set { _vertexProgramConstantFloatCount = value; } }

		#endregion VertexProgramConstantFloatCount Property

		#region VertexProgramConstantIntCount Property

		/// <summary>
		///    The number of integer constants the current hardware supports for vertex programs.
		/// </summary>
		private int _vertexProgramConstantIntCount;

		/// <summary>
		///    Max number of integer constants supported by the hardware for vertex programs.
		/// </summary>
		public int VertexProgramConstantIntCount { get { return _vertexProgramConstantIntCount; } set { _vertexProgramConstantIntCount = value; } }

		#endregion VertexProgramConstantIntCount Property

		#region VertexProgramConstantBoolCount Property

		/// <summary>
		///    The number of boolean constants the current hardware supports for vertex programs.
		/// </summary>
		private int _vertexProgramConstantBoolCount;

		/// <summary>
		///    Max number of boolean constants supported by the hardware for vertex programs.
		/// </summary>
		public int VertexProgramConstantBoolCount { get { return _vertexProgramConstantBoolCount; } set { _vertexProgramConstantBoolCount = value; } }

		#endregion VertexProgramConstantBoolCount Property

		#region MaxFragmentProgramVersion Property

		/// <summary>
		///    The best fragment program version supported by the hardware.
		/// </summary>
		private string _maxFragmentProgramVersion;

		/// <summary>
		///    Best fragment program version supported by the hardware.
		/// </summary>
		public string MaxFragmentProgramVersion { get { return _maxFragmentProgramVersion; } set { _maxFragmentProgramVersion = value; } }

		#endregion MaxFragmentProgramVersion Property

		#region FragmentProgramConstantFloatCount Property

		/// <summary>
		///    The number of floating point constants the current hardware supports for fragment programs.
		/// </summary>
		private int _fragmentProgramConstantFloatCount;

		/// <summary>
		///    Max number of floating point constants supported by the hardware for fragment programs.
		/// </summary>
		public int FragmentProgramConstantFloatCount { get { return _fragmentProgramConstantFloatCount; } set { _fragmentProgramConstantFloatCount = value; } }

		#endregion FragmentProgramConstantFloatCount Property

		#region FragmentProgramConstantIntCount Property

		/// <summary>
		///    The number of integer constants the current hardware supports for fragment programs.
		/// </summary>
		private int _fragmentProgramConstantIntCount;

		/// <summary>
		///    Max number of integer constants supported by the hardware for fragment programs.
		/// </summary>
		public int FragmentProgramConstantIntCount { get { return _fragmentProgramConstantIntCount; } set { _fragmentProgramConstantIntCount = value; } }

		#endregion FragmentProgramConstantIntCount Property

		#region FragmentProgramConstantBoolCount Property

		/// <summary>
		///    The number of boolean constants the current hardware supports for fragment programs.
		/// </summary>
		private int _fragmentProgramConstantBoolCount;

		/// <summary>
		///    Max number of boolean constants supported by the hardware for fragment programs.
		/// </summary>
		public int FragmentProgramConstantBoolCount { get { return _fragmentProgramConstantBoolCount; } set { _fragmentProgramConstantBoolCount = value; } }

		#endregion FragmentProgramConstantBoolCount Property

		#region MultiRenderTargetCount Property

		/// <summary>
		/// The number of simultaneous render targets supported
		/// </summary>
		private int _multiRenderTargetCount;

		/// <summary>
		/// The number of simultaneous render targets supported
		/// </summary>
		public int MultiRenderTargetCount { get { return _multiRenderTargetCount; } set { _multiRenderTargetCount = value; } }

		#endregion MultiRenderTargetCount Property

		#region StencilBufferBitCount Property

		/// <summary>
		///    Stencil buffer bits available.
		/// </summary>
		private int _stencilBufferBitCount;

		/// <summary>
		///		Number of stencil buffer bits suppported by the hardware.
		/// </summary>
		public int StencilBufferBitCount { get { return _stencilBufferBitCount; } set { _stencilBufferBitCount = value; } }

		#endregion StencilBufferBitCount Property

		#region MaxLights Property

		/// <summary>
		///    Maximum number of lights that can be active in the scene at any given time.
		/// </summary>
		private int _maxLights;

		/// <summary>
		///		Maximum number of lights that can be active in the scene at any given time.
		/// </summary>
		public int MaxLights { get { return _maxLights; } set { _maxLights = value; } }

		#endregion MaxLights Property

		#region VendorName Property

		/// <summary>
		/// name of the GPU vendor
		/// </summary>
		private GPUVendor _vendor = GPUVendor.Unknown;

		/// <summary>
		/// name of the GPU vendor
		/// </summary>
		public string VendorName { get { return VendorToString( _vendor ); } set { _vendor = VendorFromString( value ); } }

		#endregion DeviceName Property

		#region DeviceName Property

		/// <summary>
		/// name of the adapter
		/// </summary>
		private string _deviceName = "";

		/// <summary>
		/// Name of the display adapter
		/// </summary>
		public string DeviceName { get { return _deviceName; } set { _deviceName = value; } }

		#endregion DeviceName Property

		#region DeviceVersion Property

		/// <summary>
		/// This is used to build a database of RSC's
		/// if a RSC with same name, but newer version is introduced, the older one 
		/// will be removed
		/// </summary>
		private DriverVersion _driverVersion = new DriverVersion();

		/// <summary>
		/// The driver version string
		/// </summary>
		public DriverVersion DriverVersion { get { return _driverVersion; } set { _driverVersion = value; } }

		#endregion DeviceVersion Property

		#region MaxPointSize Property

		/// <summary>
		/// The maximum point size
		/// </summary>
		private float _maxPointSize;

		/// <summary>
		/// The maximum point size
		/// </summary>
		public float MaxPointSize { get { return _maxPointSize; } set { _maxPointSize = value; } }

		#endregion MaxPointSize Property

		#region NonPOW2TexturesLimited Property

		/// <summary>
		/// Are non-POW2 textures feature-limited?
		/// </summary>
		private bool _nonPOW2TexturesLimited;

		/// <summary>
		/// Are non-POW2 textures feature-limited?
		/// </summary>
		public bool NonPOW2TexturesLimited { get { return _nonPOW2TexturesLimited; } set { _nonPOW2TexturesLimited = value; } }

		#endregion NonPOW2TexturesLimited Property

		#region VertexTextureUnitCount Property

		/// <summary>
		/// The number of vertex texture units supported
		/// </summary>
		private int _vertexTextureUnitCount;

		/// <summary>
		/// The number of vertex texture units supported
		/// </summary>
		public int VertexTextureUnitCount { get { return _vertexTextureUnitCount; } set { _vertexTextureUnitCount = value; } }

		#endregion VertexTextureUnitCount Property

		#region VertexTextureUnitsShared Property

		/// <summary>
		/// Are vertex texture units shared with fragment processor?
		/// </summary>
		private bool _vertexTextureUnitsShared;

		/// <summary>
		/// Are vertex texture units shared with fragment processor?
		/// </summary>
		public bool VertexTextureUnitsShared { get { return _vertexTextureUnitsShared; } set { _vertexTextureUnitsShared = value; } }

		#endregion VertexTextureUnitsShared Property

		#endregion Fields and Properties

		#region Construction and Destruction

		/// <summary>
		///    Default constructor.
		/// </summary>
		public RenderSystemCapabilities()
		{
			_caps = 0;
		}

		#endregion Construction and Destruction

		#region Methods

		/// <summary>
		///    Returns true if the current hardware supports the requested feature.
		/// </summary>
		/// <param name="cap">Feature to query (i.e. Dot3 bump mapping)</param>
		/// <returns></returns>
		public bool HasCapability( Capabilities cap )
		{
			return ( _caps & cap ) > 0;
		}

		/// <summary>
		///    Sets a flag stating the specified feature is supported.
		/// </summary>
		/// <param name="cap"></param>
		public void SetCapability( Capabilities cap )
		{
			_caps |= cap;
		}

		/// <summary>
		///    Write all hardware capability information to registered listeners.
		/// </summary>
		public void Log()
		{
			LogManager logMgr = LogManager.Instance;

			logMgr.Write( "---RenderSystem capabilities---" );
			logMgr.Write( "\t-GPU Vendor: {0}", VendorName );
			logMgr.Write( "\t-Device Name: {0}", _deviceName );
			logMgr.Write( "\t-Driver Version: {0}", _driverVersion.ToString() );
			logMgr.Write( "\t-Available texture units: {0}", this.TextureUnitCount );
			logMgr.Write( "\t-Maximum lights available: {0}", this.MaxLights );
			logMgr.Write( "\t-Hardware generation of mip-maps: {0}", ConvertBool( HasCapability( Capabilities.HardwareMipMaps ) ) );
			logMgr.Write( "\t-Texture blending: {0}", ConvertBool( HasCapability( Capabilities.TextureBlending ) ) );
			logMgr.Write( "\t-Anisotropic texture filtering: {0}", ConvertBool( HasCapability( Capabilities.AnisotropicFiltering ) ) );
			logMgr.Write( "\t-Dot product texture operation: {0}", ConvertBool( HasCapability( Capabilities.Dot3 ) ) );
			logMgr.Write( "\t-Cube Mapping: {0}", ConvertBool( HasCapability( Capabilities.CubeMapping ) ) );

			logMgr.Write( "\t-Hardware stencil buffer: {0}", ConvertBool( HasCapability( Capabilities.StencilBuffer ) ) );

			if( HasCapability( Capabilities.StencilBuffer ) )
			{
				logMgr.Write( "\t\t-Stencil depth: {0} bits", _stencilBufferBitCount );
				logMgr.Write( "\t\t-Two sided stencil support: {0}", ConvertBool( HasCapability( Capabilities.TwoSidedStencil ) ) );
				logMgr.Write( "\t\t-Wrap stencil values: {0}", ConvertBool( HasCapability( Capabilities.StencilWrap ) ) );
			}

			logMgr.Write( "\t-Hardware vertex/index buffers: {0}", ConvertBool( HasCapability( Capabilities.VertexBuffer ) ) );

			logMgr.Write( "\t-Vertex programs: {0}", ConvertBool( HasCapability( Capabilities.VertexPrograms ) ) );

			if( HasCapability( Capabilities.VertexPrograms ) )
			{
				logMgr.Write( "\t\t-Max vertex program version: {0}", this.MaxVertexProgramVersion );
			}

			logMgr.Write( "\t-Fragment programs: {0}", ConvertBool( HasCapability( Capabilities.FragmentPrograms ) ) );

			if( HasCapability( Capabilities.FragmentPrograms ) )
			{
				logMgr.Write( "\t\t-Max fragment program version: {0}", this.MaxFragmentProgramVersion );
			}

			logMgr.Write( "\t-Texture compression: {0}", ConvertBool( HasCapability( Capabilities.TextureCompression ) ) );

			if( HasCapability( Capabilities.TextureCompression ) )
			{
				logMgr.Write( "\t\t-DXT: {0}", ConvertBool( HasCapability( Capabilities.TextureCompressionDXT ) ) );
				logMgr.Write( "\t\t-VTC: {0}", ConvertBool( HasCapability( Capabilities.TextureCompressionVTC ) ) );
			}

			logMgr.Write( "\t-Scissor rectangle: {0}", ConvertBool( HasCapability( Capabilities.ScissorTest ) ) );
			logMgr.Write( "\t-Hardware Occlusion Query: {0}", ConvertBool( HasCapability( Capabilities.HardwareOcculusion ) ) );
			logMgr.Write( "\t-User clip planes: {0}", ConvertBool( HasCapability( Capabilities.UserClipPlanes ) ) );
			logMgr.Write( "\t-VertexElementType.UBYTE4: {0}", ConvertBool( HasCapability( Capabilities.VertexFormatUByte4 ) ) );
			logMgr.Write( "\t-Infinite far plane projection: {0}", ConvertBool( HasCapability( Capabilities.InfiniteFarPlane ) ) );

			logMgr.Write( "\t-Max Point Size: {0} ", MaxPointSize );
			logMgr.Write( "\t-Vertex texture fetch: {0} ", ConvertBool( HasCapability( Capabilities.VertexTextureFetch ) ) );
			if( HasCapability( Capabilities.VertexTextureFetch ) )
			{
				logMgr.Write( "\t\t-Max vertex textures: {0}", VertexTextureUnitCount );
				logMgr.Write( "\t\t-Vertex textures shared: {0}", ConvertBool( VertexTextureUnitsShared ) );
			}
		}

		/// <summary>
		///     Helper method to convert true/false to yes/no.
		/// </summary>
		/// <param name="val">Bool bal.</param>
		/// <returns>"yes" if true, else "no".</returns>
		private string ConvertBool( bool val )
		{
			return val ? "yes" : "no";
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="vendorString"></param>
		/// <returns></returns>
		internal static GPUVendor VendorFromString( string vendorString )
		{
			GPUVendor ret = GPUVendor.Unknown;
			object lookUpResult = ScriptEnumAttribute.Lookup( vendorString, typeof( GPUVendor ) );

			if( lookUpResult != null )
			{
				ret = (GPUVendor)lookUpResult;
			}

			return ret;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		internal static string VendorToString( GPUVendor v )
		{
			return ScriptEnumAttribute.GetScriptAttribute( (int)v, typeof( GPUVendor ) );
		}

		#endregion Methods
	}
}