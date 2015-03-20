namespace Fync.Common
{
    public interface IFactory<TOutput>
    {
        TOutput Manufacture(object parameters);
        TOutput Manufacture();
    }
}