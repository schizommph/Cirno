using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
