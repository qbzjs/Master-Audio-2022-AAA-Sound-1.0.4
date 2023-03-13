/*! \cond PRIVATE */
using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace DarkTonic.MasterAudio {
    [Serializable]
    // ReSharper disable once CheckNamespace
    public class BusDuckInfo {
        public List<BusSingleDuckInfo> BusesToDuck = new List<BusSingleDuckInfo>();
        public PlaylistController.AudioDuckingMode DuckingMode = PlaylistController.AudioDuckingMode.NotDucking;
        public string TriggeredBySoundGroup;
        public float TimeToStartUnducking;
        public float DuckFinishTime;
        public float DuckVolumeCut;
        public bool IsActive;
    }
}
/*! \endcond */