using System;
using System.Dynamic;

namespace RedstoneScript.Lexer;

public record Token(string Value, TokenType Type);
