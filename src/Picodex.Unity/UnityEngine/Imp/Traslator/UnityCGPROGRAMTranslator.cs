
#region Namespace Declarations

using Axiom.Graphics;
using Axiom.Scripting.Compiler.AST;

#endregion Namespace Declarations

namespace Axiom.Scripting.Compiler
{
	public partial class ScriptCompiler
    {
        #region HELPER

        class PragmaTokenizer
        {
            string[] pragmaList;
            int idx = -1;
            public PragmaTokenizer(string pragma)
            {
                pragmaList = pragma.Split(' ');
            }
            public string Next()
            {
                idx++;
                while (pragmaList[idx].Trim() == "" && idx < pragmaList.Length-1) idx++;

                if (idx < pragmaList.Length-1)
                {
                    return pragmaList[idx];
                }
                else
                    return null;
            }
        }
        protected static bool _getReal(string value, out UnityEngine.Real result)
        {
            result = 0.0f;

            if (value == null)
            {
                return false;
            }
            double num = 0;
            if (double.TryParse(value,out num))
            {
                result = num;
                return true;
            }
            else
                return false;
        }

        protected static bool _getFloat(string value, out float result)
        {
            result = 0f;
            UnityEngine.Real rResult;

            if (value == null)
            {
                return false;
            }

            if (_getReal(value, out rResult))
            {
                result = rResult;
                return true;
            }

            return false;
        }

        protected static bool _getInt(string value, out int result)
        {
            result = 0;
            UnityEngine.Real rResult;

            if (value == null)
            {
                return false;
            }

            if (_getReal(value, out rResult))
            {
                result = (int)rResult;
                return true;
            }

            return false;
        }

        #endregion

        public class UnityCGPROGRAMTranslator : Translator
		{
            static GpuProgramParameters unityAutoParams = null;

            protected Technique technique;
            protected Pass pass;

            //TODO
         //   static string includeRelativePath = "./media/Shaders/CGIncludes/";

            // general params;
            string fragEntryPoint = "frag";
            string vertexEntryPoint = "vert";

            string file = "";
            uint line = 0;

            public UnityCGPROGRAMTranslator()
				: base()
			{
                technique = null;
                pass = null;
			}

			#region Translator Implementation

			/// <see cref="Translator.CheckFor"/>
			internal override bool CheckFor( Keywords nodeId, Keywords parentId )
			{
                return ((nodeId == Keywords.ID_CGPROGRAM && parentId == Keywords.ID_SUBSHADER) 
                || nodeId == Keywords.ID_CGPROGRAM && parentId == Keywords.ID_UPASS);
			}

   
            private void ProcessPragma(string pragma)
            {
                PragmaTokenizer tokenizer = new PragmaTokenizer(pragma);
                string token=null;
                do
                {
                    token = tokenizer.Next();
                    if (token != null)
                    {
                        switch (token)
                        {
                            case "vertex":
                                vertexEntryPoint = tokenizer.Next();
                                break;
                            case "fragment":
                                fragEntryPoint = tokenizer.Next();
                                break;
                        }
                    }
                } while (token != null);
            }


            private void ProcessAutoParam(ScriptCompiler compiler, GpuProgramParameters parameters, 
                string paramID, bool paramNamed, string autoParamTarget, string value2, string value3)
            {

                #region PROCESS_PARAM

                int animParametricsCount = 0;

                string name = "";
                int index = 0;

                if (paramNamed)
                {
                    name = paramID;
                }
                else
                {
                    index = int.Parse(paramID);
                }

                // Look up the auto constant
                autoParamTarget = autoParamTarget.ToLower();

				GpuProgramParameters.AutoConstantDefinition def;
                bool defFound = GpuProgramParameters.GetAutoConstantDefinition(autoParamTarget, out def);

				if( defFound )
				{
					switch( def.DataType )
					{
						#region None

						case GpuProgramParameters.AutoConstantDataType.None:
							// Set the auto constant
							try
							{
                                if (paramNamed)
								{
									parameters.SetNamedAutoConstant( name, def.AutoConstantType );
								}
								else
								{
									parameters.SetAutoConstant( index, def.AutoConstantType );
								}
							}
							catch
							{
								compiler.AddError( CompileErrorCode.InvalidParameters, file, line,
								                   "setting of constant failed" );
							}
							break;

							#endregion None

							#region Int

						case GpuProgramParameters.AutoConstantDataType.Int:
							if( def.AutoConstantType == GpuProgramParameters.AutoConstantType.AnimationParametric )
							{
								try
								{
                                    if (paramNamed)
									{
										parameters.SetNamedAutoConstant( name, def.AutoConstantType, animParametricsCount++ );
									}
									else
									{
										parameters.SetAutoConstant( index, def.AutoConstantType, animParametricsCount++ );
									}
								}
								catch
								{
									compiler.AddError( CompileErrorCode.InvalidParameters, file, line,
									                   "setting of constant failed" );
								}
							}
							else
							{
								// Only certain texture projection auto params will assume 0
								// Otherwise we will expect that 3rd parameter
								if( value2 == null )
								{
									if( def.AutoConstantType == GpuProgramParameters.AutoConstantType.TextureViewProjMatrix ||
									    def.AutoConstantType == GpuProgramParameters.AutoConstantType.TextureWorldViewProjMatrix ||
									    def.AutoConstantType == GpuProgramParameters.AutoConstantType.SpotLightViewProjMatrix ||
									    def.AutoConstantType == GpuProgramParameters.AutoConstantType.SpotLightWorldViewProjMatrix )
									{
										try
										{
                                            if (paramNamed)
											{
												parameters.SetNamedAutoConstant( name, def.AutoConstantType, 0 );
											}
											else
											{
												parameters.SetAutoConstant( index, def.AutoConstantType, 0 );
											}
										}
										catch
										{
											compiler.AddError( CompileErrorCode.InvalidParameters,file,line,
											                   "setting of constant failed" );
										}
									}
									else
									{
										compiler.AddError( CompileErrorCode.NumberExpected,file,line,
                                                           "extra parameters required by constant definition " + autoParamTarget);
									}
								}
								else
								{
									bool success = false;
									int extraInfo = 0;
									if( value3 == null )
									{
										// Handle only one extra value
                                        if (_getInt(value2, out extraInfo))
										{
											success = true;
										}
									}
									else
									{
										// Handle two extra values
										int extraInfo1 = 0, extraInfo2 = 0;
                                        if (_getInt(value2, out extraInfo1) && _getInt(value3, out extraInfo2))
										{
											extraInfo = extraInfo1 | ( extraInfo2 << 16 );
											success = true;
										}
									}

									if( success )
									{
										try
										{
                                            if (paramNamed)
											{
												parameters.SetNamedAutoConstant( name, def.AutoConstantType, extraInfo );
											}
											else
											{
												parameters.SetAutoConstant( index, def.AutoConstantType, extraInfo );
											}
										}
										catch
										{
											compiler.AddError( CompileErrorCode.InvalidParameters, file,line,
											                   "setting of constant failed" );
										}
									}
									else
									{
										compiler.AddError( CompileErrorCode.InvalidParameters, file,line,
										                   "invalid auto constant extra info parameter" );
									}
								}
							}
							break;

							#endregion Int

							#region Real

						case GpuProgramParameters.AutoConstantDataType.Real:
							if( def.AutoConstantType == GpuProgramParameters.AutoConstantType.Time ||
							    def.AutoConstantType == GpuProgramParameters.AutoConstantType.FrameTime )
							{
								UnityEngine.Real f = 1.0f;
								if( value2 != null )
								{
									_getReal( value2, out f );
								}

								try
								{
									//TODO
                                    if (paramNamed)
									{
										/*parameters->setNamedAutoConstantReal(name, def->acType, f);*/
									}
									else
									{
										parameters.SetAutoConstant( index, def.AutoConstantType, f );
									}
								}
								catch
								{
									compiler.AddError( CompileErrorCode.InvalidParameters,file,line,
									                   "setting of constant failed" );
								}
							}
							else
							{
								if( value2 != null )
								{
									UnityEngine.Real extraInfo = 0.0f;
									if( _getReal( value2, out extraInfo ) )
									{
										try
										{
											//TODO
                                            if (paramNamed)
											{
												/*parameters->setNamedAutoConstantReal(name, def->acType, extraInfo);*/
											}
											else
											{
												parameters.SetAutoConstant( index, def.AutoConstantType, extraInfo );
											}
										}
										catch
										{
											compiler.AddError( CompileErrorCode.InvalidParameters,file,line,
											                   "setting of constant failed" );
										}
									}
									else
									{
										compiler.AddError( CompileErrorCode.InvalidParameters,file,line,
										                   "incorrect float argument definition in extra parameters" );
									}
								}
								else
								{
									compiler.AddError( CompileErrorCode.NumberExpected,file,line,
                                                       "extra parameters required by constant definition " + autoParamTarget);
								}
							}
							break;

							#endregion Real
					}
				}
				else
				{
					compiler.AddError( CompileErrorCode.InvalidParameters,file,line );
                }
                #endregion
            }

          
            private void ProcessAutoParams(ScriptCompiler compiler,GpuProgramParameters parameters)
            {
                parameters.AutoAddParamName = false; // prevent bad params
                parameters.IgnoreMissingParameters = true;

                //if (unityAutoParams == null)
                //{
                //    unityAutoParams = GpuProgramManager.Instance.CreateParameters();
                //     ProcessAutoParam(compiler, parameters, "UNITY_MATRIX_MVP", true, "worldviewproj_matrix", null, null);
                //}

                // UNITY_MATRIX_MVP
                ProcessAutoParam(compiler, parameters, "glstate_matrix_mvp", true, "worldviewproj_matrix", null, null);
                // UNITY_MATRIX_MV
                ProcessAutoParam(compiler, parameters, "glstate_matrix_modelview0", true, "worldview_matrix", null, null);
                // UNITY_MATRIX_IT_MV
                ProcessAutoParam(compiler, parameters, "glstate_matrix_invtrans_modelview0", true, "inverse_transpose_worldview_matrix", null, null);

                ProcessAutoParam(compiler, parameters, "_Object2World", true, "world_matrix", null, null);
                ProcessAutoParam(compiler, parameters, "_World2Object", true, "inverse_world_matrix", null, null);
               
                //// // x is the fade value ranging within [0,1]. y is x quantized into 16 levels
                //ProcessAutoParam(compiler, parameters, "unity_LODFade", true, "worldviewproj_matrix", null, null);
                //// // w is usually 1.0, or -1.0 for odd-negative scale transforms
                //ProcessAutoParam(compiler, parameters, "unity_WorldTransformParams", true, "worldviewproj_matrix", null, null);

             
               // parameters.CopyConstantsFrom(unityAutoParams);
            }

            /// <see cref="Translator.Translate"/>
            public override void Translate(ScriptCompiler compiler, AbstractNode node)
			{
				ObjectAbstractNode obj = (ObjectAbstractNode)node;

				// Create the technique from the materia

                Pass pass = null;
                if (obj.Parent.Context is Technique)
                {
                    technique = (Technique)obj.Parent.Context;
                    if (technique.PassCount == 0)
                    {
                        pass = technique.CreatePass();
                    }
                }
                else
                {
                    //TODO pass
                    pass = (Pass)obj.Parent.Context;
                }

                // Allocate the program
                object progObj;
                Axiom.CgPrograms.CgProgram prog = null;

               
                // -------------------------------------------

                // GEST DATAS
                Axiom.Collections.NameValuePairList customParameters = new Axiom.Collections.NameValuePairList();
				string syntax = string.Empty, source = string.Empty;

                source = ((ObjectAbstractNode)node).Children[0].Value;
                string language = "cg";
                string passName = System.IO.Path.GetFileNameWithoutExtension(node.File);
                //syntax ="ps_2_0";
                syntax = "arbvp1";
             
                System.Text.StringBuilder builder = new System.Text.StringBuilder();

                // EXTRACT PROGRAM pragmas, resolve includes
                using (System.IO.StringReader reader = new System.IO.StringReader(source))
                {
                    string lineTrimmed;
                    string line = null;
                    do{
                        line = reader.ReadLine();
                        if (line != null)
                        {
                            lineTrimmed = line.TrimStart();
                            if (lineTrimmed.StartsWith("#pragma"))
                            {
                                ScriptCompilerEvent evnt = new CreateHighLevelGpuProgramScriptCompilerEvent(obj.File, passName, compiler.ResourceGroup, source, language, GpuProgramType.Vertex);
                                bool processed = compiler._fireEvent(ref evnt, out progObj);

                                ProcessPragma(lineTrimmed.Substring(7));
                            }
                            //else if (lineTrimmed.StartsWith("#include"))
                            //{
                            //    int ii = line.IndexOf("\"");
                            //    if (ii != -1)
                            //    {
                            //        line = line.Substring(0, ii+1) + includeRelativePath + line.Substring(ii+1);
                            //    }
                            //    builder.Append(line);
                            //    builder.Append("\n");
                            //}
                            else 
                            {
                                builder.Append(line);
                                builder.Append("\n");
                            }
                        }
                    }
                    while(line != null);
                }
                source = builder.ToString();

                // ALLOCATE THE PROGRAM

                // VERTEX E FRAGMENT
                string name;
                GpuProgramType type;
                for (int i = 0; i < 2; i++)
                {
                    if (i == 0)
                    {
                        name = passName + "_vs";
                        type = GpuProgramType.Vertex;
                        syntax = "arbvp1";
                    }
                    else
                    {
                        name = passName + "_fs";
                        type = GpuProgramType.Fragment;
                        syntax = "arbfp1";
                    }

                    ScriptCompilerEvent evnt = new CreateHighLevelGpuProgramScriptCompilerEvent(obj.File, name, compiler.ResourceGroup, source, language, type);

                    bool processed = compiler._fireEvent(ref evnt, out progObj);

                    // CHECK VERSION

                    if (!GpuProgramManager.Instance.IsSyntaxSupported(syntax))
                    {
                        compiler.AddError(CompileErrorCode.UnsupportedByRenderSystem, obj.File, obj.Line);
                        //Register the unsupported program so that materials that use it know that
                        //it exists but is unsupported
                        GpuProgram unsupportedProg = GpuProgramManager.Instance.Create(obj.Name, compiler.ResourceGroup, type, syntax);

                        return;
                    }

                    if (!processed)
                    {
                        prog = (Axiom.CgPrograms.CgProgram)(HighLevelGpuProgramManager.Instance.CreateProgram(name, compiler.ResourceGroup, language, type));

                        prog.SourceFile = source;
                    }
                    else
                    {
                        prog = (Axiom.CgPrograms.CgProgram)progObj;
                    }

                    // Check that allocation worked
                    if (prog == null)
                    {
                        compiler.AddError(CompileErrorCode.ObjectAllocationError, obj.File, obj.Line, "gpu program \"" + name + "\" could not be created");
                        return;
                    }

                    obj.Context = prog;

                    prog.IsMorphAnimationIncluded = false;
                    prog.PoseAnimationCount = 0;
                    prog.IsSkeletalAnimationIncluded = false;
                    prog.IsVertexTextureFetchRequired = false;
                    prog.Origin = obj.File;
                    prog.Source = source;

                
                    // PASS LINK
                    if (i == 0)
                    {
                        prog.entry = vertexEntryPoint;
                        prog.profiles = new string[] { syntax };

                        pass.SetVertexProgram(name);

                        ProcessAutoParams(compiler,pass.VertexProgramParameters);
                    }
                    else
                    {
                        prog.entry = fragEntryPoint;
                        prog.profiles = new string[] { syntax };

                        pass.SetFragmentProgram(name);

                        ProcessAutoParams(compiler,pass.FragmentProgramParameters);
                        
                    }

                }

                // Set the custom parameters
              //  prog.SetParameters(customParameters);


                // Set the custom parameters
                //foreach (KeyValuePair<string, string> currentParam in customParameters)
                //{
                //    string param = currentParam.Key;
                //    string val = currentParam.Value;

                //    if (!prog.SetParam(param, val))
                //    {
                //        UnityEngine.Debug.LogError("Error in program {0} parameter {1} is not valid.", source, param);
                //    }
                //}

                // Set up default parameters
//                if (prog.IsSupported && parameters != null)
//                {
//#warning this need GpuProgramParametersShared implementation
//                    //GpuProgramParametersSharedPtr ptr = prog->getDefaultParameters();
//                    //GpuProgramTranslator::translateProgramParameters(compiler, ptr, reinterpret_cast<ObjectAbstractNode*>(params.get()));
//                }
			}

			#endregion Translator Implementation
		}
	}
}
