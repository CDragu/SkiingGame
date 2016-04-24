using System.Device;
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
    class SaveLoad
    {
        StorageDevice device;
        string containerName = "MyGamesStorage";
        string filename = "mysave.sav";
        PlayField field;
        List<Sprite.Info> Innerspritelist = new List<Sprite.Info>();
        /// <summary>
        /// Save the Game
        /// </summary>
        /// <param name="field"></param>
        /// <param name="action"></param>
        public SaveLoad(PlayField field, string action)
        {
            this.field = field;
            if (action == "SAVE")
                this.InitiateSave();
            else if (action == "LOAD")
                this.Load();
        }
        private void InitiateSave()
        {
            foreach(Sprite sprite in field.onthefield)
            {
                Innerspritelist.Add(sprite.Save());
            }
           
                StorageDevice.BeginShowSelector(PlayerIndex.One, this.SaveToDevice, null);
          
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
                XmlSerializer serializer = new XmlSerializer(typeof(List<Sprite.Info>));
                serializer.Serialize(stream, Innerspritelist);
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
                StorageDevice.BeginShowSelector(PlayerIndex.One, this.LoadFromDevice, null);
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
                XmlSerializer serializer = new XmlSerializer(typeof(List<Sprite.Info>));
                Innerspritelist = (List<Sprite.Info>)serializer.Deserialize(stream);
                stream.Close();
                container.Dispose();               
                AfterLoad(); //Update the game based on the save game file
            }
        }
        public PlayField AfterLoad()
        {
            int i = 0;
            foreach (Sprite sprite in field.onthefield)
            {
                sprite.Load(Innerspritelist[i]);
                i++;
            }
                return this.field;
        }
    }
}
