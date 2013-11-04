﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roots.FileSystemService
{
    public class Track : ITrack
    {

        internal FileTracker tracker;
        public string FullPath { get; private set; }
        public bool IsValid { get; private set; }        
        public bool IsDeleted { get;private set;}
        public byte[] Hash { get; private set; }
        

        internal Track(FileTracker tracker, string fullPath)
        {
            this.tracker = tracker;
            this.FullPath = fullPath;
            IsValid = false;
        }

        public void Renamed(string newFullPath)
        {            
            if (FullPath == newFullPath) return;
            var oldFullPath = FullPath;
            FullPath = newFullPath;
            CheckAndSetValidity();

            if (Hash == null) Hash = CalculateHash(FullPath);

            tracker.ChangeName(oldFullPath, newFullPath);
            tracker.Notify();
        }

        public void Deleted()
        {
            if (IsDeleted) return;            
            CheckAndSetValidity();
            IsDeleted = true;
            tracker.MarkDeleted(this);
            tracker.Notify();
        }

        public void Created()
        {
            var newHash = CalculateHash(FullPath);
            CheckAndSetValidity();
            tracker.Notify();
        }

        public void Changed()
        {
            var newHash = CalculateHash(FullPath);
            if (Hash != null && Hash.SequenceEqual(newHash)) return;
            Hash = newHash;
            CheckAndSetValidity();
            tracker.Notify();
        }

        private void CheckAndSetValidity()
        {
            if (!IsValid)
            {
                OriginalFullPath = FullPath;                
            }
            IsValid = true;
        }


        public static byte[] CalculateHash(string pFileName)
        {
            // definizione stringa valore di ritorno
            string retValue = string.Empty;
            // generiamo uno stream a partire da un file che rimane in ram
            // fino a quando non abbiamo concluso il calcolo
            // il token using consente di aprire e chiudere correttamente
            // lo stream delimitando l'area di scope
            // così il GC lavora bene
            using (System.IO.FileStream fs = System.IO.File.OpenRead(pFileName))
            {
                using (fs.LockScope())
                {
                    // definizione del nostro tipo
                    System.Security.Cryptography.MD5 sscMD5 = System.Security.Cryptography.MD5.Create();
                    // recuperiamo i bytes dell'hash
                    //byte[] mHash = sscMD5.ComputeHash(fs);
                    // conversione a 64 bit                
                    //retValue = Convert.ToBase64String(mHash);

                    return sscMD5.ComputeHash(fs);
                }
            }
            //return retValue;
        }



        public string OriginalFullPath { get; private set; }
    }
}