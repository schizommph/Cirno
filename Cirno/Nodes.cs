﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Cirno
{
    class Node
    {
        public virtual ObjectClass Visit(Interpreter interpreter, Enviorment parent)
        {
            Console.WriteLine("Node visit not initialized");
            return new NovaClass();
        }
    }

    class TreeNode : Node
    {
        public List<Node> nodes { get; private set; }
        public TreeNode(List<Node> nodes)
        {
            this.nodes = nodes;
        }

        public override ObjectClass Visit(Interpreter interpreter, Enviorment parent)
        {
            return interpreter.VisitTreeNode(this, parent);
        }
    }

    class UsingNode : Node
    {
        public string path { get; private set; }
        public UsingNode(string path)
        {
            this.path = path;
        }
        public override ObjectClass Visit(Interpreter interpreter, Enviorment parent)
        {
            return interpreter.VisitUsingNode(this, parent);
        }
    }
    class GlobalNode : Node
    {
        public string name { get; private set; }
        public GlobalNode(string name)
        {
            this.name = name;
        }
        public override ObjectClass Visit(Interpreter interpreter, Enviorment parent)
        {
            return interpreter.VisitGlobalNode(this, parent);
        }
    }
    class PrintNode : Node
    {
        public Node value { get; private set; }
        public PrintNode(Node value)
        {
            this.value = value;
        }
        public override ObjectClass Visit(Interpreter interpreter, Enviorment parent)
        {
            return interpreter.VisitPrintNode(this, parent);
        }
    }
    class WhileNode : Node
    {
        public Node expr { get; private set; }
        public Node action { get; private set; }

        public WhileNode(Node expr, Node action)
        {
            this.expr = expr;
            this.action = action;
        }
        public override ObjectClass Visit(Interpreter interpreter, Enviorment parent)
        {
            return interpreter.VisitWhileNode(this, parent);
        }
    }
    class ForNode : Node
    {
        public string varName { get; private set; }
        public Node list { get; private set; }
        public TreeNode action { get; private set; }

        public ForNode(string varName, Node list, TreeNode action)
        {
            this.varName = varName;
            this.list = list;
            this.action = action;
        }
        public override ObjectClass Visit(Interpreter interpreter, Enviorment parent)
        {
            return interpreter.VisitForNode(this, parent);
        }
    }
    class IfNode : Node
    {
        public Dictionary<Node, TreeNode> ifActions { get; private set; }
        public TreeNode elseAction { get; private set; }

        public IfNode(Dictionary<Node, TreeNode> ifActions, TreeNode elseAction)
        {
            this.ifActions = ifActions;
            this.elseAction = elseAction;
        }
        public override ObjectClass Visit(Interpreter interpreter, Enviorment parent)
        {
            return interpreter.VisitIfNode(this, parent);
        }
    }

    class IdentifierNode : Node
    {
        public string name { get; private set; }
        public IdentifierNode(string name)
        {
            this.name = name;
        }
        public override ObjectClass Visit(Interpreter interpreter, Enviorment parent)
        {
            return interpreter.VisitIdentifierNode(this, parent);
        }
    }
    class SetVariableNode : Node
    {
        public string name { get; private set; }
        public Node expr { get; private set; }
        public SetVariableNode(string name, Node expr)
        {
            this.name = name;
            this.expr = expr;
        }
        public override ObjectClass Visit(Interpreter interpreter, Enviorment parent)
        {
            return interpreter.VisitSetVariableNode(this, parent);
        }
    }

    class NumberNode : Node
    {
        public float value { get; private set; }
        public NumberNode(float value)
        {
            this.value = value;
        }
        public override string ToString()
        {
            return $"<NumberNode {value}>";
        }
        public override ObjectClass Visit(Interpreter interpreter, Enviorment parent)
        {
            return interpreter.VisitNumberNode(this, parent);
        }
    }
    class BoolNode : Node
    {
        public bool value { get; private set; }
        public BoolNode(bool value)
        {
            this.value = value;
        }
        public override string ToString()
        {
            return $"<BoolNode {value}>";
        }
        public override ObjectClass Visit(Interpreter interpreter, Enviorment parent)
        {
            return interpreter.VisitBoolNode(this, parent);
        }
    }
    class NovaNode : Node
    {
        public override string ToString()
        {
            return $"<NovaNode>";
        }
        public override ObjectClass Visit(Interpreter interpreter, Enviorment parent)
        {
            return interpreter.VisitNovaNode(this, parent);
        }
    }
    class BreakNode : Node
    {
        public override string ToString()
        {
            return $"<BreakNode>";
        }
        public override ObjectClass Visit(Interpreter interpreter, Enviorment parent)
        {
            return interpreter.VisitBreakNode(this, parent);
        }
    }
    class ContinueNode : Node
    {
        public override string ToString()
        {
            return $"<ContinueNode>";
        }
        public override ObjectClass Visit(Interpreter interpreter, Enviorment parent)
        {
            return interpreter.VisitContinueNode(this, parent);
        }
    }
    class TypeOfObjectNode : Node
    {
        public Node identifier { get; private set; }
        public TypeOfObjectNode(Node identifier)
        {
            this.identifier = identifier;
        }
        public override string ToString()
        {
            return $"<TypeOfObjectNode>";
        }
        public override ObjectClass Visit(Interpreter interpreter, Enviorment parent)
        {
            return interpreter.VisitTypeOfObjectNode(this, parent);
        }
    }
    class ReturnNode : Node
    {
        public Node expr { get; private set; }
        public override string ToString()
        {
            return $"<ReturnNode {expr}>";
        }
        public ReturnNode(Node expr)
        {
            this.expr = expr;
        }
        public override ObjectClass Visit(Interpreter interpreter, Enviorment parent)
        {
            return interpreter.VisitReturnNode(this, parent);
        }
    }
    class StringNode : Node
    {
        public string value { get; private set; }
        public StringNode(string value)
        {
            this.value = value;
        }
        public override string ToString()
        {
            return $"<StringNode {value}>";
        }
        public override ObjectClass Visit(Interpreter interpreter, Enviorment parent)
        {
            return interpreter.VisitStringNode(this, parent);
        }
    }
    class ListNode : Node
    {
        public List<Node> items { get; private set; }
        public ListNode(List<Node> items)
        {
            this.items = items;
        }
        public override string ToString()
        {
            return $"<ListNode {items}>";
        }
        public override ObjectClass Visit(Interpreter interpreter, Enviorment parent)
        {
            return interpreter.VisitListNode(this, parent);
        }
    }
    class DictionaryNode : Node
    {
        public Dictionary<Node, Node> items { get; private set; }
        public DictionaryNode(Dictionary<Node, Node> items)
        {
            this.items = items;
        }
        public override string ToString()
        {
            return $"<DictionaryNode {items}>";
        }
        public override ObjectClass Visit(Interpreter interpreter, Enviorment parent)
        {
            return interpreter.VisitDictionaryNode(this, parent);
        }
    }
    class BinaryOperatorNode : Node
    {
        public Node left { get; private set; }
        public Node right { get; private set; }
        public Token op { get; private set; }
        public BinaryOperatorNode(Node left, Node right, Token op)
        {
            this.left = left;
            this.right = right;
            this.op = op;
        }
        public override string ToString()
        {
            return $"<BinaryOperatorNode {left} {right} {op}>";
        }
        public override ObjectClass Visit(Interpreter interpreter, Enviorment parent)
        {
            return interpreter.VisitBinaryOperatorNode(this, parent);
        }
    }

    class GetItemIndexNode : Node
    {
        public Node expr { get; private set; }
        public Node index { get; private set; }
        public GetItemIndexNode(Node expr, Node index)
        {
            this.expr = expr;
            this.index = index;
        }
        public override ObjectClass Visit(Interpreter interpreter, Enviorment parent)
        {
            return interpreter.VisitGetItemIndexNode(this, parent);
        }
    }
    class SetItemIndexNode : Node
    {
        public GetItemIndexNode item { get; private set; }
        public Node expr { get; private set; }
        public SetItemIndexNode(GetItemIndexNode item, Node expr)
        {
            this.item = item;
            this.expr = expr;
        }
        public override ObjectClass Visit(Interpreter interpreter, Enviorment parent)
        {
            return interpreter.VisitSetItemIndexNode(this, parent);
        }
    }
    class AddToItemNode : Node
    {
        public Node item { get; private set; }
        public Node expr { get; private set; }
        public AddToItemNode(Node item, Node expr)
        {
            this.item = item;
            this.expr = expr;
        }
        public override ObjectClass Visit(Interpreter interpreter, Enviorment parent)
        {
            return interpreter.VisitAddToItemNode(this, parent);
        }
    }
    class PopFromItemNode : Node
    {
        public Node item { get; private set; }
        public Node index { get; private set; }
        public PopFromItemNode(Node item, Node index)
        {
            this.item = item;
            this.index = index;
        }
        public override ObjectClass Visit(Interpreter interpreter, Enviorment parent)
        {
            return interpreter.VisitPopFromItemNode(this, parent);
        }
    }

    // OOP
    class CallNode : Node
    {
        public string name { get; private set; }
        public List<Node> arguments { get; private set; }

        public CallNode(string name, List<Node> arguments)
        {
            this.name = name;
            this.arguments = arguments;
        }
        public override ObjectClass Visit(Interpreter interpreter, Enviorment parent)
        {
            return interpreter.VisitCallNode(this, parent);
        }
    }
    class InnerFunctionNode : Node
    {
        public List<string> parameters { get; private set; }
        public TreeNode action { get; private set; }
        public InnerFunctionNode(List<string> parameters, TreeNode action)
        {
            this.parameters = parameters;
            this.action = action;
        }
        public virtual ObjectClass Call(List<ObjectClass> arguments, Interpreter interpreter)
        {
            try
            {
                Enviorment env = new Enviorment(null);
                for (int i = 0; i < parameters.Count; i++)
                {
                    env.SetVariable(parameters[i], arguments[i]);
                }
                return interpreter.VisitTreeNode(action, env);
            }
            catch(ReturnException e)
            {
                return e.value;
            }
        }
    }
    class FunctionNode : Node
    {
        public string name { get; private set; }
        public List<InnerFunctionNode> actions { get; private set; }
        public FunctionNode(string name)
        {
            this.name = name;
            this.actions = new List<InnerFunctionNode>();
        }
        public FunctionNode(string name, InnerFunctionNode action)
        {
            this.name = name;
            this.actions = new List<InnerFunctionNode>() { action };
        }
        public FunctionNode(string name, List<InnerFunctionNode> actions)
        {
            this.name = name;
            this.actions = actions;
        }

        public override ObjectClass Visit(Interpreter interpreter, Enviorment parent)
        {
            return interpreter.VisitFunctionNode(this, parent);
        }
        public ObjectClass Call(List<ObjectClass> arguments, Interpreter interpreter)
        {
            foreach(InnerFunctionNode action in actions)
            {
                if(action.parameters.Count == arguments.Count)
                {
                    return action.Call(arguments, interpreter);
                }
            }
            ErrorManager.AddError(new Error($"Couldn't find function \"{name}\" with parameter count \"{arguments.Count}\".", ErrorType.CorrectFunctionDoesNotExist, ErrorSafety.Fatal));
            return new NovaClass();
        }
    }
}
