using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolAnalysis
{
    // 安全门
    public class GatePushButton
    {
        public string Name;
        public string Station;
        public int EstopIndex;
        public int StationIndex;
        public int LineIndex;

        public string InPbReqIn;
        public string InPbReset;
        public string OutLampReqIn;
        public string OutLampReset;
        public string OutGreenLight;
        public string OutRedLight;
    }

    // 双手操作盒
    public class DoubleHandOB
    {
        public string Name;
        public string Station;
        public int EstopIndex;
        public int StationIndex;
        public int LineIndex;

        public string InStartPb1;
        public string InStartPb2;
        public string InResetPb;
        public string OutLcLamp;
        public string OutResetLamp;
        public string OutBlueYellow;
        public string OutGreen;
        public string OutRed;
    }

    // 单手操作盒
    public class SingleHandOB
    {
        public string Name;
        public string Station;
        public int EstopIndex;
        public int StationIndex;
        public int LineIndex;

        public string InStartPb;
        public string InResetPb;
        public string OutLcLamp;
        public string OutResetLamp;
        public string OutBlueYellow;
        public string OutGreen;
        public string OutRed;
    }

    // 光栅复位盒
    public class LightCurtainResetPb
    {
        public string Name;
        public string Station;
        public int EstopIndex;
        public int StationIndex;
        public int LineIndex;

        public string OutRed;

    }

    // 触摸屏
    public class OperatorPanel
    {
        public string Name;
        public int EstopIndex;
        public int LineIndex;

        public int InReady;
        public int InReset;
        public int InMute;
        public int InAutoRun;
        public string OutLampReady;
        public string OutLampReset;
        public string OutLampMute;
        public string OutLampAutoRun;
        public string OutHlAotuRun;
        public string OutHlSafety;
        public string OutHlFault;
    }
    
}
