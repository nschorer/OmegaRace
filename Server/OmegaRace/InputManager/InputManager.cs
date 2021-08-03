using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaRace
{
    public enum INPUTAXIS
    {
        HORIZONTAL_P1,
        HORIZONTAL_P2,
        VERTICAL_P1,
        VERTICAL_P2
    }

    public enum INPUTBUTTON
    {
        JUMP,
        START,
        P1_FIRE,
        P1_FIRE2,
        P2_FIRE,
        P2_FIRE2
    }
    

    class InputManager
    {
        private static InputManager instance;
        private static InputManager Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new InputManager();
                }
                return instance;
            }
        }

        KeyState A;
        KeyState D;
        KeyState W;
        KeyState S;
        KeyState UP_ARROW;
        KeyState DOWN_ARROW;
        KeyState LEFT_ARROW;
        KeyState RIGHT_ARROW;
        KeyState SPACE;
        KeyState F;
        KeyState C;
        KeyState I;
        KeyState K;
        KeyState J;
        KeyState L;
        KeyState H;
        KeyState N;
        KeyState ENTER;
        //MouseButtonState MOUSE1;
        //MouseButtonState MOUSE2;


        private InputManager()
        {

            A = new KeyState(Azul.AZUL_KEY.KEY_A);
            D = new KeyState(Azul.AZUL_KEY.KEY_D);
            W = new KeyState(Azul.AZUL_KEY.KEY_W);
            S = new KeyState(Azul.AZUL_KEY.KEY_S);
            UP_ARROW = new KeyState(Azul.AZUL_KEY.KEY_ARROW_UP);
            DOWN_ARROW = new KeyState(Azul.AZUL_KEY.KEY_ARROW_DOWN);
            LEFT_ARROW = new KeyState(Azul.AZUL_KEY.KEY_ARROW_LEFT);
            RIGHT_ARROW = new KeyState(Azul.AZUL_KEY.KEY_ARROW_RIGHT);
            SPACE = new KeyState(Azul.AZUL_KEY.KEY_SPACE);
            F = new KeyState(Azul.AZUL_KEY.KEY_F);
            C = new KeyState(Azul.AZUL_KEY.KEY_C);

            I = new KeyState(Azul.AZUL_KEY.KEY_I);
            J = new KeyState(Azul.AZUL_KEY.KEY_J);
            L = new KeyState(Azul.AZUL_KEY.KEY_L);
            K = new KeyState(Azul.AZUL_KEY.KEY_K);

            H = new KeyState(Azul.AZUL_KEY.KEY_H);
            N = new KeyState(Azul.AZUL_KEY.KEY_N);

            // Enter mapping is incorrect in azul dll
            ENTER = new KeyState((Azul.AZUL_KEY)126);
            
            //MOUSE1 = new MouseButtonState(Azul.AZUL_MOUSE.BUTTON_1);
            //MOUSE2 = new MouseButtonState(Azul.AZUL_MOUSE.BUTTON_2);
        }

        public static void Update()
        {
            Instance.KeyStateUpdate();
           

        }

        //   Function:      GetAxis
        //   Discription:   Gets a value of user input along an Input Axis 
        //   Parameter:     InputAxis name
        //                      What Input are you listening for.
        //                      EX:  Horizontal, will return a value based on all the inputs that are considered 
        //                          horizontal input. i.e. <-, ->, a, d
        //   Return:        int value
        //                      value returned will be either -1, 0, 1. (represents positive, none, negative, input)
        //                      e.x. if 'a' is pressed only then Horizontal will return -1.  
        public static int GetAxis(INPUTAXIS name)
        {
            int output = 0;

            switch(name)
            {
                case INPUTAXIS.HORIZONTAL_P1:
                    output = CalculateAxis(Instance.D, Instance.A);
                    break;
                case INPUTAXIS.HORIZONTAL_P2:
                    output = CalculateAxis(Instance.L, Instance.J);
                    break;
                case INPUTAXIS.VERTICAL_P1:
                    output = CalculateAxis(Instance.W, Instance.S);
                    break;
                case INPUTAXIS.VERTICAL_P2:
                    output = CalculateAxis(Instance.I, Instance.K);
                    break;

            }
            
            return output;
        }

        public static bool GetButton(INPUTBUTTON name)
        {
            bool output = false;

            switch (name)
            {
                case INPUTBUTTON.P1_FIRE:
                    output = CalculateButton(Instance.F);
                    break;
                case INPUTBUTTON.P1_FIRE2:
                    output = CalculateButton(Instance.C);
                    break;
                case INPUTBUTTON.P2_FIRE:
                    output = CalculateButton(Instance.H);
                    break;
                case INPUTBUTTON.P2_FIRE2:
                    output = CalculateButton(Instance.N);
                    break;
                case INPUTBUTTON.JUMP:
                    output = CalculateButton(Instance.SPACE);
                    break;
                case INPUTBUTTON.START:
                    output = CalculateButton(Instance.ENTER);
                    break;
            }

            return output;
        }

        public static bool GetButtonDown(INPUTBUTTON name)
        {
            bool output = false;

            switch (name)
            {
                case INPUTBUTTON.P1_FIRE:
                    output = CalculateButtonDown(Instance.F);
                    break;
                case INPUTBUTTON.P1_FIRE2:
                    output = CalculateButtonDown(Instance.C);
                    break;
                case INPUTBUTTON.P2_FIRE:
                    output = CalculateButtonDown(Instance.H);
                    break;
                case INPUTBUTTON.P2_FIRE2:
                    output = CalculateButtonDown(Instance.N);
                    break;
                case INPUTBUTTON.JUMP:
                    output = CalculateButtonDown(Instance.SPACE);
                    break;
                case INPUTBUTTON.START:
                    output = CalculateButtonDown(Instance.ENTER);
                    break;
            }

            return output;
        }

        public static bool GetButtonUp(INPUTBUTTON name)
        {
            bool output = false;

            switch (name)
            {
                case INPUTBUTTON.P1_FIRE:
                    output = CalculateButtonUp(Instance.F);
                    break;
                case INPUTBUTTON.P1_FIRE2:
                    output = CalculateButtonUp(Instance.C);
                    break;
                case INPUTBUTTON.P2_FIRE:
                    output = CalculateButtonUp(Instance.H);
                    break;
                case INPUTBUTTON.P2_FIRE2:
                    output = CalculateButtonUp(Instance.N);
                    break;
                case INPUTBUTTON.JUMP:
                    output = CalculateButtonUp(Instance.SPACE);
                    break;
                case INPUTBUTTON.START:
                    output = CalculateButtonUp(Instance.ENTER);
                    break;
            }

            return output;
        }


        private void KeyStateUpdate()
        {
            A.Update();
            W.Update();
            S.Update();
            D.Update();
            UP_ARROW.Update();
            DOWN_ARROW.Update();
            RIGHT_ARROW.Update();
            LEFT_ARROW.Update();
            SPACE.Update();
            ENTER.Update();
            //MOUSE1.Update();
            //MOUSE2.Update();
            F.Update();
            C.Update();
            J.Update();
            I.Update();
            K.Update();
            L.Update();
            H.Update();
            N.Update();
        }
        

        private static bool CalculateButton(MouseButtonState key)
        {
            return key.Pressed();
        }

        private static bool CalculateButton(KeyState key)
        {
            return key.Pressed();
        }

        private static bool CalculateButtonDown(MouseButtonState key)
        {
            return key.PressedDown();
        }
        private static bool CalculateButtonDown(KeyState key)
        {
            return key.PressedDown();
        }

        private static bool CalculateButtonUp(MouseButtonState key)
        {
            return key.PressedUp();
        }

        private static bool CalculateButtonUp(KeyState key)
        {
            return key.PressedUp();
        }

        private static int CalculateAxis(KeyState positiveKey, KeyState negativeKey)
        {
            return (positiveKey.Pressed() ? 1:0) - (negativeKey.Pressed()?1:0);
        }


    }

    


}
