using UnityEngine;

namespace vts.mvc
{
    public class SpeechNorm
    {
        public string mediator_name;
        public GameObject scroll;
        public string proxy_name;
        public SystemLanguage target;
        public SystemLanguage describe;
        public string table_name;

        public SpeechNorm(string mediator_name, GameObject scroll, string proxy_name, SystemLanguage target, SystemLanguage describe, string table_name)
        {
            this.mediator_name = mediator_name;
            this.scroll = scroll;
            this.proxy_name = proxy_name;
            this.target = target;
            this.describe = describe;
            this.table_name = table_name;
        }
    }
}
