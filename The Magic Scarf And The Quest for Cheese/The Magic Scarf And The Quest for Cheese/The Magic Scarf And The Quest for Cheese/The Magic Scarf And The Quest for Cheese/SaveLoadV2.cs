using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;

namespace The_Magic_Scarf_And_The_Quest_for_Cheese
{
    public class SaveLoadV2
    {

        public PlayField field;
        public List<Sprite.Info> Innerspritelist = new List<Sprite.Info>();
        public List<Game1.Score> Scores;
        public List<Sprite.Info> Innerspritelist2 = new List<Sprite.Info>();
        public ToSave tosave = new ToSave();
        public SkyMan.ScoreInfo skyman;
       
        public struct ToSave 
        {   
            public List<Sprite.Info> Innerspritelist2;
            
            public List<Game1.Score> Scores2;
            
            public SkyMan.ScoreInfo skyman;

            
        }

        StorageFile sampleFile;

        async void CreateFile()
        {
            // Create sample file; replace if exists.
            StorageFolder storageFolder =
                ApplicationData.Current.LocalFolder;
            StorageFile sampleFile =
                await storageFolder.CreateFileAsync("Savegame.sav",
                    CreationCollisionOption.ReplaceExisting);
        }

        public SaveLoadV2(PlayField field, string action, List<Game1.Score> score, SkyMan.ScoreInfo skyman)
        {
            this.skyman = skyman;
            this.Scores = score;
            this.field = field;
            if (action == "SAVE")
                this.InitiateSave();
            else if (action == "LOAD")
                this.InitiateLoad();
        }
        /// <summary>
        /// Save
        /// </summary>
        async void InitiateSave()
        {
            tosave.Innerspritelist2 = new List<Sprite.Info>();

            foreach (Sprite sprite in field.onthefield)
            {
                tosave.Innerspritelist2.Add(sprite.Save());//add every Sprite to the list
            }
            tosave.Scores2 = Scores;

            tosave.skyman = skyman;

            //start the saving


            //CreateFile();

            //XmlSerializer serializer = new XmlSerializer(typeof(ToSave));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ToSave));
            using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync(
                              "savegame.sav",
                              CreationCollisionOption.ReplaceExisting))
            {
                serializer.WriteObject(stream, tosave);
                stream.Dispose();
            }
            

           
        
    }

        /// <summary>
        /// Load
        /// </summary>
        /// <returns></returns>

        async void InitiateLoad()
        {
            //initiates the loading
            
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ToSave));

            var myStream = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("savegame.sav");
            
            tosave = (ToSave)serializer.ReadObject(myStream);
            AfterLoad();


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

        public List<Game1.Score> ReturnScores()//for score sync 
        {
            return tosave.Scores2;
        }
        public SkyMan.ScoreInfo ReturnSkyMan()//for player score and live sync
        {
            return tosave.skyman;
        }


    }
}
