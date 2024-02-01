

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SunamoXml._sunamo

{
    internal class SHReplace
    {
        internal static string ReplaceAllDoubleSpaceToSingle(string text, bool alsoHtml = false)
        {
            //text = SHSH.FromSpace160To32(text);

            if (alsoHtml)
            {
                text = text.Replace(" &nbsp;", " ");
                text = text.Replace("&nbsp; ", " ");
                text = text.Replace("&nbsp;", " ");
            }

            while (text.Contains(AllStrings.doubleSpace))
            {
                text = text.Replace(AllStrings.doubleSpace, AllStrings.space);// ReplaceAll2(text, AllStrings.space, );
            }

            // Here it was cycling, dont know why, therefore without while
            //while (text.Contains(AllStrings.doubleSpace16032))
            //{
            //text = ReplaceAll2(text, AllStrings.space, AllStrings.doubleSpace16032);
            //}

            //while (text.Contains(AllStrings.doubleSpace32160))
            //{
            //text = ReplaceAll2(text, AllStrings.space, AllStrings.doubleSpace32160);
            //}

            return text;
        }


        internal static string ReplaceAllWhitecharsForSpace(string c)
        {
            foreach (var item in AllChars.whiteSpacesChars)
            {
                if (item != AllChars.space)
                {
                    c = c.Replace(item, AllChars.space);
                }
            }

            return c;
        }
    }
}
