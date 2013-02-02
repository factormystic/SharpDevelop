using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICSharpCode.AvalonEdit.Document
{
    /// <summary>
    /// EventArgs with a TextDocument.
    /// </summary>
    public class TextDocumentEventArgs : EventArgs
    {
        TextDocument doc;

        /// <summary>
        /// Gets the TextDocument.
        /// </summary>
        public TextDocument Document
        {
            get
            {
                return doc;
            }
        }

        /// <summary>
        /// Creates a new TextDocumentEventArgs instance.
        /// </summary>
        public TextDocumentEventArgs(TextDocument doc)
        {
            this.doc = doc;
        }
    }
}
