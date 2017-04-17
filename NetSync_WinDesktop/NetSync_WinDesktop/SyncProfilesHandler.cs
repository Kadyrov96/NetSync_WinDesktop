using System;
using System.Collections.Generic;
using System.IO;

namespace NetSync_WinDesktop
{
    class SyncProfilesHandler
    {
        static string syncProfilesStore = Directory.GetCurrentDirectory() + "profiles.dat";
        static List<SyncProfile> availableProfilesList;
        static List<SyncProfile> selectedProfilesList;

        static public void AddNewProfile(string name, string path)
        {
            availableProfilesList = new List<SyncProfile>();
            availableProfilesList.Add(new SyncProfile(name, path, DateTime.Now.ToString()));
            SaveProfiles();
            LoadProfiles();
        }

        static public void DeleteProfile()
        {

        }

        static void LoadProfiles()
        {
            string[] syncProfilesArray = File.ReadAllLines(syncProfilesStore);
            availableProfilesList = new List<SyncProfile>();
            foreach (var profile in syncProfilesArray)
            {
                char delimiter = '_';
                string[] substrings = profile.Split(delimiter);
                availableProfilesList.Add(new SyncProfile(substrings[0], substrings[1], substrings[2]));
            }
        }

        static void SaveProfiles()
        {
            StreamWriter profileWriter;
            if (File.Exists(syncProfilesStore))
            {
                profileWriter = new StreamWriter(syncProfilesStore);

                foreach(var profile in availableProfilesList)
                    profileWriter.WriteLine(profile.ProfileName + "_" +
                        profile.ProfileSyncFolderPath + "_" + profile.SyncDateTime);
            }
            else
            {
                FileStream synProfilesStoreCreator = File.Create(syncProfilesStore);
                profileWriter = new StreamWriter(syncProfilesStore);

                foreach (var profile in availableProfilesList)
                    profileWriter.WriteLine(profile.ProfileName + "_" +
                        profile.ProfileSyncFolderPath + "_" + profile.SyncDateTime);
            }
            profileWriter.Close();
            profileWriter.Dispose();
        }
    }
}
