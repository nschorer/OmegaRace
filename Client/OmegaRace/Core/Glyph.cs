using System;

namespace OmegaRace
{
    public class Glyph
    {

        public Glyph()
        {
            this.pNext = null;
            this.pFont = null;

            this.key = -1;
            this.x = -1;
            this.y = -1;
            this.width = -1;
            this.height = -1;
        }
        public Glyph(Azul.Texture pFont, int key, int x, int y, int width, int height)
        {
            this.pNext = null;
            this.pFont = pFont;

            this.key = key;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public void Set(int key, int x, int y, int width, int height)
        {
            this.pFont = null;

            this.key = key;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        // Data: --------------------------------
        public Glyph pNext;

        // data
        public int key;
        public int x;
        public int y;
        public int width;
        public int height;

        public Azul.Texture pFont;
    }

}
