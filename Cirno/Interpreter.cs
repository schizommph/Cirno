using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cirno
{
    class Interpreter
    {
        public Interpreter()
        {

        }
        public ObjectClass Visit(Node node, Enviorment parent)
        {
            return node.Visit(this, parent);
        }
        public ObjectClass VisitTreeNode(TreeNode node, Enviorment parent)
        {
            Enviorment env = new Enviorment(parent);
            ObjectClass ret = null;
            foreach(Node branch in node.nodes)
            {
                ret = branch.Visit(this, env);
            }
            return ret;
        }
        public ObjectClass VisitNumberNode(NumberNode node, Enviorment parent)
        {
            return new NumberClass(node.value);
        }
        public ObjectClass VisitStringNode(StringNode node, Enviorment parent)
        {
            return new StringClass(node.value);
        }
        public ObjectClass VisitIdentifierNode(IdentifierNode node, Enviorment parent)
        {
            return parent.GetVariable(node.name);
        }
        public ObjectClass VisitBoolNode(BoolNode node, Enviorment parent)
        {
            return new BoolClass(node.value);
        }
        public ObjectClass VisitNovaNode(NovaNode node, Enviorment parent)
        {
            return new NovaClass();
        }
        public ObjectClass VisitBreakNode(BreakNode node, Enviorment parent)
        {
            throw new BreakException();
        }
        public ObjectClass VisitContinueNode(ContinueNode node, Enviorment parent)
        {
            throw new ContinueException();
        }
        public ObjectClass VisitReturnNode(ReturnNode node, Enviorment parent)
        {
            if(node.expr == null)
            {
                throw new ReturnException(new NovaClass());
            }
            throw new ReturnException(node.expr.Visit(this, parent));
        }
        public ObjectClass VisitGetItemIndexNode(GetItemIndexNode node, Enviorment parent)
        {
            return node.expr.Visit(this, parent).GetIndex(node.index.Visit(this, parent));
        }
        public ObjectClass VisitSetItemIndexNode(SetItemIndexNode node, Enviorment parent)
        {
            ObjectClass item = node.item.expr.Visit(this, parent);
            ObjectClass index = node.item.index.Visit(this, parent);
            ObjectClass expr = node.expr.Visit(this, parent);
            return item.SetIndex(index, expr);
        }
        public ObjectClass VisitAddToItemNode(AddToItemNode node, Enviorment parent)
        {
            ObjectClass item = node.item.Visit(this, parent);
            ObjectClass expr = node.expr.Visit(this, parent);
            return item.AddToItems(expr);
        }
        public ObjectClass VisitPopFromItemNode(PopFromItemNode node, Enviorment parent)
        {
            ObjectClass item = node.item.Visit(this, parent);
            ObjectClass index = node.index.Visit(this, parent);
            return item.PopFromItems(index);
        }
        public ObjectClass VisitListNode(ListNode node, Enviorment parent)
        {
            List<ObjectClass> items = new List<ObjectClass>();
            foreach(Node item in node.items)
            {
                items.Add(item.Visit(this, parent));
            }
            return new ListClass(items);
        }
        public ObjectClass VisitDictionaryNode(DictionaryNode node, Enviorment parent)
        {
            Dictionary<ObjectClass, ObjectClass> items = new Dictionary<ObjectClass, ObjectClass>();
            foreach (Node item in node.items.Keys)
            {
                items[item.Visit(this, parent)] = node.items[item].Visit(this, parent);
            }
            return new DictionaryClass(items);
        }
        public ObjectClass VisitSetVariableNode(SetVariableNode node, Enviorment parent)
        {
            ObjectClass expr = node.expr.Visit(this, parent);
            return parent.SetVariable(node.name, expr);
        }
        public ObjectClass VisitBinaryOperatorNode(BinaryOperatorNode node, Enviorment parent)
        {
            ObjectClass left = node.left.Visit(this, parent);
            ObjectClass right = node.right.Visit(this, parent);
            switch (node.op.type)
            {
                case TokenType.PLUS:
                    return left.Add(right);
                case TokenType.MINUS:
                    return left.Subtract(right);
                case TokenType.STAR:
                    return left.Multiply(right);
                case TokenType.SLASH:
                    return left.Divide(right);
                case TokenType.MOD:
                    return left.Mod(right);
                case TokenType.EQUALS_EQUALS:
                    return left.Equals(right);
                case TokenType.BANG_EQUALS:
                    return new BoolClass(!left.Equals(right).value);
                case TokenType.LESS_THAN:
                    return left.LessThan(right);
                case TokenType.GREATER_THAN:
                    return left.GreaterThan(right);
                case TokenType.LESS_EQUALS:
                    return new BoolClass(!left.GreaterThan(right).value);
                case TokenType.GREATER_EQUALS:
                    return new BoolClass(!left.LessThan(right).value);
                case TokenType.AND:
                    return left.And(right);
                case TokenType.OR:
                    return left.Or(right);
            }
            return new NovaClass();
        }
        public ObjectClass VisitPrintNode(PrintNode node, Enviorment parent)
        {
            ObjectClass value = node.value.Visit(this, parent);
            Console.Write(value.Out());
            return new NovaClass();
        }
        public ObjectClass VisitUsingNode(UsingNode node, Enviorment parent)
        {
            Program.RunFile(node.path + ".crn");

            return new NovaClass();
        }
        public ObjectClass VisitWhileNode(WhileNode node, Enviorment parent)
        {
            while (true)
            {
                if(node.expr.Visit(this, parent).Equals(BoolClass.False).value)
                    break;
                try
                {
                    node.action.Visit(this, parent);
                }
                catch(BreakException)
                {
                    break;
                }
                catch (ContinueException)
                {
                    continue;
                }
            }
            return new NovaClass();
        }
        public ObjectClass VisitForNode(ForNode node, Enviorment parent)
        {
            Enviorment env = new Enviorment(parent);
            ObjectClass list = node.list.Visit(this, parent);
            foreach(ObjectClass item in list.ToList().items)
            {
                env.SetVariable(node.varName, item);
                try
                {
                    node.action.Visit(this, env);
                }
                catch (BreakException)
                {
                    break;
                }
                catch (ContinueException)
                {
                    continue;
                }
            }
            return new NovaClass();
        }
        public ObjectClass VisitFunctionNode(FunctionNode node, Enviorment parent)
        {
            if (!parent.ContainsVariable(node.name))
            {
                Enviorment.SetGlobalVariable(node.name, new FunctionClass(node));
            }
            else
            {
                FunctionClass func = (FunctionClass)parent.GetVariable(node.name);
                func.node.actions.AddRange(node.actions);
            }
            return new NovaClass();
        }
        public ObjectClass VisitCallNode(CallNode node, Enviorment parent)
        {
            List<ObjectClass> arguments = new List<ObjectClass>();
            foreach(Node argument in node.arguments)
            {
                arguments.Add(argument.Visit(this, parent));
            }
            if(parent.ContainsVariable(node.name) && parent.GetVariable(node.name) is FunctionClass)
            {
                return ((FunctionClass)parent.GetVariable(node.name)).Call(arguments, this);
            }
            else
            {
                ErrorManager.AddError(new Error($"Function \"{node.name}\" does either not exist, or is not callable.", ErrorType.NoFoundVariable, ErrorSafety.Fatal));
                return new NovaClass();
            }
        }
        public ObjectClass VisitIfNode(IfNode node, Enviorment parent)
        {
            foreach(Node expr in node.ifActions.Keys)
            {
                if(expr.Visit(this, parent).Equals(BoolClass.True).value)
                {
                    Enviorment env = new Enviorment(parent);
                    return node.ifActions[expr].Visit(this, env);
                }
            }

            if (node.elseAction != null)
            {
                Enviorment elseEnv = new Enviorment(parent);
                return node.elseAction.Visit(this, elseEnv);
            }
            return new NovaClass();
        }
    }
}
