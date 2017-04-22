﻿using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Windows;

namespace NetSync_WinDesktop
{
    /// <summary>
    /// Provides methods to manage synchronization profiles.
    /// </summary>
    class SyncProfilesHandler
    {
        static string syncProfilesStore = Directory.GetCurrentDirectory() + @"\profiles.dat";
        static public List<SyncProfile> AvailableProfilesList { get; set; }
        static public List<SyncProfile> SelectedProfilesList { get; set; }

        /// <summary>
        /// Creates new sync profile using entered name and selected folder.
        /// </summary>
        static public bool AddNewProfile(string name, string path)
        {
            SyncProfile newProfile = new SyncProfile(name, path, DateTime.Now.ToString());
            if (CheckInputData(newProfile))
            {
                LoadProfiles();
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Validates input name and folder to avoid the similar profiles.
        /// </summary>
        static bool CheckInputData(SyncProfile newProfile)
        {
            if (AvailableProfilesList.Count != 0)
            {
                int searchIndex = 0;
                for (int i = 0; i < AvailableProfilesList.Count; i++)
                {
                    if (AvailableProfilesList[i].ProfileName == newProfile.ProfileName
                        || AvailableProfilesList[i].ProfileSyncFolderPath == newProfile.ProfileSyncFolderPath)
                    {
                        searchIndex = i;
                        break;
                    }
                    else
                        i++;
                }
                if (AvailableProfilesList[searchIndex].ProfileName == newProfile.ProfileName)
                {
                    MessageBox.Show("Profile with entered name is already exists. Please, enter other name",
                        "Name entering error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                else if (AvailableProfilesList[searchIndex].ProfileSyncFolderPath == newProfile.ProfileSyncFolderPath)
                {
                    MessageBox.Show("Profile with selected folder is already exists. Please, choose other folder",
                        "Folder selecting error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                else
                {
                    SaveProfile(newProfile);
                    return true;
                }
            }
            else
            {
                SaveProfile(newProfile);
                return true;
            }
        }

        /// <summary>
        /// Saves new created profile to local store.
        /// </summary>
        static void SaveProfile(SyncProfile profile)
        {
            string profileInfo = profile.ProfileName + "|" +
                    profile.ProfileSyncFolderPath + "|" + profile.SyncDateTime + Environment.NewLine;

            if (File.Exists(syncProfilesStore))
                File.AppendAllText(syncProfilesStore, profileInfo, Encoding.UTF8);
            else
                File.WriteAllText(syncProfilesStore, profileInfo, Encoding.UTF8);
        }

        /// <summary>
        /// Uploads profiles from local store.
        /// </summary>
        static public void LoadProfiles()
        {
            string[] syncProfilesArray = File.ReadAllLines(syncProfilesStore);
            LoadEmptyProfiles();
            foreach (var profile in syncProfilesArray)
            {
                string[] substrings = profile.Split('|');
                AvailableProfilesList.Add(new SyncProfile(substrings[0], substrings[1], substrings[2]));
            }
        }

        /// <summary>
        /// Creates new list to hold profiles.
        /// </summary>
        static public void LoadEmptyProfiles()
        {
            AvailableProfilesList = new List<SyncProfile>();
        }

        /// <summary>
        /// Deletes profile with entered name and saves changes in local store.
        /// </summary>
        static public void DeleteProfile(string name)
        {
            for (int i = 0; i < AvailableProfilesList.Count; i++)
            {
                if (AvailableProfilesList[i].ProfileName == name)
                    AvailableProfilesList.RemoveAt(i);
            }

            File.Delete(syncProfilesStore);
            foreach (var item in AvailableProfilesList)
                SaveProfile(item);
        }
    }
}
