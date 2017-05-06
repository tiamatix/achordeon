/*! Achordeon - MIT License

Copyright (c) 2017 Wolf Robben

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
using System.IO;
using Achordeon.Common.Extensions;
using Achordeon.Common.Helpers;
using Achordeon.Lib.DataModel;
using Achordeon.Lib.SongOptions;
using DryIoc;
using DryIoc.Experimental;

namespace Achordeon.Rendering
{
    public abstract class Renderer : IDisposable
    {
        protected SongBook Book { get; private set; }
        protected Container IoC { get; private set; }
        protected ISongOptions SongOptions { get; private set; }

        public abstract void Render(Stream ATargetStream);

        public virtual void Dispose()
        {
            Book = null;
            IoC = null;
            SongOptions = null;
        }

        protected void BeginRender()
        {
            if (SongOptions.TransposeByHalftones != 0)
            {
                IoC.Get<Log>().Using(this).TraceFormat("Transposing by {0} semitones.", SongOptions.TransposeByHalftones);
                Book.Transpose(SongOptions.TransposeByHalftones);
            }
        }

        protected void EndRender()
        {
            
        }

        protected Renderer(Container AIoC, SongBook ABook, ISongOptions ASongOptions)
        {
            AIoC.ThrowIfNullEx(nameof(AIoC));
            ABook.ThrowIfNullEx(nameof(ABook));
            ASongOptions.ThrowIfNullEx(nameof(ASongOptions));
            IoC = AIoC;
            Book = ABook;
            SongOptions = ASongOptions;
        }

    }
}
