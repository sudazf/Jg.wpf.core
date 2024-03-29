﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;

namespace Jg.wpf.controls.Converter
{
    public class MathConverter : IValueConverter
    {
        readonly Dictionary<string, IExpression> _storedExpressions = new Dictionary<string, IExpression>();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(new[] { value }, targetType, parameter, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                decimal result = Parse(parameter.ToString()).Eval(values);
                if (targetType == typeof(decimal)) return result;
                if (targetType == typeof(string)) return result.ToString(CultureInfo.InvariantCulture);
                if (targetType == typeof(int)) return (int)result;
                if (targetType == typeof(double)) return (double)result;
                if (targetType == typeof(long)) return (long)result;
                throw new ArgumentException($"Unsupported target type {targetType.FullName}");
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }

            return DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        protected virtual void ProcessException(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        private IExpression Parse(string s)
        {
            if (!_storedExpressions.TryGetValue(s, out var result))
            {
                result = new Parser().Parse(s);
                _storedExpressions[s] = result;
            }

            return result;
        }

        interface IExpression
        {
            decimal Eval(object[] args);
        }

        class Constant : IExpression
        {
            private readonly decimal _value;

            public Constant(string text)
            {
                if (!decimal.TryParse(text, out _value))
                {
                    throw new ArgumentException($"'{text}' is not a valid number");
                }
            }

            public decimal Eval(object[] args)
            {
                return _value;
            }
        }

        class Variable : IExpression
        {
            private readonly int _index;

            public Variable(string text)
            {
                if (!int.TryParse(text, out _index) || _index < 0)
                {
                    throw new ArgumentException($"'{text}' is not a valid parameter index");
                }
            }

            public Variable(int n)
            {
                _index = n;
            }

            public decimal Eval(object[] args)
            {
                if (_index >= args.Length)
                {
                    throw new ArgumentException(
                        $"MathConverter: parameter index {_index} is out of range. {args.Length} parameter(s) supplied");
                }

                return System.Convert.ToDecimal(args[_index]);
            }
        }

        class BinaryOperation : IExpression
        {
            private Func<decimal, decimal, decimal> _operation;
            private IExpression _left;
            private IExpression _right;

            public BinaryOperation(char operation, IExpression left, IExpression right)
            {
                _left = left;
                _right = right;
                switch (operation)
                {
                    case '+': _operation = (a, b) => (a + b); break;
                    case '-': _operation = (a, b) => (a - b); break;
                    case '*': _operation = (a, b) => (a * b); break;
                    case '/': _operation = (a, b) => (a / b); break;
                    default: throw new ArgumentException("Invalid operation " + operation);
                }
            }

            public decimal Eval(object[] args)
            {
                return _operation(_left.Eval(args), _right.Eval(args));
            }
        }

        class Negate : IExpression
        {
            private readonly IExpression _param;

            public Negate(IExpression param)
            {
                _param = param;
            }

            public decimal Eval(object[] args)
            {
                return -_param.Eval(args);
            }
        }

        class Parser
        {
            private string _text;
            private int _pos;

            public IExpression Parse(string text)
            {
                try
                {
                    _pos = 0;
                    this._text = text;
                    var result = ParseExpression();
                    RequireEndOfText();
                    return result;
                }
                catch (Exception ex)
                {
                    string msg = $"MathConverter: error parsing expression '{text}'. {ex.Message} at position {_pos}";

                    throw new ArgumentException(msg, ex);
                }
            }

            private IExpression ParseExpression()
            {
                IExpression left = ParseTerm();

                while (true)
                {
                    if (_pos >= _text.Length) return left;

                    var c = _text[_pos];

                    if (c == '+' || c == '-')
                    {
                        ++_pos;
                        IExpression right = ParseTerm();
                        left = new BinaryOperation(c, left, right);
                    }
                    else
                    {
                        return left;
                    }
                }
            }

            private IExpression ParseTerm()
            {
                IExpression left = ParseFactor();

                while (true)
                {
                    if (_pos >= _text.Length) return left;

                    var c = _text[_pos];

                    if (c == '*' || c == '/')
                    {
                        ++_pos;
                        IExpression right = ParseFactor();
                        left = new BinaryOperation(c, left, right);
                    }
                    else
                    {
                        return left;
                    }
                }
            }

            private IExpression ParseFactor()
            {
                SkipWhiteSpace();
                if (_pos >= _text.Length) throw new ArgumentException("Unexpected end of text");

                var c = _text[_pos];

                if (c == '+')
                {
                    ++_pos;
                    return ParseFactor();
                }

                if (c == '-')
                {
                    ++_pos;
                    return new Negate(ParseFactor());
                }

                if (c == 'x' || c == 'a') return CreateVariable(0);
                if (c == 'y' || c == 'b') return CreateVariable(1);
                if (c == 'z' || c == 'c') return CreateVariable(2);
                if (c == 't' || c == 'd') return CreateVariable(3);

                if (c == '(')
                {
                    ++_pos;
                    var expression = ParseExpression();
                    SkipWhiteSpace();
                    Require(')');
                    SkipWhiteSpace();
                    return expression;
                }

                if (c == '{')
                {
                    ++_pos;
                    var end = _text.IndexOf('}', _pos);
                    if (end < 0) { --_pos; throw new ArgumentException("Unmatched '{'"); }
                    if (end == _pos) { throw new ArgumentException("Missing parameter index after '{'"); }
                    var result = new Variable(_text.Substring(_pos, end - _pos).Trim());
                    _pos = end + 1;
                    SkipWhiteSpace();
                    return result;
                }

                const string decimalRegEx = @"(\d+\.?\d*|\d*\.?\d+)";
                var match = Regex.Match(_text.Substring(_pos), decimalRegEx);
                if (match.Success)
                {
                    _pos += match.Length;
                    SkipWhiteSpace();
                    return new Constant(match.Value);
                }
                else
                {
                    throw new ArgumentException($"Unexpeted character '{c}'");
                }
            }

            private IExpression CreateVariable(int n)
            {
                ++_pos;
                SkipWhiteSpace();
                return new Variable(n);
            }

            private void SkipWhiteSpace()
            {
                while (_pos < _text.Length && Char.IsWhiteSpace((_text[_pos]))) ++_pos;
            }

            private void Require(char c)
            {
                if (_pos >= _text.Length || _text[_pos] != c)
                {
                    throw new ArgumentException("Expected '" + c + "'");
                }

                ++_pos;
            }

            private void RequireEndOfText()
            {
                if (_pos != _text.Length)
                {
                    throw new ArgumentException("Unexpected character '" + _text[_pos] + "'");
                }
            }
        }
    }
}
