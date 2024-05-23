namespace SunamoXml;


public class CAGConsts
{
    public static T[] ToArrayT<T>(params T[] aB)
    {
        return aB;
    }
    /// <summary>
    /// Tady to musí být, SunamoValues nemůže dědit od SunamoCollectionGeneric - vzniklo by Cycle detected
    /// Těch pár řádků mě snad nezabije.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <returns></returns>
    public static List<T> ToList<T>(params T[] t)
    {
        return t.ToList();
    }
}