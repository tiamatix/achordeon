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
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Achordeon.Common.Helpers
{
    [Serializable]
    public static class XElementExtensions
    {
        public static string InnerText(this XElement ANode)
        {
            return string.Concat(ANode.Nodes().OfType<XText>().Select(t => t.Value));
        }

        public static string InnerXml(this XNode ANode)
        {
            using (var Reader = ANode.CreateReader())
            {
                Reader.MoveToContent();
                return Reader.ReadInnerXml();
            }
        }

        public static string OuterXml(this XNode ANode)
        {
            using (var Reader = ANode.CreateReader())
            {
                Reader.MoveToContent();
                return Reader.ReadOuterXml();
            }
        }

        public static string GetXPath(this XElement ANode)
        {
            var ResultBuilder = new StringBuilder();
            while (ANode != null)
            {
                switch (ANode.NodeType)
                {
                    case XmlNodeType.Element:
                        int index = FindElementIndex(ANode);
                        ResultBuilder.Insert(0, "/" + ANode.Name + "[" + index + "]");
                        ANode = ANode.Parent;
                        break;
                    default:
                        throw new ArgumentException("Only elements and attributes are supported");
                }
            }
            return ResultBuilder.ToString();
        }

        static int FindElementIndex(XElement AElement)
        {
            var ParentNode = AElement.Parent;
            if (ParentNode == null)
                return 1;
            var Parent =ParentNode;
            var Index = 1;
            foreach (XNode candidate in Parent.Nodes())
            {
                if (candidate is XElement &&  ((XElement)candidate).Name == AElement.Name)
                {
                    if (candidate == AElement)
                        return Index;
                    Index++;
                }
            }
            throw new ArgumentException("Couldn't find element within parent");
        }
    }
}
