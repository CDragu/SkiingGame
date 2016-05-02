using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SkiingGame
{
    /// <summary>
    /// objects that has a list of Sprites that is used to save the game
    /// </summary>
    public class PlayField
    {
        public List<Sprite> onthefield;
        public PlayField()
        {
            onthefield = new List<Sprite>();
        }
        public void Addtoplayfield(Sprite sprite)
        {
            onthefield.Add(sprite);
        }
    }
}
