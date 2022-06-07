using BTC.Utils.Utils;
using Newtonsoft.Json;
using SchematBlokowy.Dictionaries;
using SchematBlokowy.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchematBlokowy.Application
{
    public class SchematBlokowyService
    {

        public SchematBlokowyConverter SchematBlokowyConverter { get; set; }
        public SchematBlokowyService()
        {
            SchematBlokowyConverter = new SchematBlokowyConverter();
        }
        public CodeDTO ConvertSchematBlokowy(string model, int type)
        {
            Editor editor = JsonConvert.DeserializeObject<Editor>(model);
            List<Node> nodes = editor.Nodes.Values.ToList();
            CodeLanguageEnum codeLanguage = CodeLanguageEnum.CSharp;
            string content = "";
            switch (type)
            {
                case 0:
                    codeLanguage = CodeLanguageEnum.CSharp;
                    content = GeneraterCSharpCode(nodes);
                    break;
                case 1:
                    codeLanguage = CodeLanguageEnum.CPlusPlus;
                    content = GeneraterCPlusPlusCode(nodes);
                    break;
                case 2:
                    codeLanguage = CodeLanguageEnum.C;
                    content = GeneraterCCode(nodes);
                    break;
            }
            Code code = SchematBlokowyConverter.ToCode(content, codeLanguage);

            CodeDTO codeDTO = new CodeDTO()
            {
                Id = code.Id,
                FileName = code.CodeLanguage == CodeLanguageEnum.C ? UtilsCResource.FileName : code.CodeLanguage == CodeLanguageEnum.CPlusPlus ? UtilsCPlusPlusResource.FileName : UtilsCSharpResource.FileName,
                Content = code.Value
            };
            return codeDTO;

        }

        private string GeneraterCSharpCode(List<Node> nodes)
        {
            StringBuilder content = new StringBuilder();
            StringBuilder modelContent = new StringBuilder();
            Dictionary<string, string> variables = new Dictionary<string, string>();
            bool itsLoop = false;
            Node start = nodes.FirstOrDefault(x => x.Name == "Start");

            if (start != null)
            {
                content.AppendLine(UtilsCSharpResource.UsingSystem);
                string nameSpace = UtilsCSharpResource.Namespace.Replace("{0}", (UtilsCSharpResource.ClassProgram.Replace("{0}", UtilsCSharpResource.MethodMain)));
                content.AppendLine(nameSpace);
                content.AppendLine("");
                cSharpRecursive(start, ref nodes, ref modelContent, ref variables, ref itsLoop);
                return content.ToString().Replace("{0}", modelContent.ToString());
            }
            else
                return "";
        }

        private string cSharpRecursive(Node block, ref List<Node> nodes, ref StringBuilder modelContent, ref Dictionary<string, string> variables, ref bool itsLoop)
        {
            Node next = new Node();
            Node out1 = new Node();
            Node out2 = new Node();

            if (block == null || block.Id == 0)
                return modelContent.ToString();
            else
            {
                if (block.Outputs.Num != null)
                    next = nodes.FirstOrDefault(x => x.Id == block.Outputs.Num.Connections[0].Node);
                if (block.Outputs.Out1 != null)
                    out1 = nodes.FirstOrDefault(x => x.Id == block.Outputs.Out1.Connections[0].Node);
                if (block.Outputs.Out2 != null)
                    out2 = nodes.FirstOrDefault(x => x.Id == block.Outputs.Out2.Connections[0].Node);
            }

            switch (block.Name)
            {
                case "Blok wprowadzania danych":
                    string[] inData = block.Data.Text.Replace(';', ' ').Split(',');
                    foreach (string item in inData)
                    {
                        string type = item.TrimStart().TrimEnd().Any(x => x == ' ') ? item.TrimStart().Split(' ')[0] : "";
                        string name = item.TrimStart().TrimEnd().Any(x => x == ' ') ? item.TrimStart().Split(' ')[1].Split('=')[0] : item;
                        string value = item.Any(x => x == '=') ? item.Split('=')[1].TrimStart().TrimEnd() : "";
                        string line = "";
                        if (type.IsNullOrEmpty())
                        {
                            type = variables.FirstOrDefault(x => x.Key == name).Value;
                            switch (type)
                            {
                                case "int":
                                    line = String.Format(UtilsCSharpResource.Input, name, value.IsNullOrEmpty() ? UtilsCSharpResource.ConsoleReadInt : (value + ";"));
                                    break;
                                case "double":
                                    line = String.Format(UtilsCSharpResource.Input, name, value.IsNullOrEmpty() ? UtilsCSharpResource.ConsoleReadDouble : (value + ";"));
                                    break;
                                case "float":
                                    line = String.Format(UtilsCSharpResource.Input, name, value.IsNullOrEmpty() ? UtilsCSharpResource.ConsoleReadFloat : (value + ";"));
                                    break;
                                case "decimal":
                                    line = String.Format(UtilsCSharpResource.Input, name, value.IsNullOrEmpty() ? UtilsCSharpResource.ConsoleReadDecimal : (value + ";"));
                                    break;
                                case "string":
                                    line = String.Format(UtilsCSharpResource.Input, name, value.IsNullOrEmpty() ? UtilsCSharpResource.ConsoleReadString : (value + ";"));
                                    break;
                                case "var":
                                    line = String.Format(UtilsCSharpResource.Input, name, value.IsNullOrEmpty() ? UtilsCSharpResource.ConsoleReadString : (value + ";"));
                                    break;
                                default:
                                    line = String.Format(UtilsCSharpResource.Input, name, value.IsNullOrEmpty() ? UtilsCSharpResource.ConsoleReadString : (value + ";"));
                                    break;
                            }
                        }
                        else
                        {
                            switch (type)
                            {
                                case "int":
                                    line = String.Format(UtilsCSharpResource.DeclareAndInput, type, name, value.IsNullOrEmpty() ? UtilsCSharpResource.ConsoleReadInt : (value + ";"));
                                    variables.Add(name, "int");
                                    break;
                                case "double":
                                    line = String.Format(UtilsCSharpResource.DeclareAndInput, type, name, value.IsNullOrEmpty() ? UtilsCSharpResource.ConsoleReadDouble : (value + ";"));
                                    variables.Add(name, "double");
                                    break;
                                case "float":
                                    line = String.Format(UtilsCSharpResource.DeclareAndInput, type, name, value.IsNullOrEmpty() ? UtilsCSharpResource.ConsoleReadFloat : (value + ";"));
                                    variables.Add(name, "float");
                                    break;
                                case "decimal":
                                    line = String.Format(UtilsCSharpResource.DeclareAndInput, type, name, value.IsNullOrEmpty() ? UtilsCSharpResource.ConsoleReadDecimal : (value + ";"));
                                    variables.Add(name, "decimal");
                                    break;
                                case "string":
                                    line = String.Format(UtilsCSharpResource.DeclareAndInput, type, name, value.IsNullOrEmpty() ? UtilsCSharpResource.ConsoleReadString : (value + ";"));
                                    variables.Add(name, "string");
                                    break;
                                case "var":
                                    line = String.Format(UtilsCSharpResource.DeclareAndInput, type, name, value.IsNullOrEmpty() ? UtilsCSharpResource.ConsoleReadString : (value + ";"));
                                    variables.Add(name, "var");
                                    break;
                                default:
                                    line = String.Format(UtilsCSharpResource.DeclareAndInput, UtilsCSharpResource.VarType, name, value.IsNullOrEmpty() ? UtilsCSharpResource.ConsoleReadString : (value + ";"));
                                    variables.Add(name, "var");
                                    break;
                            }
                        }
                        modelContent.AppendLine(line);
                    }
                    return cSharpRecursive(next, ref nodes, ref modelContent, ref variables, ref itsLoop);
                case "Blok operacji":
                    string[] operationData = block.Data.Text.Replace(';', ' ').Split(',');
                    foreach (string item in operationData)
                    {
                        modelContent.AppendLine(item + ";");
                    }
                    return cSharpRecursive(next, ref nodes, ref modelContent, ref variables, ref itsLoop);
                case "Blok warunku":
                    if (itsLoop == true)
                    {
                        itsLoop = false;
                        return modelContent.ToString();
                    }
                    string conditionalData = block.Data.Text.Replace(';', ' ');
                    string conditioonalLine = "";
                    StringBuilder internalContent = new StringBuilder();
                    if (block.Inputs.Num.Connections.Length > 1)
                    {
                        itsLoop = true;
                        string loopContent = cSharpRecursive(out2, ref nodes, ref internalContent, ref variables, ref itsLoop);
                        internalContent.Clear();
                        string afterLoopContent = cSharpRecursive(out1, ref nodes, ref internalContent, ref variables, ref itsLoop);
                        conditioonalLine = UtilsCSharpResource.Loop.Replace("{0}", conditionalData).Replace("{1}", loopContent);
                        modelContent.AppendLine(conditioonalLine);
                        modelContent.AppendLine(afterLoopContent);
                    }
                    else
                    {
                        string yesContent = cSharpRecursive(out1, ref nodes, ref internalContent, ref variables, ref itsLoop);
                        internalContent.Clear();
                        string noContent = cSharpRecursive(out2, ref nodes, ref internalContent, ref variables, ref itsLoop);
                        conditioonalLine = UtilsCSharpResource.IfElse.Replace("{0}", conditionalData).Replace("{1}", yesContent).Replace("{2}", noContent);
                        modelContent.AppendLine(conditioonalLine);
                    }
                    return modelContent.ToString();
                case "Blok wypisywania danych":
                    string[] outData = block.Data.Text.Replace(';', ' ').Split(',');
                    foreach (string item in outData)
                    {
                        modelContent.AppendLine(String.Format(UtilsCSharpResource.ConsoleWrite, item));
                        modelContent.AppendLine(String.Format(UtilsCSharpResource.ConsoleWrite, "\" \""));
                    }
                    return cSharpRecursive(next, ref nodes, ref modelContent, ref variables, ref itsLoop);
                case "Start":
                    return cSharpRecursive(next, ref nodes, ref modelContent, ref variables, ref itsLoop);
                case "Koniec":
                    return modelContent.ToString();

                case "Blok petli for - poczatek":
                    string[] operationData1 = block.Data.Text.Replace(';', ';').Split(',');
                    foreach (string item in operationData1)
                    {
                        modelContent.AppendLine("for(" + item + "){");
                    }

                    return cSharpRecursive(next, ref nodes, ref modelContent, ref variables, ref itsLoop);
                case "Blok petli for - koniec":
                    modelContent.AppendLine("}");
                    return cSharpRecursive(next, ref nodes, ref modelContent, ref variables, ref itsLoop);

                case "Blok petli while - poczatek":
                    string[] operationData2 = block.Data.Text.Replace(';', ';').Split(',');
                    foreach (string item in operationData2)
                    {
                        modelContent.AppendLine("while(" + item + "){");
                    }

                    return cSharpRecursive(next, ref nodes, ref modelContent, ref variables, ref itsLoop);
                case "Blok petli while - koniec":
                    modelContent.AppendLine("}");
                    return cSharpRecursive(next, ref nodes, ref modelContent, ref variables, ref itsLoop);

                default:
                    return modelContent.ToString();
            }
        }

        private string GeneraterCPlusPlusCode(List<Node> nodes)
        {
            StringBuilder content = new StringBuilder();
            StringBuilder modelContent = new StringBuilder();
            bool itsLoop = false;
            Node start = nodes.FirstOrDefault(x => x.Name == "Start");

            if (start != null)
            {
                content.AppendLine(UtilsCPlusPlusResource.IncludeIO);
                content.AppendLine(UtilsCPlusPlusResource.IncludeString);
                content.AppendLine(UtilsCPlusPlusResource.UsingStd);
                content.AppendLine(UtilsCPlusPlusResource.MainMethod);
                content.AppendLine("");
                cPlusPlusRecursive(start, ref nodes, ref modelContent, ref itsLoop);
                return content.ToString().Replace("{0}", modelContent.ToString());
            }
            else
                return "";
        }

        private string cPlusPlusRecursive(Node block, ref List<Node> nodes, ref StringBuilder modelContent, ref bool itsLoop)
        {
            Node next = new Node();
            Node out1 = new Node();
            Node out2 = new Node();

            if (block == null || block.Id == 0)
                return modelContent.ToString();
            else
            {
                if (block.Outputs.Num != null)
                    next = nodes.FirstOrDefault(x => x.Id == block.Outputs.Num.Connections[0].Node);
                if (block.Outputs.Out1 != null)
                    out1 = nodes.FirstOrDefault(x => x.Id == block.Outputs.Out1.Connections[0].Node);
                if (block.Outputs.Out2 != null)
                    out2 = nodes.FirstOrDefault(x => x.Id == block.Outputs.Out2.Connections[0].Node);
            }

            switch (block.Name)
            {
                case "Blok wprowadzania danych":
                    string[] inData = block.Data.Text.Replace(';', ' ').Split(',');
                    foreach (string item in inData)
                    {
                        string type = item.TrimStart().TrimEnd().Any(x => x == ' ') ? item.TrimStart().Split(' ')[0] : "";
                        string name = item.TrimStart().TrimEnd().Any(x => x == ' ') ? item.TrimStart().Split(' ')[1].Split('=')[0] : item;
                        string value = item.Any(x => x == '=') ? item.Split('=')[1].TrimStart().TrimEnd() : "";
                        string line = "";
                        if (type.IsNullOrEmpty() == false)
                        {
                            switch (type)
                            {
                                case "int":
                                    line = value.IsNullOrEmpty() ? String.Format(UtilsCPlusPlusResource.Declare, type, name) : String.Format(UtilsCPlusPlusResource.DeclareAndInput, type, name, value);
                                    break;
                                case "double":
                                    line = value.IsNullOrEmpty() ? String.Format(UtilsCPlusPlusResource.Declare, type, name) : String.Format(UtilsCPlusPlusResource.DeclareAndInput, type, name, value);
                                    break;
                                case "float":
                                    line = value.IsNullOrEmpty() ? String.Format(UtilsCPlusPlusResource.Declare, type, name) : String.Format(UtilsCPlusPlusResource.DeclareAndInput, type, name, value);
                                    break;
                                case "string":
                                    line = value.IsNullOrEmpty() ? String.Format(UtilsCPlusPlusResource.Declare, type, name) : String.Format(UtilsCPlusPlusResource.DeclareAndInput, type, name, value);
                                    break;
                                default:
                                    line = "";
                                    break;
                            }
                        }
                        modelContent.AppendLine(line);
                        if (value.IsNullOrEmpty())
                            modelContent.AppendLine(String.Format(UtilsCPlusPlusResource.Cin, name));
                    }
                    return cPlusPlusRecursive(next, ref nodes, ref modelContent, ref itsLoop);
                case "Blok operacji":
                    string[] operationData = block.Data.Text.Replace(';', ' ').Split(',');
                    foreach (string item in operationData)
                    {
                        modelContent.AppendLine(item + ";");
                    }
                    return cPlusPlusRecursive(next, ref nodes, ref modelContent, ref itsLoop);
                case "Blok warunku":
                    if (itsLoop == true)
                    {
                        itsLoop = false;
                        return modelContent.ToString();
                    }
                    string conditionalData = block.Data.Text.Replace(';', ' ');
                    string conditioonalLine = "";
                    StringBuilder internalContent = new StringBuilder();
                    if (block.Inputs.Num.Connections.Length > 1)
                    {
                        itsLoop = true;
                        string loopContent = cPlusPlusRecursive(out2, ref nodes, ref internalContent, ref itsLoop);
                        internalContent.Clear();
                        string afterLoopContent = cPlusPlusRecursive(out1, ref nodes, ref internalContent, ref itsLoop);
                        conditioonalLine = UtilsCPlusPlusResource.Loop.Replace("{0}", conditionalData).Replace("{1}", loopContent);
                        modelContent.AppendLine(conditioonalLine);
                        modelContent.AppendLine(afterLoopContent);
                    }
                    else
                    {
                        string yesContent = cPlusPlusRecursive(out1, ref nodes, ref internalContent, ref itsLoop);
                        internalContent.Clear();
                        string noContent = cPlusPlusRecursive(out2, ref nodes, ref internalContent, ref itsLoop);
                        conditioonalLine = UtilsCPlusPlusResource.IfElse.Replace("{0}", conditionalData).Replace("{1}", yesContent).Replace("{2}", noContent);
                        modelContent.AppendLine(conditioonalLine);
                    }

                    return modelContent.ToString();
                case "Blok wypisywania danych":
                    string[] outData = block.Data.Text.Replace(';', ' ').Split(',');
                    foreach (string item in outData)
                    {
                        string[] internalData = item.Split('+');
                        foreach (string internalItem in internalData)
                        {
                            modelContent.AppendLine(String.Format(UtilsCPlusPlusResource.Cout, internalItem));
                            modelContent.AppendLine(String.Format(UtilsCPlusPlusResource.Cout, "endl "));
                        }
                    }
                    return cPlusPlusRecursive(next, ref nodes, ref modelContent, ref itsLoop);
                case "Start":
                    return cPlusPlusRecursive(next, ref nodes, ref modelContent, ref itsLoop);
                case "Koniec":
                    return modelContent.ToString();

                case "Blok petli for - poczatek":
                    string[] operationData1 = block.Data.Text.Replace(';', ';').Split(',');
                    foreach (string item in operationData1)
                    {
                        modelContent.AppendLine("for(" + item + "){");
                    }

                    return cPlusPlusRecursive(next, ref nodes, ref modelContent, ref itsLoop);
                case "Blok petli for - koniec":
                    modelContent.AppendLine("}");
                    return cPlusPlusRecursive(next, ref nodes, ref modelContent, ref itsLoop);

                case "Blok petli while - poczatek":
                    string[] operationData2 = block.Data.Text.Replace(';', ' ').Split(',');
                    foreach (string item in operationData2)
                    {
                        modelContent.AppendLine("while(" + item + "){");
                    }

                    return cPlusPlusRecursive(next, ref nodes, ref modelContent, ref itsLoop);
                case "Blok petli while - koniec":

                    modelContent.AppendLine("}");
                    return cPlusPlusRecursive(next, ref nodes, ref modelContent, ref itsLoop);


                default:
                    return modelContent.ToString();
            }
        }

        private string GeneraterCCode(List<Node> nodes)
        {
            StringBuilder content = new StringBuilder();
            StringBuilder modelContent = new StringBuilder();
            Dictionary<string, string> variables = new Dictionary<string, string>();
            bool itsLoop = false;
            Node start = nodes.FirstOrDefault(x => x.Name == "Start");

            if (start != null)
            {
                content.AppendLine(UtilsCResource.IncludeStdio);

                content.AppendLine(UtilsCResource.MainMethod);
                content.AppendLine("");
                cRecursive(start, ref nodes, ref modelContent, ref variables, ref itsLoop);
                return content.ToString().Replace("{0}", modelContent.ToString());
            }
            else
                return "";
        }

        private string cRecursive(Node block, ref List<Node> nodes, ref StringBuilder modelContent, ref Dictionary<string, string> variables, ref bool itsLoop)
        {
            Node next = new Node();
            Node out1 = new Node();
            Node out2 = new Node();

            if (block == null || block.Id == 0)
                return modelContent.ToString();
            else
            {
                if (block.Outputs.Num != null)
                    next = nodes.FirstOrDefault(x => x.Id == block.Outputs.Num.Connections[0].Node);
                if (block.Outputs.Out1 != null)
                    out1 = nodes.FirstOrDefault(x => x.Id == block.Outputs.Out1.Connections[0].Node);
                if (block.Outputs.Out2 != null)
                    out2 = nodes.FirstOrDefault(x => x.Id == block.Outputs.Out2.Connections[0].Node);
            }

            switch (block.Name)
            {
                case "Blok wprowadzania danych":
                    string[] inData = block.Data.Text.Replace(';', ' ').Split(',');
                    foreach (string item in inData)
                    {
                        string type = item.TrimStart().TrimEnd().Any(x => x == ' ') ? item.TrimStart().Split(' ')[0] : "";
                        string name = item.TrimStart().TrimEnd().Any(x => x == ' ') ? item.TrimStart().Split(' ')[1].Split('=')[0] : item;
                        string value = item.Any(x => x == '=') ? item.Split('=')[1].TrimStart().TrimEnd() : "";
                        string line = "";
                        string declare = "";
                        if (type.IsNullOrEmpty())
                        {
                            type = variables.FirstOrDefault(x => x.Key == name).Value;
                            switch (type)
                            {
                                case "int":
                                    line = String.Format(UtilsCResource.ScanfInt, name);
                                    break;
                                case "double":
                                    line = String.Format(UtilsCResource.ScanfDouble, name);
                                    break;
                                case "float":
                                    line = String.Format(UtilsCResource.ScanfFloat, name);
                                    break;
                                case "char":
                                    line = String.Format(UtilsCResource.ScanfChar, name);
                                    break;
                                case "long":
                                    line = String.Format(UtilsCResource.ScanfLong, name);
                                    break;
                                default:
                                    line = "";
                                    break;
                            }
                        }
                        else
                        {
                            switch (type)
                            {
                                case "int":
                                    line = String.Format(UtilsCResource.ScanfInt, name);
                                    declare = value.IsNullOrEmpty() ? String.Format(UtilsCResource.Declare, type, name) : String.Format(UtilsCResource.DeclareAndInput, type, name, value);
                                    variables.Add(name, "int");
                                    break;
                                case "double":
                                    line = String.Format(UtilsCResource.ScanfDouble, name);
                                    declare = value.IsNullOrEmpty() ? String.Format(UtilsCResource.Declare, type, name) : String.Format(UtilsCResource.DeclareAndInput, type, name, value);
                                    variables.Add(name, "double");
                                    break;
                                case "float":
                                    line = String.Format(UtilsCResource.ScanfFloat, name);
                                    declare = value.IsNullOrEmpty() ? String.Format(UtilsCResource.Declare, type, name) : String.Format(UtilsCResource.DeclareAndInput, type, name, value);
                                    variables.Add(name, "float");
                                    break;
                                case "char":
                                    line = String.Format(UtilsCResource.ScanfChar, name);
                                    declare = value.IsNullOrEmpty() ? String.Format(UtilsCResource.Declare, type, name) : String.Format(UtilsCResource.DeclareAndInput, type, name, value);
                                    variables.Add(name, "char");
                                    break;
                                case "long":
                                    line = String.Format(UtilsCResource.ScanfLong, name);
                                    declare = value.IsNullOrEmpty() ? String.Format(UtilsCResource.Declare, type, name) : String.Format(UtilsCResource.DeclareAndInput, type, name, value);
                                    variables.Add(name, "long");
                                    break;
                                default:
                                    line = "";
                                    break;
                            }
                        }
                        modelContent.AppendLine(declare);
                        if (value.IsNullOrEmpty())
                            modelContent.AppendLine(line);
                    }
                    return cRecursive(next, ref nodes, ref modelContent, ref variables, ref itsLoop);
                case "Blok operacji":
                    string[] operationData = block.Data.Text.Replace(';', ' ').Split(',');
                    foreach (string item in operationData)
                    {
                        modelContent.AppendLine(item + ";");
                    }
                    return cRecursive(next, ref nodes, ref modelContent, ref variables, ref itsLoop);
                case "Blok warunku":
                    if (itsLoop == true)
                    {
                        itsLoop = false;
                        return modelContent.ToString();
                    }
                    string conditionalData = block.Data.Text.Replace(';', ' ');
                    string conditioonalLine = "";
                    StringBuilder internalContent = new StringBuilder();
                    if (block.Inputs.Num.Connections.Length > 1)
                    {
                        itsLoop = true;
                        string loopContent = cRecursive(out2, ref nodes, ref internalContent, ref variables, ref itsLoop);
                        internalContent.Clear();
                        string afterLoopContent = cRecursive(out1, ref nodes, ref internalContent, ref variables, ref itsLoop);
                        conditioonalLine = UtilsCResource.Loop.Replace("{0}", conditionalData).Replace("{1}", loopContent);
                        modelContent.AppendLine(conditioonalLine);
                        modelContent.AppendLine(afterLoopContent);

                    }
                    else
                    {
                        string yesContent = cRecursive(out1, ref nodes, ref internalContent, ref variables, ref itsLoop);
                        internalContent.Clear();
                        string noContent = cRecursive(out2, ref nodes, ref internalContent, ref variables, ref itsLoop);
                        conditioonalLine = UtilsCResource.IfElse.Replace("{0}", conditionalData).Replace("{1}", yesContent).Replace("{2}", noContent);
                        modelContent.AppendLine(conditioonalLine);

                    }

                    return modelContent.ToString();

                case "Blok warunku-koniec":
                    return cRecursive(next, ref nodes, ref modelContent, ref variables, ref itsLoop);
                case "Blok wypisywania danych":
                    string[] outData = block.Data.Text.Replace(';', ' ').Split(',');
                    foreach (string item in outData)
                    {
                        string[] internalData = item.Split('+');
                        foreach (string internalItem in internalData)
                        {
                            if (variables.ContainsKey(internalItem.TrimStart().TrimEnd()))
                            {
                                string internalType = variables.FirstOrDefault(x => x.Key == internalItem.TrimStart().TrimEnd()).Value;
                                string internalName = variables.FirstOrDefault(x => x.Key == internalItem.TrimStart().TrimEnd()).Key;
                                switch (internalType)
                                {
                                    case "int":
                                        modelContent.AppendLine(String.Format(UtilsCResource.PrintfInt, internalName));
                                        break;
                                    case "double":
                                        modelContent.AppendLine(String.Format(UtilsCResource.PrintfDouble, internalName));
                                        break;
                                    case "float":
                                        modelContent.AppendLine(String.Format(UtilsCResource.PrintfFloat, internalName));
                                        break;
                                    case "char":
                                        modelContent.AppendLine(String.Format(UtilsCResource.PrintfChar, internalName));
                                        break;
                                    case "long":
                                        modelContent.AppendLine(String.Format(UtilsCResource.PrintfLong, internalName));
                                        break;
                                }
                            }
                            else
                                modelContent.AppendLine(String.Format(UtilsCResource.PrintfString, internalData));
                            modelContent.AppendLine(String.Format(UtilsCResource.PrintfString, "\"  \""));
                        }
                    }
                    return cRecursive(next, ref nodes, ref modelContent, ref variables, ref itsLoop);
                case "Start":
                    return cRecursive(next, ref nodes, ref modelContent, ref variables, ref itsLoop);
                case "Koniec":
                    return modelContent.ToString();

                case "Blok petli for - poczatek":
                    string[] operationData1 = block.Data.Text.Replace(';', ';').Split(',');
                    foreach (string item in operationData1)
                    {
                        modelContent.AppendLine("for(" + item + "){");
                    }

                    return cRecursive(next, ref nodes, ref modelContent, ref variables, ref itsLoop);
                case "Blok petli for - koniec":
                    modelContent.AppendLine("}");
                    return cRecursive(next, ref nodes, ref modelContent, ref variables, ref itsLoop);


                case "Blok warunku - koniec":
                    modelContent.AppendLine("");
                    return cRecursive(next, ref nodes, ref modelContent, ref variables, ref itsLoop);

                case "Blok petli while - poczatek":
                    string[] operationData2 = block.Data.Text.Replace(';', ';').Split(',');
                    foreach (string item in operationData2)
                    {
                        modelContent.AppendLine("while(" + item + "){");
                    }

                    return cRecursive(next, ref nodes, ref modelContent, ref variables, ref itsLoop);
                case "Blok petli while - koniec":
                    modelContent.AppendLine("}");
                    return cRecursive(next, ref nodes, ref modelContent, ref variables, ref itsLoop);

                default:
                    return modelContent.ToString();
            }
        }
    }
}
