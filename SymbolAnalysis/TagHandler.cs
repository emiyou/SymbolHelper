using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace SymbolAnalysis
{
    public class TagHandler
    {
        /// <summary>
        /// 读取关注的设备到
        /// </summary>
        /// <param name="fullName"></param>
        public static List<Tag> GetAllTags(string fullName)
        {
            List<Tag> tagList = new List<Tag>();
            using (StreamReader sr = new StreamReader(fullName, Encoding.GetEncoding("gb2312")))
            {
                while (!sr.EndOfStream)
                {
                    string record = sr.ReadLine();
                    Tag tag = new Tag(record);
                    if (tag.CheckResult==CheckResult.FormatError)
                    {
                        continue;
                    }
                    tag.CheckTag();
                    tag.SetBoxing();
                    tag.SetBoxingProp();
                    tag.SetStation();
                    tagList.Add(tag);
                }
            }
            return tagList;
        }

        public static List<Tag> GetCorrectTags(string symbolPath,out string handProcess)
        {
            handProcess = "";
            List<Tag> tags = GetAllTags(symbolPath);
            IEnumerable<Tag> troubleTags = tags.Where(a => a.CheckResult == CheckResult.FormatError).ToList();
            if (troubleTags.Any())
            {
                handProcess+="因格式不正确而删除的Tag:\r\n";
                foreach (var tag in troubleTags)
                {
                    handProcess+=tag+"\r\n";
                }
                handProcess+="-----------------------\r\n";
            }
            troubleTags = tags.Where(a => a.CheckResult == CheckResult.NameNull).ToList();
            if (troubleTags.Any())
            {
                handProcess += "因名字为空而删除的Tag:\r\n";
                foreach (var tag in troubleTags)
                {
                    handProcess += tag + "\r\n";
                }
                handProcess += "-----------------------\r\n";
            }
            troubleTags = tags.Where(a => a.CheckResult == CheckResult.Reserved).ToList();
            if (troubleTags.Any())
            {
                handProcess += "因预留而删除的Tag:\r\n";
                foreach (var tag in troubleTags)
                {
                    handProcess += tag + "\r\n";
                }
                handProcess += "-----------------------\r\n";
            }
            troubleTags = tags.Where(a => a.CheckResult == CheckResult.AddressNull).ToList();
            if (troubleTags.Any())
            {
                handProcess += "因地址为空而删除的Tag:\r\n";
                foreach (var tag in troubleTags)
                {
                    handProcess += tag + "\r\n";
                }
                handProcess += "-----------------------\r\n";
            }
            troubleTags = tags.Where(a => a.CheckResult == CheckResult.AddressError).ToList();
            if (troubleTags.Any())
            {
                handProcess += "因地址错误而删除的Tag:\r\n";
                foreach (var tag in troubleTags)
                {
                    handProcess += tag + "\r\n";
                }
                handProcess += "-----------------------\r\n";
            }
            troubleTags = tags.Where(a => a.CheckResult == CheckResult.BoxingNull).ToList();
            if (troubleTags.Any())
            {
                handProcess += "因箱柜名称超出标准定义而删除的Tag:\r\n";
                foreach (var tag in troubleTags)
                {
                    handProcess += tag+ "\r\n";
                }
                handProcess += "-----------------------\r\n";
            }
            troubleTags = tags.Where(a => a.CheckResult == CheckResult.DataTypeError).ToList();
            if (troubleTags.Any())
            {
                handProcess += "因数据类型错误而删除的Tag:\r\n";
                foreach (var tag in troubleTags)
                {
                    handProcess += tag + "\r\n";
                }
                handProcess += "-----------------------\r\n";
            }
            troubleTags = tags.Where(a => a.CheckResult == CheckResult.DataTypeNull).ToList();
            if (troubleTags.Any())
            {
                handProcess += "因数据类型为空而补充BOOL类型的Tag:\r\n";
                foreach (var tag in troubleTags)
                {
                    handProcess += tag + "\r\n";
                }
                handProcess += "-----------------------\r\n";
            }
            
            for (int i = 0; i < tags.Count - 1; i++)
            {
                if (tags[i].CheckResult != CheckResult.Ok && tags[i].CheckResult != CheckResult.DataTypeNull)
                {
                    continue;
                }
                for (int j = i + 1; j < tags.Count; j++)
                {
                    if (tags[j].CheckResult != CheckResult.Ok && tags[j].CheckResult != CheckResult.DataTypeNull)
                    {
                        continue;
                    }
                    if (tags[j].NameNonUnique)
                    {
                        continue;
                    }
                    if (tags[i].Name == tags[j].Name)
                    {
                        tags[j].NameNonUnique = true;
                        if (!tags[i].NameNonUnique)
                        {
                            handProcess += "因符号名均为“"+tags[i].Name+"”而删除的Tag:\r\n";
                            handProcess += tags[i] + "\r\n";
                            tags[i].NameNonUnique = true;
                        }
                        handProcess += tags[j] + "\r\n";
                    }
                }
                if (tags[i].NameNonUnique)
                {
                    handProcess += "-----------------------\r\n";
                }
                for (int j = i + 1; j < tags.Count; j++)
                {
                    if (tags[j].CheckResult != CheckResult.Ok && tags[j].CheckResult != CheckResult.DataTypeNull)
                    {
                        continue;
                    }
                    if (tags[j].AddressNonUnique)
                    {
                        continue;
                    }
                    if (tags[i].Address == tags[j].Address)
                    {
                        tags[j].AddressNonUnique = true;
                        if (!tags[i].AddressNonUnique)
                        {
                            handProcess += "因地址均为“" + tags[i].Name + "”而删除的Tag:\r\n";
                            handProcess += tags[i] + "\r\n";
                            tags[i].AddressNonUnique = true;
                        }
                        handProcess += tags[j] + "\r\n";
                    }
                }
                if (tags[i].AddressNonUnique)
                {
                    handProcess += "-----------------------\r\n";
                }
            }
            return
                tags.Where(
                    a =>
                        (a.CheckResult == CheckResult.Ok || a.CheckResult == CheckResult.DataTypeNull) &&
                        a.AddressNonUnique == false && a.NameNonUnique == false).ToList();
        }
        
    }
}
