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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Achordeon.Common.Extensions;
using Achordeon.Common.Helpers;

namespace Achordeon.Shell.Wpf.Helpers.RecetFileList
{

    public sealed class RecentFileList
    {
        private int m_MaxFilesNumber = 8;
        private readonly ObservableCollection<RecentFile> m_RecentFiles;

        public ReadOnlyObservableCollection<RecentFile> RecentFiles { get; }

        public int MaxFilesNumber
        {
            get { return m_MaxFilesNumber; }
            set
            {
                if (m_MaxFilesNumber == value)
                    return;
                if (value <= 0)
                    throw new ArgumentException("The value must be equal or larger than 1.");
                m_MaxFilesNumber = value;
                if (m_RecentFiles.Count - m_MaxFilesNumber < 1)
                    return;
                RemoveRange(m_MaxFilesNumber, m_RecentFiles.Count - m_MaxFilesNumber);
            }
        }

        private int PinCount
        {
            get { return m_RecentFiles.Count(r => r.IsPinned); }
        }

        public RecentFileList()
        {
            m_RecentFiles = new ObservableCollection<RecentFile>();
            RecentFiles = new ReadOnlyObservableCollection<RecentFile>(m_RecentFiles);
        }

        public void Load(IEnumerable<RecentFile> ARecentFiles)
        {
            if (ARecentFiles == null)
                throw new ArgumentNullException(nameof(ARecentFiles));
            Clear();
            AddRange(ARecentFiles.Take(m_MaxFilesNumber));
        }

        public void AddFile(string AFileName)
        {
            if (string.IsNullOrEmpty(AFileName))
                throw new ArgumentException("The argument fileName must not be null or empty.");
            var File = m_RecentFiles.FirstOrDefault(r => r.Path == AFileName);
            if (File != null)
            {
                var OldIndex = m_RecentFiles.IndexOf(File);
                var NewIndex = File.IsPinned ? 0 : PinCount;
                if (OldIndex == NewIndex)
                    return;
                m_RecentFiles.Move(OldIndex, NewIndex);
            }
            else
            {
                if (PinCount >= m_MaxFilesNumber)
                    return;
                if (m_RecentFiles.Count >= m_MaxFilesNumber)
                    RemoveAt(m_RecentFiles.Count - 1);
                Insert(PinCount, new RecentFile(AFileName));
            }
        }

        public void Remove(string AFileName)
        {
            var Existing = RecentFiles.FirstOrDefault(a => a.Path == AFileName);
            if (Existing == null)
                return;
            Remove(Existing);
        }

        public void Remove(RecentFile ARecentFile)
        {
            if (ARecentFile == null)
                throw new ArgumentNullException(nameof(ARecentFile));
            if (!m_RecentFiles.Remove(ARecentFile))
                throw new ArgumentException("The passed recentFile was not found in the recent files list.");
            ARecentFile.PropertyChanged -= RecentFilePropertyChanged;            
        }
   
        public void SaveToXml(XmlFile AXml)
        {
            AXml.ThrowIfNullEx(nameof(AXml));
            var FilesNode = AXml.Add("RecentFiles");
            foreach (var RecentFile in m_RecentFiles)
                RecentFile.SaveToXml(FilesNode.Add("RecentFile"));
        }

        public void LoadFromXml(XmlFile AXml)
        {
            AXml.ThrowIfNullEx(nameof(AXml));
            Clear();
            var FilesNode = AXml.SelectSingle("RecentFiles");
            if (FilesNode == null)
                return;
            foreach (var FileNode in FilesNode.SelectAll("RecentFile"))
            {
                var RecentFile = new RecentFile(FileNode);
                if (!string.IsNullOrWhiteSpace(RecentFile.Path))
                    Add(RecentFile);
            }
        }

        private void Insert(int AIndex, RecentFile ARecentFile)
        {
            ARecentFile.ThrowIfNullEx(nameof(ARecentFile));
            ARecentFile.PropertyChanged += RecentFilePropertyChanged;
            m_RecentFiles.Insert(AIndex, ARecentFile);
        }

        private void Add(RecentFile ARecentFile)
        {
            ARecentFile.ThrowIfNullEx(nameof(ARecentFile));
            ARecentFile.PropertyChanged += RecentFilePropertyChanged;
            m_RecentFiles.Add(ARecentFile);
        }

        private void AddRange(IEnumerable<RecentFile> ARecentFilesToAdd)
        {            
            foreach (var RecentFile in ARecentFilesToAdd)
                Add(RecentFile);
        }

        private void RemoveAt(int AIndex)
        {
            m_RecentFiles[AIndex].PropertyChanged -= RecentFilePropertyChanged;
            m_RecentFiles.RemoveAt(AIndex);
        }

        private void RemoveRange(int AIndex, int ACount)
        {
            for (var i = 0; i < ACount; ++i)
                RemoveAt(AIndex);
        }

        private void Clear()
        {
            foreach (var File in m_RecentFiles)
                File.PropertyChanged -= RecentFilePropertyChanged;
            m_RecentFiles.Clear();
        }

        private void RecentFilePropertyChanged(object ASender, PropertyChangedEventArgs AArgs)
        {
            var TargetProperty = nameof(RecentFile.IsPinned);
            if (AArgs.PropertyName != TargetProperty)
                return;
            var File = (RecentFile) ASender;
            var OldIndex = m_RecentFiles.IndexOf(File);
            if (File.IsPinned)
            {
                m_RecentFiles.Move(OldIndex, 0);
            }
            else
            {
                var Count = PinCount;
                if (OldIndex == Count)
                    return;
                m_RecentFiles.Move(OldIndex, Count);
            }
        }
    }
}
