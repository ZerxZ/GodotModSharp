﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodegenCS;

namespace GodotModSharp.SourceGenerators.Helper;

public class CodeBuilder
{
    public static readonly CodeBuilder       Default  = new CodeBuilder();
    private readonly       CodegenTextWriter _builder = new CodegenTextWriter();
    public                 List<string>      Using            { get; }      = new List<string>();
    public                 string            Namespace        { get; set; } = string.Empty;
    public                 string            ClassName        { get; set; } = string.Empty;
    public                 string            ClassModifier    { get; set; } = string.Empty;
    public                 string            ClassInheritance { get; set; } = string.Empty;
    public                 List<string>      ClassInterfaces  { get; set; } = new List<string>();
    public                 List<string>      ClassAttributes  { get; set; } = new List<string>();
    public                 List<string>      ClassFields      { get; set; } = new List<string>();
    public                 List<string>      ClassProperties  { get; set; } = new List<string>();
    public                 List<string>      ClassMethods     { get; set; } = new List<string>();
    public                 List<string>      ClassEnums       { get; set; } = new List<string>();

    public void Write(string     code)                                   => _builder.Write(code);
    public void WriteLine(string code)                                   => _builder.WriteLine(code);
    public void WriteLine()                                              => _builder.WriteLine();
    public void WriteFormat(string         format, params object[] args) => _builder.Write(format, args);
    public void WriteUsing(string          ns)                                                      => _builder.WriteLine($"using {ns};");
    public void WriteNamespace(string      ns)                                                      => _builder.WriteLine($"namespace {ns} {{");
    public void WriteClass(string          name)                                                    => _builder.WriteLine($"partial class {name} {{");
    public void WriteMethod(string         returnType, string name, string parameters, string body) => _builder.WriteLine($"{returnType} {name}({parameters}) {{ {body} }}");
    public void WriteProperty(string       type,       string name)                                 => _builder.WriteLine($"public {type} {name} {{ get; set; }}");
    public void WriteField(string          type,       string name)                                 => _builder.WriteLine($"public {type} {name};");
    public void WriteStaticField(string    type,       string name)                                 => _builder.WriteLine($"public static {type} {name};");
    public void WriteStaticProperty(string type,       string name)                                 => _builder.WriteLine($"public static {type} {name} {{ get; set; }}");
    public void WriteStaticMethod(string   returnType, string name, string parameters, string body) => _builder.WriteLine($"public static {returnType} {name}({parameters}) {{ {body} }}");
    public void WriteInterface(string      name) => _builder.WriteLine($"public interface {name} {{ }}");
    public void WriteEnum(string           name) => _builder.WriteLine($"public enum {name} {{ }}");
    public void WriteAttribute(string      name) => _builder.WriteLine($"[{name}]");
    public void WriteAutoGenerated()             => _builder.WriteLine("// <auto-generated/>");
    public void WriteEnd()                       => _builder.WriteLine("}");
    public string Build()
    {
        WriteAutoGenerated();
        var usingFormatted = $$"""
                               {{UsingFormattableString}}
                               """;
        _builder.WriteLine(usingFormatted);

        return _builder.ToString();
    }
    public void NamespaceFormattableString() => _builder.WriteLine($$"""
                                                                     namespace {{Namespace}}{
                                                                     }
                                                                     """);
    public void UsingFormattableString() => _builder.WriteLine($$"""
                                                                     {{Using.Select(@using => $"using {@using};")}}
                                                                     {{NamespaceFormattableString}}
                                                                 """);
    public override string ToString() => _builder.ToString();
    public void Clear()
    {
        _builder.Flush();
        Using.Clear();
        Namespace = string.Empty;
        ClassName = string.Empty;
        ClassModifier = string.Empty;
        ClassInheritance = string.Empty;
        ClassInterfaces.Clear();
        ClassAttributes.Clear();
        ClassFields.Clear();
        ClassProperties.Clear();
        ClassMethods.Clear();
        ClassEnums.Clear();
    }
}