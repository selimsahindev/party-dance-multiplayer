using System.Collections.Generic;

public static class ExtensionMethods
{
    public static void RemoveAtFast<T>(this List<T> list, int index)
    {
        int lastElementIndex = list.Count - 1;

        list[index] = list[lastElementIndex];
        list.RemoveAt(lastElementIndex);
    }
} 