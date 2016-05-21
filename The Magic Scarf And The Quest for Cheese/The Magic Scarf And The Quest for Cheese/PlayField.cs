using System.Collections.Generic;

namespace The_Magic_Scarf_And_The_Quest_for_Cheese
{
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
