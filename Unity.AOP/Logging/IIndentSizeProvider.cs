namespace Unity.AOP.Logging
{
    public interface IIndentSizeProvider
    {
        int GetDepth();

        bool IsAtPole { get; }

        void Increase();

        void Decrease();
    }
}
