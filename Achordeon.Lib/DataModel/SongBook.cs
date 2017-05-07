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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DryIoc;

namespace Achordeon.Lib.DataModel
{
    public class SongBook : ChordProObject
    {
        private readonly List<Song> m_Songs = new List<Song>();

        public override IEnumerable<ChordProObject> Subobjects => m_Songs.SelectMany(a => a.Subobjects).Union(m_Songs);

        public void RebuildBuildChordList()
        {
            foreach (var Song in Songs)
                Song.RebuildBuildChordList();
        }

        public override void Transpose(int AHalfToneSteps)
        {
            m_Songs.ForEach(a => a.Transpose(AHalfToneSteps));
        }

        public IEnumerable<Song> Songs => m_Songs;

        public void AddSong(Song ASong)
        {
            m_Songs.Add(ASong);
        }

        public SongBook(Container AIoC) : base(AIoC, 0, 0)
        {

        }

        public override void PrintChordPro(StringBuilder ABuilder)
        {
            var IsFirst = true;
            foreach (var Song in Songs)
            {
                if (!IsFirst)
                    ABuilder.AppendLine("{new_song}");
                IsFirst = false;
                Song.PrintChordPro(ABuilder);
            }
        }
    }
}
