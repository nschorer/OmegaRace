using System;
using System.Diagnostics;

namespace OmegaRace
{
    
    public class InputTest
    {
        public static void KeyboardTest()
        {

            AxisTest();

            ButtonTest();

            ButtonDownTest();

            ButtonUpTest();



        }

        public static void AxisTest()
        {
            int p1H = InputManager.GetAxis(INPUTAXIS.HORIZONTAL_P1);
            int p1V = InputManager.GetAxis(INPUTAXIS.VERTICAL_P1);

            int p2H = InputManager.GetAxis(INPUTAXIS.HORIZONTAL_P2);
            int p2V = InputManager.GetAxis(INPUTAXIS.VERTICAL_P2);

            Debug.WriteLine("p1 Horz & Vert: " + p1H + " " + p1V);
            Debug.WriteLine("p2 Horz & Vert: " + p2H + " " + p2V);
        }

        public static void ButtonTest()
        {
            /*
            if (InputManager.GetButton(INPUTBUTTON.FIRE))
            {
                Debug.WriteLine("Fire 1");
            }

            if (InputManager.GetButton(INPUTBUTTON.FIRE2))
            {
                Debug.WriteLine("Fire 2");
            }

            if (InputManager.GetButton(INPUTBUTTON.JUMP))
            {
                Debug.WriteLine("Jump");
            }
            //*/

        }

        public static void ButtonDownTest()
        {
            /*
            if (InputManager.GetButtonDown(INPUTBUTTON.FIRE))
            {
                Debug.WriteLine("Fire 1 Down");
            }

            if (InputManager.GetButtonDown(INPUTBUTTON.FIRE2))
            {
                Debug.WriteLine("Fire 2 Down");
            }

            if (InputManager.GetButtonDown(INPUTBUTTON.JUMP))
            {
                Debug.WriteLine("Jump Down");
            }
            //*/
        }

        public static void ButtonUpTest()
        {
            /*
            if (InputManager.GetButtonUp(INPUTBUTTON.FIRE))
            {
                Debug.WriteLine("Fire 1 Up");
            }

            if (InputManager.GetButtonUp(INPUTBUTTON.FIRE2))
            {
                Debug.WriteLine("Fire 2 Up");
            }

            if (InputManager.GetButtonUp(INPUTBUTTON.JUMP))
            {
                Debug.WriteLine("Jump Up");
            }
            //*/
        }

        /*
        public static void MouseTest()
        {

            // Quick and dirty test, if these work the rest do.
            // --> try move the mouse inside the window, click right, click left
            String a = "";
            String b = "";

            float xPos = 0.0f;
            float yPos = 0.0f;

            // get mouse position
            Azul.Input.GetCursor( ref xPos, ref yPos);

            // read mouse buttons
            if (Azul.Input.GetKeyState( Azul.AZUL_MOUSE.BUTTON_RIGHT ))
            {
                a = " <right>";
            }

            if (Azul.Input.GetKeyState( Azul.AZUL_MOUSE.BUTTON_LEFT ))
            {
                b = " <left>";
            }

            Console.WriteLine("({0},{1}): {2} {3} ", xPos, yPos, a, b);
        }
        */


    }
    
}
