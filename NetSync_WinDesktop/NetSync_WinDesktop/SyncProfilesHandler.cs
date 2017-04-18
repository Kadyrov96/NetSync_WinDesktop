using System;
using System.Collections.Generic;
using System.IO;

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
                char delimiter = '|';
                string[] substrings = profile.Split(delimiter);
                AvailableProfilesList.Add(new SyncProfile(substrings[0], substrings[1], substrings[2]));
            }
        }

        static public void SaveProfile(SyncProfile profile)
        {
            StreamWriter profileWriter;
            if (File.Exists(syncProfilesStore))
            {
                profileWriter = new StreamWriter(syncProfilesStore);
                profileWriter.WriteLine(profile.ProfileName + "|" +
                    profile.ProfileSyncFolderPath + "|" + profile.SyncDateTime);
            }
            else
            {
                FileStream synProfilesStoreCreator = File.Create(syncProfilesStore);
                synProfilesStoreCreator.Close();

                profileWriter = new StreamWriter(syncProfilesStore);
                profileWriter.WriteLine(profile.ProfileName + "|" +
                    profile.ProfileSyncFolderPath + "|" + profile.SyncDateTime);
            }
            profileWriter.Close();
            profileWriter.Dispose();
        }

         void SaveProfiles()
        {
            StreamWriter profileWriter;
            if (File.Exists(syncProfilesStore))
            {
                profileWriter = new StreamWriter(syncProfilesStore);

                foreach (var profile in AvailableProfilesList)
                    profileWriter.WriteLine(profile.ProfileName + "|" +
                        profile.ProfileSyncFolderPath + "|" + profile.SyncDateTime);
            }
            else
            {
                FileStream synProfilesStoreCreator = File.Create(syncProfilesStore);
                profileWriter = new StreamWriter(syncProfilesStore);

                foreach (var profile in AvailableProfilesList)
                    profileWriter.WriteLine(profile.ProfileName + "|" +
                        profile.ProfileSyncFolderPath + "|" + profile.SyncDateTime);
            }
            profileWriter.Close();
            profileWriter.Dispose();
        }
    }
}
