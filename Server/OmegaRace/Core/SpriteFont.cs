using System;
using System.Diagnostics;

namespace OmegaRace
{
    public class SpriteFont
    {
        public SpriteFont(string message, int xStart, int yStart)
        {
            this.msg = message;
            this.xStart = xStart;
            this.yStart = yStart;
        }

        public void Update()
        {
            // all the work is done in Draw
        }

        public void Draw()
        {
            String pMsg = this.msg;

            float xTmp = this.xStart;
            float yTmp = this.yStart;

            float xEnd = this.xStart;

            for (int i = 0; i < pMsg.Length; i++)
            {
                int key = Convert.ToByte(pMsg[i]);

                Glyph pGlyph = GlyphMan.Find(key);
                Debug.Assert(pGlyph != null);

                xTmp = xEnd + pGlyph.width / 2;

                Azul.Sprite pAzulSprite = new Azul.Sprite(pGlyph.pFont,
                                                            new Azul.Rect(pGlyph.x, pGlyph.y, pGlyph.width, pGlyph.height),
                                                            new Azul.Rect(xTmp, yTmp, pGlyph.width, pGlyph.height),
                                                            new Azul.Color(1.0f, 1.0f, 1.0f));

                pAzulSprite.Update();
                pAzulSprite.Render();

                // move the starting to the next character
                xEnd = pGlyph.width / 2 + xTmp;
            }

        }

        // Data: ----------------
        string msg;
        int xStart;
        int yStart;
    }
}
