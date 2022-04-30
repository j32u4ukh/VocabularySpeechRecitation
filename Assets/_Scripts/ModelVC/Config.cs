using UnityEngine;

namespace vts
{
    public class Config
    {
        public static ReciteMode[] modes = new ReciteMode[] { ReciteMode.Word, ReciteMode.Spelling, ReciteMode.Description };
        public static SystemLanguage target = SystemLanguage.English;
        public static SystemLanguage describe = SystemLanguage.Chinese;
    }
}
