namespace Unity.AOP.Mutation
{
    /// <summary>
    /// Defined a function that can convert an instance of type <typeparamref name="T"/> to an instance of type <typeparamref name="TResult"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public delegate TResult Mutate<in T, out TResult>(T source);
}
