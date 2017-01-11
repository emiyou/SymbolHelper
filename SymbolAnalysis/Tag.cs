using System;
using System.Text.RegularExpressions;

namespace SymbolAnalysis
{
    //应该并不需要这个东西
    public enum CheckResult
    {
        NameNull,
        AddressNull,
        AddressError,
        AddressOut,
        DataTypeNull,
        DataTypeError,
        BoxingNull,
        Ok
    }

    public class Tag
    {
        public string Name { get;  }
        public string Address { get; }
        public string DataType { get; set; }
        public string Comment { get;  }

        public CheckResult CheckResult { get; set; }
        public string Boxing { get; set; }
        public string Station { get; set; }
        public int BoxingPropIndex { get; set; }

        public Tag(string name, string address, string comment, string dataType)
        {
            Name = name.Trim().Substring(1,name.Trim().Length-2);
            Address = address.Trim().Substring(1,address.Trim().Length - 2);
            DataType = dataType.Trim().Substring(1,dataType.Trim().Length - 2);
            Comment = comment.Trim().Substring(1,comment.Trim().Length - 2);
        }

        private static readonly string[] BoxingKeys = new[]
        {
            "HMI","GPB","DPB","PB","JIG","LR","SR","TL","HRB", "RRB", "LRB", "LF","AGV", "FST",
            "WPDP","PDP",  "MCP",  "INV", "VFD", "MC", "MJB",  "GS", "S","GD", "GC",
              "POB", "POJB","TJB",  "TVJB", "CJB", "CVJB","VJB",
            "RJB", "PCJB",  "PCPB", "TE", "PCM", "RC", "ATC",  "GL",  "SD", "SWP",
            "MAG", "RG", "VG","TM", "WA", "TD", "MTD",  "HMM", "WKT",
            "FKT", "MPC","PC",  "LBD","JB"
        };

        private static readonly string[] StationKeys = new[]
        {
            "UB", "MB", "RF", "ENG", "FF", "SB"
        };

        public override string ToString()
        {
            return $"\"{Name.PadLeft(30)}\",\"{Address} \",\"{DataType.PadLeft(10)}\",\"{Comment}\"";
        }

        // 校验Tag，将校验结果存入CheckResult中。如果DataType为空,则填充"BOOL"
        public void CheckTag()
        {
            if (string.IsNullOrEmpty(Name))
            {
                CheckResult=CheckResult.NameNull;
                return;
            }
            if (string.IsNullOrEmpty(Address))
            {
                CheckResult=CheckResult.AddressNull;
                return;
            }
            if (Address.Substring(0) != "I" && Address.Substring(0) != "Q" &&
                Address.Substring(0) != "i" && Address.Substring(0) != "q")
            {
                CheckResult=CheckResult.AddressError;
                return;
            }
            string[] addressNum = Address.Substring(1, Address.Length - 1).Trim().Split('.');
            if (addressNum.Length != 2)
            {
                CheckResult=CheckResult.AddressError;
                return;
            }
            int addInt;
            if (!int.TryParse(addressNum[0], out addInt)||addInt<=0)
            {
                CheckResult = CheckResult.AddressError;
                return;
            }
            if (addInt >= 6000)
            {
                CheckResult=CheckResult.AddressOut;
                return;
            }
            if (!int.TryParse(addressNum[1], out addInt) || addInt < 0 || addInt > 8)
            {
                CheckResult = CheckResult.AddressError;
                return;
            }
            if (string.IsNullOrEmpty(DataType))
            {
                CheckResult=CheckResult.DataTypeNull;
                DataType = "BOOL";
            }
            if (!string.Equals(DataType, "BOOL", StringComparison.CurrentCultureIgnoreCase))
            {
                CheckResult=CheckResult.DataTypeError;
                return;
            }
            SetBoxing();
            if (Boxing == "")
            {
                CheckResult=CheckResult.BoxingNull;
                return;
            }
            if (CheckResult == CheckResult.DataTypeNull)
            {
                return;
            }
            CheckResult=CheckResult.Ok;
        }

        public void SetBoxing()
        {
            foreach (string key in BoxingKeys)
            {
                Boxing = SetBoxing(key);
                if (Boxing != "")
                {
                    return;
                }
            }
        }

        private string SetBoxing(string boxingKey)
        {
            var regex = boxingKey == "PCPB" ? new Regex("PC[0-9]{2}PB[0-9]{3}") : new Regex(boxingKey + "[0-9]{3}");
            if (regex.IsMatch(Name))
            {
                return "";
            }
            regex = boxingKey == "PCPB" ? new Regex("PC[0-9]{2}PB[0-9]{2}") : new Regex(boxingKey + "[0-9]{2}");
            if (!regex.IsMatch(Name))
            {
                return "";
            }
            return regex.Match(Name).Groups[0].Value;

            // 通过实现定义boxingKey的顺序，使PDP必定在WPDP之后判断，从而避开PDP要反查WPDP的问题；
            //if (boxingKey == "PDP")
            //{
            //    regex=new Regex("WPDP[0-9]{2}");
            //}else if (boxingKey == "JB")
            //{
            //    regex= new Regex("[M,PO,T,V,TV,C,CV,R,PC]JB[0-9]{2}");
            //}
        }

        /// <summary>
        /// 会有变更
        /// </summary>
        public void SetBoxingProp()
        {
            BoxingPropIndex = -1;
            if (Boxing.Contains("HMI"))
            {
                //if(Comment.Contains())
                return;
            }
            if (Boxing.Contains("LR"))
            {
                // TODO
                return;
            }
            if (Boxing.Contains("PB"))
            {
                // TODO
                return;
            }
            if (Boxing.Contains("DPB"))
            {
                //todo
                return;
            }
            if (Boxing.Contains("GPB"))
            {
                //TODO
            }
        }
        
        public void SetStation()
        {
            foreach (string key in StationKeys)
            {
                Station = SetStation(key);
                if (Station != "")
                {
                    return;
                }
            }
        }

        private string SetStation(string stationKey)
        {
            Regex regex = new Regex(stationKey + "[0-9]{3}");
            if (!regex.IsMatch(Name))
            {
                return "";
            }
            else
            {
                return regex.Match(Name).Groups[0].Value;
            }
        }
    }
}
