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
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <id value="$Id$"/>
// </file>

#endregion SVN Version Information

#region Namespace Declarations

using System;
using System.Collections.Generic;

#endregion Namespace Declarations

namespace Axiom.Scripting.Compiler.Parser
{
	public class ScriptParser
	{
		private enum ParserState
		{
			Ready,
			Object
		}

		public ScriptParser() {}

		public IList<ConcreteNode> Parse( IList<ScriptToken> tokens )
		{
			List<ConcreteNode> nodes = new List<ConcreteNode>();

			ParserState state = ParserState.Ready;
			ScriptToken token;
			ConcreteNode parent = null, node = null;

			int iter = 0;
			int end = tokens.Count;

			while( iter != end )
			{
				token = tokens[ iter ];
				switch( state )
				{
						#region Ready

					case ParserState.Ready:
						switch( token.type )
						{
								#region Word

							case Tokens.Word:
								switch( token.lexeme )
								{
                                    case "CGPROGRAM": // UNITY ADDON
                                        node = new ConcreteNode();
                                        node.Token = token.lexeme;
                                        node.File = token.file;
                                        node.Line = token.line;
                                        node.Type = ConcreteNodeType.Word;

                                        // cerco ENDCG
                                        System.Text.StringBuilder builder = new System.Text.StringBuilder();

                                        uint line = 0;
                                        iter++;
                                        int indent = 0;
                                        while (iter != end && ( tokens[iter].lexeme != "ENDCG"))
                                        {
                                            if (tokens[iter].type == Tokens.LeftBrace) indent++;
                                            if (tokens[iter].type == Tokens.RightBrace ||
                                                tokens[iter].lexeme =="};") indent--;

                                            if (tokens[iter].line != line)
                                            {
                                                line = tokens[iter].line;
                                                builder.Append(new string(' ', indent * 4));
                                            }
                                            else
                                                builder.Append(" ");
                                            builder.Append(tokens[iter].lexeme);
                                            iter++;
                                        }

                                        ConcreteNode temp1 = new ConcreteNode();
                                        temp1.Parent = node;
                                        temp1.Line = tokens[iter].line;
                                        temp1.Type = ConcreteNodeType.LeftBrace;
                                        temp1.Token = "{";
                                        node.Children.Add(temp1);

                                        ConcreteNode temp2 = new ConcreteNode();
                                        temp2.Parent = node;
                                        temp2.File = tokens[iter].file;
                                        temp2.Line = tokens[iter].line;
                                        temp2.Type = ConcreteNodeType.Import;
                                        temp2.Token = builder.ToString();
                                        temp1.Children.Add(temp2);

                                        temp1 = new ConcreteNode();
                                        temp1.Parent = node;
                                        temp1.Line = tokens[iter].line;
                                        temp1.Type = ConcreteNodeType.RightBrace;
                                        temp1.Token = "}";
                                        node.Children.Add(temp1);

                                        // Consume all the newlines
                                        iter = SkipNewlines(tokens, iter, end);

                                        // Insert the node
                                        if (parent != null)
                                        {
                                            node.Parent = parent;
                                            parent.Children.Add(node);
                                        }
                                        else
                                        {
                                            node.Parent = null;
                                            nodes.Add(node);
                                        }
                                        node = null;
                                        break;

										#region "import"

									case "import":
										node = new ConcreteNode();
										node.Token = token.lexeme;
										node.File = token.file;
										node.Line = token.line;
										node.Type = ConcreteNodeType.Import;

										// The next token is the target
										iter++;
										if( iter == end || ( tokens[ iter ].type != Tokens.Word && tokens[ iter ].type != Tokens.Quote ) )
										{
											throw new Exception( String.Format( "Expected import target at line {0}", node.Line ) );
										}

										ConcreteNode temp = new ConcreteNode();
										temp.Parent = node;
										temp.File = tokens[ iter ].file;
										temp.Line = tokens[ iter ].line;
										temp.Type = tokens[ iter ].type == Tokens.Word ? ConcreteNodeType.Word : ConcreteNodeType.Quote;
										if( temp.Type == ConcreteNodeType.Quote )
										{
											temp.Token = tokens[ iter ].lexeme.Substring( 1, token.lexeme.Length - 2 );
										}
										else
										{
											temp.Token = tokens[ iter ].lexeme;
										}
										node.Children.Add( temp );

										// The second-next token is the source
										iter++; // "from"
										iter++;
										if( iter == end || ( tokens[ iter ].type != Tokens.Word && tokens[ iter ].type != Tokens.Quote ) )
										{
											throw new Exception( String.Format( "Expected import source at line {0}", node.Line ) );
										}

										temp = new ConcreteNode();
										temp.Parent = node;
										temp.File = tokens[ iter ].file;
										temp.Line = tokens[ iter ].line;
										temp.Type = tokens[ iter ].type == Tokens.Word ? ConcreteNodeType.Word : ConcreteNodeType.Quote;
										if( temp.Type == ConcreteNodeType.Quote )
										{
											temp.Token = tokens[ iter ].lexeme.Substring( 1, token.lexeme.Length - 2 );
										}
										else
										{
											temp.Token = tokens[ iter ].lexeme;
										}
										node.Children.Add( temp );

										// Consume all the newlines
										iter = SkipNewlines( tokens, iter, end );

										// Insert the node
										if( parent != null )
										{
											node.Parent = parent;
											parent.Children.Add( node );
										}
										else
										{
											node.Parent = null;
											nodes.Add( node );
										}
										node = null;
										break;

										#endregion "import"

										#region "set"

									case "set":
										node = new ConcreteNode();
										node.Token = token.lexeme;
										node.File = token.file;
										node.Line = token.line;
										node.Type = ConcreteNodeType.VariableAssignment;

										// The next token is the variable
										++iter;
										if( iter == end || tokens[ iter ].type != Tokens.Variable )
										{
											throw new Exception( string.Format( "expected variable name at line {0}", node.Line ) );
										}

										temp = new ConcreteNode();
										temp.Parent = node;
										temp.File = tokens[ iter ].file;
										temp.Line = tokens[ iter ].line;
										temp.Type = ConcreteNodeType.Variable;
										temp.Token = tokens[ iter ].lexeme;
										node.Children.Add( temp );

										// The next token is the assignment
										++iter;
										if( iter == end || ( tokens[ iter ].type != Tokens.Word && tokens[ iter ].type != Tokens.Quote ) )
										{
											throw new Exception( string.Format( "expected variable name at line {0}", node.Line ) );
										}

										temp = new ConcreteNode();
										temp.Parent = node;
										temp.File = tokens[ iter ].file;
										temp.Line = tokens[ iter ].line;
										temp.Type = tokens[ iter ].type == Tokens.Word ? ConcreteNodeType.Word : ConcreteNodeType.Quote;
										if( temp.Type == ConcreteNodeType.Quote )
										{
											temp.Token = tokens[ iter ].lexeme.Substring( 1, tokens[ iter ].lexeme.Length - 2 );
										}
										else
										{
											temp.Token = tokens[ iter ].lexeme;
										}
										node.Children.Add( temp );

										// Consume all the newlines
										iter = SkipNewlines( tokens, iter, end );

										// Insert the node
										if( parent != null )
										{
											node.Parent = parent;
											parent.Children.Add( node );
										}
										else
										{
											node.Parent = null;
											nodes.Add( node );
										}
										node = null;
										break;

										#endregion "set"

									default:
										node = new ConcreteNode();
										node.File = token.file;
										node.Line = token.line;
										node.Type = token.type == Tokens.Word ? ConcreteNodeType.Word : ConcreteNodeType.Quote;
										if( node.Type == ConcreteNodeType.Quote )
										{
											node.Token = token.lexeme.Substring( 1, token.lexeme.Length - 2 );
										}
										else
										{
											node.Token = token.lexeme;
										}

										// Insert the node
										if( parent != null )
										{
											node.Parent = parent;
											parent.Children.Add( node );
										}
										else
										{
											node.Parent = null;
											nodes.Add( node );
										}

										// Set the parent
										parent = node;

										// Switch states
										state = ParserState.Object;

										node = null;

										break;
								}
								break;

								#endregion Word

								#region RightBrace

							case Tokens.RightBrace:
								// Go up one level if we can
								if( parent != null )
								{
									parent = parent.Parent;
								}

								node = new ConcreteNode();
								node.File = token.file;
								node.Line = token.line;
								node.Token = token.lexeme;
								node.Type = ConcreteNodeType.RightBrace;

								// Consume all the newlines
								iter = SkipNewlines( tokens, iter, end );

								// Insert the node
								if( parent != null )
								{
									node.Parent = parent;
									parent.Children.Add( node );
								}
								else
								{
									node.Parent = null;
									nodes.Add( node );
								}

								// Move up another level
								if( parent != null )
								{
									parent = parent.Parent;
								}

								node = null;

								break;

								#endregion RightBrace
						}
						break;

						#endregion Ready

						#region Object

					case ParserState.Object:
						switch( token.type )
						{
								#region Newline

							case Tokens.Newline:
								// Look ahead to the next non-newline token and if it isn't an {, this was a property
								int next = SkipNewlines( tokens, iter, end );
								if( next == end || tokens[ next ].type != Tokens.LeftBrace )
								{
									// Ended a property here
									if( parent != null )
									{
										parent = parent.Parent;
									}
									state = ParserState.Ready;
								}
								node = null;
								break;

								#endregion Newline

								#region Colon

							case Tokens.Colon:
								node = new ConcreteNode() {
								                          	File = token.file,
								                          	Line = token.line,
								                          	Token = token.lexeme,
								                          	Type = ConcreteNodeType.Colon
								                          };

								// The following token are the parent objects (base classes).
								// Require at least one of them.

								int j = iter + 1;
								j = SkipNewlines( tokens, j, end );
								if( j == end || ( tokens[ j ].type != Tokens.Word && tokens[ j ].type != Tokens.Quote ) )
								{
									throw new Exception( String.Format( "Expected object identifier at line {0}", node.Line ) );
								}

								while( j != end && ( tokens[ j ].type == Tokens.Word || tokens[ j ].type == Tokens.Quote ) )
								{
									ConcreteNode tempNode = new ConcreteNode() {
									                                           	Token = tokens[ j ].lexeme,
									                                           	File = tokens[ j ].file,
									                                           	Line = tokens[ j ].line,
									                                           	Type = tokens[ j ].type == Tokens.Word ? ConcreteNodeType.Word : ConcreteNodeType.Quote,
									                                           	Parent = node
									                                           };
									node.Children.Add( tempNode );
									++j;
								}

								// Move it backwards once, since the end of the loop moves it forwards again anyway
								j--;
								iter = j;

								// Insert the node
								if( parent != null )
								{
									node.Parent = parent;
									parent.Children.Add( node );
								}
								else
								{
									node.Parent = null;
									nodes.Add( node );
								}
								node = null;
								break;

								#endregion Colon

								#region LeftBrace

							case Tokens.LeftBrace:
								node = new ConcreteNode() {
								                          	File = token.file,
								                          	Line = token.line,
								                          	Token = token.lexeme,
								                          	Type = ConcreteNodeType.LeftBrace
								                          };
								// Skip newlines
								iter = SkipNewlines( tokens, iter, end );

								// Insert the node
								if( parent != null )
								{
									node.Parent = parent;
									parent.Children.Add( node );
								}
								else
								{
									node.Parent = null;
									nodes.Add( node );
								}

								// Set the parent
								parent = node;

								// Change the state
								state = ParserState.Ready;

								node = null;
								break;

								#endregion LeftBrace

								#region RightBrace

							case Tokens.RightBrace:

								// Go up one level if we can
								if( parent != null )
								{
									parent = parent.Parent;
								}

								// If the parent is currently a { then go up again
								if( parent != null && parent.Type == ConcreteNodeType.LeftBrace && parent.Parent != null )
								{
									parent = parent.Parent;
								}

								node = new ConcreteNode() {
								                          	File = token.file,
								                          	Line = token.line,
								                          	Token = token.lexeme,
								                          	Type = ConcreteNodeType.RightBrace
								                          };
								// Skip newlines
								iter = SkipNewlines( tokens, iter, end );

								// Insert the node
								if( parent != null )
								{
									node.Parent = parent;
									parent.Children.Add( node );
								}
								else
								{
									node.Parent = null;
									nodes.Add( node );
								}

								//Move up another level
								if( parent != null )
								{
									parent = parent.Parent;
								}

								node = null;
								state = ParserState.Ready;

								break;

								#endregion RightBrace

								#region Variable

							case Tokens.Variable:
								node = new ConcreteNode() {
								                          	File = token.file,
								                          	Line = token.line,
								                          	Token = token.lexeme,
								                          	Type = ConcreteNodeType.Variable
								                          };
								// Insert the node
								if( parent != null )
								{
									node.Parent = parent;
									parent.Children.Add( node );
								}
								else
								{
									node.Parent = null;
									nodes.Add( node );
								}

								node = null;
								break;

								#endregion Variable

								#region Quote

							case Tokens.Quote:
								node = new ConcreteNode() {
								                          	File = token.file,
								                          	Line = token.line,
								                          	Token = token.lexeme.Substring( 1, token.lexeme.Length - 2 ),
								                          	Type = ConcreteNodeType.Quote
								                          };
								// Insert the node
								if( parent != null )
								{
									node.Parent = parent;
									parent.Children.Add( node );
								}
								else
								{
									node.Parent = null;
									nodes.Add( node );
								}

								node = null;
								break;

								#endregion Quote

								#region Word

							case Tokens.Word:
								node = new ConcreteNode() {
								                          	File = token.file,
								                          	Line = token.line,
								                          	Token = token.lexeme,
								                          	Type = ConcreteNodeType.Word
								                          };
								// Insert the node
								if( parent != null )
								{
									node.Parent = parent;
									parent.Children.Add( node );
								}
								else
								{
									node.Parent = null;
									nodes.Add( node );
								}

								node = null;
								break;

								#endregion Word
						}
						break;

						#endregion Object
				}
				iter++;
			}

			return nodes;
		}

		public IList<ConcreteNode> ParseChunk( IList<ScriptToken> tokens )
		{
			IList<ConcreteNode> nodes = new List<ConcreteNode>();
			ConcreteNode node = null;
			foreach( ScriptToken token in tokens )
			{
				switch( token.type )
				{
					case Tokens.Variable:
						node = new ConcreteNode() {
						                          	Token = token.lexeme,
						                          	Type = ConcreteNodeType.Variable,
						                          	File = token.file,
						                          	Line = token.line,
						                          	Parent = null
						                          };
						break;

					case Tokens.Word:
						node = new ConcreteNode() {
						                          	Token = token.lexeme,
						                          	Type = ConcreteNodeType.Word,
						                          	File = token.file,
						                          	Line = token.line,
						                          	Parent = null
						                          };
						break;

					case Tokens.Quote:
						node = new ConcreteNode() {
						                          	Token = token.lexeme.Substring( 1, token.lexeme.Length - 2 ),
						                          	Type = ConcreteNodeType.Quote,
						                          	File = token.file,
						                          	Line = token.line,
						                          	Parent = null
						                          };
						break;

					default:
						throw new Exception( String.Format( "unexpected token {0} at line {1}.", token.lexeme, token.line ) );
				}

				if( node != null )
				{
					nodes.Add( node );
				}
			}
			return nodes;
		}

		private ScriptToken GetToken( IEnumerator<ScriptToken> iter, int offset )
		{
			ScriptToken token = new ScriptToken();
			while( --offset > 1 && iter.MoveNext() != false )
			{
				;
			}

			if( iter.MoveNext() != false )
			{
				token = iter.Current;
			}
			return token;
		}

		private int SkipNewlines( IList<ScriptToken> tokens, int iter, int end )
		{
			while( tokens[ iter ].type == Tokens.Newline && ++iter != end )
			{
				;
			}
			return iter;
		}
	}
}
