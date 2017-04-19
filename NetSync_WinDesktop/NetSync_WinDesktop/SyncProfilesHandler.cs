using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace NetSync_WinDesktop
{
    class SyncProfilesHandler
    {
        static string syncProfilesStore = Directory.GetCurrentDirectory() + @"\profiles.dat";
        static public List<SyncProfile> AvailableProfilesList { get; set; }
        static public List<SyncProfile> SelectedProfilesList { get; set; }

        static public void AddNewProfile(string name, string path)
        {
            AvailableProfilesList = new List<SyncProfile>();
            AvailableProfilesList.Add(new SyncProfile(name, path, DateTime.Now.ToString()));
            SaveProfile(new SyncProfile(name, path, DateTime.Now.ToString()));
            LoadProfiles();
        }

        static public void DeleteProfile()
        {

        }

        static public void LoadProfiles()
        {
            string[] syncProfilesArray = File.ReadAllLines(syncProfilesStore);
            AvailableProfilesList = new List<SyncProfile>();
            foreach (var profile in syncProfilesArray)
            {
                string[] substrings = profile.Split('|');
                AvailableProfilesList.Add(new SyncProfile(substrings[0], substrings[1], substrings[2]));
            }
        }

        static public void SaveProfile(SyncProfile profile)
        {
            string profileInfo = profile.ProfileName + "|" +
                    profile.ProfileSyncFolderPath + "|" + profile.SyncDateTime + Environment.NewLine;

            if (File.Exists(syncProfilesStore))                
                File.AppendAllText(syncProfilesStore, profileInfo, Encoding.UTF8);
            else
                File.WriteAllText(syncProfilesStore, profileInfo, Encoding.UTF8);
        }  
    }
}
