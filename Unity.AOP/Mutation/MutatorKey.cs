using System;
using System.ComponentModel;

namespace Unity.AOP.Mutation
{
    public class MutatorKey : IEquatable<MutatorKey>
    {
        private readonly string _scenario;
        private readonly Type _sourceType;
        private readonly Type _targetType;

        [ReadOnly(true)]
        public string Scenario
        {
            get { return _scenario; }
        }

        [ReadOnly(true)]
        public Type SourceType
        {
            get { return _sourceType; }
        }

        [ReadOnly(true)]
        public Type TargetType
        {
            get { return _targetType; }
        }

        public MutatorKey(string scenario, Type sourceType, Type targetType)
        {
            _scenario = scenario;
            _sourceType = sourceType;
            _targetType = targetType;
        }

        public virtual bool Equals(MutatorKey other)
        {
            if (other == null)
                return false;
            return _scenario == other._scenario && _sourceType == other._sourceType && _targetType == other._targetType;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj as MutatorKey);
        }

        public override int GetHashCode()
        {
            if (_scenario == null)
                return _sourceType.GetHashCode() ^ _targetType.GetHashCode();
            else
                return _scenario.GetHashCode() ^ _sourceType.GetHashCode() ^ _targetType.GetHashCode();
        }
    }
}
