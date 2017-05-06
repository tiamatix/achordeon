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
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Achordeon.Common.Helpers
{
    [Serializable]
    public class XmlFile : ICloneable, IComparable<XmlFile>, IComparable, ISerializable, IDisposable
    {
        public const string DATE_TIME_FORMAT = "yyyy-MM-dd HH:mm:ss";

        private static CultureInfo _Culture = CultureInfo.InvariantCulture;
        private XElement m_CursorElement;

        public XDocument Document { get; private set; }

        public static void OverrideCultureSettings(CultureInfo ANewCulture)
        {
            _Culture = ANewCulture;
        }

        public static XmlFile CreateFromString(string AXmlString)
        {
            return new XmlFile(AXmlString, true);
        }

        public static XmlFile CreateFromFile(string AXmlFileName)
        {
            return new XmlFile(AXmlFileName);
        }

        public static XmlFile CreateFromXmlFile(XmlFile AOther)
        {
            return CreateFromString(AOther.GetXmlString(false));
        }

        public static XmlFile CreateFromXmlDocument(XDocument ADoc)
        {
            return new XmlFile(ADoc);
        }

        public static XmlFile CreateFromXmlNode(XNode ANode)
        {
            return new XmlFile(ANode);
        }

        public static XmlFile CreateFromStream(Stream AStream)
        {
            using (var r = new StreamReader(AStream))
                return new XmlFile(r.ReadToEnd(), true);
        }

        public static XmlFile CreateEmpty(string ADocumentElementName)
        {
            return new XmlFile(ADocumentElementName, "1.0"); //MLHIDE
        }

        private XmlFile(string ADocumenElementName, string ADocumentVersion = "1.0")
        {
            Document = new XDocument();
            Document.Declaration = new XDeclaration(ADocumentVersion, "utf-8", "yes");
            Document.Add(new XElement(ADocumenElementName));
        }

        private XmlFile(XDocument ADoc)
        {
            Document = ADoc;
        }

        private XmlFile(XNode ANode)
        {
            Document = new XDocument();
            using (var rdr = ANode.CreateReader())
                Document.Add(XElement.Load(rdr));
        }

        private XmlFile(XDocument ADoc, XElement ANewCursorElement)
        {
            Document = ADoc;
            m_CursorElement = ANewCursorElement;
        }

        private XmlFile(string AFileName)
        {
            Document = new XDocument();
            Document.Add(XElement.Load(AFileName));
        }

        private XmlFile(string AXmlString, bool ALoadFromXmlString)
        {
            Document = new XDocument();
            if (ALoadFromXmlString)
                Document.Add(XElement.Parse(AXmlString, LoadOptions.PreserveWhitespace));
            else
                throw new Exception("XmlString(string XmlString, bool LoadFromXmlString == false)"); 
        }

        public XElement Root => Document.Root;


        public XElement CursorElement
        {
            get
            {
                if (m_CursorElement == null)
                    ResetCursorToRoot();
                return m_CursorElement;
            }        
        }

                
        public string CursorElementName
        {
            get
            {
                var e = CursorElement;
                return e == null ? string.Empty : e.Name.LocalName;
            }        
        }

        public string this[string ATagName]
        {
            get { return Get(ATagName); }
            set { Set(ATagName, value); }
        }
        public string this[string ATagName, string ADefaultValue] => Get(ATagName, ADefaultValue);

        public void ResetCursorToRoot()
        {
            m_CursorElement = Document.Root;
        }

        public void SetCursor(XElement AElement)
        {
            if (AElement.Document != Document)
                throw new XmlException("Cursor has to be set to a child element of the current file");
            m_CursorElement = AElement;
        }

        public object Clone()
        {
            return CreateFromXmlFile(this);
        }

        public XmlFile CloneAtCursor()
        {
            if (CursorElement == null)
                throw new XmlException("Cursor node is null");
            return CreateFromXmlNode(CursorElement);
        }

        public void Save(string AFileName)
        {
            Document.Save(AFileName);
        }

        public void Save(string AFileName, bool AIndend)
        {
            Save(AFileName, AIndend, null);
        }

        public void Save(string AFileName, bool AIndend, Encoding AEncoding)
        {
            if (!AIndend && AEncoding == null)
                Save(AFileName);
            else
            {
                using (var writer = new XmlTextWriter(AFileName, AEncoding))
                {
                    writer.Formatting = AIndend ? Formatting.Indented : Formatting.None;
                    Document.Save(writer);
                }
            }
        }

        public XDocument GetDocument()
        {
            return Document;
        }

        public string GetXmlString()
        {
            return Document.ToString(SaveOptions.DisableFormatting);
        }

        public MemoryStream GetStream(bool AIndend)
        {
            return GetStream(AIndend, true);
        }

        public MemoryStream GetStream(bool AIndend, bool AIncludeDeclaration)
        {
            return GetStream(AIndend, Encoding.Default, AIncludeDeclaration);
        }

        public MemoryStream GetStream(bool AIndend, Encoding AEncoding, bool AIncludeDeclaration)
        {            
            if (!AIndend)
                return new StringStream(GetXmlString());
            var result = new MemoryStream();
            var sets = new XmlWriterSettings();
            sets.CloseOutput = false;
            sets.IndentChars = "   "; 
            sets.Indent = true;
            sets.OmitXmlDeclaration = !AIncludeDeclaration;
            sets.Encoding = AEncoding;
            using (var writer = XmlWriter.Create(result, sets))
                Document.Save(writer);
            result.Seek(0, SeekOrigin.Begin);
            return result;
        }

        public string GetXmlString(bool AIndend)
        {
            return GetXmlString(AIndend, true);
        }

        public string GetXmlString(bool AIndend, bool AIncludeDeclaration)
        {
            return GetXmlString(AIndend, Encoding.Default, AIncludeDeclaration);
        }

        public string GetXmlString(bool AIndend, Encoding AEncoding, bool AIncludeDeclaration)
        {
            if (!AIndend && AIncludeDeclaration)
                return GetXmlString();
            if (AIndend && !AIncludeDeclaration)
                return Document.ToString(SaveOptions.None);
            using (var ms = GetStream(true, AIncludeDeclaration))
                return AEncoding.GetString(ms.ToArray());
        }

        public int CompareTo(XmlFile AOther)
        {
            return String.Compare(GetXmlString(), AOther.GetXmlString(), StringComparison.Ordinal);
        }

        public int CompareTo(object AOther)
        {
            if (AOther is XmlFile)
                return CompareTo((XmlFile)AOther);
            if (AOther == null)
                return 1;
            throw new Exception("Can only compare XmlFile2 to XmlFile2");
        }
                
        void ISerializable.GetObjectData(SerializationInfo AInfo, StreamingContext AContext)
        {
            AInfo.AddValue("xml", GetXmlString(false)); //MLHIDE
        }

        public XmlFile(SerializationInfo AInfo, StreamingContext AContext)
        {
            Document = new XDocument();
            Document.Add(XElement.Parse((string) AInfo.GetValue("xml", typeof (string)), LoadOptions.PreserveWhitespace));
        }

        public override string ToString()
        {
            return GetXmlString(true);
        }

        public void Dispose()
        {
            m_CursorElement = null;
            Document?.RemoveNodes();
            Document = null;
        }


        public XmlFile SelectSingle(string ANodeName)
        {
            if (CursorElement == null)
                throw new XmlException("Cursor node is null");
            if (string.IsNullOrEmpty(ANodeName))
                return this;
            var NewCursor = CursorElement.XPathSelectElement(ANodeName);            
            return NewCursor == null ? null : new XmlFile(Document, NewCursor);
        }

        public IEnumerable<XmlFile> SelectAll(string ANodeName = null)
        {
            if (CursorElement == null)
                throw new XmlException("Cursor node is null");
            if (string.IsNullOrEmpty(ANodeName))
            {
                foreach (var Element in CursorElement.Elements())
                    yield return new XmlFile(Document, Element);
                yield break;
            }
            foreach (var Element in CursorElement.XPathSelectElements(ANodeName))
                yield return new XmlFile(Document, Element);
        }

        public bool HasValue(string ATag = null)
        {
            var Tag = SelectSingle(ATag);
            return !string.IsNullOrEmpty(Tag?.CursorElement.Value);
        }

        public string Get(string ATag = null, string ADefaultValue = "")
        {
            var Tag = SelectSingle(ATag);
            if (Tag != null)
                return Tag.CursorElement.InnerText();
            return ADefaultValue;
        }

        public int GetI(string ATag = null, int ADefaultValue = 0)
        {
            var Tag = SelectSingle(ATag);
            if (Tag != null)
                return Convert.ToInt32(Tag.CursorElement.InnerText(), _Culture); 
            return ADefaultValue;
        }

        public double GetF(string ATag = null, double ADefaultValue = 0.0)
        {
            var ele = SelectSingle(ATag);
            if (ele != null)
                return Convert.ToDouble(ele.CursorElement.InnerText(), _Culture);
            return ADefaultValue;
        }

        public float GetFs(string ATag = null, float ADefaultValue = 0.0f)
        {
            var Tag = SelectSingle(ATag);
            if (Tag != null)
                return Convert.ToSingle(Tag.CursorElement.InnerText(), _Culture);
            return ADefaultValue;
        }        

        public DateTime GetDt(string ATag = null)
        {
            var Tag = SelectSingle(ATag);
            if (Tag != null)
                return DateTime.ParseExact(Tag.CursorElement.InnerText(), DATE_TIME_FORMAT, _Culture);
            return DateTime.MinValue;
        }

        public bool GetB(string ATag = null, bool ADefaultValue = false)
        {
            var Tag = SelectSingle(ATag);
            var Value = Tag?.CursorElement.InnerText();
            if (!string.IsNullOrEmpty(Value))
            {
                int i;
                if (char.IsNumber(Value, 0) && int.TryParse(Value, NumberStyles.Integer, _Culture, out i))
                    return i != 0;
                return Convert.ToBoolean(Tag.CursorElement.InnerText(), _Culture);
            }
            return ADefaultValue;
        }

        public T GetEnum<T>(string ATag = null, T ADefaultValue = default(T))
            where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("GetEnum is only allowed for enum types");
            var Value = Get(ATag);
            if (string.IsNullOrEmpty(Value))
                return ADefaultValue;
            int i;
            if (int.TryParse(Value, NumberStyles.Integer, _Culture, out i))
            {
                try
                {
                    var Result = Convert.ChangeType(i, typeof(T));
                    if (Result != null)
                        return (T)Result;
                }
                catch (Exception)
                { }
            }
            return (T)Enum.Parse(typeof(T), Value, true);
        }

        public Guid GetG(string ATag = null)
        {
            var ele = SelectSingle(ATag);
            if (ele != null)
                return new Guid(ele.CursorElement.InnerText());
            return Guid.Empty;
        }

        public byte[] GetBytes(string ATag = null)
        {
            var s = Get(ATag);
            if (string.IsNullOrEmpty(s))
                return null;
            return Convert.FromBase64String(s);
        }

        public XmlFile Add(string ATag)
        {
            if (CursorElement == null)
                throw new XmlException("Cursor node is null");
            var NewCursor = new XElement(ATag);
            CursorElement.Add(NewCursor);
            return new XmlFile(Document, NewCursor);
        }

        public XComment AddComment(string AComment)
        {
            if (CursorElement == null)
                throw new XmlException("Cursor node is null");
            var res = new XComment(AComment);
            CursorElement.Add(res);
            return res;
        }

        public void Delete(string ATag = null)
        {
            if (CursorElement == null)
                throw new XmlException("Cursor node is null");
            var Element = !string.IsNullOrEmpty(ATag) ? CursorElement.XPathSelectElement(ATag) : CursorElement;
            if (Element != null)
            {
                if (Element == CursorElement)
                    throw new XmlException("Cursor-Element must not be removed");
                bool ContainesCursor = false;
                try
                {
                    var XPath = Element.GetXPath();
                    var CursorXpath = CursorElement.GetXPath();
                    ContainesCursor = XPath.Contains(CursorXpath);
                }
                catch { }
                if (ContainesCursor)
                    throw new XmlException("Cursor-Element must not be removed");
                Element.Remove();
            }
        }

        public void Set(string ATag, string AValue)
        {
            if (CursorElement == null)
                throw new XmlException("Cursor node is null");
            CursorElement.SetElementValue(ATag, AValue);
        }

        public void Set(string ATag, int AValue)
        {
            Set(ATag, AValue.ToString(_Culture));
        }

        public void Set(string ATag, bool AValue)
        {
            Set(ATag, AValue.ToString(_Culture));
        }

        public void Set(string ATag, double AValue)
        {
            Set(ATag, AValue.ToString(_Culture));
        }

        public void Set(string ATag, byte[] ABytes)
        {
            Set(ATag, Convert.ToBase64String(ABytes));
        }

        public void Set(string ATag, Guid AValue)
        {
            Set(ATag, AValue.ToString());
        }

        public void Set(string ATag, DateTime AValue)
        {
            Set(ATag, AValue.ToString(DATE_TIME_FORMAT, _Culture)); //MLHIDE
        }

        public void SetEnum<T>(string ATag, T AValue)
             where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("AddEnum can only handle enum types");
            Set(ATag, AValue.ToString(_Culture));
        }


        public void Set(string AValue)
        {
            if (CursorElement == null)
                throw new XmlException("Cursor node is null");
            CursorElement.Value = AValue;
        }

        public void Set(int AValue)
        {
            Set(AValue.ToString(_Culture));
        }

        public void Set(bool AValue)
        {
            Set(AValue.ToString(_Culture));
        }

        public void Set(double AValue)
        {
            Set(AValue.ToString(_Culture));
        }

        public void Set(byte[] ABytes)
        {
            Set(Convert.ToBase64String(ABytes));
        }

        public void Set(Guid AValue)
        {
            Set(AValue.ToString());
        }

        public void Set(DateTime AValue)
        {
            Set(AValue.ToString(DATE_TIME_FORMAT, _Culture)); //MLHIDE
        }

        public void SetEnum<T>(T AValue)
             where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("AddEnum can only handle enum types");
            Set(AValue.ToString(_Culture));
        }

    }
}
