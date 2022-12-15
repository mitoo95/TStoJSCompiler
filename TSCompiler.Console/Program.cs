﻿using TSCompiler.Lexer;
using TSCompiler.Parser;

var code = File.ReadAllText("Code.txt").Replace(Environment.NewLine, "\n");
var input = new Input(code);
var scanner = new Scanner(input);
var parser = new Parser(scanner);


var ast = parser.Parse();
ast.ValidateSemantic();
/*var generatedCode = ast.GenerateCode();
File.WriteAllText("./genCode.txt", generatedCode);*/
