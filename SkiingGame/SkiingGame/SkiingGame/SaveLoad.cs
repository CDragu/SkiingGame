﻿using System.Device;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using System.IO;
using System.Xml.Serialization;
using System.Diagnostics;


namespace SkiingGame
{
    public class SaveLoad
    {
        StorageDevice device;
        string containerName = "MyGamesStorage";
        string filename = "007AndtheMagicScraf.sav";
        PlayField field;
        List<Sprite.Info> Innerspritelist = new List<Sprite.Info>();
        List<RunSequence.Score> Scores;
        List<Sprite.Info> Innerspritelist2 = new List<Sprite.Info>();
        ToSave tosave = new ToSave();
        SkyMan.ScoreInfo skyman;
        public struct ToSave
        {
            public List<Sprite.Info> Innerspritelist2;
            public List<RunSequence.Score> Scores2;
            public SkyMan.ScoreInfo skyman;
        }
        
        /// <summary>
        /// Save the Game
        /// </summary>
        /// <param name="field"></param>
        /// <param name="action"></param>
        public SaveLoad(PlayField field, string action, List<RunSequence.Score> score, SkyMan.ScoreInfo skyman )//makes a copy of everyting on the field and decides if to save or load
        {
            this.skyman = skyman;
            this.Scores = score;
            this.field = field;
            if (action == "SAVE")
                this.InitiateSave();
            else if (action == "LOAD")
                this.Load();
        }
        private void InitiateSave()
        {
            tosave.Innerspritelist2 = new List<Sprite.Info>();
            
            foreach (Sprite sprite in field.onthefield)
            {
                tosave.Innerspritelist2.Add(sprite.Save());//add every Sprite to the list
            }
            tosave.Scores2 = Scores;

            tosave.skyman = skyman;

            StorageDevice.BeginShowSelector(PlayerIndex.One, this.SaveToDevice, null);//start the saving
          
        }
        
        void SaveToDevice(IAsyncResult result)
        {
            device = StorageDevice.EndShowSelector(result);
            if (device != null && device.IsConnected)
            {
               
                IAsyncResult r = device.BeginOpenContainer(containerName, null, null);
                result.AsyncWaitHandle.WaitOne();
                StorageContainer container = device.EndOpenContainer(r);
                if (container.FileExists(filename))
                    container.DeleteFile(filename);
                Stream stream = container.CreateFile(filename);
                XmlSerializer serializer = new XmlSerializer(typeof(ToSave));
                serializer.Serialize(stream, tosave);
                stream.Close();
                container.Dispose();
                result.AsyncWaitHandle.Close();
            }
        }

        /// <summary>
        /// Loading the Game
        /// </summary>
        /// <param name="sprite"></param>
        
        public void Load()
        {            
            this.InitiateLoad();
        }
        private void InitiateLoad()
        {         
                StorageDevice.BeginShowSelector(PlayerIndex.One, this.LoadFromDevice, null);//initiates the loading
        }

        void LoadFromDevice(IAsyncResult result)
        {
            device = StorageDevice.EndShowSelector(result);
            IAsyncResult r = device.BeginOpenContainer(containerName, null, null);
            result.AsyncWaitHandle.WaitOne();
            StorageContainer container = device.EndOpenContainer(r);
            result.AsyncWaitHandle.Close();
            if (container.FileExists(filename))
            {
                Stream stream = container.OpenFile(filename, FileMode.Open);
                XmlSerializer serializer = new XmlSerializer(typeof(ToSave));
                tosave = (ToSave)serializer.Deserialize(stream);
                stream.Close();
                container.Dispose();               
                AfterLoad(); //Update the game based on the save game file
            }
        }
        public PlayField AfterLoad()//returns the field list 
        {
            int i = 0;
            foreach (Sprite sprite in field.onthefield)
            {
                try
                {
                sprite.Load(tosave.Innerspritelist2[i]);
                i++;
                }
                catch//if it dose not work means that there is not a save file and creates one
                {
                    InitiateSave();
                    break;
                }
                
            }
                return this.field;
        }

        public List<RunSequence.Score> ReturnScores()//for score sync 
        {
            return tosave.Scores2;
        }
        public SkyMan.ScoreInfo ReturnSkyMan()//for player score and live sync
        {
            return tosave.skyman;
        }
    }
}
