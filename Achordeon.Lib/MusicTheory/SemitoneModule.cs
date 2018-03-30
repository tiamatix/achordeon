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
using System.ComponentModel;
using ServicesLibrary;

namespace Achordeon.Lib.MusicTheory
{
    [ImmutableObject(true)]
    public class SemitoneModule : IntegerModule
    {
        public const int MIN_VALUE = 0;
        public const int MAX_VALUE = 12;

        public static SemitoneModule Min => new SemitoneModule(MIN_VALUE);

        public static SemitoneModule Max => new SemitoneModule(MAX_VALUE);

        public SemitoneModule(int AValue) : base(AValue, MAX_VALUE)
        {
        }

        public SemitoneModule() : base(MIN_VALUE, MAX_VALUE)
        {
        }

        public int IntervalIndex => Value;

        public override object Clone()
        {
            return new SemitoneModule(Value);
        }

        public static int operator +(SemitoneModule AModule, IntegerModule AFactor)
        {
            return ((IntegerModule) AModule) + AFactor;
        }

        public static int operator *(SemitoneModule AModule, SemitoneModule AFactor)
        {
            return ((IntegerModule)AModule) * AFactor;
        }

        public static int operator -(SemitoneModule AModule, SemitoneModule AFactor)
        {
            return ((IntegerModule)AModule) - AFactor;
        }

        public static int operator /(SemitoneModule AModule, SemitoneModule AFactor)
        {
            return ((IntegerModule)AModule) / AFactor;
        }

        public static SemitoneModule operator ++(SemitoneModule AModule)
        {
            return new SemitoneModule(AModule.Value + 1);
        }

        public static SemitoneModule operator --(SemitoneModule AModule)
        {
            return new SemitoneModule(AModule.Value - 1);
        }

        public static int operator +(SemitoneModule AModule, int AFactor)
        {
            return ((IntegerModule)AModule) + AFactor;
        }

        public static int operator *(SemitoneModule AModule, int AFactor)
        {
            return ((IntegerModule)AModule) * AFactor;
        }

        public static int operator -(SemitoneModule AModule, int AFactor)
        {
            return ((IntegerModule)AModule) - AFactor;
        }

        public static int operator /(SemitoneModule AModule, int AFactor)
        {
            return ((IntegerModule)AModule) / AFactor;
        }

    }
}
