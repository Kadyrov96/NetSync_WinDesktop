﻿using System.Collections.Generic;
using System.IO;
using System;

namespace NetSync_WinDesktop
{
    /// <summary>
    /// Provides methods to synchronize local and remote folders.
    /// </summary>
    class Synchroniser
    {
        //Synchronizing folder object.
        FolderHandler folderToSync;
        //Path of the file, that includes data of the last synchronization: name of the file and its hash.
        internal string syncDataStoreFullPath;
        List<string> syncDataStoreRecords_List;

        //List<FileDescript> local_folderToSyncElements;

        //results of checking folders' statements to the hashes on each devices
        SyncRecordList local_list;
        SyncRecordList remote_list;

        #region Result names' lists of files, that must be deleted or downloaded to each device
        List<string> localDelList;
        List<string> remoteDelList;

        List<string> uploadList;
        List<string> downloadList;
        #endregion

        public Synchroniser(FolderHandler _folderToSync)
        {
            folderToSync = _folderToSync;
            syncDataStoreFullPath = Directory.GetCurrentDirectory() + @"\" +
                Hasher.GetStringHash(folderToSync.FolderPath);
            //local_folderToSyncElements = new List<FileDescript>();

            local_list = new SyncRecordList();
            remote_list = new SyncRecordList();

            localDelList = new List<string>();
            remoteDelList = new List<string>();
            uploadList = new List<string>();
            downloadList = new List<string>();
        }

        /// <summary>
        /// Adds name and key into local folder's statement list.
        /// </summary>
        private void AddLocalSyncElements(string elemName, int elemFlag)
        {
            local_list.AddRecord(elemName, elemFlag);

            //local_folderToSyncElements.Add(
            //    new FileDescript { ElementName = elemName, ModificationFlag = elemFlag });
        }

        /// <summary>
        /// Makes the desicion in case when local file was not changed.
        /// </summary>
        private void SwitchIfNotChanged(int _remoteIndex, int _localIndex)
        {
            switch (remote_list.keys[_remoteIndex])
            {
                case 0:
                    remote_list.RemoveAt(_remoteIndex);
                    break;
                case 1:
                    downloadList.Add(remote_list.names[_remoteIndex]);
                    remote_list.RemoveAt(_remoteIndex);
                    break;
                case 2:
                    break;
                case 3:
                    localDelList.Add(local_list.names[_localIndex]);
                    remote_list.RemoveAt(_remoteIndex);
                    break;
            }
        }

        /// <summary>
        /// Makes the desicion in case when local file was modified.
        /// </summary>
        private void SwitchIfModified(int _remoteIndex, int _localIndex)
        {
            switch (remote_list.keys[_remoteIndex])
            {
                case 0:
                    uploadList.Add(local_list.names[_localIndex]);
                    remote_list.RemoveAt(_remoteIndex);
                    break;
                case 1:
                case 2:
                    break;
                case 3:
                    localDelList.Add(local_list.names[_localIndex]);
                    remote_list.RemoveAt(_remoteIndex);
                    break;
            }
        }

        /// <summary>
        /// Makes the desicion in case when local file was deleted.
        /// </summary>
        private void SwitchIfDeleted(int _remoteIndex, int _localIndex)
        {
            switch (remote_list.keys[_remoteIndex])
            {
                case 0:
                    remoteDelList.Add(local_list.names[_localIndex]);
                    remote_list.RemoveAt(_remoteIndex);
                    break;
                case 1:
                case 2:
                    break;
                case 3:
                    remote_list.RemoveAt(_remoteIndex);
                    break;
            }
        }

        /// <summary>
        /// Makes desicion of actions with files that based on keys of local and remote files.
        /// </summary>
        private void SwitchOnKeys(int remoteIndex, int localIndex)
        {
            switch (local_list.keys[localIndex])
            {
                case 0:
                    SwitchIfNotChanged(remoteIndex, localIndex);
                    break;
                case 1:
                    SwitchIfModified(remoteIndex, localIndex);
                    break;
                case 3:
                    SwitchIfDeleted(remoteIndex, localIndex);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Reads the file that include names and modification keys of remote files.
        /// </summary>
        private void ReadRemoteStateFile(string _dataStorage)
        {
            string[] remoteSyncData = File.ReadAllLines(_dataStorage);
            for (int i = 0; i < remoteSyncData.Length; i++)
            {
                remote_list.AddRecord(
                    remoteSyncData[i].Substring(0, remoteSyncData[i].Length - 4),
                    Convert.ToInt32(remoteSyncData[i].Substring(remoteSyncData[i].Length - 1, 1)));
            }
        }

        /// <summary>
        /// Compares hashes of the current and the last session of synchronization.
        /// </summary>
        private void CompareHashes()
        {
            //Two lists to temporary collect files' hashes and names of the last sync
            List<string> lastSyncNamesList = new List<string>();
            List<string> lastSyncHashList = new List<string>();

            for (int i = 0; i < syncDataStoreRecords_List.Count; i++)
            {
                lastSyncNamesList.Add(syncDataStoreRecords_List[i].Substring(0, syncDataStoreRecords_List[i].Length - 38));
                lastSyncHashList.Add(syncDataStoreRecords_List[i].Substring(syncDataStoreRecords_List[i].Length - 32, 32));
            }

            for (int i = 0; i < syncDataStoreRecords_List.Count; i++)
            {
                int searchIndex = lastSyncNamesList.BinarySearch(syncDataStoreRecords_List[i]);
                if (searchIndex >= 0)
                {
                    string readenHash = lastSyncHashList[searchIndex];
                    if (Hasher.GetFileHash(folderToSync.FolderElements[i]) == readenHash)
                    {
                        AddLocalSyncElements(Path.GetFileName(syncDataStoreRecords_List[i]), 0);
                        lastSyncNamesList.RemoveAt(searchIndex);
                        lastSyncHashList.RemoveAt(searchIndex);
                    }
                    else
                    {
                        AddLocalSyncElements(Path.GetFileName(syncDataStoreRecords_List[i]), 1);
                        lastSyncNamesList.RemoveAt(searchIndex);
                        lastSyncHashList.RemoveAt(searchIndex);
                    }
                }
                else
                {
                    AddLocalSyncElements(Path.GetFileName(syncDataStoreRecords_List[i]), 2);
                }
            }
            foreach (var i in lastSyncNamesList)
                AddLocalSyncElements(Path.GetFileName(i), 3);

            lastSyncNamesList.Clear();
            lastSyncHashList.Clear();
        }

        /// <summary>
        /// Creates and fills the file that should include synchronization data of the concrete folder.
        /// </summary>
        public void CreateSyncDataStore()
        {
            folderToSync.CreateServiceFile(syncDataStoreFullPath + @".dat");
            StreamWriter hashTableWriter = new StreamWriter(syncDataStoreFullPath + @".dat");
            for (int i = 0; i < folderToSync.FolderElements.Length; i++)
            {
                hashTableWriter.WriteLine(folderToSync.FolderElements[i] + "***" + Hasher.GetFileHash(folderToSync.FolderElements[i]));
                AddLocalSyncElements(Path.GetFileName(folderToSync.FolderElements[i]), 0);
            }
            hashTableWriter.Close();
            hashTableWriter.Dispose();
        }

        /// <summary>
        /// Checks changes in synchronized folder.
        /// </summary>
        public void CheckLocalChanges()
        {
            if(File.Exists(syncDataStoreFullPath + @".dat"))
            {
                string[] syncDataStoreRecords = File.ReadAllLines(syncDataStoreFullPath + @".dat");
                syncDataStoreRecords_List = new List<string>(syncDataStoreRecords);
                //DataStore file exists, but it is empty.
                if (syncDataStoreRecords_List.Count == 0)
                {
                    //Non-emptiness of the folder means that all included files were created
                    if (!(folderToSync.IsFolderEmpty()))
                    {
                        for (int i = 0; i < folderToSync.FolderElements.Length; i++)
                            AddLocalSyncElements(Path.GetFileName(folderToSync.FolderElements[i]), 2);
                    }
                }
                else
                {
                    //Checking: folder's emptiness automatically means that all files were deleted
                    if (folderToSync.IsFolderEmpty())
                    {
                        for (int i = 0; i < syncDataStoreRecords.Length; i++)
                            AddLocalSyncElements(syncDataStoreRecords[i].Substring(0, syncDataStoreRecords[i].Length - 38), 3);
                    }
                    else
                    {
                        CompareHashes();
                    }
                }
            }
            else
                CreateSyncDataStore();
            //foreach (var file in local_folderToSyncElements)
            //    local_list.AddRecord(file.ElementName, file.ModificationFlag);
        }

        /// <summary>
        /// Compares synchronization data of local and remote folder
        /// and get result lists of files that have to be downloaded or uploaded.
        /// </summary>
        public string CompareDevicesSyncData()
        {
            //Searching for remote syncDataStore file
            string[] searchResults = Directory.GetFiles(Directory.GetCurrentDirectory(), @"_rem.txt", SearchOption.AllDirectories);
            if (searchResults.Length == 1)
            {
                ReadRemoteStateFile(searchResults[0]);
                for (int localIndex = 0; localIndex < local_list.Count; localIndex++)
                {
                    int searchIndex = remote_list.names.BinarySearch(local_list.names[localIndex]);
                    if (searchIndex >= 0)
                        SwitchOnKeys(searchIndex, localIndex);
                    else
                        uploadList.Add(local_list.names[localIndex]);
                }

                foreach (var i in remote_list.names)
                    downloadList.Add(i);
            }
            else
                System.Windows.Forms.MessageBox.Show("Файл с удаленного устройства не найден");

            CreateExchangeFile();
            return syncDataStoreFullPath + "_configure" + @".txt";
        }

        /// <summary>
        /// Creates and fills configure file, 
        /// which includes data about files, that have to be downloaded or deleted.
        /// </summary>
        private void CreateExchangeFile()
        {
            folderToSync.CreateServiceFile(syncDataStoreFullPath + "_configure" + @".txt");
            StreamWriter configWriter = new StreamWriter(syncDataStoreFullPath + "_configure" + @".txt");

            foreach (var file in remoteDelList)
                configWriter.WriteLine(file + "*DELT");

            foreach (var file in downloadList)
                configWriter.WriteLine(file + "*DOWN");

            foreach (var file in uploadList)
                configWriter.WriteLine(file + "*UPLD");

            configWriter.Close();
            configWriter.Dispose();
        }
    }
}