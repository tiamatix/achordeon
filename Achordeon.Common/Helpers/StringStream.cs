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
using System.IO;
using System.Text;
using Achordeon.Common.Extensions;

namespace Achordeon.Common.Helpers
{
    public class StringStream : MemoryStream
    {
        public Encoding Encoding { get; private set; }
        public bool KeepOriginalEncoding { get; private set; }



        private void Initialize(string AString, bool AKeepOriginalEncoding)
        {
            KeepOriginalEncoding = AKeepOriginalEncoding;
            if (!String.IsNullOrEmpty(AString))
            {
                if (AKeepOriginalEncoding)
                {
                    var RawData = GetBytes(AString);
                    this.Write(RawData, 0, RawData.Length);
                    var DetectedEncoding = TextEncodingDetector.Detect(ToByteArray());
                    if (DetectedEncoding != null)
                        Encoding = DetectedEncoding;
                }
                else
                {
                    TextWriter tw = new StreamWriter(this);
                    tw.Write(AString);
                    tw.Flush();
                }
                Seek(0, SeekOrigin.Begin);
            }
        }

        private static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        private static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        /// <summary>
        /// Create an empty string stream using the systems default encoding
        /// </summary>
        public StringStream()
            : base()
        {
            Encoding = Encoding.Default;
        }

        /// <summary>
        /// Creates an empty string stream and sets the encoding to the desired encoding
        /// </summary>
        /// <param name="AEncoding"></param>
        public StringStream(Encoding AEncoding)
            : base()
        {
            AEncoding.ThrowIfNullEx(nameof(AEncoding));
            Encoding = AEncoding;
        }

        /// <summary>
        /// Creates a string stream using the system's current default encoding
        /// </summary>
        /// <param name="AString">The initial string data</param>
        public StringStream(string AString)
            : base()
        {
            Initialize(AString, false);
        }

        /// <summary>
        /// Creates a string stream. 
        /// </summary>
        /// <param name="AString">The initial string data</param>
        /// <param name="AKeepExistingEncoding">if true, StringStream tries to detect the encoding from the string data. If false or detection fails, the current default encoding is used.</param>
        public StringStream(string AString, bool AKeepExistingEncoding)
            : base()
        {
            Initialize(AString, AKeepExistingEncoding);
        }

        /// <summary>
        /// Creates a string stream. 
        /// </summary>
        /// <param name="AString">The initial string data</param>
        /// <param name="AKeepExistingEncoding">if true, StringStream tries to detect the encoding from the string data. If false or detection fails, the fallback encoding is used.</param>
        /// <param name="AFallbackEncoding">The encoding to fallback to if the detction fails</param>
        public StringStream(string AString, bool AKeepExistingEncoding, Encoding AFallbackEncoding)
            : base()
        {
            AFallbackEncoding.ThrowIfNullEx(nameof(AFallbackEncoding));
            Encoding = AFallbackEncoding;
            Initialize(AString, AKeepExistingEncoding);
        }

        /// <summary>
        /// Converts the content of this stream to a string using the current Encoding.
        /// </summary>
        /// <returns>string representation of this stream.</returns>
        public override string ToString()
        {
            return ToString(Encoding);
        }

        /// <summary>
        /// Converts the string into a raw-data byte array.
        /// </summary>
        /// <returns></returns>
        public byte[] ToByteArray()
        {
            long OldPosition = this.Position;
            try
            {
                Seek(0, SeekOrigin.Begin);
                byte[] bytes = new byte[Length];
                Read(bytes, 0, (int)Length);
                return bytes;
            }
            finally
            {
                Seek(OldPosition, SeekOrigin.Begin);
            }
        }

        /// <summary>
        /// Converts the content of this stream to a string.
        /// </summary>
        /// <param name="Encoding">The Encoding to use. If KeepOriginalEncoding is true, this parameter is ignored. </param>
        /// <returns>string representation of this stream.</returns>
        public string ToString(Encoding Encoding)
        {
            long OldPosition = this.Position;
            try
            {
                Seek(0, SeekOrigin.Begin);
                byte[] bytes = new byte[Length];
                Read(bytes, 0, (int)Length);
                if (KeepOriginalEncoding)
                    return GetString(bytes);
                return Encoding.GetString(bytes);
            }
            finally
            {
                Seek(OldPosition, SeekOrigin.Begin);
            }
        }
    }
}
