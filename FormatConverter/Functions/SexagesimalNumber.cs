using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormatConverter.Functions
{
    internal struct SexagesimalNumber : IComparable, IComparable<SexagesimalNumber>, IEquatable<SexagesimalNumber>
    {
        decimal Value;

        public decimal DecimalPart { get { return Math.Abs(Value % 1); } }

        public SexagesimalNumber(params int[] digit) : this(0, digit) { }

        public SexagesimalNumber(decimal decimalPart, params int[] digit)
        {
            Value = 0;

            for (int i = digit.Length - 1, j = 0; i >= 0; i--, j++)
            {
                if (i != 0 && (digit[i] < 0 || digit[i] >= 60)) throw new ArgumentOutOfRangeException();
                Value += Math.Abs(digit[i]) * (int)Math.Pow(60, j);
                if (digit[i] < 0) Value *= -1;
            }

            if (decimalPart >= 1 || decimalPart < 0) throw new ArgumentOutOfRangeException();
            Value += decimalPart;
        }

        public static SexagesimalNumber operator +(SexagesimalNumber n1, SexagesimalNumber n2)
        {
            return new SexagesimalNumber { Value = n1.Value + n2.Value };
        }

        public static SexagesimalNumber operator +(SexagesimalNumber n)
        {
            return new SexagesimalNumber { Value = n.Value };
        }

        public static SexagesimalNumber operator ++(SexagesimalNumber n)
        {
            return new SexagesimalNumber { Value = n.Value + 1 };
        }

        public static SexagesimalNumber operator -(SexagesimalNumber n1, SexagesimalNumber n2)
        {
            return new SexagesimalNumber { Value = n1.Value - n2.Value };
        }

        public static SexagesimalNumber operator -(SexagesimalNumber n)
        {
            return new SexagesimalNumber { Value = -n.Value };
        }

        public static SexagesimalNumber operator --(SexagesimalNumber n)
        {
            return new SexagesimalNumber { Value = n.Value + 1 };
        }

        public static SexagesimalNumber operator *(SexagesimalNumber n1, SexagesimalNumber n2)
        {
            return new SexagesimalNumber { Value = n1.Value * n2.Value };
        }

        public static SexagesimalNumber operator /(SexagesimalNumber n1, SexagesimalNumber n2)
        {
            return new SexagesimalNumber { Value = n1.Value / n2.Value };
        }

        public static SexagesimalNumber operator %(SexagesimalNumber n1, SexagesimalNumber n2)
        {
            return new SexagesimalNumber { Value = n1.Value % n2.Value };
        }

        public static bool operator ==(SexagesimalNumber n1, SexagesimalNumber n2)
        {
            return n1.Equals(n2);
        }

        public static bool operator !=(SexagesimalNumber n1, SexagesimalNumber n2)
        {
            return !n1.Equals(n2);
        }

        public static bool operator <(SexagesimalNumber n1, SexagesimalNumber n2)
        {
            return n1.Value < n2.Value;
        }

        public static bool operator <=(SexagesimalNumber n1, SexagesimalNumber n2)
        {
            return n1.Value <= n2.Value;
        }

        public static bool operator >(SexagesimalNumber n1, SexagesimalNumber n2)
        {
            return n1.Value > n2.Value;
        }

        public static bool operator >=(SexagesimalNumber n1, SexagesimalNumber n2)
        {
            return n1.Value >= n2.Value;
        }

        public static implicit operator decimal(SexagesimalNumber obj)
        {
            return obj.Value;
        }

        public static implicit operator SexagesimalNumber(short obj)
        {
            return new SexagesimalNumber { Value = obj };
        }

        public static implicit operator SexagesimalNumber(int obj)
        {
            return new SexagesimalNumber { Value = obj };
        }

        public static implicit operator SexagesimalNumber(decimal obj)
        {
            return new SexagesimalNumber { Value = obj };
        }

        public static explicit operator SexagesimalNumber(long obj)
        {
            return new SexagesimalNumber { Value = obj };
        }

        public static explicit operator short(SexagesimalNumber obj)
        {
            return Convert.ToInt16(obj.Value);
        }

        public static explicit operator int(SexagesimalNumber obj)
        {
            return Convert.ToInt32(obj.Value);
        }

        public static explicit operator long(SexagesimalNumber obj)
        {
            return Convert.ToInt64(obj.Value);
        }

        public static explicit operator float(SexagesimalNumber obj)
        {
            return Convert.ToSingle(obj.Value);
        }

        public static explicit operator double(SexagesimalNumber obj)
        {
            return Convert.ToDouble(obj.Value);
        }

        public int[] GetDigit()
        {
            List<int> list = new();

            int quotient = (int)Math.Floor(Math.Abs(Value) / 1);
            while (quotient >= 60)
            {
                list.Add(quotient % 60);
                quotient = quotient / 60;
            }
            list.Add(quotient);
            if (Value < 0) list[^1] *= -1;
 
            list.Reverse();
            return list.ToArray();
        }

        public override string ToString()
        {
            string str = "";
            if (Value < 0) str += "-";

            List<int> list = new();
            decimal remainder = Math.Abs(Value) % 1;
            int quotient = (int)Math.Floor(Math.Abs(Value) / 1);
            while (quotient >= 60)
            {
                list.Add(quotient % 60);
                quotient = quotient / 60;
            }
            list.Add(quotient);
            list.Reverse();
            str += string.Join(':', list);

            if (remainder > 0) str += remainder.ToString()[1..];

            return str;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            return obj is SexagesimalNumber && Equals((SexagesimalNumber)obj);
        }

        public int CompareTo(object? obj)
        {
            if (obj is not SexagesimalNumber) throw new ArgumentException();
            return CompareTo((SexagesimalNumber)obj);
        }

        public int CompareTo(SexagesimalNumber obj)
        {
            return Value.CompareTo(obj.Value);
        }

        public bool Equals(SexagesimalNumber obj)
        {
            return Value.Equals(obj.Value);
        }
    }
}
