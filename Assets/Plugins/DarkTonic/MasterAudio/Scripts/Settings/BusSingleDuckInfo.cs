/*! \cond PRIVATE */
using System;

// ReSharper disable once CheckNamespace
namespace DarkTonic.MasterAudio {
    [Serializable]
    // ReSharper disable once CheckNamespace
    public class BusSingleDuckInfo {
        public GroupBus Bus;
        public float StartingVolume;
        public float InitialDuckVolume;
        public float DuckRange;
    }
}
/*! \endcond */