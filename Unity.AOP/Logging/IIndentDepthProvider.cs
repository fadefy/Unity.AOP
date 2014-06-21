namespace Unity.AOP.Logging
{
    public interface IIndentDepthProvider
    {
        int GetDepth();

        bool IsAtPole { get; }

        void Increase();

        void Decrease();
    }
}
