
#region Namespace Declarations

using System;
using System.Collections.Generic;

using Axiom.Core;
using Axiom.Core.Collections;
using Axiom.Graphics;
using Axiom.Scripting.Compiler.AST;
using Axiom.Scripting.Compiler;

#endregion Namespace Declarations

using UnityEngine;
using Matrix4 = UnityEngine.Matrix4x4;
using Vector3 = UnityEngine.Vector3;
using Material = Axiom.Graphics.Material;

namespace UnityEngine.Imp
{
    /// <summary>
    /// This class manages the builtin translators
    /// </summary>
    public class UnityScriptTranslatorManager : ScriptTranslatorManager
    {
        public UnityScriptTranslatorManager()
            : base()
        {
            _translators.Add(new ScriptCompiler.UnityScriptTranslator());
            _translators.Add(new ScriptCompiler.UnitySubShaderTranslator());
            _translators.Add(new ScriptCompiler.UnityCGPROGRAMTranslator());
            _translators.Add(new ScriptCompiler.UnityPropertiesTranslator());
            _translators.Add(new ScriptCompiler.UnityTagsTranslator());
            _translators.Add(new ScriptCompiler.UnityPassTranslator());
            
        
        }
    }
}
