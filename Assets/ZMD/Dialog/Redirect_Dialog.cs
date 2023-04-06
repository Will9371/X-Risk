using System.Collections.Generic;

namespace ZMD.Dialog
{
    // Redirect as needed per-project
    public static class Redirect
    {
        public static List<OccasionInfo> playerDecisions => NarrativeHub.instance.decisions.occasions;
    }
}
