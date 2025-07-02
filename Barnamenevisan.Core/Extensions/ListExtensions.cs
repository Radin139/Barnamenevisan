namespace Barnamenevisan.Core.Extensions;

public static class ListExtensions
{
    public static void Swipe<T>(this IList<T> list, int index, int index2)
    {
        T temp = list[index];
        list[index] = list[index2];
        list[index2] = temp;
    }
}