using System.Threading;

namespace Unity.AOP.Logging
{
    public class ThreadIndentSizeProvider : IIndentSizeProvider
    {
        protected class Indent
        {
            private int _previousDepth;

            private int _depth;

            public int Depth
            {
                get { return _depth; }
                set
                {
                    _previousDepth = _depth;
                    _depth = value;
                }
            }

            public int PreviousDepth
            {
                get { return _previousDepth; }
            }

            public bool AtPole
            {
                get { return !(PreviousDepth > Depth); }
            }

            public static Indent operator ++(Indent indent)
            {
                indent.Depth++;
                return indent;
            }

            public static Indent operator --(Indent indent)
            {
                indent.Depth--;
                return indent;
            }
        }

        protected readonly ThreadLocal<Indent> _indent = new ThreadLocal<Indent>();

        public bool IsAtPole
        {
            get { return _indent.Value.AtPole; }
        }

        public int GetDepth()
        {
            return _indent.Value.Depth;
        }

        public void Increase()
        {
            _indent.Value++;
        }

        public void Decrease()
        {
            _indent.Value--;
        }
    }
}
