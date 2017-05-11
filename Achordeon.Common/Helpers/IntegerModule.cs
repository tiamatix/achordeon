/*! Achordeon - MIT License

Copyright (c) 2017 tiamatix / Wolf Robben

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
!*/
using System;
using System.ComponentModel;

namespace ServicesLibrary
{
    [ImmutableObject(true)]
    public class IntegerModule : IComparable<IntegerModule>, ICloneable
    {
        public int UpperInclude { get; }
        public int Value { get; }

        public int Span => UpperInclude;


        public IntegerModule(int AValue,int AUpperInclude)
        {
            UpperInclude = AUpperInclude;
            Value = AValue > UpperInclude ? (AValue - UpperInclude) % UpperInclude :
                    AValue < 0 ? (AValue + UpperInclude) % UpperInclude :
                    AValue == UpperInclude ? 0 : AValue;
        }

        public bool ClassEquals(IntegerModule AAotherModule)
        {
            return UpperInclude == AAotherModule.UpperInclude;
        }

        public static int operator +(IntegerModule AModule, IntegerModule AFactor)
        {
            if (!AModule.ClassEquals(AFactor))
                throw new InvalidOperationException("Cann only perform module arithmetics on modules of the same class");
            return (AFactor + AModule.Value) % AModule.Span;
        }

        public static int operator *(IntegerModule AModule, IntegerModule AFactor)
        {
            if (!AModule.ClassEquals(AFactor))
                throw new InvalidOperationException("Cann only perform module arithmetics on modules of the same class");
            return (AFactor * AModule.Value) % AModule.Span;
        }

        public static int operator -(IntegerModule AModule, IntegerModule AFactor)
        {
            if (!AModule.ClassEquals(AFactor))
                throw new InvalidOperationException("Cann only perform module arithmetics on modules of the same class");
            return (AFactor - AModule.Value) % AModule.Span;
        }

        public static int operator /(IntegerModule AModule, IntegerModule AFactor)
        {
            if (!AModule.ClassEquals(AFactor))
                throw new InvalidOperationException("Cann only perform module arithmetics on modules of the same class");
            return (AFactor / AModule.Value) % AModule.Span;
        }

        public static IntegerModule operator ++(IntegerModule AModule)
        {
            return new IntegerModule((AModule.Value + 1) % AModule.Span, AModule.UpperInclude);
        }

        public static IntegerModule operator --(IntegerModule AModule)
        {
            return new IntegerModule((AModule.Value - 1) % AModule.Span, AModule.UpperInclude);
        }

        public static int operator +(IntegerModule AModule, int AFactor)
        {
            return (AFactor + AModule.Value) % AModule.Span;
        }

        public static int operator *(IntegerModule AModule, int AFactor)
        {            
            return (AFactor * AModule.Value) % AModule.Span;
        }

        public static int operator -(IntegerModule AModule, int AFactor)
        {
            return (AFactor - AModule.Value) % AModule.Span;
        }

        public static int operator /(IntegerModule AModule, int AFactor)
        {
            return (AFactor / AModule.Value) % AModule.Span;
        }

        public int CompareTo(IntegerModule AOther)
        {
            if (AOther == null)
                return 1;
            var Result = UpperInclude.CompareTo(AOther.UpperInclude);
            if (Result == 0)
                Result = Value.CompareTo(AOther.Value);
            return Result;
        }

        public override bool Equals(object obj)
        {
            return CompareTo(obj as IntegerModule) == 0;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return 5*UpperInclude.GetHashCode() + 7*Value.GetHashCode();
            }
        }

        public virtual object Clone()
        {
            return new IntegerModule(Value, UpperInclude);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
