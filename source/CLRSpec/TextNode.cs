using System.Collections.Generic;
using System.Text;

namespace CLRSpec
{
    public class TextNode
    {
        private readonly TextNode _parent;
        private readonly List<TextNode> _nodes = new List<TextNode>();

        public TextNode(TextNode parent)
        {
            _parent = parent;
        }

        public TextNode Parent
        {
            get { return _parent; }
        }

        public List<TextNode> Nodes
        {
            get { return _nodes; }
        }

        public string Value { get; set; }

        public TextNode AppendChild(string value)
        {
            var child = new TextNode(this) {Value = value};
            _nodes.Add(child);
            return child;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(Value);
            builder.Append(" ");
            foreach (var child in Nodes)
            {
                builder.Append(child);
            }
            return builder.ToString().Trim();
        }
    }
}