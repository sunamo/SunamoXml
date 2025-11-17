// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoXml.Generators;

public class XmlGeneratorSelective : XmlGenerator
{
    /// <summary>
    ///     A1 nemůže být null, musí to být v nejhorším případě Array.Empty
    /// </summary>
    /// <param name="p"></param>
    /// <param name="vynechat"></param>
    /// <param name="p_2"></param>
    public void WriteTagWithAttrsSelective(string p, List<string> vynechat, List<string> p_2)
    {
        stringBuilder.AppendFormat("<{0} ", p);
        for (var i = 0; i < p_2.Count / 2; i++)
        {
            var nameAtt = p_2[i * 2];
            if (!vynechat.Contains(nameAtt)) stringBuilder.AppendFormat("{0}=\"{1}\"", nameAtt, p_2[i * 2 + 1]);
        }

        stringBuilder.Append(">");
    }

    public override string ToString()
    {
        return stringBuilder.ToString();
    }
}