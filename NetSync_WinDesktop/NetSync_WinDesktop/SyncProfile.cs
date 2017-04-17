using System;

namespace NetSync_WinDesktop
{
    class SyncProfile
    {
        public SyncProfile(string name, string path, string dateTime)
        {
            ProfileName = name;
            ProfileSyncFolderPath = path;
            SyncDateTime = dateTime;
        }

        public string ProfileName { get; set; }
        public string ProfileSyncFolderPath { get; set; }
        public string SyncDateTime { get; set; }
    }
}
