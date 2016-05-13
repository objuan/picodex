using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Axiom.Scripting;
using Axiom.Scripting.Compiler;
using Axiom.Scripting.Compiler.AST;
using Axiom.Core;
using Axiom.Graphics;

namespace Axiom.Scripting.Compiler
{
    public partial class ScriptCompiler
    {
        public class UnityScriptTranslator : Translator
        {
            protected Axiom.Graphics.Material _material;

            protected Dictionary<string, string> _textureAliases = new Dictionary<string, string>();

            public UnityScriptTranslator()
                : base() { }

            #region Translator Implementation

            /// <see cref="Translator.CheckFor"/>
            internal override bool CheckFor(Keywords nodeId, Keywords parentId)
            {
                return nodeId == Keywords.ID_SHADER;
            }

            /// <see cref="Translator.Translate"/>
            public override void Translate(ScriptCompiler compiler, AbstractNode node)
            {
                ObjectAbstractNode obj = (ObjectAbstractNode)node;
                if (obj != null)
                {
                    if (string.IsNullOrEmpty(obj.Name))
                    {
                        compiler.AddError(CompileErrorCode.ObjectNameExpected, obj.File, obj.Line);
                    }
                }
                else
                {
                    compiler.AddError(CompileErrorCode.ObjectNameExpected, node.File, node.Line);
                    return;
                }

                // Create a material with the given name
                object mat;
                ScriptCompilerEvent evt = new CreateMaterialScriptCompilerEvent(node.File, obj.Name, compiler.ResourceGroup);
                bool processed = compiler._fireEvent(ref evt, out mat);

                if (!processed)
                {
                    //TODO
                    // The original translated implementation of this code block was simply the following:
                    // _material = (Material)MaterialManager.Instance.Create( obj.Name, compiler.ResourceGroup );
                    // but sometimes it generates an exception due to a duplicate resource.
                    // In order to avoid the above mentioned exception, the implementation was changed, but
                    // it need to be checked when ResourceManager._add will be updated to the latest version

                    Material checkForExistingMat = (Material)MaterialManager.Instance.GetByName(obj.Name);

                    if (checkForExistingMat == null)
                    {
                        _material = (Material)MaterialManager.Instance.Create(obj.Name, compiler.ResourceGroup);
                    }
                    else
                    {
                        _material = checkForExistingMat;
                    }
                }
                else
                {
                    _material = (Material)mat;

                    if (_material == null)
                    {
                        compiler.AddError(CompileErrorCode.ObjectAllocationError, obj.File, obj.Line, "failed to find or create material \"" + obj.Name + "\"");
                    }
                }

                _material.RemoveAllTechniques();
                obj.Context = _material;
                _material.Origin = obj.File;

                foreach (AbstractNode i in obj.Children)
                {
                    if (i is PropertyAbstractNode)
                    {
                        PropertyAbstractNode prop = (PropertyAbstractNode)i;

                        //switch ((Keywords)prop.Id)
                        //{
                           

                        
                        //    default:
                        //        compiler.AddError(CompileErrorCode.UnexpectedToken, prop.File, prop.Line, "token \"" + prop.Name + "\" is not recognized");
                        //        break;
                        //} //end of switch statement
                    }
                    else if (i is ObjectAbstractNode)
                    {
                        _processNode(compiler, i);
                    }
                }

                // Apply the texture aliases
                ScriptCompilerEvent locEvt = new PreApplyTextureAliasesScriptCompilerEvent(_material, ref _textureAliases);
                compiler._fireEvent(ref locEvt);

                _material.ApplyTextureAliases(_textureAliases);
                _textureAliases.Clear();
            }

            #endregion Translator Implementation
        }
    }
}
