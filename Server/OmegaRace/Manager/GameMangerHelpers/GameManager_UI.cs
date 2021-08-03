using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaRace
{
    class GameManager_UI
    {
        Azul.Sprite p1LifeDisplay;
        Azul.Sprite p2LifeDisplay;

        //Azul.Texture fontText32;
        Azul.Texture fontText20;
        SpriteFont p1ScoreText;
        SpriteFont p2ScoreText;

        public GameManager_UI()
        {

            fontText20 = new Azul.Texture("Arial20pt.tga");
            GlyphMan.AddXml("Arial20pt.xml", fontText20);

        }

        public void Load()
        {
            p1LifeDisplay = new Azul.Sprite(GameObject.shipTexture,
            new Azul.Rect(0, 0, 32, 32), new Azul.Rect(0, 0, 30, 30), new Azul.Color(0, 1, 0));
            p1LifeDisplay.angle = 90 * PhysicWorld.MATH_PI_180;
            p2LifeDisplay = new Azul.Sprite(GameObject.shipTexture,
            new Azul.Rect(0, 0, 32, 32), new Azul.Rect(0, 0, 30, 30), new Azul.Color(0, 0.5f, 1));
            p2LifeDisplay.angle = 90 * PhysicWorld.MATH_PI_180;
        }

        public void Update()
        {
            p1ScoreText = new SpriteFont(GameManager.Instance().p1Score + "", 380, 285);
            p1ScoreText.Update();
            p2ScoreText = new SpriteFont(GameManager.Instance().p2Score + "", 420, 285);
            p2ScoreText.Update();
        }

        public void Draw()
        {
            p1ScoreText.Draw();
            p2ScoreText.Draw();
        }
    }
}
