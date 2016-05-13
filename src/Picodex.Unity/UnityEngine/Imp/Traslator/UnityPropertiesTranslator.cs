
#region Namespace Declarations

using Axiom.Graphics;
using Axiom.Scripting.Compiler.AST;

#endregion Namespace Declarations

namespace Axiom.Scripting.Compiler
{
	public partial class ScriptCompiler
	{
		public class UnityPropertiesTranslator : Translator
		{
            public UnityPropertiesTranslator()
				: base()
			{
			
			}

			#region Translator Implementation

			/// <see cref="Translator.CheckFor"/>
			internal override bool CheckFor( Keywords nodeId, Keywords parentId )
			{
				return nodeId == Keywords.ID_PROPERTIES && parentId == Keywords.ID_SHADER;
			}

			/// <see cref="Translator.Translate"/>
			public override void Translate( ScriptCompiler compiler, AbstractNode node )
			{
				ObjectAbstractNode obj = (ObjectAbstractNode)node;

				// Create the technique from the material
				Material material = (Material)obj.Parent.Context;
			
				// Get the name of the technique
				if( !string.IsNullOrEmpty( obj.Name ) )
				{
					//_technique.Name = obj.Name;
				}

				// Set the properties for the technique
				foreach( AbstractNode i in obj.Children )
				{
                    //if( i is PropertyAbstractNode )
                    //{
                    //    PropertyAbstractNode prop = (PropertyAbstractNode)i;

                    //    //switch( (Keywords)prop.Id )
                    //    //{
                    //    //    case Keywords.ID_TAGS:
                    //    //        break;

                    //    //    default:
                    //    //        compiler.AddError( CompileErrorCode.UnexpectedToken, prop.File, prop.Line, "token \"" + prop.Name + "\" is not recognized" );
                    //    //        break;
                    //    //} //end of switch statement
                    //} // end of if ( i is PropertyAbstractNode )
                    //else if( i is ObjectAbstractNode )
                    //{
                    //    _processNode( compiler, i );
                    //}
				}
			}

			#endregion Translator Implementation
		}
	}
}
