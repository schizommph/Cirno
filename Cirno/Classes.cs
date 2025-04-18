﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cirno
{
    class ObjectClass
    {
        public virtual string Out()
        {
            // err
            // change later
            return "[object Object]";
        }
        public new virtual string GetType()
        {
            return "Object";
        }

        public virtual ObjectClass Add(ObjectClass other)
        {
            ErrorManager.AddError(new Error($"Cannot add with \"{GetType()}\" and \"{other.GetType()}\"", ErrorType.UnexpectedArithmeticOperation, ErrorSafety.Warning));
            return new NovaClass();
        }
        public virtual ObjectClass Subtract(ObjectClass other)
        {
            ErrorManager.AddError(new Error($"Cannot subtract with \"{GetType()}\" and \"{other.GetType()}\"", ErrorType.UnexpectedArithmeticOperation, ErrorSafety.Warning));
            return new NovaClass();
        }
        public virtual ObjectClass Multiply(ObjectClass other)
        {
            ErrorManager.AddError(new Error($"Cannot multiply with \"{GetType()}\" and \"{other.GetType()}\"", ErrorType.UnexpectedArithmeticOperation, ErrorSafety.Warning));
            return new NovaClass();
        }
        public virtual ObjectClass Divide(ObjectClass other)
        {
            ErrorManager.AddError(new Error($"Cannot divide with \"{GetType()}\" and \"{other.GetType()}\"", ErrorType.UnexpectedArithmeticOperation, ErrorSafety.Warning));
            return new NovaClass();
        }
        public virtual ObjectClass Mod(ObjectClass other)
        {
            ErrorManager.AddError(new Error($"Cannot modulos with \"{GetType()}\" and \"{other.GetType()}\"", ErrorType.UnexpectedArithmeticOperation, ErrorSafety.Warning));
            return new NovaClass();
        }
        public virtual BoolClass Equals(ObjectClass other)
        {
            // should never matter
            return new BoolClass(false);
        }
        public virtual BoolClass GreaterThan(ObjectClass other)
        {
            ErrorManager.AddError(new Error($"Cannot compare with \"{GetType()}\" and \"{other.GetType()}\"", ErrorType.UnexpectedLogicalOperation, ErrorSafety.Warning));
            return new BoolClass(false);
        }
        public virtual BoolClass LessThan(ObjectClass other)
        {
            ErrorManager.AddError(new Error($"Cannot compare with \"{GetType()}\" and \"{other.GetType()}\"", ErrorType.UnexpectedLogicalOperation, ErrorSafety.Warning));
            return new BoolClass(false);
        }
        public virtual BoolClass And(ObjectClass other)
        {
            ErrorManager.AddError(new Error($"Cannot compare with \"{GetType()}\" and \"{other.GetType()}\"", ErrorType.UnexpectedLogicalOperation, ErrorSafety.Warning));
            return new BoolClass(false);
        }
        public virtual BoolClass Or(ObjectClass other)
        {
            ErrorManager.AddError(new Error($"Cannot compare with \"{GetType()}\" and \"{other.GetType()}\"", ErrorType.UnexpectedLogicalOperation, ErrorSafety.Warning));
            return new BoolClass(false);
        }
        public virtual ObjectClass GetIndex(ObjectClass other)
        {
            ErrorManager.AddError(new Error($"Cannot get index from \"{GetType()}\" with \"{other.GetType()}\", or cannot get index at all", ErrorType.UnexpectedIndex, ErrorSafety.Warning));
            return new NovaClass();
        }
        public virtual ObjectClass SetIndex(ObjectClass index, ObjectClass expr)
        {
            ErrorManager.AddError(new Error($"Cannot set index from \"{GetType()}\" with \"{index.GetType()}\", or cannot set index at all", ErrorType.UnexpectedIndex, ErrorSafety.Warning));
            return new NovaClass();
        }
        public virtual ObjectClass AddToItems(ObjectClass expr)
        {
            ErrorManager.AddError(new Error($"Cannot add to type \"{GetType()}\".", ErrorType.UnallowedListModification, ErrorSafety.Warning));
            return new NovaClass();
        }
        public virtual ObjectClass PopFromItems(ObjectClass expr)
        {
            ErrorManager.AddError(new Error($"Cannot pop from type \"{GetType()}\", or cannot pop at all.", ErrorType.UnallowedListModification, ErrorSafety.Warning));
            return new NovaClass();
        }
        public virtual ListClass ToList()
        {
            ErrorManager.AddError(new Error($"Cannot turn type \"{GetType()}\" into a list.", ErrorType.UnallowedListModification, ErrorSafety.Warning));
            return new ListClass(new List<ObjectClass>());
        }
    }
    class NumberClass : ObjectClass
    {
        public float value { get; private set; } = 0;
        public NumberClass(float value)
        {
            this.value = value;
        }
        public override string Out()
        {
            return $"{value}";
        }
        public override string ToString()
        {
            return "" + value;
        }
        public override string GetType()
        {
            return "Number";
        }
        public override ObjectClass Add(ObjectClass other)
        {
            if (other is NumberClass num)
            {
                return new NumberClass(value + num.value);
            }
            else if(other is StringClass str)
            {
                return new StringClass("" + value + str.value);
            }
            else
            {
                base.Add(other);
            }
            return new NovaClass();
        }
        public override ObjectClass Subtract(ObjectClass other)
        {
            if (other is NumberClass num)
            {
                return new NumberClass(value - num.value);
            }
            else if(other is StringClass str)
            {
                if(value > str.value.Length)
                {
                    ErrorManager.AddError(new Error($"Value to larger than string length to substring.", ErrorType.IndexOutOfBounds, ErrorSafety.Warning));
                    return new NovaClass();
                }
                else if(value < 0)
                {
                    ErrorManager.AddError(new Error($"Value cannot be negative when substringing.", ErrorType.IndexOutOfBounds, ErrorSafety.Warning));
                    return new NovaClass();
                }
                return new StringClass(str.value.Substring((int)value));
            }
            else
            {
                base.Subtract(other);
            }
            return new NovaClass();
        }
        public override ObjectClass Multiply(ObjectClass other)
        {
            if (other is NumberClass num)
            {
                return new NumberClass(value * num.value);
            }
            else
            {
                base.Multiply(other);
            }
            return new NovaClass();
        }
        public override ObjectClass Divide(ObjectClass other)
        {
            if (other is NumberClass num)
            {
                return new NumberClass(value / num.value);
            }
            else
            {
                base.Divide(other);
            }
            return new NovaClass();
        }
        public override ObjectClass Mod(ObjectClass other)
        {
            if (other is NumberClass num)
            {
                return new NumberClass(value % num.value);
            }
            else
            {
                base.Mod(other);
            }
            return new NovaClass();
        }
        public override BoolClass Equals(ObjectClass other)
        {
            if (other is NumberClass num)
            {
                return new BoolClass(value == num.value);
            }
            else
            {
                return new BoolClass(false);
            }
        }
        public override BoolClass GreaterThan(ObjectClass other)
        {
            if (other is NumberClass num)
            {
                return new BoolClass(value > num.value);
            }
            else
            {
                base.GreaterThan(other);
                return new BoolClass(false);
            }
        }
        public override BoolClass LessThan(ObjectClass other)
        {
            if (other is NumberClass num)
            {
                return new BoolClass(value < num.value);
            }
            else
            {
                base.LessThan(other);
                return new BoolClass(false);
            }
        }
    }
    class StringClass : ObjectClass
    {
        public string value { get; private set; } = "";
        public StringClass(string value)
        {
            this.value = value;
        }
        public override string Out()
        {
            string unescaped = value
                .Replace("\\n", "\n")
                .Replace("\\t", "\t")
                .Replace("\\r", "\r")
                .Replace("\\\"", "\"")
                .Replace("\\\\", "\\");

            return unescaped;
        }
        public override string ToString()
        {
            return $"\"{value}\"";
        }
        public override string GetType()
        {
            return "String";
        }
        public override ObjectClass Add(ObjectClass other)
        {
            return new StringClass(value + other.Out());
        }
        public override ObjectClass Subtract(ObjectClass other)
        {
            if (other is NumberClass num)
            {
                return new StringClass(
                    value.Substring(0, (int)num.value)
                );
            }
            else
            {
                base.Subtract(other);
            }
            return new NovaClass();
        }
        public override ObjectClass Multiply(ObjectClass other)
        {
            if (other is NumberClass num)
            {
                return new StringClass(string.Concat(Enumerable.Repeat(value, (int)num.value)));
            }
            else
            {
                base.Multiply(other);
            }
            return new NovaClass();
        }
        public override ObjectClass Divide(ObjectClass other)
        {
            base.Divide(other);
            return new NovaClass();
        }
        public override ObjectClass Mod(ObjectClass other)
        {
            if (other is NumberClass num)
            {
                return new NumberClass(value[(int)num.value]);
            }
            else
            {
                base.Mod(other);
            }
            return new NovaClass();
        }
        public override BoolClass Equals(ObjectClass other)
        {
            if (other is StringClass str)
            {
                return new BoolClass(value == str.value);
            }
            else
            {
                return new BoolClass(false);
            }
        }
        public override ObjectClass GetIndex(ObjectClass other)
        {
            if(other is NumberClass index)
            {
                if(index.value < 0)
                {
                    ErrorManager.AddError(new Error("Cannot get char with index below 0", ErrorType.IndexOutOfBounds, ErrorSafety.Warning));
                    return new NovaClass();
                }
                else if(index.value > this.value.Length - 1)
                {
                    ErrorManager.AddError(new Error("Cannot get char with index above string length", ErrorType.IndexOutOfBounds, ErrorSafety.Warning));
                    return new NovaClass();
                }

                return new StringClass(value.Substring((int)index.value, 1));
            }
            else
            {
                return base.GetIndex(other);
            }
        }
        public override ObjectClass SetIndex(ObjectClass index, ObjectClass expr)
        {
            if (index is NumberClass ind)
            {
                if (ind.value < 0)
                {
                    ErrorManager.AddError(new Error("Cannot get char with index below 0", ErrorType.IndexOutOfBounds, ErrorSafety.Warning));
                    return new NovaClass();
                }
                else if (ind.value > this.value.Length - 1)
                {
                    ErrorManager.AddError(new Error("Cannot get char with index above string length", ErrorType.IndexOutOfBounds, ErrorSafety.Warning));
                    return new NovaClass();
                }
                string[] str = value.Select(c => c.ToString()).ToArray();
                str[(int)ind.value] = expr.Out();
                this.value = string.Join("", str);
                return new NovaClass();
            }
            else if(index is StringClass strind)
            {
                value = value.Replace(strind.value, expr.Out());
                
                return new NovaClass();
            }
            else
            {
                return base.SetIndex(index, expr);
            }
        }
        public override ListClass ToList()
        {
            List<ObjectClass> ret = new List<ObjectClass>();
            foreach(char c in value)
            {
                ret.Add(new StringClass(Convert.ToString(c)));
            }
            return new ListClass(ret);
        }
    }
    class NovaClass : ObjectClass
    {
        public override string Out()
        {
            return $"Nova";
        }
        public override string ToString()
        {
            return $"Nova";
        }
        public override string GetType()
        {
            return "Nova";
        }
        public override BoolClass Equals(ObjectClass other)
        {
            if (other is NovaClass)
            {
                return new BoolClass(true);
            }
            else
            {
                return new BoolClass(false);
            }
        }
    }

    class BoolClass : ObjectClass
    {
        public static BoolClass True = new BoolClass(true);
        public static BoolClass False = new BoolClass(false);

        public bool value { get; private set; } = false;
        public BoolClass(bool value)
        {
            this.value = value;
        }
        public override string Out()
        {
            return $"{value}";
        }
        public override string ToString()
        {
            return $"{value}";
        }
        public override string GetType()
        {
            return "Bool";
        }
        public override BoolClass Equals(ObjectClass other)
        {
            if (other is BoolClass boolean)
            {
                return new BoolClass(value == boolean.value);
            }
            else
            {
                return new BoolClass(false);
            }
        }
        public override BoolClass And(ObjectClass other)
        {
            if (other is BoolClass boolean)
            {
                return new BoolClass(value && boolean.value);
            }
            else
            {
                return new BoolClass(false);
            }
        }
        public override BoolClass Or(ObjectClass other)
        {
            if (other is BoolClass boolean)
            {
                return new BoolClass(value || boolean.value);
            }
            else
            {
                return new BoolClass(false);
            }
        }
    }

    class ListClass : ObjectClass
    {
        public List<ObjectClass> items { get; private set; }
        public ListClass(List<ObjectClass> items)
        {
            this.items = items;
        }
        public override string Out()
        {
            return ToString();
        }
        public override string ToString()
        {
            if (items.Count == 0)
                return "[]";

            string output = "";
            foreach (ObjectClass item in items)
            {
                output += item.ToString() + ", ";
            }
            return $"[{output.Substring(0, output.Length - 2)}]";
        }
        public override string GetType()
        {
            return "List";
        }

        public override BoolClass Equals(ObjectClass other)
        {
            if(other is ListClass list)
            {
                return new BoolClass(list.items.SequenceEqual(items));
            }
            else
            {
                return base.Equals(other);
            }
        }

        public override ObjectClass GetIndex(ObjectClass other)
        {
            if (other is NumberClass index)
            {
                if (index.value < 0)
                {
                    ErrorManager.AddError(new Error("Cannot get item with index below 0", ErrorType.IndexOutOfBounds, ErrorSafety.Warning));
                    return new NovaClass();
                }
                else if (index.value > this.items.Count - 1)
                {
                    ErrorManager.AddError(new Error("Cannot get item with index above string length", ErrorType.IndexOutOfBounds, ErrorSafety.Warning));
                    return new NovaClass();
                }

                return items[(int)index.value];
            }
            else
            {
                return base.GetIndex(other);
            }
        }
        public override ObjectClass SetIndex(ObjectClass index, ObjectClass expr)
        {
            if (index is NumberClass ind)
            {
                if (ind.value < 0)
                {
                    ErrorManager.AddError(new Error("Cannot get item with index below 0", ErrorType.IndexOutOfBounds, ErrorSafety.Warning));
                    return new NovaClass();
                }
                else if (ind.value > this.items.Count - 1)
                {
                    ErrorManager.AddError(new Error("Cannot get item with index above string length", ErrorType.IndexOutOfBounds, ErrorSafety.Warning));
                    return new NovaClass();
                }

                items[(int)ind.value] = expr;
                return items[(int)ind.value];
            }
            else
            {
                return base.SetIndex(index, expr);
            }
        }
        public override ObjectClass Add(ObjectClass other)
        {
            if (other is ListClass list)
            {
                List<ObjectClass> ret = new List<ObjectClass>(items);

                foreach(ObjectClass item in list.items)
                {
                    ret.Add(item);
                }

                return new ListClass(ret);
            }
            return base.Subtract(other);
        }
        public override ObjectClass Subtract(ObjectClass other)
        {
            if(other is ListClass list)
            {
                List<ObjectClass> ret = new List<ObjectClass>(items);

                foreach (ObjectClass otherItem in list.items)
                {
                    foreach(ObjectClass item in items)
                    {
                        if (item.Equals(otherItem).value)
                        {
                            for(int i = ret.Count - 1; i >= 0; i--)
                            {
                                if (otherItem.Equals(ret[i]).value)
                                {
                                    ret.RemoveAt(i);
                                }
                            }
                        }
                    }
                }
                return new ListClass(ret);
            }
            return base.Subtract(other);
        }
        public override ObjectClass AddToItems(ObjectClass expr)
        {
            items.Add(expr);
            return expr;
        }
        public override ObjectClass PopFromItems(ObjectClass expr)
        {
            if (expr is NumberClass num)
            {
                if (num.value < 0)
                {
                    ErrorManager.AddError(new Error("Cannot pop item with index below 0", ErrorType.IndexOutOfBounds, ErrorSafety.Warning));
                    return new NovaClass();
                }
                else if (num.value > this.items.Count - 1)
                {
                    ErrorManager.AddError(new Error("Cannot pop item with index above list length", ErrorType.IndexOutOfBounds, ErrorSafety.Warning));
                    return new NovaClass();
                }
                ObjectClass item = items[(int)num.value];
                items.RemoveAt((int)num.value);
                return item;
            }
            else
            {
                return base.PopFromItems(expr);
            }
        }
        public override ListClass ToList()
        {
            return new ListClass(this.items);
        }
    }
    class DictionaryClass : ObjectClass
    {
        public Dictionary<ObjectClass, ObjectClass> items { get; private set; }
        public DictionaryClass(Dictionary<ObjectClass, ObjectClass> items)
        {
            this.items = items;
        }
        public override string Out()
        {
            return ToString();
        }
        public override string ToString()
        {
            if (items.Count == 0)
                return "{}";

            string output = "";
            foreach (ObjectClass item in items.Keys)
            {
                output += item.ToString() + ": " + items[item].ToString() + ", ";
            }
            return $"{{{output.Substring(0, output.Length - 2)}}}";
        }
        public override string GetType()
        {
            return "Dictionary";
        }

        public override BoolClass Equals(ObjectClass other)
        {
            if (other is DictionaryClass dict)
            {
                return new BoolClass(dict.items.SequenceEqual(items));
            }
            else
            {
                return base.Equals(other);
            }
        }

        public override ObjectClass GetIndex(ObjectClass other)
        {
            foreach(ObjectClass item in items.Keys)
            {
                if (item.Equals(other).value)
                {
                    return items[item];
                }
            }

            ErrorManager.AddError(new Error($"Dictionary does not have key \"{other.Out()}\".", ErrorType.NoKeyFound, ErrorSafety.Warning));
            return new NovaClass();
        }
        public override ObjectClass SetIndex(ObjectClass index, ObjectClass expr)
        {
            items[index] = expr;
            return expr;
        }
        public override ObjectClass PopFromItems(ObjectClass expr)
        {
            items.Remove(expr);
            return expr;
        }
        public override ListClass ToList()
        {
            List<ObjectClass> ret = new List<ObjectClass>();
            foreach (ObjectClass c in items.Keys)
            {
                ret.Add(c);
            }
            return new ListClass(ret);
        }
    }

    // OOP
    class FunctionClass : ObjectClass
    {
        public string name { get; private set; }
        public FunctionNode node { get; private set; }
        public FunctionClass(FunctionNode node)
        {
            this.node = node;
            this.name = node.name;

        }
        public override string Out()
        {
            // implement tostring
            return $"{ToString()}";
        }
        public override string ToString()
        {
            return $"<Function \"{node.name}\">";
        }
        public override string GetType()
        {
            return "Function";
        }
        public ObjectClass Call(List<ObjectClass> parameters, Interpreter interpreter)
        {
            return this.node.Call(parameters, interpreter);
        }
    }
}